﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.QueryParsers
{
    public class SingleEntityQueryParser<TQueryBuilder, TQueryExpressionBuilder> : IQueryParser<TQueryBuilder>
        where TQueryBuilder : ISingleEntityQueryBuilder
        where TQueryExpressionBuilder : IQueryExpressionBuilder, new()
    {
        private readonly Func<TQueryExpressionBuilder> _defaultFieldExpressionBuilderFactory;

        public SingleEntityQueryParser(Func<TQueryExpressionBuilder> defaultFieldExpressionBuilderFactory)
        {
            _defaultFieldExpressionBuilderFactory = defaultFieldExpressionBuilderFactory;
        }

        public TQueryBuilder Parse(TQueryBuilder builder, string queryString)
        {
            if (queryString is null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var items = queryString
                .Replace("\r\n", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .SafeSplit(' ', '\"', '\"');

            builder.Conditions = PerformQuerySearch(items) ?? PerformSimpleSearch(items);

            return builder;
        }

        private List<IQueryConditionBuilder> PerformQuerySearch(string[] items)
        {
            var itemCountIsCorrect = (items.Length - 3) % 4 == 0;
            if (!itemCountIsCorrect)
            {
                return default;
            }
            var nextSearchCombination = QueryCombination.And;
            var result = new List<IQueryConditionBuilder>();
            for (int i = 0; i < items.Length && itemCountIsCorrect; i += 4)
            {
                //verify that:
                //-items[i] needs to be a valid fieldname
                //-items[i + 1] needs to be a valid operator
                //-items[i + 3] needs to be a valic combination for the next condition
                var openBracket = false;
                var closeBracket = false;
                var fieldName = items[i];
                var fieldValue = items[i + 2];
                var @operator = items[i + 1];

                //remove brackets and set bracket property values for this query item.
                if (fieldName.StartsWith("("))
                {
                    openBracket = true;
                    fieldName = fieldName.Substring(1);
                }
                if (fieldValue.EndsWith(")"))
                {
                    closeBracket = true;
                    fieldValue = fieldValue.Substring(0, fieldValue.Length - 1);
                }

                var queryOperator = GetQueryOperator(@operator);
                if (queryOperator == null)
                {
                    return default;
                }

                var condition = new QueryConditionBuilder
                {
                    OpenBracket = openBracket,
                    CloseBracket = closeBracket,
                    Combination = nextSearchCombination,
                    Field = _defaultFieldExpressionBuilderFactory == null
                        ? new TQueryExpressionBuilder().WithFieldName(fieldName)
                        : _defaultFieldExpressionBuilderFactory.Invoke().WithFieldName(fieldName),
                    Operator = queryOperator.Value,
                    Value = queryOperator == QueryOperator.IsNull || queryOperator == QueryOperator.IsNotNull
                        ? null
                        : fieldValue
                };

                if (items.Length > i + 3)
                {
                    var combination = GetQueryCombination(items[i + 3]);
                    if (combination == null)
                    {
                        return default;
                    }
                    nextSearchCombination = combination.Value;
                }

                result.Add(condition);
            }

            return result;
        }

        private List<IQueryConditionBuilder> PerformSimpleSearch(string[] items)
            => items
                .Where(x => !string.IsNullOrEmpty(x))
                .Select((x, i) => new
                {
                    Index = i,
                    Value = x,
                    StartsWithPlusOrMinus = x.StartsWith("+") || x.StartsWith("-"),
                    StartsWithMinus = x.StartsWith("-")
                })
                .Select(item => new QueryConditionBuilder
                {
                    Field = _defaultFieldExpressionBuilderFactory == null
                       ? new TQueryExpressionBuilder()
                       : _defaultFieldExpressionBuilderFactory.Invoke(),
                    Combination = item.StartsWithPlusOrMinus
                       ? QueryCombination.And
                       : QueryCombination.Or,
                    Value = item.StartsWithPlusOrMinus
                       ? item.Value.Substring(1)
                       : item.Value,
                    Operator = item.StartsWithMinus
                       ? QueryOperator.NotContains
                       : QueryOperator.Contains,
                    OpenBracket = item.Index == 0,
                    CloseBracket = item.Index == items.Length - 1
                })
                .Cast<IQueryConditionBuilder>()
                .ToList();

        private static QueryCombination? GetQueryCombination(string combination)
            => combination.ToUpper(CultureInfo.InvariantCulture) switch
            {
                "AND" => QueryCombination.And,
                "OR" => QueryCombination.Or,
                _ => null,// Unknown search combination
            };

        private static QueryOperator? GetQueryOperator(string @operator)
            => @operator.ToUpper(CultureInfo.InvariantCulture) switch
        {
            var x when
                x == "=" ||
                x == "==" => QueryOperator.Equal,
            var x when
                x == "<>" ||
                x == "!=" ||
                x == "#" => QueryOperator.NotEqual,
            "<" => QueryOperator.Lower,
            ">" => QueryOperator.Greater,
            "<=" => QueryOperator.LowerOrEqual,
            ">=" => QueryOperator.GreaterOrEqual,
            "CONTAINS" => QueryOperator.Contains,
            var x when
                x == "NOTCONTAINS" ||
                x == "NOT CONTAINS" => QueryOperator.NotContains,
            "IS" => QueryOperator.IsNull,
            var x when
                x == "ISNOT" ||
                x == "IS NOT" => QueryOperator.IsNotNull,
            var x when
            x == "STARTS WITH" ||
            x == "STARTSWITH" => QueryOperator.StartsWith,
            var x when
                x == "ENDS WITH" ||
                x == "ENDSWITH" => QueryOperator.EndsWith,
            var x when
            x == "NOT STARTS WITH" ||
            x == "NOTSTARTSWITH" => QueryOperator.NotStartsWith,
            var x when
            x == "NOT ENDS WITH" ||
            x == "NOTENDSWITH" => QueryOperator.NotEndsWith,
            _ => null // Unknown operator
        };
    }
}
