using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.SqlServer.Functions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryExpressionExtensions
    {
        #region Built-in Sql functions
        /// <summary>Gets the length of this expression.</summary>
        public static IQueryExpression Len(this IQueryExpression instance)
            => instance.With(function: new LengthFunction(instance.Function));

        /// <summary>Trims the value of this expression.</summary>
        public static IQueryExpression Trim(this IQueryExpression instance)
            => instance.With(function: new TrimFunction(instance.Function));

        /// <summary>Gets the upper-cased value of this expression.</summary>
        public static IQueryExpression Upper(this IQueryExpression instance)
            => instance.With(function: new UpperFunction(instance.Function));

        /// <summary>Gets the lower-cased value of this expression.</summary>
        public static IQueryExpression Lower(this IQueryExpression instance)
            => instance.With(function: new LowerFunction(instance.Function));

        /// <summary>Gets the left part of this expression.</summary>
        public static IQueryExpression Left(this IQueryExpression instance, int length)
            => instance.With(function: new LeftFunction(length, instance.Function));

        /// <summary>Gets the right part of this expression.</summary>
        public static IQueryExpression Right(this IQueryExpression instance, int length)
            => instance.With(function: new RightFunction(length, instance.Function));

        /// <summary>Gets the year of this date expression.</summary>
        public static IQueryExpression Year(this IQueryExpression instance)
            => instance.With(function: new YearFunction(instance.Function));

        /// <summary>Gets the month of this date expression.</summary>
        public static IQueryExpression Month(this IQueryExpression instance)
            => instance.With(function: new MonthFunction(instance.Function));

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpression Day(this IQueryExpression instance)
            => instance.With(function: new DayFunction(instance.Function));

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpression Coalesce(this IQueryExpression instance, params IQueryExpression[] innerExpressions)
            => new CoalesceFunction(instance.FieldName, instance.Function, innerExpressions);

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpression Coalesce(this IQueryExpression instance, params string[] innerFieldNames)
            => Coalesce(instance, innerFieldNames.Select(innerFieldName => new QueryExpression(innerFieldName)).ToArray());

        /// <summary>Gets the count of this expression.</summary>
        public static IQueryExpression Count(this IQueryExpression instance)
            => instance.With(function: new CountFunction(instance.Function));

        /// <summary>Gets the sum of this expression.</summary>
        public static IQueryExpression Sum(this IQueryExpression instance)
            => instance.With(function: new SumFunction(instance.Function));
        #endregion
    }
}
