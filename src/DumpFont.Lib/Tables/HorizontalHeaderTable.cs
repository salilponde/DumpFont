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
            sb.AppendTitleValueLine("MajorVersion", MajorVersion);
            sb.AppendTitleValueLine("MinorVersion", MinorVersion);
            sb.AppendTitleValueLine("Ascender", Ascender);
            sb.AppendTitleValueLine("Descender", Descender);
            sb.AppendTitleValueLine("LineGap", LineGap);
            sb.AppendTitleValueLine("AdvanceWidthMax", AdvanceWidthMax);
            sb.AppendTitleValueLine("MinLeftSideBearing", MinLeftSideBearing);
            sb.AppendTitleValueLine("MinRightSideBearing", MinRightSideBearing);
            sb.AppendTitleValueLine("ModifXMaxExtentied", XMaxExtent);
            sb.AppendTitleValueLine("CaretSlopeRise", CaretSlopeRise);
            sb.AppendTitleValueLine("CaretSlopeRun", CaretSlopeRun);
            sb.AppendTitleValueLine("CaretOffset", CaretOffset);
            sb.AppendTitleValueLine("MetricDataFormat", MetricDataFormat);
            sb.AppendTitleValueLine("NumberOfHMetrics", NumberOfHMetrics);
            return sb.ToString();
        }
    }
}
