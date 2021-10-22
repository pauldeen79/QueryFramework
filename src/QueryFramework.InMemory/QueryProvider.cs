using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;

namespace QueryFramework.InMemory
{
    public class QueryProvider<T> : IQueryProcessor<T>
    {
        private IEnumerable<T> SourceData { get; }
        private IExpressionEvaluator<T> ValueRetriever { get; }

        public QueryProvider(IEnumerable<T> sourceData)
        {
            SourceData = sourceData;
            ValueRetriever = new ExpressionEvaluator<T>(new ValueProvider());
        }

        public IQueryResult<T> Query(ISingleEntityQuery query)
        {
            var filteredRecords = new List<T>();

            foreach (var item in SourceData)
            {
                if (ItemIsValid(item, query.Conditions))
                {
                    filteredRecords.Add(item);
                }
            }

            var set = GetPagedData(query, filteredRecords);

            if (query.OrderByFields.Any())
            {
                set = set.OrderBy(x => new OrderByWrapper<T>(x, query.OrderByFields, ValueRetriever));
            }

            return new QueryResult<T>(set, filteredRecords.Count);
        }

        private static IEnumerable<T> GetPagedData(ISingleEntityQuery query, List<T> filteredRecords)
        {
            IEnumerable<T> result = filteredRecords;
            if (query.Offset != null)
            {
                result = result.Skip(query.Offset.Value);
            }
            if (query.Limit != null)
            {
                result = result.Take(query.Limit.Value);
            }

            return result;
        }

        private bool ItemIsValid(T item, IReadOnlyCollection<IQueryCondition> conditions)
        {
            var builder = new StringBuilder();
            foreach (var condition in conditions)
            {
                if (builder.Length > 0)
                {
                    builder.Append(condition.Combination == QueryCombination.And ? "&" : "|");
                }

                var value = ValueRetriever.GetValue(item, condition.Field);
                var prefix = condition.OpenBracket ? "(" : string.Empty;
                var suffix = condition.CloseBracket ? ")" : string.Empty;
                var result = Evaluate(condition, value);
                builder.Append(prefix)
                       .Append(result ? "T" : "F")
                       .Append(suffix);
            }

            return EvaluateBooleanExpression(builder.ToString());
        }

        private static bool EvaluateBooleanExpression(string expression)
        {
            var result = true;
            var @operator = "&";
            var openIndex = -1;
            int closeIndex;
            do
            {
                closeIndex = expression.IndexOf(")");
                if (closeIndex > -1)
                {
                    openIndex = expression.LastIndexOf("(", closeIndex);
                    if (openIndex > -1)
                    {
                        result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                        expression = (openIndex == 0 ? string.Empty : expression.Substring(0, openIndex - 1))
                            + (result ? "T" : "F")
                            + (closeIndex == expression.Length ? string.Empty : expression.Substring(closeIndex + 1));
                    }
                }
            } while (closeIndex > -1 && openIndex > -1);

            foreach (var character in expression)
            {
                bool currentResult;
                switch (character)
                {
                    case '&':
                        @operator = "&";
                        break;
                    case '|':
                        @operator = "|";
                        break;
                    case 'T':
                    case 'F':
                        currentResult = character == 'T';
                        result = @operator == "&"
                            ? result && currentResult
                            : result || currentResult;
                        break;
                }
            }

            return result;
        }

        private static bool Evaluate(IQueryCondition condition, object value)
        {
            var conditionValueString = condition.Value == null
                ? null
                : condition.Value.ToString();
            
            if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
            {
                return predicate.Invoke(new OperatorData(value, conditionValueString, condition.Value));
            }

            throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported query operator: {condition.Operator}");
        }
    }
}
