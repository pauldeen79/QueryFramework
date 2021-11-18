using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>
        where TQuery : ISingleEntityQuery
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
            => _retriever.FindPaged(GenerateCommand(query, false),
                                    GenerateCommand(query, true),
                                    query.Offset.GetValueOrDefault(),
                                    query.Limit.GetValueOrDefault());

        private IDatabaseCommand GenerateCommand(TQuery query, bool countOnly)
            => _databaseCommandGenerator.Generate
            (
                query.Validate(_settings.ValidateFieldNames).ProcessDynamicQuery(),
                _settings.WithDefaultTableName(typeof(TResult).Name),
                countOnly
            );
    }
}
