using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using CrossCutting.Data.Sql.Extensions;
using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core.Extensions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Executes the select command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="query">The query.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideLimit">The override limit.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name.</param>
        /// <param name="getAllFieldsDelegate">Optional delegate to get all field names.</param>
        public static IDataReader ExecuteSelectCommand(this IDbCommand command,
                                                       ISingleEntityQuery query,
                                                       string fields = null,
                                                       string tableName = null,
                                                       int? overrideLimit = null,
                                                       string defaultOrderBy = null,
                                                       string defaultWhere = null,
                                                       bool countOnly = false,
                                                       bool validateFieldNames = true,
                                                       Func<string, string> getFieldNameDelegate = null,
                                                       Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                       Func<IQueryExpression, bool> expressionValidationDelegate = null)
            => command.FillSelectCommand(query,
                                         tableName,
                                         fields,
                                         defaultOrderBy,
                                         defaultWhere,
                                         overrideLimit,
                                         countOnly,
                                         validateFieldNames,
                                         getFieldNameDelegate,
                                         getAllFieldsDelegate,
                                         expressionValidationDelegate).ExecuteReader();

        /// <summary>
        /// Fills the select command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="query">The query.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="overrideLimit">The override limit.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name.</param>
        /// <param name="getAllFieldsDelegate">Optional delegate to get all field names.</param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static IDbCommand FillSelectCommand(this IDbCommand command,
                                                   ISingleEntityQuery query,
                                                   string tableName = null,
                                                   string fields = null,
                                                   string defaultOrderBy = null,
                                                   string defaultWhere = null,
                                                   int? overrideLimit = null,
                                                   bool countOnly = false,
                                                   bool validateFieldNames = true,
                                                   Func<string, string> getFieldNameDelegate = null,
                                                   Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                   Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query is IDynamicQuery dynamicQuery)
            {
                query = dynamicQuery.Process();
            }

            query = query.Validate(validateFieldNames);
            var fieldSelectionQuery = query as IFieldSelectionQuery;
            var groupingQuery = query as IGroupingQuery;
            var stringBuilder = new StringBuilder()
                .AppendPagingOuterQuery
                (
                    query,
                    fields,
                    null,
                    countOnly,
                    validateFieldNames,
                    getFieldNameDelegate,
                    getAllFieldsDelegate,
                    expressionValidationDelegate
                )
                .AppendSelectAndDistinctClause(fieldSelectionQuery, countOnly)
                .AppendTopClause(query, overrideLimit, countOnly)
                .AppendCountOrSelectFields
                (
                    query,
                    fields,
                    countOnly,
                    validateFieldNames,
                    getFieldNameDelegate,
                    getAllFieldsDelegate,
                    expressionValidationDelegate
                )
                .AppendPagingPrefix
                (
                    query,
                    countOnly,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate
                )
                .AppendFromClause()
                .AppendTableName(query.GetTableName(tableName))
                .AppendWhereClause
                (
                    query.Conditions,
                    out int paramCounter,
                    command,
                    defaultWhere,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate
                )
                .AppendGroupByClause
                (
                    groupingQuery?.GroupByFields,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate
                )
                .AppendHavingClause
                (
                    groupingQuery?.HavingFields,
                    ref paramCounter,
                    command,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate
                )
                .AppendOrderByClause
                (
                    query.OrderByFields,
                    defaultOrderBy,
                    query.Offset,
                    countOnly,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate)
                .AppendPagingSuffix
                (
                    query,
                    overrideLimit,
                    countOnly
                );

            command.CommandText = stringBuilder.ToString();

            return command.AddQueryParameters(query);
        }

        /// <summary>Adds the query parameters.</summary>
        /// <param name="command">The command.</param>
        /// <param name="query">The query.</param>
        public static IDbCommand AddQueryParameters(this IDbCommand command, ISingleEntityQuery query)
        {
            if (query is IParameterizedQuery parameterizedQuery)
            {
                foreach (var parameter in parameterizedQuery.Parameters)
                {
                    command.AddParameter(parameter.Name, parameter.Value);
                }
            }

            return command;
        }

        /// <summary>Appends the query condition.</summary>
        /// <param name="command">The command.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="fieldPrefix">The field prefix.</param>
        /// <param name="paramCounter">The parameter counter.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="skipFirstCombination">if set to <c>true</c> [skip first combination].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <exception cref="ArgumentOutOfRangeException">queryCondition</exception>
        internal static int AppendQueryCondition(this IDbCommand command,
                                                 StringBuilder stringBuilder,
                                                 int paramCounter,
                                                 IQueryCondition queryCondition,
                                                 string fieldPrefix = null,
                                                 bool skipFirstCombination = false,
                                                 bool validateFieldNames = true,
                                                 Func<string, string> getFieldNameDelegate = null,
                                                 Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (paramCounter > 0 && !skipFirstCombination)
            {
                stringBuilder
                    .Append(" ")
                    .Append(queryCondition.Combination.ToSql())
                    .Append(" ");
            }

            if (queryCondition.OpenBracket)
            {
                stringBuilder.Append("(");
            }

            var customFieldName = getFieldNameDelegate == null
                ? null
                : getFieldNameDelegate(queryCondition.Field.FieldName);

            if (getFieldNameDelegate != null && customFieldName == null && validateFieldNames)
            {
                throw new ArgumentOutOfRangeException(nameof(queryCondition), string.Format("Query conditions contains unknown field [{0}]", queryCondition.Field.FieldName));
            }

            var field = queryCondition.Field.With(fieldName: customFieldName ?? queryCondition.Field.FieldName);

            if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(field))
            {
                throw new ArgumentOutOfRangeException(nameof(queryCondition), string.Format("Query conditions contains invalid expression [{0}]", field));
            }

            if (!queryCondition.Operator.In(QueryOperator.Contains,
                                            QueryOperator.NotContains,
                                            QueryOperator.EndsWith,
                                            QueryOperator.NotEndsWith,
                                            QueryOperator.StartsWith,
                                            QueryOperator.NotStartsWith)
               )
            {
                stringBuilder.Append(queryCondition.Operator.ToNot());

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    stringBuilder.Append("COALESCE(");
                }

                stringBuilder.Append(field.Expression.WithPrefix(fieldPrefix));

                if (queryCondition.Operator.In(QueryOperator.IsNullOrEmpty, QueryOperator.IsNotNullOrEmpty))
                {
                    stringBuilder.Append(",'')");
                }
            }

            var paramName = GetQueryParameterName(paramCounter, queryCondition.Value);

            AppendOperatorAndValue(command, stringBuilder, paramCounter, queryCondition, field, paramName, fieldPrefix);

            if (queryCondition.CloseBracket)
            {
                stringBuilder.Append(")");
            }

            return paramCounter + 1;
        }

        private static void AppendOperatorAndValue(IDbCommand command,
                                                   StringBuilder stringBuilder,
                                                   int paramCounter,
                                                   IQueryCondition queryCondition,
                                                   IQueryExpression field,
                                                   string paramName,
                                                   string fieldPrefix)
        {
            if (queryCondition.Operator == QueryOperator.IsNull)
            {
                stringBuilder.Append(" IS NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNull)
            {
                stringBuilder.Append(" IS NOT NULL");
            }
            else if (queryCondition.Operator == QueryOperator.IsNullOrEmpty)
            {
                stringBuilder.Append(" = ''");
            }
            else if (queryCondition.Operator == QueryOperator.IsNotNullOrEmpty)
            {
                stringBuilder.Append(" <> ''");
            }
            else if (queryCondition.Operator == QueryOperator.Contains)
            {
                stringBuilder.AppendFormat("CHARINDEX({1}, {0}) > 0", field.Expression.WithPrefix(fieldPrefix), paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotContains)
            {
                stringBuilder.AppendFormat("CHARINDEX({1}, {0}) = 0", field.Expression.WithPrefix(fieldPrefix), paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.StartsWith)
            {
                stringBuilder.AppendFormat("LEFT({0}, {1}) = {2}", field.Expression.WithPrefix(fieldPrefix), queryCondition.Value.ToStringWithDefault(string.Empty).Length, paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotStartsWith)
            {
                stringBuilder.AppendFormat("LEFT({0}, {1}) <> {2}", field.Expression.WithPrefix(fieldPrefix), queryCondition.Value.ToStringWithDefault(string.Empty).Length, paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.EndsWith)
            {
                stringBuilder.AppendFormat("RIGHT({0}, {1}) = {2}", field.Expression.WithPrefix(fieldPrefix), queryCondition.Value.ToStringWithDefault(string.Empty).Length, paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else if (queryCondition.Operator == QueryOperator.NotEndsWith)
            {
                stringBuilder.AppendFormat("RIGHT({0}, {1}) <> {2}", field.Expression.WithPrefix(fieldPrefix), queryCondition.Value.ToStringWithDefault(string.Empty).Length, paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
            else
            {
                stringBuilder.AppendFormat(" {0} {1}", queryCondition.Operator.ToSql(), paramName);
                AppendParameterIfNecessary(command, paramCounter, queryCondition);
            }
        }

        private static void AppendParameterIfNecessary(IDbCommand command, int paramCounter, IQueryCondition queryCondition)
        {
            if (!(queryCondition.Value is IQueryParameterValue))
            {
                command.AddParameter(string.Format("p{0}", paramCounter),
                                     queryCondition.Value is KeyValuePair<string, object> keyValuePair
                                        ? keyValuePair.Value
                                        : queryCondition.Value);
            }
        }

        private static string GetQueryParameterName(int paramCounter, object value)
        {
            if (value is KeyValuePair<string, object> keyValuePair)
            {
                return "@" + keyValuePair.Key;
            }

            if (value is IQueryParameterValue queryParameterValue)
            {
                return "@" + queryParameterValue.Name;
            }

            return "@" + string.Format("p{0}", paramCounter);
        }
    }
}
