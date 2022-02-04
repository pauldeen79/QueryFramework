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
        var sqlExpression = "LEN({0})";
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
        actual.Should().Be(string.Format(sqlExpression, expression.FieldName));
    }

    [Fact]
    public void GetSqlExpression_Throws_On_Unknown_Function()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new DefaultQueryExpressionEvaluator(new[] { functionParserMock.Object });
        var sqlExpression = string.Empty;
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IQueryExpressionFunction>(), It.IsAny<IQueryExpressionEvaluator>(), out sqlExpression)).Returns(false);
        var queryExpressionFunctionBuilderMock = new Mock<IQueryExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IQueryExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new QueryExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();

        // Act & Assert
        sut.Invoking(x => x.GetSqlExpression(expression))
           .Should().ThrowExactly<ArgumentException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported function: IQueryExpressionFunctionProxy");
    }
}
