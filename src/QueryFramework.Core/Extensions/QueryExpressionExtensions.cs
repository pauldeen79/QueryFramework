using System;
using System.Linq;
using System.Text.RegularExpressions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionExtensions
    {
        //functionname(fieldname)
        private static readonly Regex s_single_word_function_regex = new Regex(@"^[a-zA-Z]*\([a-zA-Z]*\)$", RegexOptions.Compiled);

        /// <summary>Creates a new instance from the current instance, with the specified values. (or the same value, if it's not specified)</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="expression">The expression.</param>
        public static IQueryExpression With(this IQueryExpression instance,
                                            string fieldName = "",
                                            string? expression = null)
            => new QueryExpression(fieldName == string.Empty
                                        ? instance.FieldName
                                        : fieldName,
                                   expression ?? GetRawExpression(instance));

        public static string? GetRawExpression(this IQueryExpression instance)
            => instance.Expression == instance.FieldName
                ? null
                : instance.Expression;

        public static bool IsSingleWordFunction(this IQueryExpression instance)
            => instance.GetRawExpression() == null
            || (s_single_word_function_regex.IsMatch(instance.GetRawExpression()) && !_dangerousWords.Any(x => (instance.GetRawExpression() ?? string.Empty).IndexOf(x, StringComparison.OrdinalIgnoreCase) > -1));

        public static bool IsSafeFunction(this IQueryExpression instance)
        {
            var expression = instance is IExpressionContainer c
                ? c.SourceExpression ?? string.Empty
                : instance.Expression;

            return expression.Replace("{0}", string.Empty).All(IsValid) && !_dangerousWords.Any(x => expression.IndexOf(x, StringComparison.OrdinalIgnoreCase) > -1);
        }

        public static IQueryExpressionBuilder ToBuilder(this IQueryExpression instance)
            => instance is ICustomQueryExpression customQueryExpression
                ? customQueryExpression.CreateBuilder()
                : new QueryExpressionBuilder(instance);

        private static bool IsValid(char arg)
            => IsInRange(arg, 'a', 'z')
                || IsInRange(arg, 'A', 'Z')
                || IsInRange(arg, '0', '9')
                || _validCharacters.Contains(arg);

        private static bool IsInRange(char arg, char lower, char higher)
            => arg >= lower && arg <= higher;

        private static char[] _validCharacters = new[]
        {
            ' ',
            '(',
            ')',
            '[',
            ']',
            '<',
            '>',
            ',',
            '-',
            '+',
            '*',
            '/'
        };

        private static string[] _dangerousWords = new[]
        {
            "DROP ",
            "ALTER ",
            "CREATE ",
            "INSERT ",
            "UPDATE ",
            "MERGE ",
            "DELETE ",
            "TRUNCATE ",
            "ENABLE ",
            "DISABLE ",
            "BACKUP ",
            "RESTORE ",
            "CLOSE ",
            "DENY ",
            "GRANT ",
            "REVOKE ",
            "SETUSER ",
            "EXECUTE ",
            "DBCC ",
            "SP_EXEC ",
            "RENAME ",
            "SET "
        };
    }
}
