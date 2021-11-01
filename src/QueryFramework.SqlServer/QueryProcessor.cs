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
        private readonly IDatabaseCommandProcessor<TResult> _processor;
        private readonly IQueryProcessorSettings _settings;
        private readonly IDatabaseCommandGenerator _databaseCommandGenerator;

        public QueryProcessor(IDatabaseCommandProcessor<TResult> processor,
                              IQueryProcessorSettings settings,
                              IDatabaseCommandGenerator databaseCommandGenerator)
        {
            _processor = processor;
            _settings = settings;
            _databaseCommandGenerator = databaseCommandGenerator;
        }

        public IReadOnlyCollection<TResult> FindMany(TQuery query)
            => _processor.FindMany(GenerateCommand(query, false));

        public TResult? FindOne(TQuery query)
            => _processor.FindOne(GenerateCommand(query, false));

        public IPagedResult<TResult> FindPaged(TQuery query)
            => _processor.FindPaged(GenerateCommand(query, false), GenerateCommand(query, true), query.Offset.GetValueOrDefault(), query.Limit.GetValueOrDefault());

        private IDatabaseCommand GenerateCommand(TQuery query, bool countOnly)
        {
            if (query is IDynamicQuery dynamicQuery)
            {
                query = (TQuery)dynamicQuery.Process();
            }

            query.Validate(_settings.ValidateFieldNames);
            return _databaseCommandGenerator.Generate(query,
                                                      _settings.WithDefaultTableName(typeof(TResult).Name),
                                                      countOnly);
        }
    }
}
