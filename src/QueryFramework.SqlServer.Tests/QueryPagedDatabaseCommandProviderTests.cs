namespace QueryFramework.SqlServer.Tests;

public class QueryPagedDatabaseCommandProviderTests : TestBase<QueryPagedDatabaseCommandProvider>
{
    public QueryPagedDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<Mock<IQueryExpressionEvaluator>>();
        evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<IQueryExpression>()))
                     .Returns<IQueryExpression>(x => new DefaultQueryExpressionEvaluator(Enumerable.Empty<IFunctionParser>()).GetSqlExpression(x));
    }

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
    {
        // Act
        Sut.Invoking(x => x.CreatePaged(new Mock<ISingleEntityQuery>().Object, operation, 0, 0))
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
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field".IsEqualTo("Value")).Build(),
                                     DatabaseOperation.Select,
                                     0,
                                     0).DataCommand;

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

    [Fact]
    public void CreatePaged_With_Source_Argument_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
    {
        // Arrange
        const int pageSize = 10;
        var settingsMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettings>>();
        settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        settingsMock.SetupGet(x => x.OverridePageSize).Returns(pageSize);
        var fieldProviderMock = Fixture.Freeze<Mock<IQueryFieldProvider>>();
        fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

        // Act
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field".IsEqualTo("Value")).Build(),
                                     DatabaseOperation.Select,
                                     0,
                                     pageSize);

        // Assert
        actual.DataCommand.CommandText.Should().Be("SELECT TOP 10 * FROM MyTable WHERE Field = @p0");
        actual.DataCommand.CommandParameters.Should().NotBeNull();
        var parameters = actual.DataCommand.CommandParameters as IEnumerable<KeyValuePair<string, object>>;
        parameters.Should().NotBeNull();
        if (parameters != null)
        {
            parameters.Should().ContainSingle();
            parameters.First().Key.Should().Be("p0");
            parameters.First().Value.Should().Be("Value");
        }
    }
}
