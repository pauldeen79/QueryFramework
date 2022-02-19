namespace QueryFramework.SqlServer.Tests;

public class DefaultSqlExpressionEvaluatorTests
{
    [Fact]
    public void GetSqlExpression_Returns_FieldName_When_Function_Is_Null()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new DefaultSqlExpressionEvaluator(new[] { functionParserMock.Object });
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();

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
        var sut = new DefaultSqlExpressionEvaluator(new[] { functionParserMock.Object });
        var sqlExpression = "LEN({0})";
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IExpressionFunction>(), It.IsAny<ISqlExpressionEvaluator>(), out sqlExpression)).Returns(true);
        var queryExpressionFunctionBuilderMock = new Mock<IExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new FieldExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();

        // Act
        var actual = sut.GetSqlExpression(expression);

        // Assert
        actual.Should().Be(string.Format(sqlExpression, expression.GetFieldName()));
    }

    [Fact]
    public void GetSqlExpression_Throws_On_Unknown_Function()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new DefaultSqlExpressionEvaluator(new[] { functionParserMock.Object });
        var sqlExpression = string.Empty;
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IExpressionFunction>(), It.IsAny<ISqlExpressionEvaluator>(), out sqlExpression)).Returns(false);
        var queryExpressionFunctionBuilderMock = new Mock<IExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new FieldExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();

        // Act & Assert
        sut.Invoking(x => x.GetSqlExpression(expression))
           .Should().ThrowExactly<ArgumentException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported function: IExpressionFunctionProxy");
    }
}
