using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>
        where TQuery : ISingleEntityQuery
        where TResult : class
    {
        private IDatabaseEntityRetriever<TResult> Retriever { get; }
        private IQueryProcessorSettings Settings { get; }
        private IPagedDatabaseCommandProvider<TQuery> DatabaseCommandProvider { get; }

        public QueryProcessor(IDatabaseEntityRetriever<TResult> retriever,
                              IQueryProcessorSettings settings,
                              IPagedDatabaseCommandProvider<TQuery> databaseCommandProvider)
        {
            Retriever = retriever;
            Settings = settings;
            DatabaseCommandProvider = databaseCommandProvider;
        }

        public IReadOnlyCollection<TResult> FindMany(TQuery query)
            => Retriever.FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

        public TResult? FindOne(TQuery query)
            => Retriever.FindOne(GenerateCommand(query, 1).DataCommand);

        public IPagedResult<TResult> FindPaged(TQuery query)
            => Retriever.FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

        private IPagedDatabaseCommand GenerateCommand(TQuery query, int limit)
            => DatabaseCommandProvider.CreatePaged
            (
                query.Validate(Settings.ValidateFieldNames),
                DatabaseOperation.Select,
                query.Offset.GetValueOrDefault(),
                limit
            );
    }
}
