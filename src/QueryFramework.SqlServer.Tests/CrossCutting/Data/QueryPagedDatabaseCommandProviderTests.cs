namespace QueryFramework.SqlServer.Tests.CrossCutting.Data;

public class QueryPagedDatabaseCommandProviderTests : TestBase<QueryPagedDatabaseCommandProvider>
{
    private readonly ParameterBag _parameterBag = new ParameterBag();

    public QueryPagedDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<Mock<ISqlExpressionEvaluator>>();
        DefaultSqlExpressionEvaluatorHelper.UseRealSqlExpressionEvaluator(evaluatorMock, _parameterBag);
    }

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void CreatePaged_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
    {
        // Act
        Sut.Invoking(x => x.CreatePaged(new Mock<ISingleEntityQuery>().Object, operation, 0, 0))
           .Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be("operation");
    }

    [Fact]
    public void CreatePaged_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
    {
        // Arrange
        var settingsMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettings>>();
        settingsMock.SetupGet(x => x.TableName)
                    .Returns("MyTable");
        var settingsProviderMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettingsProvider>>();
        var settings = settingsMock.Object;
        settingsProviderMock.Setup(x => x.TryGet<ISingleEntityQuery>(out settings))
                            .Returns(true);
        var fieldInfoMock = Fixture.Freeze<Mock<IQueryFieldInfo>>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                     .Returns<string>(x => x);
        var queryFieldInfo = fieldInfoMock.Object;
        var queryFieldInfoFactory = Fixture.Freeze<Mock<IQueryFieldInfoFactory>>();
        queryFieldInfoFactory.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                             .Returns(queryFieldInfo);

        // Act
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field".IsEqualTo("Value")).Build(),
                                     DatabaseOperation.Select,
                                     0,
                                     0).DataCommand;

        // Assert
        actual.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
        actual.CommandParameters.Should().NotBeNull();
    }

    [Fact]
    public void CreatePaged_Generates_Correct_Command_When_DatabaseOperation_Is_Select_And_PageSize_Is_Filled()
    {
        // Arrange
        const int pageSize = 10;
        var settingsMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettings>>();
        settingsMock.SetupGet(x => x.TableName)
                    .Returns("MyTable");
        settingsMock.SetupGet(x => x.OverridePageSize)
                    .Returns(pageSize);
        var settingsProviderMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettingsProvider>>();
        var settings = settingsMock.Object;
        settingsProviderMock.Setup(x => x.TryGet<ISingleEntityQuery>(out settings))
                            .Returns(true);
        var fieldInfoMock = Fixture.Freeze<Mock<IQueryFieldInfo>>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                     .Returns<string>(x => x);
        var queryFieldInfo = fieldInfoMock.Object;
        var queryFieldInfoFactory = Fixture.Freeze<Mock<IQueryFieldInfoFactory>>();
        queryFieldInfoFactory.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                             .Returns(queryFieldInfo);

        // Act
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field".IsEqualTo("Value")).Build(),
                                     DatabaseOperation.Select,
                                     0,
                                     pageSize);

        // Assert
        actual.DataCommand.CommandText.Should().Be("SELECT TOP 10 * FROM MyTable WHERE Field = @p0");
        actual.DataCommand.CommandParameters.Should().NotBeNull();
    }

    [Fact]
    public void CreatePaged_Throws_When_DatabaseOperation_Is_Select_But_No_Database_Entity_Retriever_Provider_Is_Registered()
    {
        // Act
        Sut.Invoking(x => x.CreatePaged(new SingleEntityQueryBuilder().Build(), DatabaseOperation.Select, 0, 0))
           .Should().ThrowExactly<InvalidOperationException>()
           .And.Message.Should().Be("No database entity retriever provider was found for query type [QueryFramework.Core.Queries.SingleEntityQuery]");
    }

    [Fact]
    public void CreatePaged_Throws_When_DatabaseOperation_Is_Select_But_Registered_Database_Entity_Retriever_Provider_Returns_False()
    {
        // Arrange
        var settingsProviderMock = Fixture.Freeze<Mock<IPagedDatabaseEntityRetrieverSettingsProvider>>();
        IPagedDatabaseEntityRetrieverSettings? settings = null;
        settingsProviderMock.Setup(x => x.TryGet<ISingleEntityQuery>(out settings))
                            .Returns(true);

        // Act
        Sut.Invoking(x => x.CreatePaged(new SingleEntityQueryBuilder().Build(), DatabaseOperation.Select, 0, 0))
           .Should().ThrowExactly<InvalidOperationException>()
           .And.Message.Should().Be("Database entity retriever provider for query type [QueryFramework.Core.Queries.SingleEntityQuery] provided an empty result");
    }
}
