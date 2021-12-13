using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Core.Builders;
using Xunit;

namespace QueryFramework.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class QuerySortOrderBuilderTests
    {
        [Fact]
        public void Can_Create_QuerySortOrder_From_Builder()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var sut = new QuerySortOrderBuilder
            {
                Field = new QueryExpressionBuilder(fieldName: "fieldname", function: function),
                Order = QuerySortOrderDirection.Descending
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Field.FieldName.Should().Be(sut.Field.FieldName);
            actual.Field.Function.Should().BeSameAs(sut.Field.Function);
            actual.Order.Should().Be(sut.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_QuerySortOrder()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var input = new QuerySortOrder
            (
                expression: new QueryExpression(fieldName: "fieldname", function: function),
                order: QuerySortOrderDirection.Descending
            );

            // Act
            var actual = new QuerySortOrderBuilder(input);

            // Assert
            actual.Field.Function.Should().BeSameAs(input.Field.Function);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.Order.Should().Be(input.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_Values()
        {
            // Act
            var function = new Mock<IQueryExpressionFunction>().Object;
            var actual = new QuerySortOrderBuilder(expression: new QueryExpression(fieldName: "fieldname", function: function),
                                                   order: QuerySortOrderDirection.Descending);

            // Assert
            actual.Field.Function.Should().BeSameAs(function);
            actual.Field.FieldName.Should().Be("fieldname");
            actual.Order.Should().Be(QuerySortOrderDirection.Descending);
        }
    }
}
