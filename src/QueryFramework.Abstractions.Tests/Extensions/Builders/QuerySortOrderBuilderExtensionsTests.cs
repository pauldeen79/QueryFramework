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
        public void WithField_String_Updates_Field()
        {
            // Act
            Sut.Object.WithField("fieldname");

            // Assert
            FieldMock.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}
