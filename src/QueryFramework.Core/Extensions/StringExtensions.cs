using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Core.Functions;

namespace QueryFramework.Core.Extensions
{
    public static class StringExtensions
    {
        #region Generated Code
        /// <summary>Creates a query expression with the Contains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesContain(this string fieldName,
                                                  object? value = null,
                                                  bool openBracket = false,
                                                  bool closeBracket = false,
                                                  QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.Contains,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the EndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesEndWith(this string fieldName,
                                                  object? value = null,
                                                  bool openBracket = false,
                                                  bool closeBracket = false,
                                                  QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.EndsWith,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the Equal query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsEqualTo(this string fieldName,
                                                object? value = null,
                                                bool openBracket = false,
                                                bool closeBracket = false,
                                                QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.Equal,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the GreterOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsGreaterOrEqualThan(this string fieldName,
                                                           object? value = null,
                                                           bool openBracket = false,
                                                           bool closeBracket = false,
                                                           QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.GreaterOrEqual,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the GreaterThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsGreaterThan(this string fieldName,
                                                    object? value = null,
                                                    bool openBracket = false,
                                                    bool closeBracket = false,
                                                    QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.Greater,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsNotNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotNull(this string fieldName,
                                                bool openBracket = false,
                                                bool closeBracket = false,
                                                QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.IsNotNull,
                              null,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsNotNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotNullOrEmpty(this string fieldName,
                                                       bool openBracket = false,
                                                       bool closeBracket = false,
                                                       QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.IsNotNullOrEmpty,
                              null,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNull(this string fieldName,
                                             bool openBracket = false,
                                             bool closeBracket = false,
                                             QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.IsNull,
                              null,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNullOrEmpty(this string fieldName,
                                                    bool openBracket = false,
                                                    bool closeBracket = false,
                                                    QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.IsNullOrEmpty,
                              null,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsLowerOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsLowerOrEqualThan(this string fieldName,
                                                         object? value = null,
                                                         bool openBracket = false,
                                                         bool closeBracket = false,
                                                         QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.LowerOrEqual,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the IsLowerThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsLowerThan(this string fieldName,
                                                  object? value = null,
                                                  bool openBracket = false,
                                                  bool closeBracket = false,
                                                  QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.Lower,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the NotContains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotContain(this string fieldName,
                                                     object? value = null,
                                                     bool openBracket = false,
                                                     bool closeBracket = false,
                                                     QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.NotContains,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the NotEndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotEndWith(this string fieldName,
                                                     object? value = null,
                                                     bool openBracket = false,
                                                     bool closeBracket = false,
                                                     QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.NotEndsWith,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the NotEqual query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotEqualTo(this string fieldName,
                                                   object? value = null,
                                                   bool openBracket = false,
                                                   bool closeBracket = false,
                                                   QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.NotEqual,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the NotStartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotStartWith(this string fieldName,
                                                       object? value = null,
                                                       bool openBracket = false,
                                                       bool closeBracket = false,
                                                       QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.NotStartsWith,
                              value,
                              openBracket,
                              closeBracket,
                              combination);

        /// <summary>Creates a query expression with the StartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesStartWith(this string fieldName,
                                                    object? value = null,
                                                    bool openBracket = false,
                                                    bool closeBracket = false,
                                                    QueryCombination combination = QueryCombination.And)
        => new QueryCondition(fieldName,
                              QueryOperator.StartsWith,
                              value,
                              openBracket,
                              closeBracket,
                              combination);
        #endregion

        #region Built-in functions
        /// <summary>Gets the length of this field.</summary>
        public static IQueryExpression Len(this string fieldName)
            => new QueryExpression(fieldName, new LengthFunction());

        /// <summary>Trims the value of this field.</summary>
        public static IQueryExpression SqlTrim(this string fieldName)
            => new QueryExpression(fieldName, new TrimFunction());

        /// <summary>Gets the upper-cased value of this field.</summary>
        public static IQueryExpression Upper(this string fieldName)
            => new QueryExpression(fieldName, new UpperFunction());

        /// <summary>Gets the lower-cased value of this field.</summary>
        public static IQueryExpression Lower(this string fieldName)
            => new QueryExpression(fieldName, new LowerFunction());

        /// <summary>Gets the left part of this expression.</summary>
        /// <param name="length">Number of positions</param>
        public static IQueryExpression Left(this string fieldName, int length)
            => new QueryExpression(fieldName, new LeftFunction(length));

        /// <summary>Gets the right part of this expression.</summary>
        /// <param name="length">Number of positions</param>
        public static IQueryExpression Right(this string fieldName, int length)
            => new QueryExpression(fieldName, new RightFunction(length));

        /// <summary>Gets the year of this date field.</summary>
        public static IQueryExpression Year(this string fieldName)
            => new QueryExpression(fieldName, new YearFunction());

        /// <summary>Gets the month of this date field.</summary>
        public static IQueryExpression Month(this string fieldName)
            => new QueryExpression(fieldName, new MonthFunction());

        /// <summary>Gets the day of this date expression.</summary>
        public static IQueryExpression Day(this string fieldName)
            => new QueryExpression(fieldName, new DayFunction());

        /// <summary>Gets the first non-null value of the specified expressions.</summary>
        public static IQueryExpression Coalesce(this string fieldName, params IQueryExpression[] innerExpressions)
            => new CoalesceFunction(fieldName, null, innerExpressions);

        /// <summary>Gets the first non-null value of the specified fields.</summary>
        public static IQueryExpression Coalesce(this string fieldName, params string[] innerFieldNames)
            => Coalesce(fieldName, innerFieldNames.Select(innerFieldName => new QueryExpression(innerFieldName)).ToArray());

        /// <summary>Gets the count of this field.</summary>
        public static IQueryExpression Count(this string fieldName)
            => new QueryExpression(fieldName, new CountFunction());

        /// <summary>Gets the sum of this field.</summary>
        public static IQueryExpression Sum(this string fieldName)
            => new QueryExpression(fieldName, new SumFunction());
        #endregion
    }
}
