namespace QueryFramework.Core.Tests.Builders;

public class QueryExpressionBuilderTests
{
    [Fact]
    public void Can_Create_QueryExpression_From_Builder()
    {
        // Arrange
        var function = new Mock<IQueryExpressionFunction>().Object;
        var functionBuilderMock = new Mock<IQueryExpressionFunctionBuilder>();
        functionBuilderMock.Setup(x => x.Build()).Returns(function);

        var sut = new QueryExpressionBuilder
        {
            Function = functionBuilderMock.Object,
            FieldName = "fieldname"
        };

        // Act
        var actual = sut.Build();

        // Assert
        actual.Function.Should().BeSameAs(function);
        actual.FieldName.Should().Be(sut.FieldName);
    }

    [Fact]
    public void Can_Create_QueryExpressionBuilder_From_QueryExpression()
    {
        // Arrange
        var functionMock = new Mock<IQueryExpressionFunction>();
        var builder = new Mock<IQueryExpressionFunctionBuilder>().Object;
        functionMock.Setup(x => x.ToBuilder()).Returns(builder);
        var input = new QueryExpression
        (
            function: functionMock.Object,
            fieldName: "fieldname"
        );

        // Act
        var actual = new QueryExpressionBuilder(input);

        // Assert
        actual.Function.Should().BeSameAs(builder);
        actual.FieldName.Should().Be(input.FieldName);
    }
}
