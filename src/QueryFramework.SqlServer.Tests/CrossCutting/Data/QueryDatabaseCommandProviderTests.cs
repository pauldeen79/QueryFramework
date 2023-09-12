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
        fieldInfoFactory.Create(Arg.Any<ISingleEntityQuery>())
                        .Returns(new DefaultQueryFieldInfo());
        settingsProviderMock.TryGet<ISingleEntityQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>()!)
                            .Returns(x => { x[0] = settingsMock; return true; });
        var pagedProviderMock = Fixture.Freeze<IContextPagedDatabaseCommandProvider<ISingleEntityQuery>>();
        pagedProviderMock.CreatePaged(Arg.Any<ISingleEntityQuery>(),
                                      Arg.Any<DatabaseOperation>(),
                                      Arg.Any<int>(),
                                      Arg.Any<int>(),
                                      Arg.Any<object?>())
                         .Returns(x
                         => new QueryPagedDatabaseCommandProvider
                         (
                             fieldInfoFactory,
                             new[] { settingsProviderMock },
                             evaluatorMock
                         ).CreatePaged(x.ArgAt<ISingleEntityQuery>(0), x.ArgAt<DatabaseOperation>(1), x.ArgAt<int>(2), x.ArgAt<int>(3), x.ArgAt<object?>(4)));
    }

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
    {
        // Act
        Sut.Invoking(x => x.Create(Substitute.For<ISingleEntityQuery>(), operation))
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
    }
}
