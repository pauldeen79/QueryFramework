using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>, IPagedDatabaseCommandProvider<TQuery>
        where TQuery : ISingleEntityQuery, new()
        where TResult : class
    {
        private readonly IDatabaseEntityRetriever<TResult> _retriever;
        private readonly IQueryProcessorSettings _settings;
        private readonly IDatabaseCommandGenerator _databaseCommandGenerator;

        public QueryProcessor(IDatabaseEntityRetriever<TResult> retriever,
                              IQueryProcessorSettings settings,
                              IDatabaseCommandGenerator databaseCommandGenerator)
        {
            _retriever = retriever;
            _settings = settings;
            _databaseCommandGenerator = databaseCommandGenerator;
        }

        public IReadOnlyCollection<TResult> FindMany(TQuery query)
            => _retriever.FindMany(GenerateCommand(query, false));

        public TResult? FindOne(TQuery query)
            => _retriever.FindOne(GenerateCommand(query, false));

        public IPagedResult<TResult> FindPaged(TQuery query)
            => _retriever.FindPaged(new PagedDatabaseCommand(GenerateCommand(query, false),
                                                             GenerateCommand(query, true),
                                                             query.Offset.GetValueOrDefault(),
                                                             query.Limit.GetValueOrDefault()));

        IDatabaseCommand IDatabaseCommandProvider<TQuery>.Create(TQuery source, DatabaseOperation operation)
            => GenerateCommand(source, false);

        IDatabaseCommand IDatabaseCommandProvider.Create(DatabaseOperation operation)
            => GenerateCommand(new TQuery(), false);

        IPagedDatabaseCommand IPagedDatabaseCommandProvider<TQuery>.CreatePaged(TQuery source, DatabaseOperation operation, int offset, int pageSize)
            => new PagedDatabaseCommand(GenerateCommand(source, false), GenerateCommand(source, true), offset, pageSize);

        IPagedDatabaseCommand IPagedDatabaseCommandProvider.CreatePaged(DatabaseOperation operation, int offset, int pageSize)
            => new PagedDatabaseCommand(GenerateCommand(new TQuery(), false), GenerateCommand(new TQuery(), true), offset, pageSize);

        private IDatabaseCommand GenerateCommand(TQuery query, bool countOnly)
            => _databaseCommandGenerator.Generate
            (
                query.Validate(_settings.ValidateFieldNames).ProcessDynamicQuery(),
                _settings.WithDefaultTableName(typeof(TResult).Name),
                countOnly
            );
    }
}
