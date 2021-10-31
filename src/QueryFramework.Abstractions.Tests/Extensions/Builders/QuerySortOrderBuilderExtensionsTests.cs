using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions.Builders
{
    [ExcludeFromCodeCoverage]
    public class QuerySortOrderBuilderExtensionsTests
    {
        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQuerySortOrderBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);

            // Act
            sut.Object.Clear();

            // Assert
            sut.VerifySet(x => x.Order = default, Times.Once);
            fieldMock.VerifySet(x => x.Expression = default, Times.Once);
            fieldMock.VerifySet(x => x.FieldName = string.Empty, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQuerySortOrderBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);
            var updateMock = new Mock<IQuerySortOrder>();
            var updateFieldMock = new Mock<IQueryExpression>();
            updateMock.SetupGet(x => x.Field).Returns(updateFieldMock.Object);
            updateMock.SetupGet(x => x.Order).Returns(QuerySortOrderDirection.Descending);
            updateFieldMock.SetupGet(x => x.Expression).Returns("expression");
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            sut.Object.Update(updateMock.Object);

            // Assert
            sut.VerifySet(x => x.Order = QuerySortOrderDirection.Descending, Times.Once);
            fieldMock.VerifySet(x => x.Expression = "expression", Times.Once);
            fieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithOrder_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQuerySortOrderBuilder>();

            // Act
            sut.Object.WithOrder(QuerySortOrderDirection.Descending);

            // Assert
            sut.VerifySet(x => x.Order = QuerySortOrderDirection.Descending, Times.Once);
        }

        [Fact]
        public void WithField_QueryExpressionBuilder_Updates_Field()
        {
            // Arrange
            var sut = new Mock<IQuerySortOrderBuilder>();
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
            var sut = new Mock<IQuerySortOrderBuilder>();
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
            var sut = new Mock<IQuerySortOrderBuilder>();
            var fieldMock = new Mock<IQueryExpressionBuilder>();
            sut.SetupGet(x => x.Field).Returns(fieldMock.Object);

            // Act
            sut.Object.WithField("fieldname");

            // Assert
            fieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}
