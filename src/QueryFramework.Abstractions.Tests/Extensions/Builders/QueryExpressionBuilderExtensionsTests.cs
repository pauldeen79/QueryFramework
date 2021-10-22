using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions.Builders
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionBuilderExtensionsTests
    {
        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryExpressionBuilder>();

            // Act
            sut.Object.Clear();

            // Assert
            sut.VerifySet(x => x.Expression = default, Times.Once);
            sut.VerifySet(x => x.FieldName = default, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryExpressionBuilder>();
            var updateMock = new Mock<IQueryExpression>();
            updateMock.SetupGet(x => x.Expression).Returns("expression");
            updateMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            sut.Object.Update(updateMock.Object);

            // Assert
            sut.VerifySet(x => x.Expression = "expression", Times.Once);
            sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithExpression_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryExpressionBuilder>();

            // Act
            sut.Object.WithExpression("expression");

            // Assert
            sut.VerifySet(x => x.Expression = "expression", Times.Once);
        }

        [Fact]
        public void WithFieldName_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryExpressionBuilder>();

            // Act
            sut.Object.WithFieldName("fieldname");

            // Assert
            sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}
