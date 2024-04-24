namespace QueryFramework.SqlServer.Tests;

public class DefaultQueryProcessorTests : TestBase<DefaultQueryProcessor>
{
    public DefaultQueryProcessorTests()
    {
        Fixture.Freeze<IPagedDatabaseCommandProvider<IQuery>>()
               .CreatePaged(Arg.Any<IQuery>(), DatabaseOperation.Select, Arg.Any<int>(), Arg.Any<int>())
               .Returns(new PagedDatabaseCommand(new SqlTextCommand("SELECT ...", DatabaseOperation.Select),
                                                 new SqlTextCommand("SELECT COUNT(*)...", DatabaseOperation.Select),
                                                 0,
                                                 0));
    }

    [Fact]
    public void FindPaged_Returns_MappedEntities()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = Sut.FindPaged<MyEntity>(new SingleEntityQueryBuilder().BuildTyped());

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
        actual.TotalRecordCount.Should().Be(1);
    }

    [Fact]
    public async Task FindPagedAsync_Returns_MappedEntities()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = await Sut.FindPagedAsync<MyEntity>(new SingleEntityQueryBuilder().BuildTyped());

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
        actual.TotalRecordCount.Should().Be(1);
    }

    [Fact]
    public void FindPaged_Fills_TotalRecordCount_On_Paged_Query()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }], totalRecordCount: 10);
        var query = new SingleEntityQueryBuilder().Take(1).Build();

        // Act
        var actual = Sut.FindPaged<MyEntity>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
        actual.TotalRecordCount.Should().Be(10);
    }

    [Fact]
    public async Task FindPagedAsync_Fills_TotalRecordCount_On_Paged_Query()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }], totalRecordCount: 10);
        var query = new SingleEntityQueryBuilder().Take(1).Build();

        // Act
        var actual = await Sut.FindPagedAsync<MyEntity>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
        actual.TotalRecordCount.Should().Be(10);
    }

    [Fact]
    public void FindOne_Returns_MappedEntity()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = Sut.FindOne<MyEntity>(new SingleEntityQueryBuilder().Where("Property").IsEqualTo("Some value").Build());

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("Value");
    }

    [Fact]
    public async Task FindOneAsync_Returns_MappedEntity()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = await Sut.FindOneAsync<MyEntity>(new SingleEntityQueryBuilder().Where("Property").IsEqualTo("Some value").Build());

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("Value");
    }

    [Fact]
    public void FindMany_Returns_MappedEntities()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = Sut.FindMany<MyEntity>(new SingleEntityQueryBuilder().BuildTyped());

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
    }

    [Fact]
    public async Task FindManyAsync_Returns_MappedEntities()
    {
        // Arrange
        SetupSourceData([new MyEntity { Property = "Value" }]);

        // Act
        var actual = await Sut.FindManyAsync<MyEntity>(new SingleEntityQueryBuilder().BuildTyped());

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Value");
    }

    private void SetupSourceData(IEnumerable<MyEntity> data, int? totalRecordCount = null)
    {
        var retrieverMock = Fixture.Freeze<IDatabaseEntityRetriever<MyEntity>>();

        // For FindOne/FindMany
        retrieverMock.FindOne(Arg.Any<IDatabaseCommand>()).Returns(data.FirstOrDefault());
        retrieverMock.FindOneAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>()).Returns(data.FirstOrDefault());
        retrieverMock.FindMany(Arg.Any<IDatabaseCommand>()).Returns(data.ToList());
        retrieverMock.FindManyAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>()).Returns(data.ToList());

        // For FindPaged
        retrieverMock.FindPaged(Arg.Any<IPagedDatabaseCommand>())
                     .Returns(x => new PagedResult<MyEntity>(data, totalRecordCount ?? data.Count(), x.ArgAt<IPagedDatabaseCommand>(0).Offset, x.ArgAt<IPagedDatabaseCommand>(0).PageSize));
        retrieverMock.FindPagedAsync(Arg.Any<IPagedDatabaseCommand>(), Arg.Any<CancellationToken>())
                     .Returns(x => new PagedResult<MyEntity>(data, totalRecordCount ?? data.Count(), x.ArgAt<IPagedDatabaseCommand>(0).Offset, x.ArgAt<IPagedDatabaseCommand>(0).PageSize));

        // Hook up the database entity retriever to the SQL Database processor
        var result = retrieverMock;
        Fixture.Freeze<IDatabaseEntityRetrieverFactory>()
               .Create<MyEntity>(Arg.Any<IQuery>())
               .Returns(result);
    }
}
