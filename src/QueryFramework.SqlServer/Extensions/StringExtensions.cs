using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.SqlServer.Functions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class StringExtensions
    {
        #region Built-in Sql functions
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
