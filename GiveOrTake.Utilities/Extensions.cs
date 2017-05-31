using System.Text.RegularExpressions;

namespace GiveOrTake.Utilities
{
    public static class Extensions
    {
        public static string Capitalize(this string value)
        {
            if (value == string.Empty) return string.Empty;
            return Regex.Replace(value, @"^[a-zA-Z]", m => m.Value.ToUpper());
        }
    }
}
