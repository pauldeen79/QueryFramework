using System;
using System.Collections.Generic;
using System.Data;
using CrossCutting.Data.Sql.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;

namespace QueryFramework.SqlServer.Extensions
{
    public static class DbConnectionExtensions
    {
        /// <summary>Executes the query on this connection using the specified parameters.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="query">The query.</param>
        /// <param name="mapDelegate">The map delegate.</param>
        /// <param name="defaultOverrideLimit">The default override limit.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="selectFields">The select fields.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        /// <param name="finalizeAction">The finalize action.</param>
        public static IQueryResult<T> Query<T>(this IDbConnection connection,
                                               ISingleEntityQuery query,
                                               Func<IDataReader, T> mapDelegate,
                                               int? defaultOverrideLimit = null,
                                               string tableName = null,
                                               string selectFields = null,
                                               string defaultOrderBy = null,
                                               string defaultWhere = null,
                                               Func<IReadOnlyCollection<T>, Exception, IReadOnlyCollection<T>> finalizeAction = null,
                                               bool validateFieldNames = true,
                                               Func<string, string> getFieldNameDelegate = null,
                                               Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                               Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var items = new List<T>();

            try
            {
                var totalRecordCount = -1;
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                using (var reader = cmd.ExecuteSelectCommand(query,
                                                             selectFields,
                                                             tableName,
                                                             defaultOverrideLimit,
                                                             defaultOrderBy,
                                                             defaultWhere,
                                                             false,
                                                             validateFieldNames,
                                                             getFieldNameDelegate,
                                                             getAllFieldsDelegate,
                                                             expressionValidationDelegate))
                {
                    items.AddRange(reader.FindMany(mapDelegate));
                }

                if (items.Count > 0
                    && items.Count == query.Limit.DetermineLimit(defaultOverrideLimit))
                {
                    using (var recordCountCmd = connection.CreateCommand())
                    {
                        totalRecordCount = (int)recordCountCmd.FillSelectCommand
                        (
                            query,
                            tableName,
                            selectFields,
                            defaultOrderBy,
                            defaultWhere,
                            defaultOverrideLimit,
                            true,
                            validateFieldNames,
                            getFieldNameDelegate,
                            getAllFieldsDelegate,
                            expressionValidationDelegate
                        ).ExecuteScalar();
                    }
                }
                else
                {
                    totalRecordCount = items.Count;
                }

                return new QueryResult<T>(finalizeAction == null
                                            ? items
                                            : finalizeAction.Invoke(items, null),
                                          totalRecordCount);
            }
            catch (Exception ex)
            {
                finalizeAction?.Invoke(items, ex);
                throw;
            }
        }

        /// <summary>Executes the query on this connection using the specified parameters.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="query">The query.</param>
        /// <param name="mapDelegate">The map delegate.</param>
        /// <param name="defaultOverrideLimit">The default override limit.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="selectFields">The select fields.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        /// <param name="finalizeAction">The finalize action.</param>
        public static T FindOne<T>(this IDbConnection connection,
                                   ISingleEntityQuery query,
                                   Func<IDataReader, T> mapDelegate,
                                   int? defaultOverrideLimit = null,
                                   string tableName = null,
                                   string selectFields = null,
                                   string defaultOrderBy = null,
                                   string defaultWhere = null,
                                   Func<T, Exception, T> finalizeAction = null,
                                   bool validateFieldNames = true,
                                   Func<string, string> getFieldNameDelegate = null,
                                   Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                   Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            T result = default;

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                using (var reader = cmd.ExecuteSelectCommand(query,
                                                             selectFields,
                                                             tableName,
                                                             defaultOverrideLimit,
                                                             defaultOrderBy,
                                                             defaultWhere,
                                                             false,
                                                             validateFieldNames,
                                                             getFieldNameDelegate,
                                                             getAllFieldsDelegate,
                                                             expressionValidationDelegate))
                {
                    result = reader.FindOne(mapDelegate);
                }

                return finalizeAction == null
                    ? result
                    : finalizeAction.Invoke(result, null);
            }
            catch (Exception ex)
            {
                finalizeAction?.Invoke(result, ex);
                throw;
            }
        }

        /// <summary>Executes the query on this connection using the specified parameters.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="query">The query.</param>
        /// <param name="mapDelegate">The map delegate.</param>
        /// <param name="defaultOverrideLimit">The default override limit.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="selectFields">The select fields.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <param name="defaultWhere">The default where.</param>
        /// <param name="validateFieldNames">if set to <c>true</c> [validate field names].</param>
        /// <param name="getFieldNameDelegate">The get field name delegate.</param>
        /// <param name="getAllFieldsDelegate">The get all fields delegate.</param>
        /// <param name="finalizeAction">The finalize action.</param>
        public static IReadOnlyCollection<T> FindMany<T>(this IDbConnection connection,
                                                         ISingleEntityQuery query,
                                                         Func<IDataReader, T> mapDelegate,
                                                         int? defaultOverrideLimit = null,
                                                         string tableName = null,
                                                         string selectFields = null,
                                                         string defaultOrderBy = null,
                                                         string defaultWhere = null,
                                                         Func<IReadOnlyCollection<T>, Exception, IReadOnlyCollection<T>> finalizeAction = null,
                                                         bool validateFieldNames = true,
                                                         Func<string, string> getFieldNameDelegate = null,
                                                         Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                                         Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var items = new List<T>();

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                using (var reader = cmd.ExecuteSelectCommand(query,
                                                             selectFields,
                                                             tableName,
                                                             defaultOverrideLimit,
                                                             defaultOrderBy,
                                                             defaultWhere,
                                                             false,
                                                             validateFieldNames,
                                                             getFieldNameDelegate,
                                                             getAllFieldsDelegate,
                                                             expressionValidationDelegate))
                {
                    items.AddRange(reader.FindMany(mapDelegate));
                }

                return finalizeAction == null
                    ? items
                    : finalizeAction.Invoke(items, null);
            }
            catch (Exception ex)
            {
                finalizeAction?.Invoke(items, ex);
                throw;
            }
        }
    }
}
