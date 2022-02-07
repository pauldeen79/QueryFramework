namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class SqlHelpers
{
    internal static void ExpressionSqlShouldBe(IQueryExpressionBuilder expression, string expectedSqlForExpression)
    {
        // Arrange
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.SetupGet(x => x.TableName)
                    .Returns(nameof(MyEntity));
        var settingsProviderMock = new Mock<IPagedDatabaseEntityRetrieverSettingsProvider>();
        var settings = settingsMock.Object;
        settingsProviderMock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out settings))
                            .Returns(true);
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                     .Returns<string>(x => x);
        var queryFieldInfo = fieldInfoMock.Object;
        var queryFieldInfoFactory = new Mock<IQueryFieldInfoFactory>();
        queryFieldInfoFactory.Setup(x => x.Create(It.IsAny<ISingleEntityQuery>()))
                             .Returns(queryFieldInfo);
        var query = new SingleEntityQueryBuilder().Where
        (
            new QueryConditionBuilder()
                .WithField(expression)
                .WithOperator(QueryOperator.Equal)
                .WithValue("test")
        ).Build();
        var serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsProviderMock.Object)
            .AddSingleton(queryFieldInfoFactory.Object)
            .AddSingleton(ctx =>
            {
                var mock = new Mock<IPagedDatabaseCommandProviderProvider>();
                var result = ctx.GetRequiredService<IPagedDatabaseCommandProvider<ISingleEntityQuery>>();
                mock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out result))
                    .Returns(true);
                return mock.Object;
            })
            .BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IDatabaseCommandProvider<ISingleEntityQuery>>();

        // Act
        var actual = provider
            .Create(query, DatabaseOperation.Select)
            .CommandText;

        // Assert
        actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
    }
}
