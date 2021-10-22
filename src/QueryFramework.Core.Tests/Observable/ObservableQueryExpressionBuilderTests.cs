using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Core.Observable;
using Xunit;

namespace QueryFramework.Core.Tests.Observable
{
    [ExcludeFromCodeCoverage]
    public class ObservableQueryExpressionBuilderTests
    {
        [Fact]
        public void Can_Create_QueryExpression_From_Builder()
        {
            // Arrange
            var sut = new ObservableQueryExpressionBuilder
            {
                Expression = "expression",
                FieldName = "fieldname"
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Expression.Should().Be(sut.Expression);
            actual.FieldName.Should().Be(sut.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_QueryExpression()
        {
            // Arrange
            var input = new QueryExpression
            (
                expression: "expression",
                fieldName: "fieldname"
            );

            // Act
            var actual = new ObservableQueryExpressionBuilder(input);

            // Assert
            actual.Expression.Should().Be(input.Expression);
            actual.FieldName.Should().Be(input.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_Values()
        {
            // Act
            var actual = new ObservableQueryExpressionBuilder(expression: "expression", fieldName: "fieldname");

            // Assert
            actual.Expression.Should().Be("expression");
            actual.FieldName.Should().Be("fieldname");
        }

        [Fact]
        public void Can_Observe_Change_On_All_Properties()
        {
            // Arrange
            var sut = new ObservableQueryExpressionBuilder();
            var changedPropertiesList = new List<string>();
            sut.PropertyChanged += (sender, args) =>
            {
                changedPropertiesList.Add(args.PropertyName);
            };

            // Act
            sut.Expression = "expression";
            sut.FieldName = "field";

            // Assert
            changedPropertiesList.Should().HaveCount(2);
            changedPropertiesList.ElementAt(0).Should().Be("Expression");
            changedPropertiesList.ElementAt(1).Should().Be("FieldName");
        }
    }
}
