using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Observable;
using Xunit;

namespace QueryFramework.Core.Tests.Observable
{
    [ExcludeFromCodeCoverage]
    public class ObservableQuerySortOrderBuilderTests
    {
        [Fact]
        public void Can_Create_QuerySortOrder_From_Builder()
        {
            // Arrange
            var sut = new ObservableQuerySortOrderBuilder
            {
                Field = new ObservableQueryExpressionBuilder(fieldName: "fieldname", expression: "expression"),
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
            var actual = new ObservableQuerySortOrderBuilder(input);

            // Assert
            actual.Field.Expression.Should().Be(input.Field.Expression);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.Order.Should().Be(input.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_Values()
        {
            // Act
            var actual = new ObservableQuerySortOrderBuilder(expression: new QueryExpression(fieldName: "fieldname", expression: "expression"), order: QuerySortOrderDirection.Descending);

            // Assert
            actual.Field.Expression.Should().Be("expression");
            actual.Field.FieldName.Should().Be("fieldname");
            actual.Order.Should().Be(QuerySortOrderDirection.Descending);
        }

        [Fact]
        public void Can_Observe_Change_On_All_Properties()
        {
            // Arrange
            var sut = new ObservableQuerySortOrderBuilder();
            var changedPropertiesList = new List<string?>();
            sut.PropertyChanged += (sender, args) =>
            {
                changedPropertiesList.Add(args.PropertyName);
            };

            // Act
            sut.Field = new ObservableQueryExpressionBuilder();
            sut.Order = QuerySortOrderDirection.Descending;

            // Assert
            changedPropertiesList.Should().HaveCount(2);
            changedPropertiesList.ElementAt(0).Should().Be("Field");
            changedPropertiesList.ElementAt(1).Should().Be("Order");
        }
    }
}
