using System;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;
using QueryFramework.SqlServer.Tests.Fixtures;
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
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Len();

            // Assert
            ExpressionSqlShouldBe(actual, "LEN(Field)");
        }

        [Fact]
        public void Can_Use_Len_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Len();

            // Assert
            ExpressionSqlShouldBe(actual, "LEN(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Trim_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Trim();

            // Assert
            ExpressionSqlShouldBe(actual, "TRIM(Field)");
        }

        [Fact]
        public void Can_Use_Trim_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Upper();

            // Act
            var actual = sut.Trim();

            // Assert
            ExpressionSqlShouldBe(actual, "TRIM(UPPER(Field))");
        }

        [Fact]
        public void Can_Use_Upper_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Upper();

            // Assert
            ExpressionSqlShouldBe(actual, "UPPER(Field)");
        }

        [Fact]
        public void Can_Use_Upper_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Upper();

            // Assert
            ExpressionSqlShouldBe(actual, "UPPER(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Lower_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Lower();

            // Assert
            ExpressionSqlShouldBe(actual, "LOWER(Field)");
        }

        [Fact]
        public void Can_Use_Lower_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Lower();

            // Assert
            ExpressionSqlShouldBe(actual, "LOWER(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Left_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Left(2);

            // Assert
            ExpressionSqlShouldBe(actual, "LEFT(Field, 2)");
        }

        [Fact]
        public void Can_Use_Left_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Left(2);

            // Assert
            ExpressionSqlShouldBe(actual, "LEFT(TRIM(Field), 2)");
        }

        [Fact]
        public void Can_Use_Right_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Right(2);

            // Assert
            ExpressionSqlShouldBe(actual, "RIGHT(Field, 2)");
        }

        [Fact]
        public void Can_Use_Right_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Right(2);

            // Assert
            ExpressionSqlShouldBe(actual, "RIGHT(TRIM(Field), 2)");
        }

        [Fact]
        public void Can_Use_Year_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Year();

            // Assert
            ExpressionSqlShouldBe(actual, "YEAR(Field)");
        }

        [Fact]
        public void Can_Use_Year_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Coalesce(new QueryExpression("Field", "TODAY()"));

            // Act
            var actual = sut.Year();

            // Assert
            ExpressionSqlShouldBe(actual, "YEAR(COALESCE(Field, TODAY()))");
        }

        [Fact]
        public void Can_Use_Month_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Month();

            // Assert
            ExpressionSqlShouldBe(actual, "MONTH(Field)");
        }

        [Fact]
        public void Can_Use_Month_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Coalesce(new QueryExpression("Field", "TODAY()"));

            // Act
            var actual = sut.Month();

            // Assert
            ExpressionSqlShouldBe(actual, "MONTH(COALESCE(Field, TODAY()))");
        }

        [Fact]
        public void Can_Use_Day_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Day();

            // Assert
            ExpressionSqlShouldBe(actual, "DAY(Field)");
        }

        [Fact]
        public void Can_Use_Day_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Coalesce(new QueryExpression("Field", "TODAY()"));

            // Act
            var actual = sut.Day();

            // Assert
            ExpressionSqlShouldBe(actual, "DAY(COALESCE(Field, TODAY()))");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_Basic_QueryExpression_With_FieldName()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Coalesce("default");

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_Basic_QueryExpression_With_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Coalesce(new QueryExpression("default"));

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
        }

        [Fact]
        public void Can_Use_Coalesce_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Coalesce("default");

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(TRIM(Field), default)");
        }

        [Fact]
        public void Can_Nest_Multiple_Coalesce_Funcions()
        {
            // Arrange
            var sut = new QueryExpression("Field1").Coalesce("Field2");

            // Act
            var actual = sut.Coalesce("Field3");

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(COALESCE(Field1, Field2), Field3)");
        }

        [Fact]
        public void Can_Use_Count_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Count();

            // Assert
            ExpressionSqlShouldBe(actual, "COUNT(Field)");
        }

        [Fact]
        public void Can_Use_Count_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Count();

            // Assert
            ExpressionSqlShouldBe(actual, "COUNT(TRIM(Field))");
        }

        [Fact]
        public void Can_Use_Sum_Function_On_Basic_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("Field");

            // Act
            var actual = sut.Sum();

            // Assert
            ExpressionSqlShouldBe(actual, "SUM(Field)");
        }

        [Fact]
        public void Can_Use_Sum_Function_On_QueryExpression_With_Existing_Expression()
        {
            // Arrange
            var sut = new QueryExpression("Field").Trim();

            // Act
            var actual = sut.Sum();

            // Assert
            ExpressionSqlShouldBe(actual, "SUM(TRIM(Field))");
        }

        private static void ExpressionSqlShouldBe(IQueryExpression expression, string expectedSqlForExpression)
        {
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            var mapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            mapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(connection, mapperMock.Object, new QueryProcessorSettings(tableName: "Table"), new DatabaseCommandGenerator());
            var query = new SingleEntityQuery(new[] { new QueryCondition(expression, QueryOperator.Equal, "test") });
            sut.FindMany(query);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be($"SELECT * FROM Table WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
