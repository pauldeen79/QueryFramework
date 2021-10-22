using System.Data.Stub;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Extensions;
using QueryFramework.SqlServer.QueryExpressions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.QueryExpressions
{
    [ExcludeFromCodeCoverage]
    public class DateTests
    {
        [Fact]
        public void Can_Use_SqlDate_In_Coalesce_Expression()
        {
            // Arrange
            var expression = new QueryExpression("Field");

            // Act
            var actual = expression.Coalesce(new Date(new System.DateTime(2000, 1, 1)));

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(Field, '2000-01-01 00:00:00')");
        }

        private static void ExpressionSqlShouldBe(IQueryExpression expression, string expectedSqlForExpression)
        {
            using var connection = new DbConnection();
            using var command = connection.CreateCommand();
            var query = new SingleEntityQuery(new[] { new QueryCondition(expression, QueryOperator.Equal, "test") });
            command.FillSelectCommand(query, tableName: "Table");

            // Assert
            command.CommandText.Should().Be($"SELECT * FROM Table WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
