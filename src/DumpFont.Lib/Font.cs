using DumpFont.Extensions;
using DumpFont.Tables;
using System;
using System.IO;
using System.Linq;

namespace DumpFont
{
    public class Font
    {
        public OffsetTable OffsetTable { get; internal set; }
        public HeadTable HeadTable { get; internal set; }
        public MaximumProfileTableBase MaximumProfileTable { get; internal set; }
        public HorizontalHeaderTable HorizontalHeaderTable { get; internal set; }
        public HorizontalMetricsTable HorizontalMetricsTable { get; internal set; }
        public PostScriptTable PostScriptTable { get; internal set; }
        public Os2Table Os2Table { get; internal set; }
        public CharacterMapTable CharacterMapTable { get; internal set; }

        public static Font ReadFromFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException($"File {filename} not found");

            var fileStream = File.OpenRead(filename);
            return ReadFromStream(fileStream);
        }

        public static Font ReadFromStream(Stream stream)
        {
            var font = new Font();
            var reader = new BinaryReader(stream);

            // Offset Table
            font.OffsetTable = OffsetTable.Read(reader);

            // Head
            var headTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == HeadTable.Tag);
            stream.Seek(headTableRecord.Offset, SeekOrigin.Begin);
            font.HeadTable = HeadTable.Read(reader);

            // Horizontal Header
            var horizontalHeaderTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == HorizontalHeaderTable.Tag);
            stream.Seek(horizontalHeaderTableRecord.Offset, SeekOrigin.Begin);
            font.HorizontalHeaderTable = HorizontalHeaderTable.Read(reader);

            // Maximum Profile
            var maximumProfileTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == MaximumProfileTableBase.Tag);
            stream.Seek(maximumProfileTableRecord.Offset, SeekOrigin.Begin);
            var maximumProfileTableVersion = Math.Round(reader.ReadFixedPointNumber(), 2);
            if (maximumProfileTableVersion == 0.5m)
            {
                font.MaximumProfileTable = MaximumProfileTableV0_5.Read(reader, maximumProfileTableVersion);
            }
            else if (maximumProfileTableVersion == 1.0m)
            {
                font.MaximumProfileTable = MaximumProfileTableV1.Read(reader, maximumProfileTableVersion);
            }

            // Horizontal Metrics
            var horizontalMetricsTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == HorizontalMetricsTable.Tag);
            stream.Seek(horizontalMetricsTableRecord.Offset, SeekOrigin.Begin);
            font.HorizontalMetricsTable = HorizontalMetricsTable.Read(reader,
                font.HorizontalHeaderTable.NumberOfHMetrics,
                font.MaximumProfileTable.NumGlyphs);

            // PostScript
            var postScriptTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == PostScriptTable.Tag);
            stream.Seek(postScriptTableRecord.Offset, SeekOrigin.Begin);
            font.PostScriptTable = PostScriptTable.Read(reader);

            // OS/2 and Windows
            var os2TableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == Os2Table.Tag);
            stream.Seek(os2TableRecord.Offset, SeekOrigin.Begin);
            font.Os2Table = Os2Table.Read(reader);

            // CharacterMap
            var characterMapTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == CharacterMapTable.Tag);
            stream.Seek(characterMapTableRecord.Offset, SeekOrigin.Begin);
            font.CharacterMapTable = CharacterMapTable.Read(reader);

            return font;
        }
    }
}
