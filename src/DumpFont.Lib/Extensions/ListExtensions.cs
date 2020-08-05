using System.Collections.Generic;
using System.Linq;

namespace DumpFont.Extensions
{
    public static class ListExtensions
    {
        public static string CountString<T>(this List<T> list)
        {
            return $"({list.Count()} records)";
        }
    }
}
