using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
            var sut = new QuerySortOrderBuilder
            {
                Field = new QueryExpressionBuilder(fieldName: "fieldname", expression: "expression"),
                Order = QuerySortOrderDirection.Descending
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Field.FieldName.Should().Be(sut.Field.FieldName);
            actual.Field.Expression.Should().Be(sut.Field.Expression);
            actual.Order.Should().Be(sut.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_QuerySortOrder()
        {
            // Arrange
            var input = new QuerySortOrder
            (
                expression: new QueryExpression(fieldName: "fieldname", expression: "expression"),
                order: QuerySortOrderDirection.Descending
            );

            // Act
            var actual = new QuerySortOrderBuilder(input);

            // Assert
            actual.Field.Expression.Should().Be(input.Field.Expression);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.Order.Should().Be(input.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_Values()
        {
            // Act
            var actual = new QuerySortOrderBuilder(expression: new QueryExpression(fieldName: "fieldname", expression: "expression"), order: QuerySortOrderDirection.Descending);

            // Assert
            actual.Field.Expression.Should().Be("expression");
            actual.Field.FieldName.Should().Be("fieldname");
            actual.Order.Should().Be(QuerySortOrderDirection.Descending);
        }
    }
}
