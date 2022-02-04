namespace QueryFramework.SqlServer.Tests;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider<ISingleEntityQuery>>
{
    public QueryDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<Mock<IQueryExpressionEvaluator>>();
        evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<IQueryExpression>()))
                     .Returns<IQueryExpression>(x => new QueryExpressionEvaluator(Enumerable.Empty<IFunctionParser>()).GetSqlExpression(x));
        // Use real paged database command provider
        var pagedProviderMock = Fixture.Freeze<Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>>();
        pagedProviderMock.Setup(x => x.CreatePaged(It.IsAny<ISingleEntityQuery>(), It.IsAny<DatabaseOperation>(), It.IsAny<int>(), It.IsAny<int>()))
                         .Returns<ISingleEntityQuery, DatabaseOperation, int, int>((source, operation, offset, pageSize)
                         => new QueryPagedDatabaseCommandProvider<ISingleEntityQuery>
                         (
                             new DefaultQueryFieldProvider(),
                             Fixture.Create<Mock<IPagedDatabaseEntityRetrieverSettings>>().Object,
                             evaluatorMock.Object
                         ).CreatePaged(source, operation, offset, pageSize));
    }

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
    {
        // Act
        Sut.Invoking(x => x.Create(new Mock<ISingleEntityQuery>().Object, operation))
           .Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be("operation");
    }

    [Fact]
    public void Create_With_Source_Argument_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
    {
        // Arrange
        var settingsMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettings>>();
        settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        var fieldProviderMock = Fixture.Freeze<Mock<IQueryFieldProvider>>();
        fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

        // Act
        var actual = Sut.Create(new SingleEntityQueryBuilder().Where("Field".IsEqualTo("Value")).Build(), DatabaseOperation.Select);

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
        actual.CommandParameters.Should().NotBeNull();
        var parameters = actual.CommandParameters as IEnumerable<KeyValuePair<string, object>>;
        parameters.Should().NotBeNull();
        if (parameters != null)
        {
            parameters.Should().ContainSingle();
            parameters.First().Key.Should().Be("p0");
            parameters.First().Value.Should().Be("Value");
        }
    }
}
