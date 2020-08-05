using DumpFont.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class Os2Table
    {
        public const string Tag = "OS/2";
        public ushort Version { get; private set; }
        public short XAvgCharWidth { get; private set; }
        public ushort UsWeightClass { get; private set; }
        public ushort UsWidthClass { get; private set; }
        public ushort FsType { get; private set; }
        public short YSubscriptXSize { get; private set; }
        public short YSubscriptYSize { get; private set; }
        public short YSubscriptXOffset { get; private set; }
        public short YSubscriptYOffset { get; private set; }
        public short YSuperscriptXSize { get; private set; }
        public short YSuperscriptYSize { get; private set; }
        public short YSuperscriptXOffset { get; private set; }
        public short YSuperscriptYOffset { get; private set; }
        public short YStrikeoutSize { get; private set; }
        public short YStrikeoutPosition { get; private set; }
        public short SFamilyClass { get; private set; }
        public byte[] Panose { get; private set; }          // Size 10
        public uint UlUnicodeRange1 { get; private set; }
        public uint UlUnicodeRange2 { get; private set; }
        public uint UlUnicodeRange3 { get; private set; }
        public uint UlUnicodeRange4 { get; private set; }
        public string AchVendID { get; private set; }       // Size 4
        public ushort FsSelection { get; private set; }
        public ushort UsFirstCharIndex { get; private set; }
        public ushort UsLastCharIndex { get; private set; }
        public short STypoAscender { get; private set; }
        public short STypoDescender { get; private set; }
        public short STypoLineGap { get; private set; }
        public ushort UsWinAscent { get; private set; }
        public ushort UsWinDescent { get; private set; }
        public uint UlCodePageRange1 { get; private set; }
        public uint UlCodePageRange2 { get; private set; }
        public short SxHeight { get; private set; }
        public short SCapHeight { get; private set; }
        public ushort UsDefaultChar { get; private set; }
        public ushort UsBreakChar { get; private set; }
        public ushort UsMaxContext { get; private set; }
        public ushort UsLowerOpticalPointSize { get; private set; }
        public ushort UsUpperOpticalPointSize { get; private set; }

        public static Os2Table Read(BinaryReader reader)
        {
            var instance = new Os2Table
            {
                Version = reader.ReadUInt16BigEndian(),
                XAvgCharWidth = reader.ReadInt16BigEndian(),
                UsWeightClass = reader.ReadUInt16BigEndian(),
                UsWidthClass = reader.ReadUInt16BigEndian(),
                FsType = reader.ReadUInt16BigEndian(),
                YSubscriptXSize = reader.ReadInt16BigEndian(),
                YSubscriptYSize = reader.ReadInt16BigEndian(),
                YSubscriptXOffset = reader.ReadInt16BigEndian(),
                YSubscriptYOffset = reader.ReadInt16BigEndian(),
                YSuperscriptXSize = reader.ReadInt16BigEndian(),
                YSuperscriptYSize = reader.ReadInt16BigEndian(),
                YSuperscriptXOffset = reader.ReadInt16BigEndian(),
                YSuperscriptYOffset = reader.ReadInt16BigEndian(),
                YStrikeoutSize = reader.ReadInt16BigEndian(),
                YStrikeoutPosition = reader.ReadInt16BigEndian(),
                SFamilyClass = reader.ReadInt16BigEndian(),
                Panose = reader.ReadBytes(10),
                UlUnicodeRange1 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange2 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange3 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange4 = reader.ReadUInt32BigEndian(),
                AchVendID = reader.ReadTag(),
                FsSelection = reader.ReadUInt16BigEndian(),
                UsFirstCharIndex = reader.ReadUInt16BigEndian(),
                UsLastCharIndex = reader.ReadUInt16BigEndian(),
                STypoAscender = reader.ReadInt16BigEndian(),
                STypoDescender = reader.ReadInt16BigEndian(),
                STypoLineGap = reader.ReadInt16BigEndian(),
                UsWinAscent = reader.ReadUInt16BigEndian(),
                UsWinDescent = reader.ReadUInt16BigEndian(),
                UlCodePageRange1 = reader.ReadUInt32BigEndian(),
                UlCodePageRange2 = reader.ReadUInt32BigEndian(),
                SxHeight = reader.ReadInt16BigEndian(),
                SCapHeight = reader.ReadInt16BigEndian(),
                UsDefaultChar = reader.ReadUInt16BigEndian(),
                UsBreakChar = reader.ReadUInt16BigEndian(),
                UsMaxContext = reader.ReadUInt16BigEndian(),
                UsLowerOpticalPointSize = reader.ReadUInt16BigEndian(),
                UsUpperOpticalPointSize = reader.ReadUInt16BigEndian()
            };

            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValueLine("Version", Version);
            sb.AppendTitleValueLine("XAvgCharWidth", XAvgCharWidth);
            sb.AppendTitleValueLine("UsWeightClass", UsWeightClass);
            sb.AppendTitleValueLine("UsWidthClass", UsWidthClass);
            sb.AppendTitleValueLine("FsType", FsType);
            sb.AppendTitleValueLine("YSubscriptXSize", YSubscriptXSize);
            sb.AppendTitleValueLine("YSubscriptYSize", YSubscriptYSize);
            sb.AppendTitleValueLine("YSubscriptXOffset", YSubscriptXOffset);
            sb.AppendTitleValueLine("YSubscriptYOffset", YSubscriptYOffset);
            sb.AppendTitleValueLine("YSuperscriptXSize", YSuperscriptXSize);
            sb.AppendTitleValueLine("YSuperscriptYSize", YSuperscriptYSize);
            sb.AppendTitleValueLine("YSuperscriptXOffset", YSuperscriptXOffset);
            sb.AppendTitleValueLine("YSuperscriptYOffset", YSuperscriptYOffset);
            sb.AppendTitleValueLine("YStrikeoutSize", YStrikeoutSize);
            sb.AppendTitleValueLine("YStrikeoutPosition", YStrikeoutPosition);
            sb.AppendTitleValueLine("SFamilyClass", SFamilyClass);
            var panose = new List<string>();
            foreach (var b in Panose)
            {
                panose.Add(b.ToString());
            }
            var panoseValues = $"[{string.Join(", ", panose)}]";
            sb.AppendTitleValueLine("Panose", panoseValues);
            sb.AppendTitleValueLine("UlUnicodeRange1", UlUnicodeRange1);
            sb.AppendTitleValueLine("UlUnicodeRange2", UlUnicodeRange2);
            sb.AppendTitleValueLine("UlUnicodeRange3", UlUnicodeRange3);
            sb.AppendTitleValueLine("UlUnicodeRange4", UlUnicodeRange4);
            sb.AppendTitleValueLine("AchVendID", AchVendID);
            sb.AppendTitleValueLine("FsSelection", FsSelection);
            sb.AppendTitleValueLine("UsFirstCharIndex", UsFirstCharIndex);
            sb.AppendTitleValueLine("UsLastCharIndex", UsLastCharIndex);
            sb.AppendTitleValueLine("STypoAscender", STypoAscender);
            sb.AppendTitleValueLine("STypoDescender", STypoDescender);
            sb.AppendTitleValueLine("STypoLineGap", STypoLineGap);
            sb.AppendTitleValueLine("UsWinAscent", UsWinAscent);
            sb.AppendTitleValueLine("UsWinDescent", UsWinDescent);
            sb.AppendTitleValueLine("UlCodePageRange1", UlCodePageRange1);
            sb.AppendTitleValueLine("UlCodePageRange2", UlCodePageRange2);
            sb.AppendTitleValueLine("SxHeight", SxHeight);
            sb.AppendTitleValueLine("SCapHeight", SCapHeight);
            sb.AppendTitleValueLine("UsDefaultChar", UsDefaultChar);
            sb.AppendTitleValueLine("UsBreakChar", UsBreakChar);
            sb.AppendTitleValueLine("UsMaxContext", UsMaxContext);
            sb.AppendTitleValueLine("UsLowerOpticalPointSize", UsLowerOpticalPointSize);
            sb.AppendTitleValueLine("UsUpperOpticalPointSize", UsUpperOpticalPointSize);
            return sb.ToString();
        }
    }
}
