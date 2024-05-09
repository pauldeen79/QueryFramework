namespace QueryFramework.SqlServer.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();
    private readonly IDatabaseEntityRetriever<TestEntity> _retrieverMock;

    public IntegrationTests()
    {
        _retrieverMock = Substitute.For<IDatabaseEntityRetriever<TestEntity>>();
        var databaseEntityRetrieverProviderMock = Substitute.For<IDatabaseEntityRetrieverProvider>();
        databaseEntityRetrieverProviderMock.TryCreate(Arg.Any<IQuery>(), out Arg.Any<IDatabaseEntityRetriever<TestEntity>?>())
                                           .Returns(x => { x[1] = _retrieverMock; return true; });
        _serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer(x =>
                x.AddSingleton(_retrieverMock)
                 .AddSingleton(databaseEntityRetrieverProviderMock)
                 .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, TestEntityQueryProcessorSettingsProvider>()
            ).BuildServiceProvider();
    }

    [Fact]
    public void Can_Query_All_Records()
    {
        // Arrange
        var query = new TestQuery();
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.FindMany(Arg.Any<IDatabaseCommand>())
                      .Returns(expectedResult);

        // Act
        var actual = CreateSut().FindMany<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Can_Query_All_Records_Async()
    {
        // Arrange
        var query = new TestQuery();
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.FindManyAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>())
                      .Returns(expectedResult);

        // Act
        var actual = await CreateSut().FindManyAsync<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Query_Filtered_Records()
    {
        // Arrange
        var query = new TestQuery(new SingleEntityQueryBuilder().Where("Name").IsEqualTo("Test").Build());
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.FindMany(Arg.Any<IDatabaseCommand>())
                      .Returns(expectedResult);

        // Act
        var actual = CreateSut().FindMany<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Can_Query_Filtered_Records_Async()
    {
        // Arrange
        var query = new TestQuery(new SingleEntityQueryBuilder().Where("Name").IsEqualTo("Test").Build());
        var expectedResult = new[] { new TestEntity(), new TestEntity() };
        _retrieverMock.FindManyAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>())
                      .Returns(expectedResult);

        // Act
        var actual = await CreateSut().FindManyAsync<TestEntity>(query);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Single_Expression()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1").IsEqualTo("Value")
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(1);
        dict?.Keys.Should().BeEquivalentTo("@p0");
        dict?.Values.Should().BeEquivalentTo(new[] { "Value" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Single_Expression_With_Parameters()
    {
        // Arrange
        var query = new ParameterizedQueryBuilder()
            .AddParameter("MyParameter", "Value")
            .Where("Field1").IsEqualToParameter("MyParameter")
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(2);
        dict?.Keys.Should().BeEquivalentTo("MyParameter", "@p0");
        dict?.Values.Should().BeEquivalentTo(new[] { "Value", "Value" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Two_Expressions_With_And_Combination()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1").IsEqualTo("Value1")
            .And("Field2").IsNotEqualTo("Value2")
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0 AND Field2 <> @p1");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(2);
        dict?.Keys.Should().BeEquivalentTo("@p0", "@p1");
        dict?.Values.Should().BeEquivalentTo(new[] { "Value1", "Value2" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Two_Expressions_With_Or_Combination()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1").IsEqualTo("Value1")
            .Or("Field2").IsGreaterThan("Value2")
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0 OR Field2 > @p1");
        actual.CommandParameters.Should().NotBeNull();
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Three_Expressions_With_Different_Combinations_And_Group()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1").IsEqualTo("Value")
            .AndAny
            (
                ComposableEvaluatableBuilderHelper.Create("Field2", new EqualsOperatorBuilder(), "A"),
                ComposableEvaluatableBuilderHelper.Create("Field2", new EqualsOperatorBuilder(), "B")
            )
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0 AND (Field2 = @p1 OR Field2 = @p2)");
        actual.CommandParameters.Should().NotBeNull();
    }

    [Fact]
    public void Can_Get_SqlStatement_For_OrderBy_Clause_With_Function()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .OrderBy(new QuerySortOrderBuilder()
                .WithFieldNameExpression(new ToUpperCaseExpressionBuilder()
                    .WithExpression(new TypedFieldExpressionBuilder<string>()
                        .WithExpression(new ContextExpressionBuilder())
                        .WithFieldNameExpression("Field1")
                    )
                )
            ).Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity ORDER BY UPPER(Field1) ASC");
    }

    [Fact]
    public void Can_Get_SqlStatement_For_OrderBy_Clause_With_SqlInjection_Safe_Code()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .OrderBy(new QuerySortOrderBuilder()
                .WithFieldNameExpression(new ToUpperCaseExpressionBuilder()
                    .WithExpression(new TypedConstantExpressionBuilder<string>()
                        .WithValue("Sql injection, here we go")
                    )
                )
            ).Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity ORDER BY UPPER(@p0) ASC");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(1);
        dict?.Keys.Should().BeEquivalentTo("@p0");
        dict?.Values.Should().BeEquivalentTo(new[] { "Sql injection, here we go" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_OrderBy_Clause_With_DefaultExpression()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .OrderBy(new QuerySortOrderBuilder()
                .WithFieldNameExpression(new DefaultExpressionBuilder<string>())
            ).Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity ORDER BY @p0 ASC");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(1);
        dict?.Keys.Should().BeEquivalentTo("@p0");
        dict?.Values.Should().BeEquivalentTo(new object?[] { null });
    }

    public void Dispose() => _serviceProvider.Dispose();
}
