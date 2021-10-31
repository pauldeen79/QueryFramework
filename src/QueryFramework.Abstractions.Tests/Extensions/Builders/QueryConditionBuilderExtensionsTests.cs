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
        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);

            // Act
            sut.Object.Clear();

            // Assert
            sut.VerifySet(x => x.CloseBracket = default, Times.Once);
            sut.VerifySet(x => x.Combination = default, Times.Once);
            fieldMock.VerifySet(x => x.Expression = default, Times.Once);
            fieldMock.VerifySet(x => x.FieldName = string.Empty, Times.Once);
            sut.VerifySet(x => x.OpenBracket = default, Times.Once);
            sut.VerifySet(x => x.Operator = default, Times.Once);
            sut.VerifySet(x => x.Value = default, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);
            var updateMock = new Mock<IQueryCondition>();
            var updateFieldMock = new Mock<IQueryExpression>();
            updateMock.SetupGet(x => x.Field).Returns(updateFieldMock.Object);
            updateMock.SetupGet(x => x.CloseBracket).Returns(true);
            updateMock.SetupGet(x => x.Combination).Returns(QueryCombination.Or);
            updateMock.SetupGet(x => x.OpenBracket).Returns(true);
            updateMock.SetupGet(x => x.Operator).Returns(QueryOperator.Greater);
            updateMock.SetupGet(x => x.Value).Returns(12345);
            updateFieldMock.SetupGet(x => x.Expression).Returns("expression");
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            sut.Object.Update(updateMock.Object);

            // Assert
            sut.VerifySet(x => x.CloseBracket = true, Times.Once);
            sut.VerifySet(x => x.Combination = QueryCombination.Or, Times.Once);
            fieldMock.VerifySet(x => x.Expression = "expression", Times.Once);
            fieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
            sut.VerifySet(x => x.OpenBracket = true, Times.Once);
            sut.VerifySet(x => x.Operator = QueryOperator.Greater, Times.Once);
            sut.VerifySet(x => x.Value = 12345, Times.Once);
        }

        [Fact]
        public void WithOpenBracket_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();

            // Act
            sut.Object.WithOpenBracket(true);

            // Assert
            sut.VerifySet(x => x.OpenBracket = true, Times.Once);
        }

        [Fact]
        public void WithCloseBracket_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();

            // Act
            sut.Object.WithCloseBracket(true);

            // Assert
            sut.VerifySet(x => x.CloseBracket = true, Times.Once);
        }

        [Fact]
        public void WithField_QueryExpressionBuilder_Updates_Field()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);
            var updateFieldBuilderMock = new Mock<IQueryExpressionBuilder>();
            updateFieldBuilderMock.SetupGet(x => x.Expression).Returns("expression");
            updateFieldBuilderMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            sut.Object.WithField(updateFieldBuilderMock.Object);

            // Assert
            sut.VerifySet(x => x.Field = updateFieldBuilderMock.Object, Times.Once);
        }

        [Fact]
        public void WithField_QueryExpression_Updates_Field()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);
            var updateFieldMock = new Mock<IQueryExpression>();
            updateFieldMock.SetupGet(x => x.Expression).Returns("expression");
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            sut.Object.WithField(updateFieldMock.Object);

            // Assert
            fieldMock.VerifySet(x => x.Expression = "expression", Times.Once);
            fieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithField_String_Updates_Field()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);

            // Act
            sut.Object.WithField("fieldname");

            // Assert
            fieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithOperator_Updates_Operator()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();

            // Act
            sut.Object.WithOperator(QueryOperator.Contains);

            // Assert
            sut.VerifySet(x => x.Operator = QueryOperator.Contains, Times.Once);
        }

        [Fact]
        public void WithValue_Updates_Value()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();

            // Act
            sut.Object.WithValue(12345);

            // Assert
            sut.VerifySet(x => x.Value = 12345, Times.Once);
        }

        [Fact]
        public void WithCombination_Updates_Combination()
        {
            // Arrange
            var sut = new Mock<IQueryConditionBuilder>();

            // Act
            sut.Object.WithCombination(QueryCombination.Or);

            // Assert
            sut.VerifySet(x => x.Combination = QueryCombination.Or, Times.Once);
        }
    }
}
