using System.Data.Stub;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        [Fact]
        public void Can_Use_Len_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Len();

            // Assert
            ExpressionSqlShouldBe(actual, "LEN(Field)");
        }

        [Fact]
        public void Can_Use_SqlTrim_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.SqlTrim();

            // Assert
            ExpressionSqlShouldBe(actual, "TRIM(Field)");
        }

        [Fact]
        public void Can_Use_Upper_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Upper();

            // Assert
            ExpressionSqlShouldBe(actual, "UPPER(Field)");
        }

        [Fact]
        public void Can_Use_Lower_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Lower();

            // Assert
            ExpressionSqlShouldBe(actual, "LOWER(Field)");
        }

        [Fact]
        public void Can_Use_Left_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Left(1);

            // Assert
            ExpressionSqlShouldBe(actual, "LEFT(Field, 1)");
        }

        [Fact]
        public void Can_Use_Right_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Right(1);

            // Assert
            ExpressionSqlShouldBe(actual, "RIGHT(Field, 1)");
        }

        [Fact]
        public void Can_Use_Year_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Year();

            // Assert
            ExpressionSqlShouldBe(actual, "YEAR(Field)");
        }

        [Fact]
        public void Can_Use_Month_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Month();

            // Assert
            ExpressionSqlShouldBe(actual, "MONTH(Field)");
        }

        [Fact]
        public void Can_Use_Day_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Day();

            // Assert
            ExpressionSqlShouldBe(actual, "DAY(Field)");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Coalesce("default");

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
        }

        [Fact]
        public void Can_Use_Count_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Count();

            // Assert
            ExpressionSqlShouldBe(actual, "COUNT(Field)");
        }

        [Fact]
        public void Can_Use_Sum_Function_On_FieldName_String()
        {
            // Arrange
            var sut = "Field";

            // Act
            var actual = sut.Sum();

            // Assert
            ExpressionSqlShouldBe(actual, "SUM(Field)");
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
