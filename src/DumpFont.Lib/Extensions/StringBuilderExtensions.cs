using System.Text;

namespace DumpFont.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendTitleValue(this StringBuilder builder, string title, object value)
        {
            builder.Append(string.Format("{0,-24}", $"{title}"));
            builder.AppendLine(value.ToString());
        }

        public static void AppendTableRow(this StringBuilder builder, params string[] values)
        {
            foreach (var value in values)
            {
                builder.Append(string.Format("{0,-24}", value));
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
    }
}
