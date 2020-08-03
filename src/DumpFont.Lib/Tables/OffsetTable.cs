using DumpFont.Constants;
using DumpFont.Exceptions;
using DumpFont.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class OffsetTable
    {
        public byte[] SfntVersion { get; private set; }
        public ushort NumTables { get; private set; }
        public ushort SearchRange { get; private set; }
        public ushort EntrySelector { get; private set; }
        public ushort RangeShift { get; private set; }
        public List<TableRecord> TableRecords { get; private set; }

        public static OffsetTable Read(BinaryReader reader)
        {
            var instance = new OffsetTable
            {
                SfntVersion = reader.ReadBytes(4)
            };

            var isTrueType = FontFileSignature.IsTrueType(instance.SfntVersion);
            if (!isTrueType) throw new FontReaderException("Only TrueType fonts are supported currently");

            instance.NumTables = reader.ReadUInt16BigEndian();
            instance.SearchRange = reader.ReadUInt16BigEndian();
            instance.EntrySelector = reader.ReadUInt16BigEndian();
            instance.RangeShift = reader.ReadUInt16BigEndian();

            instance.TableRecords = new List<TableRecord>();
            for (int i = 0; i < instance.NumTables; i++)
            {
                instance.TableRecords.Add(TableRecord.Read(reader));
            }

            return instance;
        }
    }

    public class TableRecord
    {
        public string TableTag { get; private set; }
        public uint Checksum { get; private set; }
        public uint Offset { get; private set; }
        public uint Length { get; private set; }

        public static TableRecord Read(BinaryReader reader)
        {
            var instance = new TableRecord
            {
                TableTag = Encoding.ASCII.GetString(reader.ReadBytes(4)),
                Checksum = reader.ReadUInt32BigEndian(),
                Offset = reader.ReadUInt32BigEndian(),
                Length = reader.ReadUInt32BigEndian()
            };
            return instance;
        }
    }
}
