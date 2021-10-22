using System;
using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryOperatorExtensions
    {
        /// <summary>
        /// Converts the QueryOperator to SQL.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentOutOfRangeException">instance</exception>
        /// <remarks>
        /// Note that only a sub set is supported. For some constructs, you have to use LIKE/LEFT/RIGHT/CHARINDEX and implement this yourself.
        /// </remarks>
        public static string ToSql(this QueryOperator instance)
        {
            switch (instance)
            {
                case QueryOperator.Equal:
                    return "=";
                case QueryOperator.GreaterOrEqual:
                    return ">=";
                case QueryOperator.Greater:
                    return ">";
                case QueryOperator.LowerOrEqual:
                    return "<=";
                case QueryOperator.Lower:
                    return "<";
                case QueryOperator.NotEqual:
                    return "<>";
                default:
                    throw new ArgumentOutOfRangeException(nameof(instance), $"Unsupported query operator: {instance}");
            }
        }

        /// <summary>
        /// Creates a NOT keyword when the operator is negative.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static string ToNot(this QueryOperator instance)
            => instance.In(QueryOperator.NotContains,
                           QueryOperator.NotEndsWith,
                           QueryOperator.NotStartsWith)
                ? "NOT "
                : string.Empty;
    }
}
