using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Functions;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionBuilderExtensions
    {
        public static IQueryExpressionFunction? GetFunction(this IQueryExpressionBuilder instance)
            => instance.Build() as IQueryExpressionFunction ?? instance.Function;

        #region Built-in functions
        /// <summary>Gets the length of this expression.</summary>
        public static IQueryExpressionBuilder Len(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new LengthFunction(instance.GetFunction()));

        /// <summary>Trims the value of this expression.</summary>
        public static IQueryExpressionBuilder Trim(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new TrimFunction(instance.GetFunction()));

        /// <summary>Gets the upper-cased value of this expression.</summary>
        public static IQueryExpressionBuilder Upper(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new UpperFunction(instance.GetFunction()));

        /// <summary>Gets the lower-cased value of this expression.</summary>
        public static IQueryExpressionBuilder Lower(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new LowerFunction(instance.GetFunction()));

        /// <summary>Gets the left part of this expression.</summary>
        public static IQueryExpressionBuilder Left(this IQueryExpressionBuilder instance, int length)
            => instance.WithFunction(new LeftFunction(length, instance.GetFunction()));

        /// <summary>Gets the right part of this expression.</summary>
        public static IQueryExpressionBuilder Right(this IQueryExpressionBuilder instance, int length)
            => instance.WithFunction(new RightFunction(length, instance.GetFunction()));

        /// <summary>Gets the year of this date expression.</summary>
        public static IQueryExpressionBuilder Year(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new YearFunction(instance.GetFunction()));

        /// <summary>Gets the month of this date expression.</summary>
        public static IQueryExpressionBuilder Month(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new MonthFunction(instance.GetFunction()));

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpressionBuilder Day(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new DayFunction(instance.GetFunction()));

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpressionBuilder Coalesce(this IQueryExpressionBuilder instance, params IQueryExpressionBuilder[] innerExpressions)
            => instance.WithFunction(new CoalesceFunctionBuilder().WithFunction(instance.GetFunction()).AddInnerExpressions(innerExpressions).WithFieldName(instance.FieldName).Build() as CoalesceFunction);

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpressionBuilder Coalesce(this IQueryExpressionBuilder instance, params string[] innerFieldNames)
            => Coalesce(instance, innerFieldNames.Select(innerFieldName => new QueryExpressionBuilder().WithFieldName(innerFieldName)).ToArray());

        /// <summary>Gets the count of this expression.</summary>
        public static IQueryExpressionBuilder Count(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new CountFunction(instance.GetFunction()));

        /// <summary>Gets the sum of this expression.</summary>
        public static IQueryExpressionBuilder Sum(this IQueryExpressionBuilder instance)
            => instance.WithFunction(new SumFunction(instance.GetFunction()));
        #endregion
    }
}
