using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Builders;
using CrossCutting.Data.Sql.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    internal static class PagedSelectCommandBuilderExtensions
    {
        internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                         ISingleEntityQuery query,
                                                         IPagedDatabaseEntityRetrieverSettings settings,
                                                         IQueryFieldProvider fieldProvider,
                                                         IFieldSelectionQuery? fieldSelectionQuery)
            => fieldSelectionQuery?.GetAllFields != false
                ? instance.AppendSelectFieldsForAllFields(settings, fieldProvider)
                : instance.AppendSelectFieldsForSpecifiedFields(fieldSelectionQuery, fieldProvider);

        private static PagedSelectCommandBuilder AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                                IPagedDatabaseEntityRetrieverSettings settings,
                                                                                IQueryFieldProvider fieldProvider)
        {
            var allFields = fieldProvider.GetAllFields();
            if (!allFields.Any())
            {
                instance.Select(settings.Fields.WhenNullOrWhitespace("*"));
            }
            else
            {
                var paramCounter = 0;
                foreach (var fieldName in allFields)
                {
                    if (paramCounter > 0)
                    {
                        instance.Select(", ");
                    }

                    instance.Select(fieldName);
                    paramCounter++;
                }
            }

            return instance;
        }

        private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                      IFieldSelectionQuery fieldSelectionQuery,
                                                                                      IQueryFieldProvider fieldProvider)
        {
            var paramCounter = 0;
            foreach (var expression in fieldSelectionQuery.GetSelectFields(fieldProvider))
            {
                if (paramCounter > 0)
                {
                    instance.Select(", ");
                }

                var correctedFieldName = fieldProvider.GetDatabaseFieldName(expression.FieldName);
                if (correctedFieldName == null)
                {
                    throw new InvalidOperationException($"Query fields contains unknown field in expression [{expression}]");
                }

                //Note that for now, we assume that custom expressions don't override field name logic, only expression logic
                var correctedExpression = new QueryExpression(correctedFieldName, expression.GetRawExpression());

                if (!fieldProvider.ValidateExpression(correctedExpression))
                {
                    throw new InvalidOperationException($"Query fields contains invalid expression [{expression}]");
                }

                instance.Select(correctedExpression.Expression);
                paramCounter++;
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder Distinct(this PagedSelectCommandBuilder instance,
                                                           IFieldSelectionQuery? fieldSelectionQuery)
            => instance.DistinctValues(fieldSelectionQuery?.Distinct == true);

        internal static PagedSelectCommandBuilder Top(this PagedSelectCommandBuilder instance,
                                                      ISingleEntityQuery query,
                                                      IPagedDatabaseEntityRetrieverSettings settings)
        {
            var limit = query.Limit.IfNotGreaterThan(settings.OverridePageSize);

            return limit > 0 
                ? instance.WithTop(limit)
                : instance;
        }

        internal static PagedSelectCommandBuilder Offset(this PagedSelectCommandBuilder instance,
                                                         ISingleEntityQuery query)
            => query.Offset.GetValueOrDefault() > 0
                ? instance.Skip(query.Offset.GetValueOrDefault())
                : instance;

        internal static PagedSelectCommandBuilder From(this PagedSelectCommandBuilder instance,
                                                       ISingleEntityQuery query,
                                                       IPagedDatabaseEntityRetrieverSettings settings)
            => instance.From(query.GetTableName(settings.TableName));

        internal static PagedSelectCommandBuilder Where(this PagedSelectCommandBuilder instance,
                                                        ISingleEntityQuery query,
                                                        IPagedDatabaseEntityRetrieverSettings settings,
                                                        IQueryFieldProvider fieldProvider,
                                                        out int paramCounter)
        {
            if ((query.Conditions?.Any() != true)
                && string.IsNullOrEmpty(settings.DefaultWhere))
            {
                paramCounter = 0;
                return instance;
            }

            paramCounter = 0;

            if (!string.IsNullOrEmpty(settings.DefaultWhere))
            {
                instance.Where(settings.DefaultWhere);
            }

            foreach (var queryCondition in query.Conditions ?? Enumerable.Empty<IQueryCondition>())
            {
                paramCounter = instance.AppendQueryCondition
                (
                    paramCounter,
                    queryCondition,
                    settings,
                    fieldProvider,
                    queryCondition.Combination == QueryCombination.And
                        ? instance.And
                        : instance.Or
                );
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder GroupBy(this PagedSelectCommandBuilder instance,
                                                          IGroupingQuery? groupingQuery,
                                                          IPagedDatabaseEntityRetrieverSettings settings,
                                                          IQueryFieldProvider fieldProvider)
        {
            if (groupingQuery?.GroupByFields?.Any() != true)
            {
                return instance;
            }

            var fieldCounter = 0;
            foreach (var groupBy in groupingQuery.GroupByFields)
            {
                if (fieldCounter > 0)
                {
                    instance.GroupBy(", ");
                }

                var correctedFieldName = fieldProvider.GetDatabaseFieldName(groupBy.FieldName);
                if (correctedFieldName == null)
                {
                    throw new InvalidOperationException($"Query group by fields contains unknown field [{groupBy.FieldName}]");
                }
                var corrected = new QueryExpression(correctedFieldName, groupBy.GetRawExpression());
                if (!fieldProvider.ValidateExpression(corrected))
                {
                    throw new InvalidOperationException($"Query group by fields contains invalid expression [{corrected}]");
                }
                instance.GroupBy(corrected.Expression);
                fieldCounter++;
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder Having(this PagedSelectCommandBuilder instance,
                                                         IGroupingQuery? groupingQuery,
                                                         IPagedDatabaseEntityRetrieverSettings settings,
                                                         IQueryFieldProvider fieldProvider,
                                                         ref int paramCounter)
        {
            if (groupingQuery?.HavingFields?.Any() != true)
            {
                return instance;
            }

            var fieldCounter = 0;
            foreach (var having in groupingQuery.HavingFields)
            {
                if (fieldCounter > 0)
                {
                    instance.Having($" {having.Combination.ToSql()} ");
                }
                paramCounter = instance.AppendQueryCondition
                (
                    paramCounter,
                    having,
                    settings,
                    fieldProvider,
                    instance.Having
                );
                fieldCounter++;
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
                                                          ISingleEntityQuery query,
                                                          IPagedDatabaseEntityRetrieverSettings settings,
                                                          IQueryFieldProvider fieldProvider)
        {
            if (query.Offset.HasValue && query.Offset.Value >= 0)
            {
                //do not use order by (this will be taken care of by the row_number function)
                return instance;
            }
            else if (query.OrderByFields?.Any() == true
                || !string.IsNullOrEmpty(settings.DefaultOrderBy))
            {
                return instance.AppendOrderBy(query.OrderByFields ?? Enumerable.Empty<IQuerySortOrder>(), settings, fieldProvider);
            }
            else
            {
                return instance;
            }
        }

        private static PagedSelectCommandBuilder AppendOrderBy(this PagedSelectCommandBuilder instance,
                                                               IEnumerable<IQuerySortOrder> orderByFields,
                                                               IPagedDatabaseEntityRetrieverSettings settings,
                                                               IQueryFieldProvider fieldProvider)
        {
            var fieldCounter = 0;
            foreach (var querySortOrder in orderByFields)
            {
                if (fieldCounter > 0)
                {
                    instance.OrderBy(", ");
                }

                var newFieldName = fieldProvider.GetDatabaseFieldName(querySortOrder.Field.FieldName);
                if (newFieldName == null)
                {
                    throw new InvalidOperationException(string.Format("Query order by fields contains unknown field [{0}]", querySortOrder.Field.FieldName));
                }
                var newQuerySortOrder = new QuerySortOrder(newFieldName, querySortOrder.Order);
                if (!fieldProvider.ValidateExpression(newQuerySortOrder.Field))
                {
                    throw new InvalidOperationException($"Query order by fields contains invalid expression [{newQuerySortOrder.Field}]");
                }
                instance.OrderBy($"{newQuerySortOrder.Field.Expression} {newQuerySortOrder.ToSql()}");

                fieldCounter++;
            }

            if (fieldCounter == 0 && !string.IsNullOrEmpty(settings.DefaultOrderBy))
            {
                instance.OrderBy(settings.DefaultOrderBy);
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder WithParameters(this PagedSelectCommandBuilder instance,
                                                                 IParameterizedQuery? parameterizedQuery)
        {
            if (parameterizedQuery != null)
            {
                foreach (var parameter in parameterizedQuery.Parameters)
                {
                    instance.AppendParameter(parameter.Name, parameter.Value);
                }
            }

            return instance;
        }

        internal static int AppendQueryCondition(this PagedSelectCommandBuilder instance,
                                                 int paramCounter,
                                                 IQueryCondition queryCondition,
                                                 IPagedDatabaseEntityRetrieverSettings settings,
                                                 IQueryFieldProvider fieldProvider,
                                                 Func<string, PagedSelectCommandBuilder> actionDelegate)
        {
            var builder = new StringBuilder();

            if (queryCondition.OpenBracket)
            {
                builder.Append("(");
            }

            var customFieldName = fieldProvider.GetDatabaseFieldName(queryCondition.Field.FieldName);
            if (customFieldName == null)
            {
                throw new InvalidOperationException($"Query conditions contains unknown field [{queryCondition.Field.FieldName}]");
            }

            var field = queryCondition.Field.With(fieldName: customFieldName);

            if (!fieldProvider.ValidateExpression(field))
            {
                throw new InvalidOperationException($"Query conditions contains invalid expression [{field}]");
            }

            if (!queryCondition.Operator.In(QueryOperator.Contains,
                                            QueryOperator.NotContains,
                                            QueryOperator.EndsWith,
                                            QueryOperator.NotEndsWith,
                                            QueryOperator.StartsWith,
                                            QueryOperator.NotStartsWith)
               )
            {
                builder.Append(queryCondition.Operator.ToNot());

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    builder.Append("COALESCE(");
                }

                builder.Append(field.Expression);

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    builder.Append(",'')");
                }
            }

            var paramName = GetQueryParameterName(paramCounter, queryCondition.Value);

            AppendOperatorAndValue(instance, paramCounter, queryCondition, field, paramName, builder);

            if (queryCondition.CloseBracket)
            {
                builder.Append(")");
            }

            actionDelegate.Invoke(builder.ToString());

            return paramCounter + 1;
        }

        private static void AppendOperatorAndValue(PagedSelectCommandBuilder instance,
                                                   int paramCounter,
                                                   IQueryCondition queryCondition,
                                                   IQueryExpression field,
                                                   string paramName,
                                                   StringBuilder builder)
        {
            if (queryCondition.Operator == QueryOperator.IsNull)
            {
                builder.Append(" IS NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNull)
            {
                builder.Append(" IS NOT NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNullOrEmpty)
            {
                builder.Append(" = ''");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNullOrEmpty)
            {
                builder.Append(" <> ''");
            }
            else if (queryCondition.Operator == QueryOperator.Contains)
            {
                builder.Append($"CHARINDEX({paramName}, {field.Expression}) > 0");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotContains)
            {
                builder.Append($"CHARINDEX({paramName}, {field.Expression}) = 0");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.StartsWith)
            {
                builder.Append($"LEFT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotStartsWith)
            {
                builder.Append($"LEFT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.EndsWith)
            {
                builder.Append($"RIGHT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotEndsWith)
            {
                builder.Append($"RIGHT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else
            {
                builder.Append($" {queryCondition.Operator.ToSql()} {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
        }

        private static void AppendParameterIfNecessary(PagedSelectCommandBuilder instance,
                                                       int paramCounter,
                                                       IQueryCondition queryCondition)
        {
            if (!(queryCondition.Value is IQueryParameterValue))
            {
                instance.AppendParameter(string.Format("p{0}", paramCounter),
                                         queryCondition.Value is KeyValuePair<string, object> keyValuePair
                                             ? keyValuePair.Value
                                             : queryCondition.Value ?? new object());
            }
        }

        private static string GetQueryParameterName(int paramCounter, object? value)
        {
            if (value is KeyValuePair<string, object> keyValuePair)
            {
                return $"@{keyValuePair.Key}";
            }

            if (value is IQueryParameterValue queryParameterValue)
            {
                return $"@{queryParameterValue.Name}";
            }

            return $"@p{paramCounter}";
        }
    }
}
