using QueryFramework.Abstractions.Builders;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    internal static class SqlHelpers
    {
        internal static void ExpressionSqlShouldBe(IQueryExpressionBuilder expression, string expectedSqlForExpression)
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.TableName)
                        .Returns(nameof(MyEntity));
            var fieldProvider = new DefaultQueryFieldProvider();
            var query = new SingleEntityQueryBuilder().Where
            (
                new QueryConditionBuilder()
                    .WithField(expression)
                    .WithOperator(QueryOperator.Equal)
                    .WithValue("test")
            ).Build();

            // Act
            var actual = new QueryPagedDatabaseCommandProvider<ISingleEntityQuery>(fieldProvider, settingsMock.Object)
                .Create(query, DatabaseOperation.Select)
                .CommandText;

            // Assert
            actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
