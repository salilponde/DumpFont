using DumpFont.Constants;
using DumpFont.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class CharacterMapTable
    {
        public const string Tag = "cmap";
        public ushort Version { get; private set; }
        public ushort NumTables { get; private set; }
        public List<EncodingRecord> EncodingRecords { get; private set; }

        public Dictionary<EncodingRecord, int> EncodingFormats { get; private set; }
        public Dictionary<EncodingRecord, FormatSubTableBase> Encodings { get; private set; }

        public static CharacterMapTable Read(BinaryReader reader)
        {
            var tableOffset = reader.BaseStream.Position;

            var instance = new CharacterMapTable
            {
                Version = reader.ReadUInt16BigEndian(),
                NumTables = reader.ReadUInt16BigEndian(),
                EncodingRecords = new List<EncodingRecord>()
            };
            for (int i = 0; i < instance.NumTables; i++)
            {
                instance.EncodingRecords.Add(EncodingRecord.Read(reader));
            }

            instance.EncodingFormats = new Dictionary<EncodingRecord, int>();
            instance.Encodings = new Dictionary<EncodingRecord, FormatSubTableBase>();
            for (int i = 0; i < instance.EncodingRecords.Count; i++)
            {
                var encodingRecord = instance.EncodingRecords[i];
                reader.BaseStream.Seek(tableOffset + encodingRecord.Offset, SeekOrigin.Begin);
                var format = reader.ReadUInt16BigEndian();
                instance.EncodingFormats[encodingRecord] = format;

                if (format == 4)
                {
                    instance.Encodings[encodingRecord] = Format4SubTable.Read(format, reader);
                }
                else
                {
                    //throw new FontReaderException($"Unsupported cmap Format Table format {format}");
                }
            }

            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("Version", Version);
            sb.AppendTitleValue("NumTables", NumTables);
            sb.AppendTitleValue("EncodingRecord[]", $"({EncodingRecords.Count} records)");
            sb.AppendLine();
            sb.AppendTableRowLine("PlatformID", "EncodingID", "Offset", "(Format)");
            foreach (var encodingRecord in EncodingRecords)
            {
                sb.Append(encodingRecord.Dump());
                sb.AppendTableRowLine(EncodingFormats[encodingRecord].ToString());
            }
            return sb.ToString();
        }
    }

    public class EncodingRecord
    {
        public ushort PlatformID { get; private set; }
        public ushort EncodingID { get; private set; }
        public uint Offset { get; private set; }

        public static EncodingRecord Read(BinaryReader reader)
        {
            var instance = new EncodingRecord
            {
                PlatformID = reader.ReadUInt16BigEndian(),
                EncodingID = reader.ReadUInt16BigEndian(),
                Offset = reader.ReadUInt32BigEndian()
            };
            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTableRow(
                $"{PlatformID} ({(PlatformId)PlatformID})",
                $"{EncodingID} ({(WindowsEncodingId)EncodingID})",
                Offset.ToString());
            return sb.ToString();
        }
    }

    public abstract class FormatSubTableBase
    {
        public ushort Format { get; protected set; }
        public abstract bool TryGetGlyphId(int c, out ushort glyphId);
        public abstract string Dump();
    }

    public class Format4SubTable : FormatSubTableBase
    {
        public ushort Length { get; private set; }
        public ushort Language { get; private set; }
        public ushort SegCountX2 { get; private set; }
        public ushort SearchRange { get; private set; }
        public ushort EntrySelector { get; private set; }
        public ushort RangeShift { get; private set; }
        public ushort[] EndCode { get; private set; }
        public ushort ReservedPad { get; private set; }
        public ushort[] StartCode { get; private set; }
        public short[] IdDelta { get; private set; }
        public ushort[] IdRangeOffset { get; private set; }
        public ushort[] GlyphIdArray { get; private set; }

        public static Format4SubTable Read(ushort format, BinaryReader reader)
        {
            var instance = new Format4SubTable
            {
                Format = format,
                Length = reader.ReadUInt16BigEndian(),
                Language = reader.ReadUInt16BigEndian(),
                SegCountX2 = reader.ReadUInt16BigEndian()
            };

            int segCount = instance.SegCountX2 / 2;
            instance.SearchRange = reader.ReadUInt16BigEndian();
            instance.EntrySelector = reader.ReadUInt16BigEndian();
            instance.RangeShift = reader.ReadUInt16BigEndian();
            instance.EndCode = reader.ReadUInt16BigEndianArray(segCount);
            instance.ReservedPad = reader.ReadUInt16BigEndian();
            instance.StartCode = reader.ReadUInt16BigEndianArray(segCount);
            instance.IdDelta = reader.ReadInt16BigEndianArray(segCount);
            instance.IdRangeOffset = reader.ReadUInt16BigEndianArray(segCount);

            var glyphIdArrayLength = (instance.Length - (16 + segCount * 8)) / 2;
            instance.GlyphIdArray = reader.ReadUInt16BigEndianArray(glyphIdArrayLength);

            return instance;
        }

        public override string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("Format", Format);
            sb.AppendTitleValue("Length", Length);
            sb.AppendTitleValue("Language", Language);
            sb.AppendTitleValue("SegCountX2", SegCountX2);
            sb.AppendTitleValue("SearchRange", SearchRange);
            sb.AppendTitleValue("EntrySelector", EntrySelector);
            sb.AppendTitleValue("RangeShift", RangeShift);
            sb.AppendTitleValue("EndCode[]", $"({EndCode.Length} records)");
            sb.AppendTitleValue("StartCode[]", $"({StartCode.Length} records)");
            sb.AppendTitleValue("IdDelta[]", $"({IdDelta.Length} records)");
            sb.AppendTitleValue("IdRangeOffset[]", $"({IdRangeOffset.Length} records)");
            sb.AppendTitleValue("GlyphIdArray[]", $"({GlyphIdArray.Length} records)");
            return sb.ToString();
        }

        public override bool TryGetGlyphId(int c, out ushort glyphId)
        {
            var segmentCount = SegCountX2 / 2;
            var charCode = (uint)c;

            for (int i = 0; i < segmentCount; i++)
            {
                if (EndCode[i] >= charCode && StartCode[i] <= charCode)
                {
                    if (IdRangeOffset[i] == 0)
                    {
                        glyphId = (ushort)((IdDelta[i] + charCode) & 0xFFFF);
                        return true;
                    }
                    else
                    {
                        long offset = IdRangeOffset[i] / 2 + (charCode - StartCode[i]);
                        glyphId = GlyphIdArray[offset - segmentCount + i];
                        return true;
                    }
                }
            }

            glyphId = 0;
            return false;
        }
    }
}
