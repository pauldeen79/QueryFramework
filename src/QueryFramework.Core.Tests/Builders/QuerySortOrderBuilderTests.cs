using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
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
            var functionBuilderMock = new Mock<IQueryExpressionFunctionBuilder>();
            var function = new Mock<IQueryExpressionFunction>().Object;
            functionBuilderMock.Setup(x => x.Build()).Returns(function);
            var sut = new QuerySortOrderBuilder
            {
                Field = new QueryExpressionBuilder().WithFieldName("fieldname").WithFunction(functionBuilderMock.Object),
                Order = QuerySortOrderDirection.Descending
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Field.FieldName.Should().Be(sut.Field.FieldName);
            actual.Field.Function.Should().BeSameAs(function);
            actual.Order.Should().Be(sut.Order);
        }

        [Fact]
        public void Can_Create_QuerySortOrderBuilder_From_QuerySortOrder()
        {
            // Arrange
            var functionMock = new Mock<IQueryExpressionFunction>();
            var builder = new Mock<IQueryExpressionFunctionBuilder>().Object;
            functionMock.Setup(x => x.ToBuilder()).Returns(builder);
            var input = new QuerySortOrder
            (
                field: new QueryExpression(fieldName: "fieldname", function: functionMock.Object),
                order: QuerySortOrderDirection.Descending
            );

            // Act
            var actual = new QuerySortOrderBuilder(input);

            // Assert
            actual.Field.Function.Should().BeSameAs(builder);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.Order.Should().Be(input.Order);
        }
    }
}
