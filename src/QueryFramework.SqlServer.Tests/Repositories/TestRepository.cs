using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : Repository<TestEntity, TestEntityIdentity>, ITestRepository
    {
        public TestRepository(IDatabaseCommandProcessor<TestEntity> databaseCommandProcessor,
                              IDatabaseEntityRetriever<TestEntity> entityRetriever,
                              IPagedDatabaseCommandProvider<TestEntityIdentity> identityDatabaseCommandProvider,
                              IPagedDatabaseCommandProvider genericCommandProvider,
                              IDatabaseCommandProvider<TestEntity> entityDatabaseCommandProvider)
            : base(databaseCommandProcessor, entityRetriever, identityDatabaseCommandProvider, genericCommandProvider, entityDatabaseCommandProvider)
        {
        }

        // Added as an example to work with Sql directly from the repository, in case query framework does not support the sql constructs you need
        public TestEntity? FindUsingCommand(TestEntityIdentity identity)
            => EntityRetriever.FindOne(new SelectCommandBuilder()
                .From("[Table]")
                .Where("[Id] = @Id")
                .AppendParameters(identity)
                .Build());
    }
}
