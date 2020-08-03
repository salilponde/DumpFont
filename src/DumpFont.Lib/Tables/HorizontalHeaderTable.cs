using DumpFont.Extensions;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class HorizontalHeaderTable
    {
        public const string Tag = "hhea";
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public short Ascender { get; private set; }
        public short Descender { get; private set; }
        public short LineGap { get; private set; }
        public ushort AdvanceWidthMax { get; private set; }
        public short MinLeftSideBearing { get; private set; }
        public short MinRightSideBearing { get; private set; }
        public short XMaxExtent { get; private set; }
        public short CaretSlopeRise { get; private set; }
        public short CaretSlopeRun { get; private set; }
        public short CaretOffset { get; private set; }
        public byte[] Reserved { get; private set; }
        public short MetricDataFormat { get; private set; }
        public ushort NumberOfHMetrics { get; private set; }

        public static HorizontalHeaderTable Read(BinaryReader reader)
        {
            var instance = new HorizontalHeaderTable
            {
                MajorVersion = reader.ReadUInt16BigEndian(),
                MinorVersion = reader.ReadUInt16BigEndian(),
                Ascender = reader.ReadInt16BigEndian(),
                Descender = reader.ReadInt16BigEndian(),
                LineGap = reader.ReadInt16BigEndian(),
                AdvanceWidthMax = reader.ReadUInt16BigEndian(),
                MinLeftSideBearing = reader.ReadInt16BigEndian(),
                MinRightSideBearing = reader.ReadInt16BigEndian(),
                XMaxExtent = reader.ReadInt16BigEndian(),
                CaretSlopeRise = reader.ReadInt16BigEndian(),
                CaretSlopeRun = reader.ReadInt16BigEndian(),
                CaretOffset = reader.ReadInt16BigEndian(),
                Reserved = reader.ReadBytes(8),
                MetricDataFormat = reader.ReadInt16BigEndian(),
                NumberOfHMetrics = reader.ReadUInt16BigEndian()
            };
            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("MajorVersion", MajorVersion);
            sb.AppendTitleValue("MinorVersion", MinorVersion);
            sb.AppendTitleValue("Ascender", Ascender);
            sb.AppendTitleValue("Descender", Descender);
            sb.AppendTitleValue("LineGap", LineGap);
            sb.AppendTitleValue("AdvanceWidthMax", AdvanceWidthMax);
            sb.AppendTitleValue("MinLeftSideBearing", MinLeftSideBearing);
            sb.AppendTitleValue("MinRightSideBearing", MinRightSideBearing);
            sb.AppendTitleValue("ModifXMaxExtentied", XMaxExtent);
            sb.AppendTitleValue("CaretSlopeRise", CaretSlopeRise);
            sb.AppendTitleValue("CaretSlopeRun", CaretSlopeRun);
            sb.AppendTitleValue("CaretOffset", CaretOffset);
            sb.AppendTitleValue("MetricDataFormat", MetricDataFormat);
            sb.AppendTitleValue("NumberOfHMetrics", NumberOfHMetrics);
            return sb.ToString();
        }
    }
}
