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
            sb.AppendTitleValue("Version", Math.Round(Version, 2));
            sb.AppendTitleValue("ItalicAngle", Math.Round(ItalicAngle, 2));
            sb.AppendTitleValue("UnderlinePosition", UnderlinePosition);
            sb.AppendTitleValue("UnderlineThickness", UnderlineThickness);
            sb.AppendTitleValue("IsFixedPitch", IsFixedPitch);
            sb.AppendTitleValue("MinMemType42", MinMemType42);
            sb.AppendTitleValue("MaxMemType42", MaxMemType42);
            sb.AppendTitleValue("MinMemType1", MinMemType1);
            sb.AppendTitleValue("MaxMemType1", MaxMemType1);
            return sb.ToString();
        }
    }
}
