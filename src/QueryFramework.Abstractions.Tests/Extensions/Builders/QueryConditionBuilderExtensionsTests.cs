using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions.Builders
{
    [ExcludeFromCodeCoverage]
    public class QueryConditionBuilderExtensionsTests
    {
        private Mock<IQueryConditionBuilder> Sut { get; }
        private Mock<IQueryExpressionBuilder> FieldMock { get; }

        public QueryConditionBuilderExtensionsTests()
        {
            FieldMock = new Mock<IQueryExpressionBuilder>();
            Sut = new Mock<IQueryConditionBuilder>();
            Sut.SetupGet(x => x.Field).Returns(FieldMock.Object);
        }

        [Fact]
        public void WithOpenBracket_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithOpenBracket(true);

            // Assert
            Sut.VerifySet(x => x.OpenBracket = true, Times.Once);
        }

        [Fact]
        public void WithCloseBracket_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithCloseBracket(true);

            // Assert
            Sut.VerifySet(x => x.CloseBracket = true, Times.Once);
        }

        [Fact]
        public void WithField_QueryExpressionBuilder_Updates_Field()
        {
            // Arrange
            var updateFieldBuilderMock = new Mock<IQueryExpressionBuilder>();
            var function = new Mock<IQueryExpressionFunction>().Object;
            updateFieldBuilderMock.SetupGet(x => x.Function).Returns(function);
            updateFieldBuilderMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            Sut.Object.WithField(updateFieldBuilderMock.Object);

            // Assert
            Sut.VerifySet(x => x.Field = updateFieldBuilderMock.Object, Times.Once);
        }

        [Fact]
        public void WithField_String_Updates_Field()
        {
            // Act
            Sut.Object.WithField("fieldname");

            // Assert
            FieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithOperator_Updates_Operator()
        {
            // Act
            Sut.Object.WithOperator(QueryOperator.Contains);

            // Assert
            Sut.VerifySet(x => x.Operator = QueryOperator.Contains, Times.Once);
        }

        [Fact]
        public void WithValue_Updates_Value()
        {
            // Act
            Sut.Object.WithValue(12345);

            // Assert
            Sut.VerifySet(x => x.Value = 12345, Times.Once);
        }

        [Fact]
        public void WithCombination_Updates_Combination()
        {
            // Act
            Sut.Object.WithCombination(QueryCombination.Or);

            // Assert
            Sut.VerifySet(x => x.Combination = QueryCombination.Or, Times.Once);
        }
    }
}
