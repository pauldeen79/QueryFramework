using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Functions;

namespace QueryFramework.Core.Extensions
{
    public static class StringExtensions
    {
        #region Generated Code
        /// <summary>Creates a query expression with the Contains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesContain(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.Contains)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the EndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesEndWith(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.EndsWith)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the Equal query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsEqualTo(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.Equal)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the GreterOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsGreaterOrEqualThan(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.GreaterOrEqual)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the GreaterThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsGreaterThan(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.Greater)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the IsNotNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        public static IQueryConditionBuilder IsNotNull(this string fieldName)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.IsNotNull);

        /// <summary>Creates a query expression with the IsNotNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        public static IQueryConditionBuilder IsNotNullOrEmpty(this string fieldName)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.IsNotNullOrEmpty);

        /// <summary>Creates a query expression with the IsNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        public static IQueryConditionBuilder IsNull(this string fieldName)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.IsNull);

        /// <summary>Creates a query expression with the IsNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        public static IQueryConditionBuilder IsNullOrEmpty(this string fieldName)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.IsNullOrEmpty);

        /// <summary>Creates a query expression with the IsLowerOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsLowerOrEqualThan(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.LowerOrEqual)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the IsLowerThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsLowerThan(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.Lower)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the NotContains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesNotContain(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.NotContains)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the NotEndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesNotEndWith(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.NotEndsWith)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the NotEqual query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsNotEqualTo(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.NotEqual)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the NotStartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesNotStartWith(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.NotStartsWith)
                                          .WithValue(value);

        /// <summary>Creates a query expression with the StartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesStartWith(this string fieldName, object? value)
            => new QueryConditionBuilder().WithField(fieldName)
                                          .WithOperator(QueryOperator.StartsWith)
                                          .WithValue(value);
        #endregion

        #region Built-in functions
        /// <summary>Gets the length of this field.</summary>
        public static IQueryExpressionBuilder Len(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new LengthFunction());

        /// <summary>Trims the value of this field.</summary>
        public static IQueryExpressionBuilder SqlTrim(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new TrimFunction());

        /// <summary>Gets the upper-cased value of this field.</summary>
        public static IQueryExpressionBuilder Upper(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new UpperFunction());

        /// <summary>Gets the lower-cased value of this field.</summary>
        public static IQueryExpressionBuilder Lower(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new LowerFunction());

        /// <summary>Gets the left part of this expression.</summary>
        /// <param name="length">Number of positions</param>
        public static IQueryExpressionBuilder Left(this string fieldName, int length)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new LeftFunction(length));

        /// <summary>Gets the right part of this expression.</summary>
        /// <param name="length">Number of positions</param>
        public static IQueryExpressionBuilder Right(this string fieldName, int length)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new RightFunction(length));

        /// <summary>Gets the year of this date field.</summary>
        public static IQueryExpressionBuilder Year(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new YearFunction());

        /// <summary>Gets the month of this date field.</summary>
        public static IQueryExpressionBuilder Month(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new MonthFunction());

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpressionBuilder Day(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new DayFunction());

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpressionBuilder Coalesce(this string fieldName, params IQueryExpression[] innerExpressions)
            => new CoalesceFunctionBuilder().WithFieldName(fieldName)
                                            .AddInnerExpressions(innerExpressions.Select(x => new QueryExpressionBuilder(x)));

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpressionBuilder Coalesce(this string fieldName, params string[] innerFieldNames)
            => Coalesce(fieldName, innerFieldNames.Select(innerFieldName => new QueryExpression(innerFieldName)).ToArray());

        /// <summary>Gets the count of this field.</summary>
        public static IQueryExpressionBuilder Count(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new CountFunction());

        /// <summary>Gets the sum of this field.</summary>
        public static IQueryExpressionBuilder Sum(this string fieldName)
            => new QueryExpressionBuilder().WithFieldName(fieldName).WithFunction(new SumFunction());
        #endregion
    }
}
