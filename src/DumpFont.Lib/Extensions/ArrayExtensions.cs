using System;

namespace DumpFont.Extensions
{
    public static class ArrayExtensions
    {
        public static string CountString(this Array array)
        {
            return $"({array.Length} records)";
        }
    }
}
