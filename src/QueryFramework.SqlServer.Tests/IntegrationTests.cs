namespace QueryFramework.SqlServer.Tests;

public class IntegrationTests
{
    private readonly QueryProcessor<TestQuery, TestEntity> _sut;
    private readonly Mock<IDatabaseEntityRetriever<TestEntity>> _retrieverMock;
    private readonly Mock<IQueryExpressionEvaluator> _evaluatorMock;

    public IntegrationTests()
    {
        _retrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
        _evaluatorMock = new Mock<IQueryExpressionEvaluator>();
        var settings = new PagedDatabaseEntityRetrieverSettings("MyTable", "", "", "", null);
        _sut = new QueryProcessor<TestQuery, TestEntity>
        (
            _retrieverMock.Object,
            new QueryPagedDatabaseCommandProvider<TestQuery>(new DefaultQueryFieldProvider(), settings, _evaluatorMock.Object)
        );
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
        var actual = _sut.FindMany(query);

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
        var actual = _sut.FindMany(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }
}
