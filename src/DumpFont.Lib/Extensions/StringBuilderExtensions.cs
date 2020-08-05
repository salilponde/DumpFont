using System.Text;

namespace DumpFont.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendTitleValueLine(this StringBuilder builder, string title, object value)
        {
            builder.Append(string.Format("{0,-24}", $"{title}"));
            builder.AppendLine(value.ToString());
        }

        public static void AppendTableCell(this StringBuilder builder, object value)
        {
            builder.Append(string.Format("{0,-24}", $"{value}"));
        }

        public static void AppendTableRow(this StringBuilder builder, params string[] values)
        {
            foreach (var value in values)
            {
                builder.AppendTableCell(value);
            }
        }

        public static void AppendTableRowLine(this StringBuilder builder, params string[] values)
        {
            foreach (var value in values)
            {
                builder.Append(string.Format("{0,-24}", value));
            }
            builder.AppendLine();
        }

        public static void AppendTableShortRowLine(this StringBuilder builder, params string[] values)
        {
            foreach (var value in values)
            {
                builder.Append(string.Format("{0,-12}", value));
            }
            builder.AppendLine();
        }
    }
}
