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
        private Mock<IQueryExpressionBuilder> Sut { get; }

        public QueryExpressionBuilderExtensionsTests()
        {
            Sut = new Mock<IQueryExpressionBuilder>();
        }

        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Act
            Sut.Object.Clear();

            // Assert
            Sut.VerifySet(x => x.Expression = default, Times.Once);
            Sut.VerifySet(x => x.FieldName = string.Empty, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var updateMock = new Mock<IQueryExpression>();
            updateMock.SetupGet(x => x.Expression).Returns("expression");
            updateMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            Sut.Object.Update(updateMock.Object);

            // Assert
            Sut.VerifySet(x => x.Expression = "expression", Times.Once);
            Sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithExpression_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithExpression("expression");

            // Assert
            Sut.VerifySet(x => x.Expression = "expression", Times.Once);
        }

        [Fact]
        public void WithFieldName_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithFieldName("fieldname");

            // Assert
            Sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}
