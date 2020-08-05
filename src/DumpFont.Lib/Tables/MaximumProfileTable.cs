using DumpFont.Extensions;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public abstract class MaximumProfileTableBase
    {
        public const string Tag = "maxp";
        public decimal Version { get; protected set; }
        public ushort NumGlyphs { get; protected set; }

        public abstract string Dump();
    }

    public class MaximumProfileTableV0_5 : MaximumProfileTableBase
    {
        public static MaximumProfileTableV0_5 Read(BinaryReader reader, decimal version)
        {
            var instance = new MaximumProfileTableV0_5
            {
                Version = version,
                NumGlyphs = reader.ReadUInt16BigEndian()
            };
            return instance;
        }

        public override string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValueLine("Version", Version);
            sb.AppendTitleValueLine("NumGlyphs", NumGlyphs);
            return sb.ToString();
        }
    }
    public class MaximumProfileTableV1 : MaximumProfileTableBase
    {
        public ushort MaxPoints { get; private set; }
        public ushort MaxContours { get; private set; }
        public ushort MaxCompositePoints { get; private set; }
        public ushort MaxCompositeContours { get; private set; }
        public ushort MaxZones { get; private set; }
        public ushort MaxTwilightPoints { get; private set; }
        public ushort MaxStorage { get; private set; }
        public ushort MaxFunctionDefs { get; private set; }
        public ushort MaxInstructionDefs { get; private set; }
        public ushort MaxStackElements { get; private set; }
        public ushort MaxSizeOfInstructions { get; private set; }
        public ushort MaxComponentElements { get; private set; }
        public ushort MaxComponentDepth { get; private set; }

        public static MaximumProfileTableV1 Read(BinaryReader reader, decimal version)
        {
            var instance = new MaximumProfileTableV1
            {
                Version = version,
                NumGlyphs = reader.ReadUInt16BigEndian(),
                MaxPoints = reader.ReadUInt16BigEndian(),
                MaxContours = reader.ReadUInt16BigEndian(),
                MaxCompositePoints = reader.ReadUInt16BigEndian(),
                MaxCompositeContours = reader.ReadUInt16BigEndian(),
                MaxZones = reader.ReadUInt16BigEndian(),
                MaxTwilightPoints = reader.ReadUInt16BigEndian(),
                MaxStorage = reader.ReadUInt16BigEndian(),
                MaxFunctionDefs = reader.ReadUInt16BigEndian(),
                MaxInstructionDefs = reader.ReadUInt16BigEndian(),
                MaxStackElements = reader.ReadUInt16BigEndian(),
                MaxSizeOfInstructions = reader.ReadUInt16BigEndian(),
                MaxComponentElements = reader.ReadUInt16BigEndian(),
                MaxComponentDepth = reader.ReadUInt16BigEndian()
            };
            return instance;
        }

        public override string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValueLine("Version", Version);
            sb.AppendTitleValueLine("NumGlyphs", NumGlyphs);
            sb.AppendTitleValueLine("MaxPoints", MaxPoints);
            sb.AppendTitleValueLine("MaxContours", MaxContours);
            sb.AppendTitleValueLine("MaxCompositePoints", MaxCompositePoints);
            sb.AppendTitleValueLine("MaxCompositeContours", MaxCompositeContours);
            sb.AppendTitleValueLine("MaxZones", MaxZones);
            sb.AppendTitleValueLine("MaxTwilightPoints", MaxTwilightPoints);
            sb.AppendTitleValueLine("MaxStorage", MaxStorage);
            sb.AppendTitleValueLine("MaxFunctionDefs", MaxFunctionDefs);
            sb.AppendTitleValueLine("MaxInstructionDefs", MaxInstructionDefs);
            sb.AppendTitleValueLine("MaxStackElements", MaxStackElements);
            sb.AppendTitleValueLine("MaxSizeOfInstructions", MaxSizeOfInstructions);
            sb.AppendTitleValueLine("MaxComponentElements", MaxComponentElements);
            sb.AppendTitleValueLine("MaxComponentDepth", MaxComponentDepth);
            return sb.ToString();
        }
    }
}
