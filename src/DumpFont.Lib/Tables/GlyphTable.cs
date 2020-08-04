using DumpFont.Extensions;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class GlyphTable
    {
        public const string Tag = "glyf";
        public uint TableOffset { get; private set; }
        private BinaryReader _reader;

        public GlyphTable(BinaryReader reader, uint tableOffset)
        {
            _reader = reader;
            TableOffset = tableOffset;
        }

        public GlyphHeader ReadHeader(uint blockLocation)
        {
            _reader.BaseStream.Position = TableOffset + blockLocation;
            return GlyphHeader.Read(_reader);
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
            sb.AppendTitleValue("NumberOfContours", NumberOfContours);
            sb.AppendTitleValue("XMin", XMin);
            sb.AppendTitleValue("YMin", YMin);
            sb.AppendTitleValue("XMax", XMax);
            sb.AppendTitleValue("YMax", YMax);
            return sb.ToString();
        }
    }
}
