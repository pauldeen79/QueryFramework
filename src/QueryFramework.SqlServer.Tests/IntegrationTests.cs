namespace QueryFramework.SqlServer.Tests;

public class IntegrationTests
{
    private QueryProcessor<TestQuery, TestEntity> Sut { get; }
    public Mock<IDatabaseEntityRetriever<TestEntity>> RetrieverMock { get; }

    public IntegrationTests()
    {
        RetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
        var settings = new PagedDatabaseEntityRetrieverSettings("MyTable", "", "", "", null);
        Sut = new QueryProcessor<TestQuery, TestEntity>
        (
            RetrieverMock.Object,
            new QueryPagedDatabaseCommandProvider<TestQuery>(new DefaultQueryFieldProvider(), settings)
        );
    }

    [Fact]
    public void Can_Query_All_Records()
    {
        // Arrange
        var query = new TestQuery();
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                     .Returns(expectedResult);

        // Act
        var actual = Sut.FindMany(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Query_Filtered_Records()
    {
        // Arrange
        var query = new TestQuery(new SingleEntityQueryBuilder().Where("Name".IsEqualTo("Test")).Build());
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>()))
                     .Returns(expectedResult);

        // Act
        var actual = Sut.FindMany(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }
}
