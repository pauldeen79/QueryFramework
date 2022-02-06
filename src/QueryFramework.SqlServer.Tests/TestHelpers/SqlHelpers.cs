namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class SqlHelpers
{
    internal static void ExpressionSqlShouldBe(IQueryExpressionBuilder expression, string expectedSqlForExpression)
    {
        // Arrange
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.SetupGet(x => x.TableName)
                    .Returns(nameof(MyEntity));
        var query = new SingleEntityQueryBuilder().Where
        (
            new QueryConditionBuilder()
                .WithField(expression)
                .WithOperator(QueryOperator.Equal)
                .WithValue("test")
        ).Build();
        var serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsMock.Object)
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
