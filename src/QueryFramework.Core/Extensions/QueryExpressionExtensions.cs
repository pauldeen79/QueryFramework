using System.Text.RegularExpressions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionExtensions
    {
        private static readonly Regex s_single_word_function_regex = new Regex(@"^[a-zA-Z]*\([a-zA-Z]*\)$", RegexOptions.Compiled); //functionname(fieldname)

        /// <summary>Creates a new instance from the current instance, with the specified values. (or the same value, if it's not specified)</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="expression">The expression.</param>
        public static IQueryExpression With(this IQueryExpression instance, string fieldName = null, string expression = null)
            => new QueryExpression(fieldName ?? instance.FieldName, expression ?? GetRawExpression(instance));

        public static string GetRawExpression(this IQueryExpression instance)
            => instance.Expression == instance.FieldName
                ? null
                : instance.Expression;

        public static bool IsSingleWordFunction(this IQueryExpression instance)
            => s_single_word_function_regex.IsMatch(instance.GetRawExpression());

        public static IQueryExpressionBuilder ToBuilder(this IQueryExpression instance)
        {
            if (instance is ICustomQueryExpression customQueryExpression)
            {
                return customQueryExpression.CreateBuilder();
            }

            return new QueryExpressionBuilder(instance);
        }
    }
}
