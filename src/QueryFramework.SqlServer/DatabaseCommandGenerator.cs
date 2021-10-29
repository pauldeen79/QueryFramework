using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class DatabaseCommandGenerator : IDatabaseCommandGenerator
    {
        public IDatabaseCommand Generate<TQuery>(TQuery query,
                                                 IQueryProcessorSettings settings,
                                                 IQueryFieldProvider fieldProvider,
                                                 bool countOnly)
            where TQuery : ISingleEntityQuery
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (fieldProvider == null)
            {
                throw new ArgumentNullException(nameof(fieldProvider));
            }

            var fieldSelectionQuery = query as IFieldSelectionQuery;
            var groupingQuery = query as IGroupingQuery;
            return new DatabaseCommandBuilder()
                .AppendPagingOuterQuery(query, settings, fieldProvider, countOnly)
                .AppendSelectAndDistinctClause(fieldSelectionQuery, countOnly)
                .AppendTopClause(query, settings, countOnly)
                .AppendCountOrSelectFields(query, settings, fieldProvider, countOnly)
                .AppendPagingPrefix(query, settings, fieldProvider, countOnly)
                .AppendFromClause()
                .AppendTableName(query, settings)
                .AppendWhereClause(query, settings, fieldProvider, out int paramCounter)
                .AppendGroupByClause(groupingQuery, settings, fieldProvider)
                .AppendHavingClause(groupingQuery, settings, fieldProvider, ref paramCounter)
                .AppendOrderByClause(query, settings, fieldProvider, countOnly)
                .AppendPagingSuffix(query, settings, countOnly)
                .AddQueryParameters(query)
                .Build();
        }
    }
}
