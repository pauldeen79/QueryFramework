namespace QueryFramework.SqlServer.Tests;

public class DefaultQueryExpressionEvaluatorTests
{
    [Fact]
    public void GetSqlExpression_Returns_FieldName_When_Function_Is_Null()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new DefaultQueryExpressionEvaluator(new[] { functionParserMock.Object });
        var expression = new QueryExpressionBuilder().WithFieldName("Test").Build();

        // Act
        var actual = sut.GetSqlExpression(expression);

        // Assert
        actual.Should().Be("Test");
    }

    [Fact]
    public void GetSqlExpression_Returns_Function_When_Function_Is_Not_Null()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new DefaultQueryExpressionEvaluator(new[] { functionParserMock.Object });
        var sqlExpression = "LEN(Test)";
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IQueryExpressionFunction>(), It.IsAny<IQueryExpressionEvaluator>(), out sqlExpression)).Returns(true);
        var queryExpressionFunctionBuilderMock = new Mock<IQueryExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IQueryExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new QueryExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();

        // Act
        var actual = sut.GetSqlExpression(expression);

        // Assert
        actual.Should().Be(sqlExpression);
    }
}
