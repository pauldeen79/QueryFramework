namespace QueryFramework.SqlServer.Tests.CrossCutting.Data;

public class QueryDatabaseCommandProviderTests : TestBase<QueryDatabaseCommandProvider>
{
    private readonly ParameterBag _parameterBag = new ParameterBag();

    public QueryDatabaseCommandProviderTests()
    {
        // Use real query expression evaluator
        var evaluatorMock = Fixture.Freeze<ISqlExpressionEvaluator>();
        DefaultSqlExpressionEvaluatorHelper.UseRealSqlExpressionEvaluator(evaluatorMock, _parameterBag);
        // Use real paged database command provider
        var settingsProviderMock = Fixture.Create<IPagedDatabaseEntityRetrieverSettingsProvider>();
        var settingsMock = Fixture.Create<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.TableName
                    .Returns("MyTable");
        var fieldInfoFactory = Substitute.For<IQueryFieldInfoFactory>();
        fieldInfoFactory.Create(Arg.Any<IQuery>())
                        .Returns(new DefaultQueryFieldInfo());
        settingsProviderMock.TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>()!)
                            .Returns(x => { x[0] = settingsMock; return true; });
        var pagedProviderMock = Fixture.Freeze<IPagedDatabaseCommandProvider<IQuery>>();
        pagedProviderMock.CreatePaged(Arg.Any<IQuery>(),
                                      Arg.Any<DatabaseOperation>(),
                                      Arg.Any<int>(),
                                      Arg.Any<int>())
                         .Returns(x
                         => new QueryPagedDatabaseCommandProvider
                         (
                             fieldInfoFactory,
                             [settingsProviderMock],
                             evaluatorMock
                         ).CreatePaged(x.ArgAt<IQuery>(0), x.ArgAt<DatabaseOperation>(1), x.ArgAt<int>(2), x.ArgAt<int>(3)));
    }

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
    {
        // Act
        Action a = () => Sut.Create(Substitute.For<IQuery>(), operation);
        a.ShouldThrow<ArgumentOutOfRangeException>()
         .ParamName.ShouldBe("operation");
    }

    [Fact]
    public void Create_With_Source_Argument_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
    {
        // Act
        var actual = Sut.Create(new SingleEntityQueryBuilder().Where("Field").IsEqualTo("Value").BuildTyped(), DatabaseOperation.Select);

        // Assert
        actual.CommandText.ShouldBe("SELECT * FROM MyTable WHERE Field = @p0");
        actual.CommandParameters.ShouldNotBeNull();
    }
}
