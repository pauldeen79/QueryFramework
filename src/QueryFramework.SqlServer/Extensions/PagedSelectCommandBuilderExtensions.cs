﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Builders;
using CrossCutting.Data.Sql.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    internal static class PagedSelectCommandBuilderExtensions
    {
        internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                         IPagedDatabaseEntityRetrieverSettings settings,
                                                         IQueryFieldProvider fieldProvider,
                                                         IFieldSelectionQuery? fieldSelectionQuery)
            => fieldSelectionQuery == null || fieldSelectionQuery.GetAllFields
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
                instance.Select(string.Join(", ", allFields.Select(x => fieldProvider.GetDatabaseFieldName(x)).OfType<string>()));
            }

            return instance;
        }

        private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                      IFieldSelectionQuery fieldSelectionQuery,
                                                                                      IQueryFieldProvider fieldProvider)
        {
            var paramCounter = 0;
            foreach (var expression in fieldSelectionQuery.Fields)
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
                var correctedExpression = new QueryExpression(correctedFieldName, expression.Function);

                instance.Select(correctedExpression.GetSqlExpression());
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
            if (!query.Conditions.Any() && string.IsNullOrEmpty(settings.DefaultWhere))
            {
                paramCounter = 0;
                return instance;
            }

            paramCounter = 0;

            if (!string.IsNullOrEmpty(settings.DefaultWhere))
            {
                instance.Where(settings.DefaultWhere);
            }

            foreach (var queryCondition in query.Conditions)
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
            if (groupingQuery == null || !groupingQuery.GroupByFields.Any())
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
                var corrected = new QueryExpression(correctedFieldName, groupBy.Function);
                instance.GroupBy(corrected.GetSqlExpression());
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
            if (groupingQuery == null || !groupingQuery.HavingFields.Any())
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
            else if (query.OrderByFields.Any() || !string.IsNullOrEmpty(settings.DefaultOrderBy))
            {
                return instance.AppendOrderBy(query.OrderByFields, settings, fieldProvider);
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
            foreach (var querySortOrder in orderByFields.Select((field, index) => new { SortOrder = field, Index = index }))
            {
                if (querySortOrder.Index > 0)
                {
                    instance.OrderBy(", ");
                }

                var newFieldName = fieldProvider.GetDatabaseFieldName(querySortOrder.SortOrder.Field.FieldName);
                if (newFieldName == null)
                {
                    throw new InvalidOperationException(string.Format("Query order by fields contains unknown field [{0}]", querySortOrder.SortOrder.Field.FieldName));
                }
                var newQuerySortOrder = new QuerySortOrder(new QueryExpression(newFieldName, null), querySortOrder.SortOrder.Order);
                instance.OrderBy($"{newQuerySortOrder.Field.GetSqlExpression()} {newQuerySortOrder.ToSql()}");
            }

            if (!orderByFields.Any() && !string.IsNullOrEmpty(settings.DefaultOrderBy))
            {
                instance.OrderBy(settings.DefaultOrderBy);
            }

            return instance;
        }

        internal static PagedSelectCommandBuilder WithParameters(this PagedSelectCommandBuilder instance,
                                                                 IParameterizedQuery? parameterizedQuery)
            => parameterizedQuery == null
                ? instance
                : parameterizedQuery.Parameters.Aggregate(instance, (acc, parameter) => acc.AppendParameter(parameter.Name, parameter.Value));

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

            var field = new QueryExpression(customFieldName, queryCondition.Field.Function);

            if (!queryCondition.Operator.In(QueryOperator.Contains,
                                            QueryOperator.NotContains,
                                            QueryOperator.EndsWith,
                                            QueryOperator.NotEndsWith,
                                            QueryOperator.StartsWith,
                                            QueryOperator.NotStartsWith))
            {
                builder.Append(queryCondition.Operator.ToNot());

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    builder.Append("COALESCE(");
                }

                builder.Append(field.GetSqlExpression());

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
            var sqlToAppend = queryCondition.Operator switch
            {
                QueryOperator.IsNull => " IS NULL",
                QueryOperator.IsNotNull => " IS NOT NULL",
                QueryOperator.IsNullOrEmpty => " = ''",
                QueryOperator.IsNotNullOrEmpty => " <> ''",
                QueryOperator.Contains => $"CHARINDEX({paramName}, {field.GetSqlExpression()}) > 0",
                QueryOperator.NotContains => $"CHARINDEX({paramName}, {field.GetSqlExpression()}) = 0",
                QueryOperator.StartsWith => $"LEFT({field.GetSqlExpression()}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}",
                QueryOperator.NotStartsWith => $"LEFT({field.GetSqlExpression()}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}",
                QueryOperator.EndsWith => $"RIGHT({field.GetSqlExpression()}, {queryCondition.Value.ToStringWithNullCheck().Length}) = {paramName}",
                QueryOperator.NotEndsWith => $"RIGHT({field.GetSqlExpression()}, {queryCondition.Value.ToStringWithNullCheck().Length}) <> {paramName}",
                _ => $" {queryCondition.Operator.ToSql()} {paramName}"
            };

            builder.Append(sqlToAppend);

            if (!queryCondition.Operator.In(QueryOperator.IsNull,
                                            QueryOperator.IsNotNull,
                                            QueryOperator.IsNullOrEmpty,
                                            QueryOperator.IsNotNullOrEmpty))
            {
                AppendParameterIfNecessary(instance, paramCounter, queryCondition);
            }
        }

        private static void AppendParameterIfNecessary(PagedSelectCommandBuilder instance,
                                                       int paramCounter,
                                                       IQueryCondition queryCondition)
        {
            if (queryCondition.Value is IQueryParameterValue)
            {
                return;
            }

            instance.AppendParameter(string.Format("p{0}", paramCounter),
                                     queryCondition.Value is KeyValuePair<string, object> keyValuePair
                                         ? keyValuePair.Value
                                         : queryCondition.Value ?? new object());
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
