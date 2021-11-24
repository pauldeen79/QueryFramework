using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Core.Builders;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    internal static class DatabaseCommandBuilderExtensions
    {
        internal static DatabaseCommandBuilder AppendPagingOuterQuery(this DatabaseCommandBuilder instance,
                                                                      ISingleEntityQuery query,
                                                                      IQueryProcessorSettings settings,
                                                                      IQueryFieldProvider fieldProvider,
                                                                      bool countOnly)
            => query.Offset.HasValue && query.Offset.Value >= 0 && !countOnly
                ? instance
                    .Append("SELECT ")
                    .AppendSelectFields(query, settings, fieldProvider, countOnly)
                    .AppendFromClause()
                    .Append("(")
                : instance;

        internal static DatabaseCommandBuilder AppendSelectFields(this DatabaseCommandBuilder instance,
                                                                  ISingleEntityQuery query,
                                                                  IQueryProcessorSettings settings,
                                                                  IQueryFieldProvider fieldProvider,
                                                                  bool countOnly)
        {
            var fieldSelectionQuery = query as IFieldSelectionQuery;
            if (countOnly)
            {
                return instance.Append("COUNT(*)");
            }
            else if (fieldSelectionQuery?.GetAllFields != false)
            {
                return instance.AppendSelectFieldsForAllFields(settings, fieldProvider);
            }
            else
            {
                return instance.AppendSelectFieldsForSpecifiedFields(settings, fieldSelectionQuery, fieldProvider);
            }
        }

        private static DatabaseCommandBuilder AppendSelectFieldsForAllFields(this DatabaseCommandBuilder instance,
                                                                             IQueryProcessorSettings settings,
                                                                             IQueryFieldProvider fieldProvider)
        {
            var allFields = fieldProvider.GetAllFields();
            if (!allFields.Any())
            {
                instance.Append(settings.Fields.WhenNullOrWhitespace("*"));
            }
            else
            {
                var paramCounter = 0;
                foreach (var fieldName in allFields)
                {
                    if (paramCounter > 0)
                    {
                        instance.Append(", ");
                    }

                    instance.Append(fieldName);
                    paramCounter++;
                }
            }

            return instance;
        }

        private static DatabaseCommandBuilder AppendSelectFieldsForSpecifiedFields(this DatabaseCommandBuilder instance,
                                                                                   IQueryProcessorSettings settings,
                                                                                   IFieldSelectionQuery fieldSelectionQuery,
                                                                                   IQueryFieldProvider fieldProvider)
        {
            var paramCounter = 0;
            foreach (var expression in fieldSelectionQuery.GetSelectFields(fieldProvider))
            {
                if (paramCounter > 0)
                {
                    instance.Append(", ");
                }

                var correctedExpression = expression;
                var correctedFieldName = fieldProvider.GetDatabaseFieldName(expression.FieldName);
                if (correctedFieldName == null && settings.ValidateFieldNames)
                {
                    throw new InvalidOperationException($"Query fields contains unknown field in expression [{expression}]");
                }

                if (correctedFieldName != null)
                {
                    //Note that for now, we assume that custom expressions don't override field name logic, only expression logic
                    correctedExpression = new QueryExpression(correctedFieldName, expression.GetRawExpression());
                }

                if (!fieldProvider.ValidateExpression(correctedExpression))
                {
                    throw new InvalidOperationException($"Query fields contains invalid expression [{expression}]");
                }

                instance.Append(correctedExpression.Expression);
                paramCounter++;
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendSelectAndDistinctClause(this DatabaseCommandBuilder instance,
                                                                             IFieldSelectionQuery? fieldSelectionQuery,
                                                                             bool countOnly)
        {
            instance.Append("SELECT ");
            if (fieldSelectionQuery?.Distinct == true && !countOnly)
            {
                instance.Append("DISTINCT ");
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendTopClause(this DatabaseCommandBuilder instance,
                                                               ISingleEntityQuery query,
                                                               IQueryProcessorSettings settings,
                                                               bool countOnly)
        {
            if ((query.Offset == null || query.Offset.Value <= 0 || query.OrderByFields.Count == 0)
                && ((query.Limit.HasValue && query.Limit.Value > 0) || (settings.OverrideLimit.HasValue && settings.OverrideLimit.Value >= 0))
                && !countOnly)
            {
                return instance.Append($"TOP {query.Limit.DetermineLimit(settings.OverrideLimit)} ");
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendCountOrSelectFields(this DatabaseCommandBuilder instance,
                                                                         ISingleEntityQuery query,
                                                                         IQueryProcessorSettings settings,
                                                                         IQueryFieldProvider fieldProvider,
                                                                         bool countOnly)
            => countOnly
                ? instance.Append("COUNT(*)")
                : instance.AppendSelectFields(query, settings, fieldProvider, countOnly);

        internal static DatabaseCommandBuilder AppendPagingPrefix(this DatabaseCommandBuilder instance,
                                                                  ISingleEntityQuery query,
                                                                  IQueryProcessorSettings settings,
                                                                  IQueryFieldProvider fieldProvider,
                                                                  bool countOnly)
        {
            if (query.Offset.HasValue
                && query.Offset.Value >= 0
                && !countOnly)
            {
                var orderByBuilder = new StringBuilder();
                foreach (var querySortOrder in query.OrderByFields)
                {
                    if (orderByBuilder.Length > 0)
                    {
                        orderByBuilder.Append(", ");
                    }

                    var fieldName = GetOrderByFieldName(settings, fieldProvider, querySortOrder);
                    var corrected = new QuerySortOrder(new QueryExpression(fieldName ?? querySortOrder.Field.FieldName, querySortOrder.Field.GetRawExpression()), querySortOrder.Order);
                    if (!fieldProvider.ValidateExpression(corrected.Field))
                    {
                        throw new InvalidOperationException($"Query OrderByFields contains invalid expression [{corrected.Field}]");
                    }
                    orderByBuilder
                        .Append(corrected.Field.Expression)
                        .Append(" ")
                        .Append(querySortOrder.ToSql());
                }

                if (orderByBuilder.Length == 0)
                {
                    orderByBuilder.Append("(SELECT 0)");
                }

                return instance.Append($", ROW_NUMBER() OVER (ORDER BY {orderByBuilder}) as sq_row_number");
            }

            return instance;
        }

        private static string? GetOrderByFieldName(IQueryProcessorSettings settings,
                                                   IQueryFieldProvider fieldProvider,
                                                   IQuerySortOrder querySortOrder)
        {
            var fieldName = fieldProvider.GetDatabaseFieldName(querySortOrder.Field.FieldName);
            if (fieldName == null && settings.ValidateFieldNames)
            {
                throw new InvalidOperationException($"Query OrderByFields contains unknown field [{querySortOrder.Field}]");
            }

            return fieldName;
        }

        internal static DatabaseCommandBuilder AppendFromClause (this DatabaseCommandBuilder instance)
            => instance.Append(" FROM ");

        internal static DatabaseCommandBuilder AppendTableName(this DatabaseCommandBuilder instance,
                                                               ISingleEntityQuery query,
                                                               IQueryProcessorSettings settings)
            => instance.Append(query.GetTableName(settings.TableName));

        internal static DatabaseCommandBuilder AppendWhereClause(this DatabaseCommandBuilder instance,
                                                                 ISingleEntityQuery query,
                                                                 IQueryProcessorSettings settings,
                                                                 IQueryFieldProvider fieldProvider,
                                                                 out int paramCounter)
        {
            if ((query.Conditions?.Any() != true)
                && string.IsNullOrEmpty(settings.DefaultWhere))
            {
                paramCounter = 0;
                return instance;
            }

            instance.Append(" WHERE ");

            if (!string.IsNullOrEmpty(settings.DefaultWhere))
            {
                instance.Append(settings.DefaultWhere);

                if (query.Conditions.Any())
                {
                    instance.Append(" AND ");
                }
            }

            paramCounter = settings.InitialParameterNumber; //When parameters length = 2, this means 0 and 1. Then we have to start with 2.

            foreach (var queryCondition in query.Conditions ?? Enumerable.Empty<IQueryCondition>())
            {
                paramCounter = instance.AppendQueryCondition
                (
                    paramCounter,
                    queryCondition,
                    settings,
                    fieldProvider
                );
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendGroupByClause(this DatabaseCommandBuilder instance,
                                                                   IGroupingQuery? groupingQuery,
                                                                   IQueryProcessorSettings settings,
                                                                   IQueryFieldProvider fieldProvider)
        {
            if (groupingQuery?.GroupByFields?.Any() != true)
            {
                return instance;
            }

            instance.Append(" GROUP BY ");
            var fieldCounter = 0;
            foreach (var groupBy in groupingQuery.GroupByFields)
            {
                if (fieldCounter > 0)
                {
                    instance.Append(", ");
                }

                var correctedFieldName = fieldProvider.GetDatabaseFieldName(groupBy.FieldName);
                if (correctedFieldName == null && settings.ValidateFieldNames)
                {
                    throw new InvalidOperationException($"Query group by fields contains unknown field [{groupBy.FieldName}]");
                }
                var corrected = new QueryExpression(correctedFieldName ?? groupBy.FieldName, groupBy.GetRawExpression());
                if (!fieldProvider.ValidateExpression(corrected))
                {
                    throw new InvalidOperationException($"Query group by fields contains invalid expression [{corrected}]");
                }
                instance.Append(corrected.Expression);
                fieldCounter++;
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendHavingClause(this DatabaseCommandBuilder instance,
                                                                  IGroupingQuery? groupingQuery,
                                                                  IQueryProcessorSettings settings,
                                                                  IQueryFieldProvider fieldProvider,
                                                                  ref int paramCounter)
        {
            if (groupingQuery?.HavingFields?.Any() != true)
            {
                return instance;
            }

            instance.Append(" HAVING ");
            var fieldCounter = 0;
            foreach (var having in groupingQuery.HavingFields)
            {
                paramCounter = instance.AppendQueryCondition
                (
                    paramCounter,
                    having,
                    settings,
                    fieldProvider,
                    fieldCounter == 0
                );
                fieldCounter++;
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendOrderByClause(this DatabaseCommandBuilder instance,
                                                                   ISingleEntityQuery query,
                                                                   IQueryProcessorSettings settings,
                                                                   IQueryFieldProvider fieldProvider,
                                                                   bool countOnly)
        {
            if (query.Offset.HasValue && query.Offset.Value >= 0)
            {
                //do not use order by (this will be taken care of by the row_number function)
                return instance;
            }
            else if (countOnly)
            {
                //do not use order by, only count the number of records
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

        private static DatabaseCommandBuilder AppendOrderBy(this DatabaseCommandBuilder instance,
                                                            IEnumerable<IQuerySortOrder> orderByFields,
                                                            IQueryProcessorSettings settings,
                                                            IQueryFieldProvider fieldProvider)
        {
            instance.Append(" ORDER BY ");
            var fieldCounter = 0;
            foreach (var querySortOrder in orderByFields)
            {
                if (fieldCounter > 0)
                {
                    instance.Append(", ");
                }

                var newFieldName = fieldProvider.GetDatabaseFieldName(querySortOrder.Field.FieldName);
                if (newFieldName == null && settings.ValidateFieldNames)
                {
                    throw new InvalidOperationException(string.Format("Query order by fields contains unknown field [{0}]", querySortOrder.Field.FieldName));
                }
                var newQuerySortOrder = new QuerySortOrder(newFieldName ?? querySortOrder.Field.FieldName, querySortOrder.Order);
                if (!fieldProvider.ValidateExpression(newQuerySortOrder.Field))
                {
                    throw new InvalidOperationException($"Query order by fields contains invalid expression [{newQuerySortOrder.Field}]");
                }
                instance
                    .Append(newQuerySortOrder.Field.Expression)
                    .Append(" ")
                    .Append(newQuerySortOrder.ToSql());

                fieldCounter++;
            }

            if (fieldCounter == 0 && !string.IsNullOrEmpty(settings.DefaultOrderBy))
            {
                instance.Append(settings.DefaultOrderBy);
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AppendPagingSuffix(this DatabaseCommandBuilder instance,
                                                                  ISingleEntityQuery query,
                                                                  IQueryProcessorSettings settings,
                                                                  bool countOnly)
        {
            if (query.Offset.HasValue && query.Offset.Value > 0 && !countOnly)
            {
                if (query.Limit.DetermineLimit(settings.OverrideLimit) > 0)
                {
                    return instance.Append($") sq WHERE sq.sq_row_number BETWEEN {query.Offset.Value + 1} and {query.Offset.Value + query.Limit.DetermineLimit(settings.OverrideLimit)};");
                }
                else if (query.Limit.DetermineLimit(settings.OverrideLimit) == 0)
                {
                    return instance.Append($") sq WHERE sq.sq_row_number = 0;");
                }
                else
                {
                    return instance.Append($") sq WHERE sq.sq_row_number > {query.Offset.Value};");
                }
            }

            return instance;
        }

        internal static DatabaseCommandBuilder AddQueryParameters(this DatabaseCommandBuilder instance,
                                                                  ISingleEntityQuery query)
        {
            if (query is IParameterizedQuery parameterizedQuery)
            {
                foreach (var parameter in parameterizedQuery.Parameters)
                {
                    instance.AppendParameter(parameter.Name, parameter.Value);
                }
            }

            return instance;
        }

        internal static int AppendQueryCondition(this DatabaseCommandBuilder instance,
                                                 int paramCounter,
                                                 IQueryCondition queryCondition,
                                                 IQueryProcessorSettings settings,
                                                 IQueryFieldProvider fieldProvider,
                                                 bool skipFirstCombination = false)
        {
            if (paramCounter > 0 && !skipFirstCombination)
            {
                instance
                    .Append(" ")
                    .Append(queryCondition.Combination.ToSql())
                    .Append(" ");
            }

            if (queryCondition.OpenBracket)
            {
                instance.Append("(");
            }

            var customFieldName = fieldProvider.GetDatabaseFieldName(queryCondition.Field.FieldName);
            if (customFieldName == null && settings.ValidateFieldNames)
            {
                throw new InvalidOperationException($"Query conditions contains unknown field [{queryCondition.Field.FieldName}]");
            }

            var field = queryCondition.Field.With(fieldName: customFieldName ?? queryCondition.Field.FieldName);

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
                instance.Append(queryCondition.Operator.ToNot());

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    instance.Append("COALESCE(");
                }

                instance.Append(field.Expression);

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    instance.Append(",'')");
                }
            }

            var paramName = GetQueryParameterName(paramCounter, queryCondition.Value);

            AppendOperatorAndValue(instance, paramCounter, queryCondition, field, paramName);

            if (queryCondition.CloseBracket)
            {
                instance.Append(")");
            }

            return paramCounter + 1;
        }

        private static void AppendOperatorAndValue(DatabaseCommandBuilder instance,
                                                   int paramCounter,
                                                   IQueryCondition queryCondition,
                                                   IQueryExpression field,
                                                   string paramName)
        {
            if (queryCondition.Operator == QueryOperator.IsNull)
            {
                instance.Append(" IS NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNull)
            {
                instance.Append(" IS NOT NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNullOrEmpty)
            {
                instance.Append(" = ''");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNullOrEmpty)
            {
                instance.Append(" <> ''");
            }
            else if (queryCondition.Operator == QueryOperator.Contains)
            {
                instance.Append($"CHARINDEX({paramName}, {field.Expression}) > 0");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotContains)
            {
                instance.Append($"CHARINDEX({paramName}, {field.Expression}) = 0");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.StartsWith)
            {
                instance.Append($"LEFT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotStartsWith)
            {
                instance.Append($"LEFT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.EndsWith)
            {
                instance.Append($"RIGHT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotEndsWith)
            {
                instance.Append($"RIGHT({field.Expression}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
            else
            {
                instance.Append($" {queryCondition.Operator.ToSql()} {paramName}");
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
        }

        private static void AppendParameterIfNecessary(DatabaseCommandBuilder instance,
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
