using System;
using System.Text.RegularExpressions;

namespace ESOA.Common
{
    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static string EditFilterTermString(this string filterTerm)
        {
            if (string.IsNullOrEmpty(filterTerm)) return null;
            string result = Regex.Replace(filterTerm, @"[\[%_]", new MatchEvaluator(match => $"[{match.Value}]"));
            return $"%{result}%";
        }
    }
}
