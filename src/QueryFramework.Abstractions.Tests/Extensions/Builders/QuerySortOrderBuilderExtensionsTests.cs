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
        private Mock<IQuerySortOrderBuilder> Sut { get; }
        private Mock<IQueryExpressionBuilder> FieldMock { get; }

        public QuerySortOrderBuilderExtensionsTests()
        {
            Sut = new Mock<IQuerySortOrderBuilder>();
            FieldMock = new Mock<IQueryExpressionBuilder>();
            Sut.SetupGet(x => x.Field).Returns(FieldMock.Object);
        }

        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Act
            Sut.Object.Clear();

            // Assert
            Sut.VerifySet(x => x.Order = default, Times.Once);
            FieldMock.VerifySet(x => x.Function = default, Times.Once);
            FieldMock.VerifySet(x => x.FieldName = string.Empty, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var updateMock = new Mock<IQuerySortOrder>();
            var updateFieldMock = new Mock<IQueryExpression>();
            var function = new Mock<IQueryExpressionFunction>().Object;
            updateMock.SetupGet(x => x.Field).Returns(updateFieldMock.Object);
            updateMock.SetupGet(x => x.Order).Returns(QuerySortOrderDirection.Descending);
            updateFieldMock.SetupGet(x => x.Function).Returns(function);
            updateFieldMock.SetupGet(x => x.FieldName).Returns("fieldname");

            // Act
            Sut.Object.Update(updateMock.Object);

            // Assert
            Sut.VerifySet(x => x.Order = QuerySortOrderDirection.Descending, Times.Once);
            FieldMock.VerifySet(x => x.Function = function, Times.Once);
            FieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }

        [Fact]
        public void WithOrder_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithOrder(QuerySortOrderDirection.Descending);

            // Assert
            Sut.VerifySet(x => x.Order = QuerySortOrderDirection.Descending, Times.Once);
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
    }
}
