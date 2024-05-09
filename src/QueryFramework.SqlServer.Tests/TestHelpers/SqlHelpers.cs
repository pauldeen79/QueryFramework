namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class SqlHelpers
{
    internal static void ExpressionSqlShouldBe<T>(ComposableEvaluatableFieldNameBuilderWrapper<T> expression, string expectedSqlForExpression, object? context)
        where T : IQueryBuilder
    {
        // Arrange
        var queryBuilder = expression.IsEqualTo("test");

        // Act
        var actual = GetExpressionCommand
        (
            queryBuilder.Build(),
            context
        ).CommandText;

        // Assert
        actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
    }

    internal static void ExpressionSqlShouldBe<T>(ITypedExpressionBuilder<T> expression, string expectedSqlForExpression, object? context)
        => ExpressionSqlShouldBe(new ExpressionBuilderWrapper<T>(expression), expectedSqlForExpression, context);

    internal static void ExpressionSqlShouldBe(ExpressionBuilder expression, string expectedSqlForExpression, object? context)
    {
        // Arrange & Act
        var actual = GetExpressionCommand
        (
            new SingleEntityQueryBuilder()
                .Where(expression).IsEqualTo("test")
                .BuildTyped(),
            context
        ).CommandText;

        // Assert
        actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
    }

    internal static IDatabaseCommand GetExpressionCommand(IQuery query, object? context)
    {
        // Arrange
        var settingsMock = Substitute.For<IPagedDatabaseEntityRetrieverSettings>();
        settingsMock.TableName
                    .Returns(nameof(MyEntity));
        var settingsProviderMock = Substitute.For<IPagedDatabaseEntityRetrieverSettingsProvider>();
        settingsProviderMock.TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>()!)
                            .Returns(x => { x[0] = settingsMock; return true; });
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                     .Returns(x => x.ArgAt<string>(0));
        var queryFieldInfo = fieldInfoMock;
        var queryFieldInfoFactory = Substitute.For<IQueryFieldInfoFactory>();
        queryFieldInfoFactory.Create(Arg.Any<IQuery>())
                             .Returns(queryFieldInfo);
        using var serviceProvider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsProviderMock)
            .AddSingleton(queryFieldInfoFactory)
            .BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IContextDatabaseCommandProvider<IQuery>>();

        // Act
        return provider.Create(query, DatabaseOperation.Select, context);
    }
}
