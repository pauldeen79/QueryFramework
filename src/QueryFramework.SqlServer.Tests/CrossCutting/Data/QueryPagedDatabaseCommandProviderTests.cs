namespace QueryFramework.SqlServer.Tests.CrossCutting.Data;

public class QueryPagedDatabaseCommandProviderTests : TestBase<QueryPagedDatabaseCommandProvider>
{
    private readonly ParameterBag _parameterBag = new ParameterBag();

    public QueryPagedDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
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
        Action a = () => Sut.CreatePaged(Substitute.For<IQuery>(), operation, 0, 0);
        a.ShouldThrow<ArgumentOutOfRangeException>()
         .ParamName.ShouldBe("operation");
    }

    [Fact]
    public void CreatePaged_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
    {
        // Arrange
        var settingsMock = Fixture.Freeze<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.TableName
                    .Returns("MyTable");
        var settingsProviderMock = Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>();
        settingsProviderMock.TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>()!)
                            .Returns(x => { x[0] = settingsMock; return true; });
        var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                     .Returns(x => x.ArgAt<string>(0));
        var queryFieldInfo = fieldInfoMock;
        var queryFieldInfoFactory = Fixture.Freeze<IQueryFieldInfoFactory>();
        queryFieldInfoFactory.Create(Arg.Any<IQuery>())
                             .Returns(queryFieldInfo);

        // Act
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field").IsEqualTo("Value").BuildTyped(),
                                     DatabaseOperation.Select,
                                     0,
                                     0).DataCommand;

        // Assert
        actual.CommandText.ShouldBe("SELECT * FROM MyTable WHERE Field = @p0");
        actual.CommandParameters.ShouldNotBeNull();
    }

    [Fact]
    public void CreatePaged_Generates_Correct_Command_When_DatabaseOperation_Is_Select_And_PageSize_Is_Filled()
    {
        // Arrange
        const int pageSize = 10;
        var settingsMock = Fixture.Freeze<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.TableName
                    .Returns("MyTable");
        settingsMock.OverridePageSize
                    .Returns(pageSize);
        var settingsProviderMock = Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>();
        settingsProviderMock.TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>()!)
                            .Returns(x => { x[0] = settingsMock; return true; });
        var fieldInfoMock = Fixture.Freeze<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                     .Returns(x => x.ArgAt<string>(0));
        var queryFieldInfo = fieldInfoMock;
        var queryFieldInfoFactory = Fixture.Freeze<IQueryFieldInfoFactory>();
        queryFieldInfoFactory.Create(Arg.Any<IQuery>())
                             .Returns(queryFieldInfo);

        // Act
        var actual = Sut.CreatePaged(new SingleEntityQueryBuilder().Where("Field").IsEqualTo("Value").BuildTyped(),
                                     DatabaseOperation.Select,
                                     0,
                                     pageSize);

        // Assert
        actual.DataCommand.CommandText.ShouldBe("SELECT TOP 10 * FROM MyTable WHERE Field = @p0");
        actual.DataCommand.CommandParameters.ShouldNotBeNull();
    }

    [Fact]
    public void CreatePaged_Throws_When_DatabaseOperation_Is_Select_But_No_Database_Entity_Retriever_Provider_Is_Registered()
    {
        // Act
        Action a = () => Sut.CreatePaged(new SingleEntityQueryBuilder().BuildTyped(), DatabaseOperation.Select, 0, 0);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("No database entity retriever provider was found for query type [QueryFramework.Core.Queries.SingleEntityQuery]");
    }

    [Fact]
    public void CreatePaged_Throws_When_DatabaseOperation_Is_Select_But_Registered_Database_Entity_Retriever_Provider_Returns_False()
    {
        // Arrange
        var settingsProviderMock = Fixture.Freeze<IPagedDatabaseEntityRetrieverSettingsProvider>();
        IPagedDatabaseEntityRetrieverSettings? settings = null;
        settingsProviderMock.TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings?>())
                            .Returns(x => { x[0] = settings; return true; });

        // Act
        Action a = () => Sut.CreatePaged(new SingleEntityQueryBuilder().BuildTyped(), DatabaseOperation.Select, 0, 0);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Database entity retriever provider for query type [QueryFramework.Core.Queries.SingleEntityQuery] provided an empty result");
    }
}
