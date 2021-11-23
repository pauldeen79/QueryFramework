using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : Repository<TestEntity, TestEntityIdentity>, ITestRepository
    {
        private readonly IPagedDatabaseCommandProvider<ITestQuery> _queryDatabaseCommandProvider;

        public TestRepository(IDatabaseCommandProcessor<TestEntity> databaseCommandProcessor,
                              IDatabaseEntityRetriever<TestEntity> entityRetriever,
                              IPagedDatabaseCommandProvider<TestEntityIdentity> identityDatabaseCommandProvider,
                              IDatabaseCommandProvider<TestEntity> entityDatabaseCommandProvider,
                              IPagedDatabaseCommandProvider<ITestQuery> queryDatabaseCommandProvider)
            : base(databaseCommandProcessor, entityRetriever, identityDatabaseCommandProvider, entityDatabaseCommandProvider)
        {
            _queryDatabaseCommandProvider = queryDatabaseCommandProvider;
        }

        public TestEntity? FindOne(ITestQuery query)
            => EntityRetriever.FindOne(_queryDatabaseCommandProvider.Create(query, DatabaseOperation.Select));

        public IReadOnlyCollection<TestEntity> FindMany(ITestQuery query)
            => EntityRetriever.FindMany(_queryDatabaseCommandProvider.Create(query, DatabaseOperation.Select));

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
            => EntityRetriever.FindPaged(_queryDatabaseCommandProvider.CreatePaged(query, DatabaseOperation.Select, query.Offset.GetValueOrDefault(), query.Limit.GetValueOrDefault()));

        // Added as an example to work with Sql directly from the repository, in case query framework does not support the sql constructs you need
        public TestEntity? FindUsingCommand(TestEntityIdentity identity)
            => EntityRetriever.FindOne(new SelectCommandBuilder()
                .From("[Table]")
                .Where("[Id] = @Id")
                .AppendParameters(identity)
                .Build());
    }
}
