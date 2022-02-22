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
            .AddExpressionFramework()
            .AddQueryFrameworkSqlServer(x =>
                x.AddSingleton(_retrieverMock.Object)
                 .AddSingleton(databaseEntityRetrieverProviderMock.Object)
                 .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, TestEntityQueryProcessorSettingsProvider>()
            ).BuildServiceProvider();
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

    [Fact]
    public void Can_Get_SqlStatement_For_Single_Expression()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1".IsEqualTo("Value"))
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(1);
        dict?.Keys.Should().BeEquivalentTo(new[] { "@p0" });
        dict?.Values.Should().BeEquivalentTo(new[] { "Value" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Two_Expressions_With_And_Combination()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1".IsEqualTo("Value1"))
            .And("Field2".IsNotEqualTo("Value2"))
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0 AND Field2 <> @p1");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(2);
        dict?.Keys.Should().BeEquivalentTo(new[] { "@p0", "@p1" });
        dict?.Values.Should().BeEquivalentTo(new[] { "Value1", "Value2" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Two_Expressions_With_Or_Combination()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .Where("Field1".IsEqualTo("Value1"))
            .Or("Field2".IsGreaterThan("Value2"))
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
            .Where("Field1".IsEqualTo("Value"))
            .AndAny
            (
                "Field2".IsEqualTo("A"),
                "Field2".IsEqualTo("B")
            )
            .Build();

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE Field1 = @p0 AND (Field2 = @p1 OR Field2 = @p2)");
        actual.CommandParameters.Should().NotBeNull();
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Expression_With_ConditionFunction_With_False_Result()
    {
        // Arrange
        var query = CreateQuery(Operator.Equal);

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE NULL = @p1");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(2);
        dict?.Keys.Should().BeEquivalentTo(new[] { "@p0", "@p1" });
        dict?.Values.Should().BeEquivalentTo(new[] { "test", "something" });
    }

    [Fact]
    public void Can_Get_SqlStatement_For_Expression_With_ConditionFunction_With_True_Result()
    {
        // Arrange
        var query = CreateQuery(Operator.NotEqual);

        // Act
        var actual = SqlHelpers.GetExpressionCommand(query);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyEntity WHERE @p0 = @p1");
        actual.CommandParameters.Should().NotBeNull();
        var dict = actual.CommandParameters as IDictionary<string, object>;
        dict.Should().NotBeNull();
        dict.Should().HaveCount(2);
        dict?.Keys.Should().BeEquivalentTo(new[] { "@p0", "@p1" });
        dict?.Values.Should().BeEquivalentTo(new[] { "test", "something" });
    }

    private static ISingleEntityQuery CreateQuery(Operator @operator)
        => new SingleEntityQueryBuilder()
        .Where
        (
            new ConditionBuilder()
                .WithLeftExpression
                (
                    new ConstantExpressionBuilder()
                        .WithValue("test")
                        .WithFunction
                        (
                            new ConditionFunctionBuilder()
                                .AddConditions(new ConditionBuilder().WithLeftExpression(new ConstantExpressionBuilder()
                                                                     .WithValue("A"))
                                                                     .WithOperator(@operator)
                                                                     .WithRightExpression(new ConstantExpressionBuilder()
                                                                     .WithValue("B")))
                        )
                )
                .WithOperator(Operator.Equal)
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("something"))
        )
        .Build();

    public void Dispose() => _serviceProvider.Dispose();
}
