namespace QueryFramework.SqlServer.Tests.CrossCutting.Data;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider>
{
    public QueryDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<Mock<ISqlExpressionEvaluator>>();
        evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<IExpression>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<int>()))
                     .Returns<IExpression, IQueryFieldInfo, int>((x, y, z) => new DefaultSqlExpressionEvaluator
                     (
                         new ISqlExpressionEvaluatorProvider[] { new FieldExpressionEvaluatorProvider(), new ConstantExpressionEvaluatorProvider() },
                         Enumerable.Empty<IFunctionParser>()
                     ).GetSqlExpression(x, y, z));
        // Use real paged database command provider
        var settingsFactoryMock = Fixture.Create<Mock<IPagedDatabaseEntityRetrieverSettingsFactory>>();
        var settingsMock = Fixture.Create<Mock<IPagedDatabaseEntityRetrieverSettings>>();
        settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        var fieldInfoFactory = new Mock<IQueryFieldInfoFactory>();
        fieldInfoFactory.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>())).Returns(new DefaultQueryFieldInfo());
        settingsFactoryMock.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                           .Returns(settingsMock.Object);
        var pagedProviderMock = Fixture.Freeze<Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>>();
        pagedProviderMock.Setup(x => x.CreatePaged(It.IsAny<ISingleEntityQuery>(),
                                                   It.IsAny<DatabaseOperation>(),
                                                   It.IsAny<int>(),
                                                   It.IsAny<int>()))
                         .Returns<ISingleEntityQuery, DatabaseOperation, int, int>((source, operation, offset, pageSize)
                         => new QueryPagedDatabaseCommandProvider
                         (
                             fieldInfoFactory.Object,
                             settingsFactoryMock.Object,
                             evaluatorMock.Object
                         ).CreatePaged(source, operation, offset, pageSize));

        var result = pagedProviderMock.Object;
        var pagedDatabaseCommandProviderFactoryMock = Fixture.Freeze<Mock<IPagedDatabaseCommandProviderFactory>>();
        pagedDatabaseCommandProviderFactoryMock.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                                               .Returns(result);
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
