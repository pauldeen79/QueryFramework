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
        public void Clear_Clears_All_Properties()
        {
            // Act
            Sut.Object.Clear();

            // Assert
            Sut.VerifySet(x => x.CloseBracket = default, Times.Once);
            Sut.VerifySet(x => x.Combination = default, Times.Once);
            FieldMock.VerifySet(x => x.Function = default, Times.Once);
            FieldMock.VerifySet(x => x.FieldName = string.Empty, Times.Once);
            Sut.VerifySet(x => x.OpenBracket = default, Times.Once);
            Sut.VerifySet(x => x.Operator = default, Times.Once);
            Sut.VerifySet(x => x.Value = default, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var updateMock = new Mock<IQueryCondition>();
            var updateFieldMock = new Mock<IQueryExpression>();
            var function = new Mock<IQueryExpressionFunction>().Object;
            updateMock.SetupGet(x => x.Field).Returns(updateFieldMock.Object);
            updateMock.SetupGet(x => x.CloseBracket).Returns(true);
            updateMock.SetupGet(x => x.Combination).Returns(QueryCombination.Or);
            updateMock.SetupGet(x => x.OpenBracket).Returns(true);
            updateMock.SetupGet(x => x.Operator).Returns(QueryOperator.Greater);
            updateMock.SetupGet(x => x.Value).Returns(12345);
            updateFieldMock.SetupGet(x => x.Function).Returns(function);
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            Sut.Object.Update(updateMock.Object);

            // Assert
            Sut.VerifySet(x => x.CloseBracket = true, Times.Once);
            Sut.VerifySet(x => x.Combination = QueryCombination.Or, Times.Once);
            FieldMock.VerifySet(x => x.Function = function, Times.Once);
            FieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
            Sut.VerifySet(x => x.OpenBracket = true, Times.Once);
            Sut.VerifySet(x => x.Operator = QueryOperator.Greater, Times.Once);
            Sut.VerifySet(x => x.Value = 12345, Times.Once);
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
        public void WithField_QueryExpression_Updates_Field()
        {
            // Arrange
            var updateFieldMock = new Mock<IQueryExpression>();
            var function = new Mock<IQueryExpressionFunction>().Object;
            updateFieldMock.SetupGet(x => x.Function).Returns(function);
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            Sut.Object.WithField(updateFieldMock.Object);

            // Assert
            FieldMock.VerifySet(x => x.Function = function, Times.Once);
            FieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
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
