using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal static class SqlHelpers
    {
        internal static void ExpressionSqlShouldBe(IQueryExpression expression, string expectedSqlForExpression)
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.TableName)
                        .Returns(nameof(MyEntity));
            var fieldProvider = new DefaultQueryFieldProvider();
            var query = new SingleEntityQuery(null, null, new[] { new QueryCondition(expression, QueryOperator.Equal, "test") }, Enumerable.Empty<IQuerySortOrder>());

            // Act
            var actual = new QueryPagedDatabaseCommandProvider<SingleEntityQuery>(fieldProvider, settingsMock.Object)
                .Create(query, DatabaseOperation.Select)
                .CommandText;

            // Assert
            actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
