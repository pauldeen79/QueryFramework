using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Core.Commands;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class QueryPagedDatabaseCommandProvider<TQuery> : IPagedDatabaseCommandProvider<TQuery>
        where TQuery : ISingleEntityQuery, new()
    {
        private IQueryFieldProvider FieldProvider { get; }
        private IQueryProcessorSettings Settings { get; }

        public QueryPagedDatabaseCommandProvider(IQueryFieldProvider fieldProvider, IQueryProcessorSettings settings)
        {
            FieldProvider = fieldProvider;
            Settings = settings;
        }

        public IDatabaseCommand Create(TQuery source, DatabaseOperation operation)
            => CreatePaged(source, operation, 0, 0).DataCommand;

        public IDatabaseCommand Create(DatabaseOperation operation)
            => Create(new TQuery(), operation);

        public IPagedDatabaseCommand CreatePaged(TQuery source, DatabaseOperation operation, int offset, int pageSize)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
            }

            var fieldSelectionQuery = source as IFieldSelectionQuery;
            var groupingQuery = source as IGroupingQuery;
            return new PagedDatabaseCommand(CreateCommand(source, fieldSelectionQuery, groupingQuery, offset, pageSize, false),
                                            CreateCommand(source, fieldSelectionQuery, groupingQuery, offset, pageSize, true),
                                            offset,
                                            pageSize);
            
        }

        public IPagedDatabaseCommand CreatePaged(DatabaseOperation operation, int offset, int pageSize)
            => CreatePaged(new TQuery(), operation, offset, pageSize);

        private IDatabaseCommand CreateCommand(TQuery source,
                                               IFieldSelectionQuery? fieldSelectionQuery,
                                               IGroupingQuery? groupingQuery,
                                               int offset,
                                               int limit,
                                               bool countOnly)
        {
            var settings = Settings.WithPageInfo(limit, offset);
            return new DatabaseCommandBuilder()
                .AppendPagingOuterQuery(source, settings, FieldProvider, countOnly)
                .AppendSelectAndDistinctClause(fieldSelectionQuery, countOnly)
                .AppendTopClause(source, settings, countOnly)
                .AppendCountOrSelectFields(source, settings, FieldProvider, countOnly)
                .AppendPagingPrefix(source, settings, FieldProvider, countOnly)
                .AppendFromClause()
                .AppendTableName(source, settings)
                .AppendWhereClause(source, settings, FieldProvider, out int paramCounter)
                .AppendGroupByClause(groupingQuery, settings, FieldProvider)
                .AppendHavingClause(groupingQuery, settings, FieldProvider, ref paramCounter)
                .AppendOrderByClause(source, settings, FieldProvider, countOnly)
                .AppendPagingSuffix(source, settings, countOnly)
                .AddQueryParameters(source)
                .Build();
        }
    }
}
