using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QueryFramework.InMemory
{
    internal static class Functions
    {
        internal static readonly Dictionary<string, Func<object, IEnumerable<string>, object>> Items = new Dictionary<string, Func<object, IEnumerable<string>, object>>(StringComparer.CurrentCultureIgnoreCase)
        {
            { "LEN", (value, _) => GetStringValue(value).Length },
            { "LEFT", (value, arguments) => GetStringValue(value).Substring(0, int.TryParse(arguments.FirstOrDefault() ?? string.Empty, out int length) ? length : 0) },
            { "RIGHT", (value, arguments) =>
                {
                    var s = GetStringValue(value);
                    return s.Substring(s.Length - (int.TryParse(arguments.FirstOrDefault() ?? string.Empty, out int length) ? length : 0));
                } },
            { "UPPER", (value, _) => GetStringValue(value).ToUpper(CultureInfo.CurrentCulture) },
            { "LOWER", (value, _) => GetStringValue(value).ToLower(CultureInfo.CurrentCulture) },
            { "TRIM", (value, _) => GetStringValue(value).Trim() },
        };

        private static string GetStringValue(object value)
            => value?.ToString() ?? string.Empty;
    }
}
