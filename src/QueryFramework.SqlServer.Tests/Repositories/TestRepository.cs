namespace QueryFramework.SqlServer.Tests.Repositories;

public class TestRepository : Repository<TestEntity, TestEntityIdentity>, ITestRepository
{
    public TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                          IDatabaseEntityRetriever<TestEntity> entityRetriever,
                          IDatabaseCommandProvider<TestEntityIdentity> identitySelectCommandProvider,
                          IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                          IDatabaseCommandProvider entitySelectCommandProvider,
                          IDatabaseCommandProvider<TestEntity> entityCommandProvider)
        : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
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
