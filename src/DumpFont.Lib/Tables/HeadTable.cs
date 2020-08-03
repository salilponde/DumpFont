using DumpFont.Extensions;
using System;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class HeadTable
    {
        public const string Tag = "head"; 
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public decimal FontRevision { get; private set; }          // Fixed Point Number
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Modified { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }
        public ushort MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public short IndexToLocFormat { get; private set; }
        public short GlyphDataFormat { get; private set; }

        public static HeadTable Read(BinaryReader reader)
        {
            var instance = new HeadTable
            {
                MajorVersion = reader.ReadUInt16BigEndian(),
                MinorVersion = reader.ReadUInt16BigEndian(),
                FontRevision = reader.ReadFixedPointNumber(),
                CheckSumAdjustment = reader.ReadUInt32BigEndian(),
                MagicNumber = reader.ReadUInt32BigEndian(),
                Flags = reader.ReadUInt16BigEndian(),
                UnitsPerEm = reader.ReadUInt16BigEndian(),
                Created = reader.ReadLongDateTime(),
                Modified = reader.ReadLongDateTime(),
                XMin = reader.ReadInt16BigEndian(),
                YMin = reader.ReadInt16BigEndian(),
                XMax = reader.ReadInt16BigEndian(),
                YMax = reader.ReadInt16BigEndian(),
                MacStyle = reader.ReadUInt16BigEndian(),
                LowestRecPPEM = reader.ReadUInt16BigEndian(),
                FontDirectionHint = reader.ReadInt16BigEndian(),
                IndexToLocFormat = reader.ReadInt16BigEndian(),
                GlyphDataFormat = reader.ReadInt16BigEndian()
            };
            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("MajorVersion", MajorVersion);
            sb.AppendTitleValue("MinorVersion", MinorVersion);
            sb.AppendTitleValue("FontRevision", Math.Round(FontRevision, 2));
            sb.AppendTitleValue("CheckSumAdjustment", CheckSumAdjustment);
            sb.AppendTitleValue("MagicNumber", MagicNumber);
            sb.AppendTitleValue("Flags", Convert.ToString(Flags, 2).PadLeft(16, '0'));
            sb.AppendTitleValue("UnitsPerEm", UnitsPerEm);
            sb.AppendTitleValue("Created", Created);
            sb.AppendTitleValue("Modified", Modified);
            sb.AppendTitleValue("XMin", XMin);
            sb.AppendTitleValue("YMin", YMin);
            sb.AppendTitleValue("XMax", XMax);
            sb.AppendTitleValue("YMax", YMax);
            sb.AppendTitleValue("MacStyle", MacStyle);
            sb.AppendTitleValue("LowestRecPPEM", LowestRecPPEM);
            sb.AppendTitleValue("FontDirectionHint", FontDirectionHint);
            sb.AppendTitleValue("IndexToLocFormat", IndexToLocFormat);
            sb.AppendTitleValue("GlyphDataFormat", GlyphDataFormat);
            return sb.ToString();
        }
    }
}
