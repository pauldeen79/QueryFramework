using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryExpressionExtensions
    {
        #region Built-in Sql functions
        /// <summary>Gets the length of this expression.</summary>
        public static IQueryExpression Len(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "LEN({0})" : "LEN(" + instance.Expression + ")");

        /// <summary>Trims the value of this expression.</summary>
        public static IQueryExpression Trim(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "TRIM({0})" : "TRIM(" + instance.Expression + ")");

        /// <summary>Gets the upper-cased value of this expression.</summary>
        public static IQueryExpression Upper(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "UPPER({0})" : "UPPER(" + instance.Expression + ")");

        /// <summary>Gets the lower-cased value of this expression.</summary>
        public static IQueryExpression Lower(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "LOWER({0})" : "LOWER(" + instance.Expression + ")");

        /// <summary>Gets the left part of this expression.</summary>
        public static IQueryExpression Left(this IQueryExpression instance, int length)
            => instance.With(expression: instance.FieldName == instance.Expression ? "LEFT({0}, " + length + ")" : "LEFT(" + instance.Expression + ", " + length + ")");

        /// <summary>Gets the right part of this expression.</summary>
        public static IQueryExpression Right(this IQueryExpression instance, int length)
            => instance.With(expression: instance.FieldName == instance.Expression ? "RIGHT({0}, " + length + ")" : "RIGHT(" + instance.Expression + ", " + length + ")");

        /// <summary>Gets the year of this date expression.</summary>
        public static IQueryExpression Year(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "YEAR({0})" : "YEAR(" + instance.Expression + ")");

        /// <summary>Gets the month of this date expression.</summary>
        public static IQueryExpression Month(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "MONTH({0})" : "MONTH(" + instance.Expression + ")");

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpression Day(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "DAY({0})" : "DAY(" + instance.Expression + ")");

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpression Coalesce(this IQueryExpression instance, params IQueryExpression[] innerExpressions)
            => instance.With(expression: instance.FieldName == instance.Expression
                ? "COALESCE({0}, " + string.Join(", ", innerExpressions.Select(innerExpression => innerExpression.Expression)) + ")"
                : "COALESCE(" + instance.Expression + ", " + string.Join(", ", innerExpressions.Select(innerExpression => innerExpression.Expression)) + ")");

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpression Coalesce(this IQueryExpression instance, params string[] innerFieldNames)
            => Coalesce(instance, innerFieldNames.Select(innerFieldName => new QueryExpression(innerFieldName)).ToArray());

        /// <summary>Gets the count of this expression.</summary>
        public static IQueryExpression Count(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "COUNT({0})" : "COUNT(" + instance.Expression + ")");

        /// <summary>Gets the sum of this expression.</summary>
        public static IQueryExpression Sum(this IQueryExpression instance)
            => instance.With(expression: instance.FieldName == instance.Expression ? "SUM({0})" : "SUM(" + instance.Expression + ")");
        #endregion
    }
}
