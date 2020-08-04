using DumpFont.Extensions;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public abstract class IndexLocationTableBase
    {
        public const string Tag = "loca";
        public abstract uint GetLocation(int glyphIndex);
        public abstract string Dump();
    }

    public class IndexLocationShortFormatTable : IndexLocationTableBase
    {
        public ushort[] Offsets { get; private set; }

        public static IndexLocationShortFormatTable Read(BinaryReader reader, int numGlyphs)
        {
            var instance = new IndexLocationShortFormatTable
            {
                Offsets = new ushort[numGlyphs + 1]
            };
            for (int i = 0; i < numGlyphs + 1; i++)
            {
                instance.Offsets[i] = reader.ReadUInt16BigEndian();
            }
            return instance;
        }

        public override uint GetLocation(int glyphIndex)
        {
            // In short format the offset divided by 2 is stored. Hence, multiplying by 2.
            return Offsets[glyphIndex] * (uint) 2;     
        }

        public override string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("Offsets[]", $"({Offsets.Length} records)");
            for (int i = 0; i < Offsets.Length; i++)
            {
                sb.AppendTitleValue(i.ToString(), Offsets[i]);
            }
            return sb.ToString();
        }
    }

    public class IndexLocationLongFormatTable : IndexLocationTableBase
    {
        public uint[] Offsets { get; private set; }

        public static IndexLocationLongFormatTable Read(BinaryReader reader, int numGlyphs)
        {
            var instance = new IndexLocationLongFormatTable
            {
                Offsets = new uint[numGlyphs + 1]
            };
            for (int i = 0; i < numGlyphs + 1; i++)
            {
                instance.Offsets[i] = reader.ReadUInt32BigEndian();
            }
            return instance;
        }

        public override uint GetLocation(int glyphIndex)
        {
            return Offsets[glyphIndex];
        }

        public override string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValue("Offsets[]", $"({Offsets.Length} records)");
            for(int i = 0; i < Offsets.Length; i++)
            {
                sb.AppendTitleValue(i.ToString(), Offsets[i]);
            }
            return sb.ToString();
        }
    }
}
