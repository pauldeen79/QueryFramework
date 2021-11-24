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
        private IPagedDatabaseCommandProvider<ITestQuery> QueryDatabaseCommandProvider { get; }

        public TestRepository(IDatabaseCommandProcessor<TestEntity> databaseCommandProcessor,
                              IDatabaseEntityRetriever<TestEntity> entityRetriever,
                              IPagedDatabaseCommandProvider<TestEntityIdentity> identityDatabaseCommandProvider,
                              IDatabaseCommandProvider<TestEntity> entityDatabaseCommandProvider,
                              IPagedDatabaseCommandProvider<ITestQuery> queryDatabaseCommandProvider)
            : base(databaseCommandProcessor, entityRetriever, identityDatabaseCommandProvider, entityDatabaseCommandProvider)
        {
            QueryDatabaseCommandProvider = queryDatabaseCommandProvider;
        }

        public TestEntity? FindOne(ITestQuery query)
            => EntityRetriever.FindOne(QueryDatabaseCommandProvider.Create(query, DatabaseOperation.Select));

        public IReadOnlyCollection<TestEntity> FindMany(ITestQuery query)
            => EntityRetriever.FindMany(QueryDatabaseCommandProvider.Create(query, DatabaseOperation.Select));

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
            => EntityRetriever.FindPaged(QueryDatabaseCommandProvider.CreatePaged(query, DatabaseOperation.Select, query.Offset.GetValueOrDefault(), query.Limit.GetValueOrDefault()));

        // Added as an example to work with Sql directly from the repository, in case query framework does not support the sql constructs you need
        public TestEntity? FindUsingCommand(TestEntityIdentity identity)
            => EntityRetriever.FindOne(new SelectCommandBuilder()
                .From("[Table]")
                .Where("[Id] = @Id")
                .AppendParameters(identity)
                .Build());
    }
}
