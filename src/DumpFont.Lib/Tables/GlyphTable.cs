using DumpFont.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DumpFont.Tables
{
    public class GlyphTable
    {
        public const string Tag = "glyf";
        public uint TableOffset { get; private set; }
        private readonly BinaryReader _reader;

        public GlyphTable(BinaryReader reader, uint tableOffset)
        {
            _reader = reader;
            TableOffset = tableOffset;
        }

        public Glyph ReadGlyph(uint blockLocation)
        {
            _reader.BaseStream.Position = TableOffset + blockLocation;
            return Glyph.Read(_reader);
        }
    }

    public class Glyph
    {
        public GlyphHeader Header { get; private set; }
        public SimpleGlyph SimpleGlyph { get; private set; }

        public static Glyph Read(BinaryReader reader)
        {
            var glyph = new Glyph
            {
                Header = GlyphHeader.Read(reader)
            };

            if (glyph.Header.NumberOfContours >= 0)
            {
                glyph.SimpleGlyph = SimpleGlyph.Read(reader, glyph.Header.NumberOfContours);
            }

            return glyph;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.Append(Header.Dump());
            sb.Append(SimpleGlyph.Dump());
            return sb.ToString();
        }
    }

    public class GlyphHeader
    {
        public short NumberOfContours { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }

        public static GlyphHeader Read(BinaryReader reader)
        {
            var instance = new GlyphHeader
            {
                NumberOfContours = reader.ReadInt16BigEndian(),
                XMin = reader.ReadInt16BigEndian(),
                YMin = reader.ReadInt16BigEndian(),
                XMax = reader.ReadInt16BigEndian(),
                YMax = reader.ReadInt16BigEndian()
            };
            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValueLine("NumberOfContours", NumberOfContours);
            sb.AppendTitleValueLine("XMin", XMin);
            sb.AppendTitleValueLine("YMin", YMin);
            sb.AppendTitleValueLine("XMax", XMax);
            sb.AppendTitleValueLine("YMax", YMax);
            return sb.ToString();
        }
    }

    [Flags]
    public enum Flags : byte
    {
        ControlPoint = 0x0,
        OnCurvePoint = 0x1,
        XShortVector = 0x2,
        YShortVector = 0x4,
        RepeatFlat = 0x8,
        XSameOrPositiveShortVector = 0x10,
        YSameOrPositiveShortVector = 0x20,
        OverlapSimple = 0x40,
        Reserved = 0x80
    }

    public class SimpleGlyph
    {
        public ushort[] EndPtsOfContours { get; private set; }
        public ushort InstructionLength { get; private set; }
        public byte[] Instructions { get; private set; }
        public Flags[] FlagsList { get; private set; }
        public short[] XCoordinates { get; private set; }
        public short[] YCoordinates { get; private set; }

        public bool[] OnOff { get; private set; }        // Calculated

        public static SimpleGlyph Read(BinaryReader reader, int numberOfContours)
        {
            var instance = new SimpleGlyph
            {
                EndPtsOfContours = reader.ReadUInt16BigEndianArray(numberOfContours),
                InstructionLength = reader.ReadUInt16BigEndian()
            };
            instance.Instructions = reader.ReadBytes(instance.InstructionLength);

            int pointCount = 0;
            if (numberOfContours > 0) pointCount = instance.EndPtsOfContours[numberOfContours - 1] + 1;

            instance.FlagsList = ReadFlags(reader, pointCount);
            instance.XCoordinates = ReadCoordinates(reader, pointCount, instance.FlagsList, Flags.XShortVector, Flags.XSameOrPositiveShortVector);
            instance.YCoordinates = ReadCoordinates(reader, pointCount, instance.FlagsList, Flags.YShortVector, Flags.YSameOrPositiveShortVector);

            instance.OnOff = new bool[instance.FlagsList.Length];
            for (int i = instance.FlagsList.Length - 1; i >= 0; --i)
            {
                instance.OnOff[i] = instance.FlagsList[i].HasFlag(Flags.OnCurvePoint);
            }

            return instance;
        }

        private static Flags[] ReadFlags(BinaryReader reader, int flagCount)
        {
            var result = new Flags[flagCount];
            int c = 0;
            int repeatCount = 0;
            Flags flag = default;
            while (c < flagCount)
            {
                if (repeatCount > 0)
                {
                    repeatCount--;
                }
                else
                {
                    flag = (Flags)reader.ReadByte();
                    if (flag.HasFlag(Flags.RepeatFlat))
                    {
                        repeatCount = reader.ReadByte();
                    }
                }
                result[c++] = flag;
            }
            return result;
        }

        private static short[] ReadCoordinates(BinaryReader reader, int pointCount, Flags[] flags, Flags isByte, Flags signOrSame)
        {
            var coord = new short[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                short delta;
                if (flags[i].HasFlag(isByte))
                {
                    var b = reader.ReadByte();
                    delta = flags[i].HasFlag(signOrSame) ? b : (short)-b;
                }
                else if (flags[i].HasFlag(signOrSame))
                {
                    delta = 0;
                }
                else
                {
                    delta = reader.ReadInt16BigEndian();
                }
                coord[i] = delta;
            }
            return coord;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            var endPtsOfContoursValue = string.Join(", ", EndPtsOfContours.Select(x => x.ToString()));
            sb.AppendTitleValueLine("EndPtsOfContours[]", $"[{endPtsOfContoursValue}]");
            sb.AppendTitleValueLine("InstructionLength", InstructionLength);
            var instructionsValue = string.Join(", ", Instructions.Select(x => x.ToString()));
            sb.AppendTitleValueLine("Instructions[]", Instructions.CountString());
            sb.AppendTitleValueLine("Flags[]", FlagsList.CountString());
            sb.AppendLine();
            if (XCoordinates.Length > 0)
            {
                sb.AppendTableShortRowLine("Index", "X", "Y", "XAbs", "YAbs", "On/Off");
                var xAbs = XCoordinates[0];
                var yAbs = YCoordinates[0];

                for (int i = 0; i < XCoordinates.Length; i++)
                {
                    var x = XCoordinates[i];
                    var y = YCoordinates[i];

                    if (i > 0)
                    {
                        xAbs += x;
                        yAbs += y;
                    }

                    sb.AppendTableShortRowLine(
                        i.ToString(),
                        x.ToString(),
                        y.ToString(),
                        xAbs.ToString(),
                        yAbs.ToString(),
                        OnOff[i] ? "On" : "Off");
                }
            }
            return sb.ToString();
        }
    }
}
