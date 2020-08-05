using DumpFont.Extensions;
using System;
using System.IO;
using System.Text;

namespace DumpFont.Tables
{
    public class PostScriptTable
    {
        public const string Tag = "post";
        public decimal Version { get; private set; }                // Fixed Point Number
        public decimal ItalicAngle { get; private set; }            // Fixed Point Number
        public short UnderlinePosition { get; private set; }
        public short UnderlineThickness { get; private set; }
        public uint IsFixedPitch { get; private set; }
        public uint MinMemType42 { get; private set; }
        public uint MaxMemType42 { get; private set; }
        public uint MinMemType1 { get; private set; }
        public uint MaxMemType1 { get; private set; }

        public static PostScriptTable Read(BinaryReader reader)
        {
            var instance = new PostScriptTable
            {
                Version = reader.ReadFixedPointNumber(),
                ItalicAngle = reader.ReadFixedPointNumber(),
                UnderlinePosition = reader.ReadInt16BigEndian(),
                UnderlineThickness = reader.ReadInt16BigEndian(),
                IsFixedPitch = reader.ReadUInt32BigEndian(),
                MinMemType42 = reader.ReadUInt32BigEndian(),
                MaxMemType42 = reader.ReadUInt32BigEndian(),
                MinMemType1 = reader.ReadUInt32BigEndian(),
                MaxMemType1 = reader.ReadUInt32BigEndian()
            };
            return instance;
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            sb.AppendTitleValueLine("Version", Math.Round(Version, 2));
            sb.AppendTitleValueLine("ItalicAngle", Math.Round(ItalicAngle, 2));
            sb.AppendTitleValueLine("UnderlinePosition", UnderlinePosition);
            sb.AppendTitleValueLine("UnderlineThickness", UnderlineThickness);
            sb.AppendTitleValueLine("IsFixedPitch", IsFixedPitch);
            sb.AppendTitleValueLine("MinMemType42", MinMemType42);
            sb.AppendTitleValueLine("MaxMemType42", MaxMemType42);
            sb.AppendTitleValueLine("MinMemType1", MinMemType1);
            sb.AppendTitleValueLine("MaxMemType1", MaxMemType1);
            return sb.ToString();
        }
    }
}
