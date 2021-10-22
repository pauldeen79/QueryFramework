using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>Appends the select fields.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="skipFields">The skip fields.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="fields">The fields.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static StringBuilder AppendSelectFields(this StringBuilder instance,
                                                       ISingleEntityQuery query,
                                                       string[] skipFields,
                                                       bool countOnly,
                                                       string fields = null,
                                                       bool validateFieldNames = true,
                                                       Func<string, string> getFieldNameDelegate = null,
                                                       Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                       Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            var fieldSelectionQuery = query as IFieldSelectionQuery;
            if (countOnly)
            {
                instance.Append("COUNT(*)");
            }
            else if (fieldSelectionQuery?.GetAllFields != false)
            {
                AppendSelectFieldsForAllFields(instance, fields, getAllFieldsDelegate);
            }
            else
            {
                AppendSelectFieldsForSpecifiedFields(instance, skipFields, validateFieldNames, getFieldNameDelegate, expressionValidationDelegate, fieldSelectionQuery);
            }

            return instance;
        }

        private static void AppendSelectFieldsForAllFields(StringBuilder instance, string fields, Func<IEnumerable<string>> getAllFieldsDelegate)
        {
            if (getAllFieldsDelegate == null)
            {
                instance.Append(fields.WhenNullOrWhitespace("*"));
            }
            else
            {
                var paramCounter = 0;
                foreach (var fieldName in getAllFieldsDelegate())
                {
                    if (paramCounter > 0)
                    {
                        instance.Append(", ");
                    }

                    instance.Append(fieldName);
                    paramCounter++;
                }
            }
        }

        private static void AppendSelectFieldsForSpecifiedFields(StringBuilder instance, string[] skipFields, bool validateFieldNames, Func<string, string> getFieldNameDelegate, Func<IQueryExpression, bool> expressionValidationDelegate, IFieldSelectionQuery fieldSelectionQuery)
        {
            var paramCounter = 0;
            foreach (var expression in fieldSelectionQuery.GetSelectFields(skipFields))
            {
                if (paramCounter > 0)
                {
                    instance.Append(", ");
                }

                var correctedExpression = expression;
                if (getFieldNameDelegate != null)
                {
                    var correctedFieldName = getFieldNameDelegate(expression.FieldName);
                    if (correctedFieldName == null && validateFieldNames)
                    {
                        throw new InvalidOperationException(string.Format("Query fields contains unknown field in expression [{0}]", expression));
                    }

                    if (correctedFieldName != null)
                    {
                        //Note that for now, we assume that custom expressions don't override field name logic, only expression logic
                        correctedExpression = new QueryExpression(correctedFieldName, expression.GetRawExpression());
                    }
                }

                if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(correctedExpression))
                {
                    throw new InvalidOperationException(string.Format("Query fields contains invalid expression [{0}]", expression));
                }

                instance.Append(correctedExpression.Expression);
                paramCounter++;
            }
        }

        /// <summary>
        /// Appends the where clause.
        /// </summary>
        /// <param name="instance">The string builder.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="paramCounter">New parameter counter.</param>
        /// <param name="command">The command.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name</param>
        /// <exception cref="ArgumentOutOfRangeException">conditions</exception>
        public static StringBuilder AppendWhereClause(this StringBuilder instance,
                                                      IEnumerable<IQueryCondition> conditions,
                                                      out int paramCounter,
                                                      IDbCommand command,
                                                      string defaultWhere = null,
                                                      bool validateFieldNames = true,
                                                      Func<string, string> getFieldNameDelegate = null,
                                                      Func<IQueryExpression, bool> expressionValidationDelegate = null)
            => instance.AppendWhereClause
            (
                conditions,
                0,
                out paramCounter,
                command,
                defaultWhere,
                validateFieldNames,
                getFieldNameDelegate,
                expressionValidationDelegate
            );

        /// <summary>
        /// Appends the where clause.
        /// </summary>
        /// <param name="instance">The string builder.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="parameterCount">Number of parameters.</param>
        /// <param name="paramCounter">New parameter counter.</param>
        /// <param name="command">The command.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name</param>
        /// <exception cref="ArgumentOutOfRangeException">conditions</exception>
        private static StringBuilder AppendWhereClause(this StringBuilder instance,
                                                      IEnumerable<IQueryCondition> conditions,
                                                      int parameterCount,
                                                      out int paramCounter,
                                                      IDbCommand command,
                                                      string defaultWhere = null,
                                                      bool validateFieldNames = true,
                                                      Func<string, string> getFieldNameDelegate = null,
                                                      Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if ((conditions?.Any() != true)
                && string.IsNullOrEmpty(defaultWhere))
            {
                paramCounter = 0;
                return instance;
            }

            instance.Append(" WHERE ");

            if (!string.IsNullOrEmpty(defaultWhere))
            {
                instance.Append(defaultWhere);

                if (conditions.Any())
                {
                    instance.Append(" AND ");
                }
            }

            paramCounter = parameterCount; //When parameters length = 2, this means 0 and 1. Then we have to start with 2.

            foreach (var queryCondition in conditions.NotNull())
            {
                paramCounter = command.AppendQueryCondition
                (
                    instance,
                    paramCounter,
                    queryCondition,
                    null,
                    validateFieldNames: validateFieldNames,
                    getFieldNameDelegate: getFieldNameDelegate,
                    expressionValidationDelegate: expressionValidationDelegate
                );
            }

            return instance;
        }

        /// <summary>
        /// Appends the order by clause.
        /// </summary>
        /// <param name="instance">The string builder.</param>
        /// <param name="orderByFields">The order by fields.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name</param>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static StringBuilder AppendOrderByClause(this StringBuilder instance,
                                                        IEnumerable<IQuerySortOrder> orderByFields,
                                                        string defaultOrderBy,
                                                        int? offset,
                                                        bool countOnly,
                                                        bool validateFieldNames = true,
                                                        Func<string, string> getFieldNameDelegate = null,
                                                        Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (offset.HasValue && offset.Value >= 0)
            {
                //do not use order by (this will be taken care of by the row_number function)
                return instance;
            }
            else if (countOnly)
            {
                //do not use order by, only count the number of records
                return instance;
            }
            else if (orderByFields?.Any() == true
                || !string.IsNullOrEmpty(defaultOrderBy))
            {
                instance.Append(" ORDER BY ");
                var fieldCounter = 0;
                foreach (var querySortOrder in orderByFields.NotNull())
                {
                    if (fieldCounter > 0)
                    {
                        instance.Append(", ");
                    }

                    if (getFieldNameDelegate != null)
                    {
                        AppendOrderByForDynamicFieldNames(instance, validateFieldNames, getFieldNameDelegate, expressionValidationDelegate, querySortOrder);
                    }
                    else
                    {
                        AppendOrderByForStaticFieldNames(instance, expressionValidationDelegate, querySortOrder);
                    }
                    fieldCounter++;
                }

                if (fieldCounter == 0 && !string.IsNullOrEmpty(defaultOrderBy))
                {
                    instance.Append(defaultOrderBy);
                }
            }

            return instance;
        }

        private static void AppendOrderByForStaticFieldNames(StringBuilder instance, Func<IQueryExpression, bool> expressionValidationDelegate, IQuerySortOrder querySortOrder)
        {
            if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(querySortOrder.Field))
            {
                throw new InvalidOperationException(string.Format("Query order by fields contains invalid expression [{0}]", querySortOrder.Field));
            }
            instance
                .Append(querySortOrder.Field.Expression)
                .Append(" ")
                .Append(querySortOrder.ToSql());
        }

        private static void AppendOrderByForDynamicFieldNames(StringBuilder instance, bool validateFieldNames, Func<string, string> getFieldNameDelegate, Func<IQueryExpression, bool> expressionValidationDelegate, IQuerySortOrder querySortOrder)
        {
            var newFieldName = getFieldNameDelegate(querySortOrder.Field.FieldName);
            if (newFieldName == null && validateFieldNames)
            {
                throw new InvalidOperationException(string.Format("Query order by fields contains unknown field [{0}]", querySortOrder.Field.FieldName));
            }
            var newQuerySortOrder = new QuerySortOrder(newFieldName ?? querySortOrder.Field.FieldName, querySortOrder.Order);
            if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(newQuerySortOrder.Field))
            {
                throw new InvalidOperationException(string.Format("Query order by fields contains invalid expression [{0}]", newQuerySortOrder.Field));
            }
            instance
                .Append(newQuerySortOrder.Field.Expression)
                .Append(" ")
                .Append(newQuerySortOrder.ToSql());
        }

        /// <summary>
        /// Appends the group by clause.
        /// </summary>
        /// <param name="instance">The string builder.</param>
        /// <param name="groupByFields">The group by fields.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name</param>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static StringBuilder AppendGroupByClause
        (
            this StringBuilder instance,
            IEnumerable<IQueryExpression> groupByFields,
            bool validateFieldNames = true,
            Func<string, string> getFieldNameDelegate = null,
            Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (groupByFields?.Any() != true)
            {
                return instance;
            }

            instance.Append(" GROUP BY ");
            var fieldCounter = 0;
            foreach (var groupBy in groupByFields)
            {
                if (fieldCounter > 0)
                {
                    instance.Append(", ");
                }

                if (getFieldNameDelegate != null)
                {
                    AppendGroupByForDynamicFieldNames(instance, validateFieldNames, getFieldNameDelegate, expressionValidationDelegate, groupBy);
                }
                else
                {
                    AppendGroupByForStaticFieldNames(instance, expressionValidationDelegate, groupBy);
                }
                fieldCounter++;
            }

            return instance;
        }

        private static void AppendGroupByForStaticFieldNames(StringBuilder instance, Func<IQueryExpression, bool> expressionValidationDelegate, IQueryExpression groupBy)
        {
            if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(groupBy))
            {
                throw new InvalidOperationException(string.Format("Query group by fields contains invalid expression [{0}]", groupBy));
            }
            instance.Append(groupBy.Expression);
        }

        private static void AppendGroupByForDynamicFieldNames(StringBuilder instance, bool validateFieldNames, Func<string, string> getFieldNameDelegate, Func<IQueryExpression, bool> expressionValidationDelegate, IQueryExpression groupBy)
        {
            var correctedFieldName = getFieldNameDelegate(groupBy.FieldName);
            if (correctedFieldName == null && validateFieldNames)
            {
                throw new InvalidOperationException(string.Format("Query group by fields contains unknown field [{0}]", groupBy.FieldName));
            }
            var corrected = new QueryExpression(correctedFieldName ?? groupBy.FieldName, groupBy.GetRawExpression());
            if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(corrected))
            {
                throw new InvalidOperationException(string.Format("Query group by fields contains invalid expression [{0}]", corrected));
            }
            instance.Append(corrected.Expression);
        }

        /// <summary>
        /// Appends the having clause.
        /// </summary>
        /// <param name="instance">The string builder.</param>
        /// <param name="havingFields">The having conditions.</param>
        /// <param name="paramCounter">Current parameter counter</param>
        /// <param name="command">The command to append parameters to.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">Optional delegate to get custom field name</param>
        /// <returns>New parameter counter.</returns>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static StringBuilder AppendHavingClause(this StringBuilder instance,
                                                       IEnumerable<IQueryCondition> havingFields,
                                                       ref int paramCounter,
                                                       IDbCommand command,
                                                       bool validateFieldNames = true,
                                                       Func<string, string> getFieldNameDelegate = null,
                                                       Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (havingFields?.Any() != true)
            {
                return instance;
            }

            instance.Append(" HAVING ");
            var fieldCounter = 0;
            foreach (var having in havingFields)
            {
                paramCounter = command.AppendQueryCondition
                (
                    instance,
                    paramCounter,
                    having,
                    string.Empty,
                    fieldCounter == 0,
                    validateFieldNames,
                    getFieldNameDelegate,
                    expressionValidationDelegate
                );
                fieldCounter++;
            }

            return instance;
        }

        /// <summary>Appends the paging outer query.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="skipFields">The skip fields.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        public static StringBuilder AppendPagingOuterQuery(this StringBuilder instance,
                                                           ISingleEntityQuery query,
                                                           string fields,
                                                           string[] skipFields,
                                                           bool countOnly,
                                                           bool validateFieldNames = true,
                                                           Func<string, string> getFieldNameDelegate = null,
                                                           Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                           Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.Offset.HasValue && query.Offset.Value >= 0 && !countOnly)
            {
                return instance
                    .Append("SELECT ")
                    .AppendSelectFields
                    (
                        query,
                        skipFields,
                        countOnly,
                        fields,
                        validateFieldNames,
                        getFieldNameDelegate,
                        getAllFieldsDelegate,
                        expressionValidationDelegate
                    )
                    .AppendFromClause()
                    .Append("(");
            }

            return instance;
        }

        /// <summary>Appends from clause.</summary>
        /// <param name="instance">The instance.</param>
        public static StringBuilder AppendFromClause(this StringBuilder instance)
            => instance.Append(" FROM ");

        public static StringBuilder AppendSelectAndDistinctClause(this StringBuilder instance,
                                                                  IFieldSelectionQuery fieldSelectionQuery,
                                                                  bool countOnly)
        {
            instance.Append("SELECT ");
            if (fieldSelectionQuery?.Distinct == true && !countOnly)
            {
                instance.Append("DISTINCT ");
            }

            return instance;
        }

        /// <summary>Appends the top clause.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="overrideLimit">The override limit.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        public static StringBuilder AppendTopClause(this StringBuilder instance,
                                                    ISingleEntityQuery query,
                                                    int? overrideLimit,
                                                    bool countOnly)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if ((query.Offset == null || query.Offset.Value <= 0 || query.OrderByFields.Count == 0)
                && ((query.Limit.HasValue && query.Limit.Value >= 0) || (overrideLimit.HasValue && overrideLimit.Value >= 0))
                && !countOnly)
            {
                return instance.AppendFormat("TOP {0} ", query.Limit.DetermineLimit(overrideLimit));
            }

            return instance;
        }

        /// <summary>Appends the count(*) or select fields.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        public static StringBuilder AppendCountOrSelectFields(this StringBuilder instance,
                                                              ISingleEntityQuery query,
                                                              string fields,
                                                              bool countOnly,
                                                              bool validateFieldNames = true,
                                                              Func<string, string> getFieldNameDelegate = null,
                                                              Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                              Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (countOnly)
            {
                return instance.Append("COUNT(*)");
            }
            else
            {
                return instance.AppendSelectFields(query, 
                                                   null,
                                                   countOnly,
                                                   fields,
                                                   validateFieldNames,
                                                   getFieldNameDelegate,
                                                   getAllFieldsDelegate,
                                                   expressionValidationDelegate);
            }
        }

        /// <summary>Appends the name of the table.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <exception cref="ArgumentOutOfRangeException">tableName - Table name is required</exception>
        public static StringBuilder AppendTableName(this StringBuilder instance, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentOutOfRangeException(nameof(tableName), "Table name is required");
            }

            return instance.Append(tableName);
        }

        /// <summary>Appends the paging prefix.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <exception cref="ArgumentOutOfRangeException">query</exception>
        public static StringBuilder AppendPagingPrefix(this StringBuilder instance,
                                                       ISingleEntityQuery query,
                                                       bool countOnly,
                                                       bool validateFieldNames = true,
                                                       Func<string, string> getFieldNameDelegate = null,
                                                       Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

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

                    var fieldName = GetOrderByFieldName(validateFieldNames, getFieldNameDelegate, querySortOrder);
                    var corrected = new QuerySortOrder(new QueryExpression(fieldName ?? querySortOrder.Field.FieldName, querySortOrder.Field.GetRawExpression()), querySortOrder.Order);
                    if (expressionValidationDelegate != null && !expressionValidationDelegate.Invoke(corrected.Field))
                    {
                        throw new InvalidOperationException(string.Format("Query OrderByFields contains invalid expression [{0}]", corrected.Field));
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

                return instance.AppendFormat(", ROW_NUMBER() OVER (ORDER BY {0}) as sq_row_number", orderByBuilder);
            }

            return instance;
        }

        private static string GetOrderByFieldName(bool validateFieldNames, Func<string, string> getFieldNameDelegate, IQuerySortOrder querySortOrder)
        {
            var fieldName = getFieldNameDelegate == null
                ? null
                : getFieldNameDelegate(querySortOrder.Field.FieldName);

            if (getFieldNameDelegate != null
                && fieldName == null
                && validateFieldNames)
            {
                throw new InvalidOperationException(string.Format("Query OrderByFields contains unknown field [{0}]", querySortOrder.Field));
            }

            return fieldName;
        }

        /// <summary>Appends the paging suffix.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="overrideLimit">The override limit.</param>
        /// <param name="countOnly">if set to <c>true</c> [count only].</param>
        public static StringBuilder AppendPagingSuffix(this StringBuilder instance,
                                                       ISingleEntityQuery query,
                                                       int? overrideLimit,
                                                       bool countOnly)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.Offset.HasValue && query.Offset.Value > 0 && !countOnly)
            {
                if (query.Limit.DetermineLimit(overrideLimit) > 0)
                {
                    return instance.AppendFormat(") sq WHERE sq.sq_row_number BETWEEN {0} and {1};", query.Offset.Value + 1, query.Offset.Value + query.Limit.DetermineLimit(overrideLimit));
                }
                else
                {
                    return instance.AppendFormat(") sq WHERE sq.sq_row_number > {0};", query.Offset.Value);
                }
            }

            return instance;
        }
    }
}
