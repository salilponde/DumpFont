using DumpFont;
using DumpFont.Tables;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace DumpFontConsole
{
    class Program
    {
        static void Main(params string[] args)
        {
            var rootCommand = new RootCommand();
            rootCommand.AddOption(new Option<string>(aliases: new string[] { "--file", "-f" }, description: "Font file to read"));
            rootCommand.AddOption(new Option<string>(aliases: new string[] { "--table", "-t" }, description: "Table to dump"));
            rootCommand.AddOption(new Option<int>(aliases: new string[] { "--encoding", "-e" }, description: "Encoding index in cmap table"));
            rootCommand.AddOption(new Option<uint?>(aliases: new string[] { "--glyphindex", "-g" }, description: "Get glyph index of character code"));
            rootCommand.Handler = CommandHandler.Create<string, string, int, uint?>(Execute);
            rootCommand.InvokeAsync(args).Wait();
        }

        static void Execute(string file, string table, int encoding = -1, uint? glyphindex = null)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                Console.WriteLine("Please specify filename using -f switch");
                return;
            }

            var font = Font.ReadFromFile(file);
            if (string.IsNullOrWhiteSpace(table) && encoding == -1 && glyphindex == null)
            {
                foreach (var tableRecord in font.OffsetTable.TableRecords)
                {
                    Console.WriteLine(tableRecord.TableTag);
                }
            }
            else if (encoding != -1)
            {
                if (encoding < 0)
                {
                    Console.WriteLine($"Invalid encoding index {encoding}. Indexes begin with 0");
                }
                else if (encoding > font.CharacterMapTable.EncodingRecords.Count)
                {
                    Console.WriteLine($"Invalid encoding index {encoding}. Font has {font.CharacterMapTable.EncodingRecords.Count} encoding records");
                }
                else
                {
                    var encodingRecord = font.CharacterMapTable.EncodingRecords[encoding];
                    if (!font.CharacterMapTable.Encodings.ContainsKey(encodingRecord))
                    {
                        var format = font.CharacterMapTable.EncodingFormats[encodingRecord];
                        Console.WriteLine($"Dumping encoding format {format} is currently not supported");
                    }
                    else
                    {
                        Console.WriteLine(font.CharacterMapTable.Encodings[encodingRecord].Dump());
                    }
                }
            }
            else if (glyphindex != null)
            {
                var found = font.CharacterMapTable.Encodings.Last().Value.TryGetGlyphId((int)glyphindex, out var index);
                if (!found) Console.WriteLine($"Mapping not found for character code {glyphindex}");
                else Console.WriteLine($"{index}");
            }
            else
            {
                switch (table)
                {
                    case CharacterMapTable.Tag:
                        Console.WriteLine(font.CharacterMapTable.Dump());
                        break;
                    case HeadTable.Tag:
                        Console.WriteLine(font.HeadTable.Dump());
                        break;
                    case HorizontalHeaderTable.Tag:
                        Console.WriteLine(font.HorizontalHeaderTable.Dump());
                        break;
                    case HorizontalMetricsTable.Tag:
                        Console.WriteLine(font.HorizontalMetricsTable.Dump());
                        break;
                    case MaximumProfileTableBase.Tag:
                        Console.WriteLine(font.MaximumProfileTable.Dump());
                        break;
                    case Os2Table.Tag:
                        Console.WriteLine(font.Os2Table.Dump());
                        break;
                    case PostScriptTable.Tag:
                        Console.WriteLine(font.PostScriptTable.Dump());
                        break;
                    default:
                        if (font.OffsetTable.TableRecords.SingleOrDefault(x => x.TableTag == table) != null)
                        {
                            Console.WriteLine($"Table {table} exists but currently dumping it is not supported");
                        }
                        else
                        {
                            Console.WriteLine($"Table {table} does not exist");
                        }
                        break;
                }
            }
        }
    }
}
