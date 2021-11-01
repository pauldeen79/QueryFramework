using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;

namespace QueryFramework.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string with a separator, with possibility to escape the separator if it's between open and close tokens.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="noSplitOpenToken">The no split open token.</param>
        /// <param name="noSplitCloseToken">The no split close token.</param>
        /// <returns>
        /// The split string.
        /// </returns>
        public static string[] SafeSplit(this string value, char separator, char noSplitOpenToken, char noSplitCloseToken)
        {
            var lst = new List<string>();
            var lastSeparatorPostition = -1;
            var nestedCount = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == separator && nestedCount == 0)
                {
                    lst.Add(value.Substring(lastSeparatorPostition + 1, i - lastSeparatorPostition - 1));
                    lastSeparatorPostition = i;
                }

                if (noSplitOpenToken == noSplitCloseToken)
                {
                    if (value[i] == noSplitOpenToken)
                    {
                        nestedCount = GetNextNestedCountForEqualSplitOpenAndCloseToken(nestedCount);
                    }
                }
                else
                {
                    nestedCount = GetNextNestedCountForUnequalSplitOpenAndCloseToken(nestedCount, value[i], noSplitOpenToken, noSplitCloseToken);
                }
            }

            AddRemainder(value, lst, lastSeparatorPostition);

            return lst.Select(x => RemoveQuotes(x, noSplitOpenToken, noSplitCloseToken)).ToArray();
        }

        private static int GetNextNestedCountForUnequalSplitOpenAndCloseToken(int nestedCount, char current, char noSplitOpenToken, char noSplitCloseToken)
        {
            if (current == noSplitOpenToken)
            {
                return nestedCount + 1;
            }

            if (current == noSplitCloseToken)
            {
                return nestedCount - 1;
            }

            return nestedCount;
        }

        private static int GetNextNestedCountForEqualSplitOpenAndCloseToken(int nestedCount)
            => nestedCount == 0
                ? 1
                : 0;

        private static void AddRemainder(string value, List<string> lst, int lastSeparatorPostition)
        {
            if (lastSeparatorPostition != value.Length - 1)
            {
                lst.Add(value.Substring(lastSeparatorPostition + 1));
            }
            else
            {
                lst.Add(string.Empty);
            }
        }

        private static string RemoveQuotes(string arg, char noSplitOpenToken, char noSplitCloseToken)
        {
            if (arg.StartsWith(noSplitOpenToken.ToString()) && arg.EndsWith(noSplitCloseToken.ToString()))
            {
                return arg.Substring(1, arg.Length - 2);
            }

            return arg;
        }

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
    }
}
