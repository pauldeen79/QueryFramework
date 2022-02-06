namespace QueryFramework.SqlServer.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();
    private readonly Mock<IDatabaseEntityRetriever<TestEntity>> _retrieverMock;
    private readonly Mock<IQueryExpressionEvaluator> _evaluatorMock;
    private readonly Mock<IDatabaseEntityRetrieverProvider> _databaseEntityRetrieverProviderMock;

    public IntegrationTests()
    {
        _retrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
        _evaluatorMock = new Mock<IQueryExpressionEvaluator>();
        _databaseEntityRetrieverProviderMock = new Mock<IDatabaseEntityRetrieverProvider>();
        _databaseEntityRetrieverProviderMock.Setup(x => x.GetRetriever<TestEntity>())
                                            .Returns(_retrieverMock.Object);
        var settings = new PagedDatabaseEntityRetrieverSettings("MyTable", "", "", "", null);

        _serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddQueryFrameworkSqlServer<ISingleEntityQuery>()
            .AddSingleton(_retrieverMock.Object)
            .AddSingleton(_evaluatorMock.Object)
            .AddSingleton(_databaseEntityRetrieverProviderMock.Object)
            .AddSingleton<IPagedDatabaseEntityRetrieverSettings>(settings)
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
