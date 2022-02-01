using System.Linq;
using CrossCutting.Common.Extensions;
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
            => instance.Build() as IQueryExpressionFunction ?? instance?.Function?.Build();

        #region Built-in functions
        /// <summary>Gets the length of this expression.</summary>
        public static IQueryExpressionBuilder Len(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new LengthFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Trims the value of this expression.</summary>
        public static IQueryExpressionBuilder Trim(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new TrimFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the upper-cased value of this expression.</summary>
        public static IQueryExpressionBuilder Upper(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new UpperFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the lower-cased value of this expression.</summary>
        public static IQueryExpressionBuilder Lower(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new LowerFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the left part of this expression.</summary>
        public static IQueryExpressionBuilder Left(this IQueryExpressionBuilder instance, int length)
            => instance.Chain(x => x.Function = new LeftFunctionBuilder().WithInnerFunction(instance).WithLength(length));

        /// <summary>Gets the right part of this expression.</summary>
        public static IQueryExpressionBuilder Right(this IQueryExpressionBuilder instance, int length)
            => instance.Chain(x => x.Function = new RightFunctionBuilder().WithInnerFunction(instance).WithLength(length));

        /// <summary>Gets the year of this date expression.</summary>
        public static IQueryExpressionBuilder Year(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new YearFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the month of this date expression.</summary>
        public static IQueryExpressionBuilder Month(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new MonthFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpressionBuilder Day(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new DayFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpressionBuilder Coalesce(this IQueryExpressionBuilder instance, params IQueryExpressionBuilder[] innerExpressions)
            => instance.Chain(x => x.Function = new CoalesceFunctionBuilder().WithFunction(instance.Function).AddInnerExpressions(innerExpressions).WithFieldName(instance.FieldName));

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpressionBuilder Coalesce(this IQueryExpressionBuilder instance, params string[] innerFieldNames)
            => Coalesce(instance, innerFieldNames.Select(innerFieldName => new QueryExpressionBuilder().WithFieldName(innerFieldName)).ToArray());

        /// <summary>Gets the count of this expression.</summary>
        public static IQueryExpressionBuilder Count(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new CountFunctionBuilder().WithInnerFunction(instance));

        /// <summary>Gets the sum of this expression.</summary>
        public static IQueryExpressionBuilder Sum(this IQueryExpressionBuilder instance)
            => instance.Chain(x => x.Function = new SumFunctionBuilder().WithInnerFunction(instance));
        #endregion
    }
}
