namespace QueryFramework.SqlServer.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();
    private readonly Mock<IDatabaseEntityRetriever<TestEntity>> _retrieverMock;

    public IntegrationTests()
    {
        _retrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
        var databaseEntityRetrieverProviderMock = new Mock<IDatabaseEntityRetrieverProvider>();
        var retriever = _retrieverMock.Object;
        databaseEntityRetrieverProviderMock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out retriever))
                                           .Returns(true);
        _serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(_retrieverMock.Object)
            .AddSingleton(databaseEntityRetrieverProviderMock.Object)
            .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, TestEntityQueryProcessorSettingsProvider >()
            .AddSingleton<IQueryFieldInfoProvider, DefaultQueryFieldInfoProvider>()
            .AddSingleton(ctx =>
            {
                var mock = new Mock<IPagedDatabaseCommandProviderProvider>();
                var result = ctx.GetRequiredService<IPagedDatabaseCommandProvider<ISingleEntityQuery>>();
                mock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out result))
                    .Returns(true);
                return mock.Object;
            })
            .BuildServiceProvider();
    }

    [Fact]
    public void Can_Query_All_Records()
    {
        // Arrange
        var query = new TestQuery();
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                      .Returns(expectedResult);

        // Act
        var actual = CreateSut().FindMany<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Query_Filtered_Records()
    {
        // Arrange
        var query = new TestQuery(new SingleEntityQueryBuilder().Where("Name".IsEqualTo("Test")).Build());
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                      .Returns(expectedResult);

        // Act
        var actual = CreateSut().FindMany<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    public void Dispose() => _serviceProvider.Dispose();
}
