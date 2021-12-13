using System.Diagnostics.CodeAnalysis;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.SqlServer.QueryExpressions;
using QueryFramework.SqlServer.Tests.TestHelpers;
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
            SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(Field, '2000-01-01 00:00:00')");
        }
    }
}
