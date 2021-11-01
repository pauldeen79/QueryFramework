using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class DatabaseCommandGenerator : IDatabaseCommandGenerator
    {
        private readonly IQueryFieldProvider _fieldProvider;

        public DatabaseCommandGenerator(IQueryFieldProvider fieldProvider)
        {
            _fieldProvider = fieldProvider;
        }

        public IDatabaseCommand Generate<TQuery>(TQuery query,
                                                 IQueryProcessorSettings settings,
                                                 bool countOnly)
            where TQuery : ISingleEntityQuery
        {
            var fieldSelectionQuery = query as IFieldSelectionQuery;
            var groupingQuery = query as IGroupingQuery;
            return new DatabaseCommandBuilder()
                .AppendPagingOuterQuery(query, settings, _fieldProvider, countOnly)
                .AppendSelectAndDistinctClause(fieldSelectionQuery, countOnly)
                .AppendTopClause(query, settings, countOnly)
                .AppendCountOrSelectFields(query, settings, _fieldProvider, countOnly)
                .AppendPagingPrefix(query, settings, _fieldProvider, countOnly)
                .AppendFromClause()
                .AppendTableName(query, settings)
                .AppendWhereClause(query, settings, _fieldProvider, out int paramCounter)
                .AppendGroupByClause(groupingQuery, settings, _fieldProvider)
                .AppendHavingClause(groupingQuery, settings, _fieldProvider, ref paramCounter)
                .AppendOrderByClause(query, settings, _fieldProvider, countOnly)
                .AppendPagingSuffix(query, settings, countOnly)
                .AddQueryParameters(query)
                .Build();
        }
    }
}
