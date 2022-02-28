namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class SqlHelpers
{
    internal static void ExpressionSqlShouldBe(IExpressionBuilder expression, string expectedSqlForExpression)
    {
        // Arrange & Act
        var actual = GetExpressionCommand
        (
            new SingleEntityQueryBuilder()
                .Where
                (
                    new ConditionBuilder()
                        .WithLeftExpression(expression)
                        .WithOperator(Operator.Equal)
                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("test"))
                )
                .Build()
        ).CommandText;


        // Assert
        actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
    }

    internal static IDatabaseCommand GetExpressionCommand(ISingleEntityQuery query)
    {
        // Arrange
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.SetupGet(x => x.TableName)
                    .Returns(nameof(MyEntity));
        var settingsProviderMock = new Mock<IPagedDatabaseEntityRetrieverSettingsProvider>();
        var settings = settingsMock.Object;
        settingsProviderMock.Setup(x => x.TryGet<ISingleEntityQuery>(out settings))
                            .Returns(true);
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                     .Returns<string>(x => x);
        var queryFieldInfo = fieldInfoMock.Object;
        var queryFieldInfoFactory = new Mock<IQueryFieldInfoFactory>();
        queryFieldInfoFactory.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                             .Returns(queryFieldInfo);
        using var serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsProviderMock.Object)
            .AddSingleton(queryFieldInfoFactory.Object)
            .BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IDatabaseCommandProvider<ISingleEntityQuery>>();

        // Act
        return provider.Create(query, DatabaseOperation.Select);
    }
}
