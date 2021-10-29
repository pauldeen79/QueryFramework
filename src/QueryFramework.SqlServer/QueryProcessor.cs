using System;
using System.Collections.Generic;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>
        where TQuery : ISingleEntityQuery
        where TResult : class
    {
        private readonly IDbConnection _connection;
        private readonly IDataReaderMapper<TResult> _mapper;
        private readonly IQueryProcessorSettings _settings;
        private readonly IDatabaseCommandGenerator _databaseCommandGenerator;
        private readonly IQueryFieldProvider _fieldProvider;

        public QueryProcessor(IDbConnection connection,
                              IDataReaderMapper<TResult> mapper,
                              IQueryProcessorSettings settings,
                              IDatabaseCommandGenerator databaseCommandGenerator,
                              IQueryFieldProvider fieldProvider)
        {
            _connection = connection;
            _mapper = mapper;
            _settings = settings;
            _databaseCommandGenerator = databaseCommandGenerator;
            _fieldProvider = fieldProvider;
        }

        public IReadOnlyCollection<TResult> FindMany(TQuery query)
            => _connection.FindMany(GenerateCommand(query, false), _mapper.Map);

        public TResult FindOne(TQuery query)
            => _connection.FindOne(GenerateCommand(query, false), _mapper.Map);

        public IQueryResult<TResult> FindPaged(TQuery query)
        {
            //TODO: Use FindPaged method from CrossCutting.Data.Sql
            var items = new List<TResult>(_connection.FindMany(GenerateCommand(query, false), _mapper.Map));
            var totalRecordCount = -1;

            if (items.Count > 0
                && items.Count == query.Limit.DetermineLimit(_settings.OverrideLimit))
            {
                totalRecordCount = (int)_connection.ExecuteScalar(GenerateCommand(query, true));
            }
            else
            {
                totalRecordCount = items.Count;
            }

            return new QueryResult<TResult>(items, totalRecordCount);
        }

        private IDatabaseCommand GenerateCommand(TQuery query, bool countOnly)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query is IDynamicQuery dynamicQuery)
            {
                query = (TQuery)dynamicQuery.Process();
            }

            query.Validate(_settings.ValidateFieldNames);
            return _databaseCommandGenerator.Generate(query, _settings.WithDefaultTableName(typeof(TResult).Name), _fieldProvider, countOnly);
        }
    }
}
