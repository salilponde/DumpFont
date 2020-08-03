using System.Linq;
using System.Text;

namespace DumpFont.Constants
{
    public static class FontFileSignature   // Or sfntversion
    {
        public static byte[] TrueType = new byte[] { 0x00, 0x01, 0x00, 0x00 };
        public static byte[] TrueTypeApple = Encoding.ASCII.GetBytes("true");
        public static byte[] TrueTypeType1 = Encoding.ASCII.GetBytes("typ1");
        public static byte[] Cff = Encoding.ASCII.GetBytes("OTTO");
        public static byte[] TrueTypeFontCollection = Encoding.ASCII.GetBytes("ttcf");
        public static byte[] Woff = Encoding.ASCII.GetBytes("wOFF");
        public static byte[] Woff2 = Encoding.ASCII.GetBytes("wOF2");

        public static bool IsTrueType(byte[] value)
        {
            return TrueType.SequenceEqual(value)
                    || TrueTypeApple.SequenceEqual(value)
                    || TrueTypeType1.SequenceEqual(value);
        }

        public static bool IsCff(byte[] value)
        {
            return Cff.SequenceEqual(value);
        }

        public static bool IsTrueTypeFontCollection(byte[] value)
        {
            return TrueTypeFontCollection.SequenceEqual(value);
        }

        public static bool IsWoff(byte[] value)
        {
            return Woff.SequenceEqual(value);
        }

        public static bool IsWoff2(byte[] value)
        {
            return Woff2.SequenceEqual(value);
        }
    }
}
