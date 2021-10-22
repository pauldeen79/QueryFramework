using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionExtensionsTests
    {
        [Fact]
        public void Can_Use_With_ExtensionMethod_To_Modify_QueryExpression()
        {
            // Arrange
            var sut = new QueryExpression("field", "expression");

            // Act
            var actual = sut.With("newname", "newexpression");

            // Assert
            actual.Expression.Should().Be("newexpression");
            actual.FieldName.Should().Be("newname");
        }

        [Fact]
        public void Can_Use_With_ExtensionMethod_With_DefaultValues_To_Create_Instance_With_Same_Values()
        {
            // Arrange
            var sut = new QueryExpression("field", "expression");

            // Act
            var actual = sut.With(null, null);

            // Assert
            actual.Expression.Should().Be(sut.Expression);
            actual.FieldName.Should().Be(sut.FieldName);
        }

        [Theory]
        [InlineData("field", null)]
        [InlineData("expression", "expression")]
        public void GetRawExpression_Return_Correct_Result(string inputExpression, string expectedOutputExpression)
        {
            // Arrange
            var sut = new QueryExpression("field", inputExpression);

            // Act
            var actual = sut.GetRawExpression();

            // Assert
            actual.Should().Be(expectedOutputExpression);
        }

        [Theory]
        [InlineData("Trim(FieldName)", true)]
        [InlineData("Trim(GO; DROP TABLE MyTable; GO;--)", false)]
        public void IsSingleWordFunction_Returns_Correct_Result(string inputExpression, bool expectedResult)
        {
            // Arrange
            var sut = new QueryExpression("field", inputExpression);

            // Act
            var actual = sut.IsSingleWordFunction();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Fact]
        public void ToBuilder_Returns_CustomBuilder_When_Available()
        {
            // Arrange
            var sut = new QueryExpressionMock();

            // Act
            var actual = sut.ToBuilder();

            // Assert
            actual.Should().BeOfType<QueryExpressionBuilderMock>();
        }

        [Fact]
        public void ToBuilder_Returns_DefaultBuilder_When_CustomBuilder_Is_Not_Available()
        {
            // Arrange
            var sut = new QueryExpression("fieldname");

            // Act
            var actual = sut.ToBuilder();

            // Assert
            actual.Should().BeOfType<QueryExpressionBuilder>();
        }

        [ExcludeFromCodeCoverage]
        private class QueryExpressionMock : ICustomQueryExpression
        {
            public string FieldName { get; set; }

            public string Expression { get; set; }

            public IQueryExpressionBuilder CreateBuilder()
            {
                return new QueryExpressionBuilderMock
                {
                    FieldName = FieldName,
                    Expression = Expression
                };
            }
        }

        [ExcludeFromCodeCoverage]
        private class QueryExpressionBuilderMock : IQueryExpressionBuilder
        {
            public string Expression { get; set; }
            public string FieldName { get; set; }

            public IQueryExpression Build()
            {
                return new QueryExpressionMock
                {
                    FieldName = FieldName,
                    Expression = Expression
                };
            }
        }
    }
}
