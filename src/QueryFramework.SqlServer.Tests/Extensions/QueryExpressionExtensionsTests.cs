using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionExtensionsTests
    {
        [Fact]
        public void Can_Use_Len_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Len();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(Field)");
        }

        [Fact]
        public void Can_Use_Len_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Len();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Trim_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Trim();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(Field)");
        }

        [Fact]
        public void Can_Use_Trim_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Upper();

            // Act
            var actual = sut.Trim();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(UPPER(Field))");
        }

        [Fact]
        public void Can_Use_Upper_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Upper();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(Field)");
        }

        [Fact]
        public void Can_Use_Upper_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Upper();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Lower_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Lower();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(Field)");
        }

        [Fact]
        public void Can_Use_Lower_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Lower();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Left_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Left(2);

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(Field, 2)");
        }

        [Fact]
        public void Can_Use_Left_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Left(2);

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(TRIM(Field), 2)");
        }

        [Fact]
        public void Can_Use_Right_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Right(2);

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(Field, 2)");
        }

        [Fact]
        public void Can_Use_Right_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Right(2);

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(TRIM(Field), 2)");
        }

        [Fact]
        public void Can_Use_Year_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Year();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "YEAR(Field)");
        }

        [Fact]
        public void Can_Use_Year_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field1").Coalesce(new QueryExpressionBuilder().WithFieldName("Field2"));

            // Act
            var actual = sut.Year();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "YEAR(COALESCE(Field1, Field2))");
        }

        [Fact]
        public void Can_Use_Month_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Month();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "MONTH(Field)");
        }

        [Fact]
        public void Can_Use_Month_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field1").Coalesce(new QueryExpressionBuilder().WithFieldName("Field2"));

            // Act
            var actual = sut.Month();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "MONTH(COALESCE(Field1, Field2))");
        }

        [Fact]
        public void Can_Use_Day_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Day();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "DAY(Field)");
        }

        [Fact]
        public void Can_Use_Day_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field1").Coalesce("Field2");

            // Act
            var actual = sut.Day();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "DAY(COALESCE(Field1, Field2))");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_Basic_QueryExpression_With_FieldName()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Coalesce("default");

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_Basic_QueryExpression_With_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Coalesce(new QueryExpressionBuilder().WithFieldName("default"));

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Coalesce("default");

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(TRIM(Field), default)");
        }

        [Fact]
        public void Can_Nest_Multiple_Coalesce_Functions()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field1").Coalesce("Field2");

            // Act
            var actual = sut.Coalesce("Field3");

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(COALESCE(Field1, Field2), Field3)");
        }

        [Fact]
        public void Can_Use_Count_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Count();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(Field)");
        }

        [Fact]
        public void Can_Use_Count_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Count();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Sum_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field");

            // Act
            var actual = sut.Sum();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(Field)");
        }

        [Fact]
        public void Can_Use_Sum_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpressionBuilder().WithFieldName("Field").Trim();

            // Act
            var actual = sut.Sum();

            // Assert
            SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(TRIM(Field))");
        }
    }
}
