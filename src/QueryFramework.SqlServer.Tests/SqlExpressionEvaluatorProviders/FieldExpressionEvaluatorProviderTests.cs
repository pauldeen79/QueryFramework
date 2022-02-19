namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_OfType_FieldExpression()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new FieldExpressionEvaluatorProvider(new[] { functionParserMock.Object });
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_FieldName_When_Function_Is_Null()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new FieldExpressionEvaluatorProvider(new[] { functionParserMock.Object });
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test");
    }

    [Fact]
    public void TryGetSqlExpression_Returns_Function_When_Function_Is_Not_Null()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new FieldExpressionEvaluatorProvider(new[] { functionParserMock.Object });
        var sqlExpression = "LEN({0})";
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IExpressionFunction>(), It.IsAny<ISqlExpressionEvaluator>(), out sqlExpression)).Returns(true);
        var queryExpressionFunctionBuilderMock = new Mock<IExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new FieldExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be(string.Format(sqlExpression, expression.GetFieldName()));
    }

    [Fact]
    public void GetSqlExpression_Throws_On_Unknown_Function()
    {
        // Arrange
        var functionParserMock = new Mock<IFunctionParser>();
        var sut = new FieldExpressionEvaluatorProvider(new[] { functionParserMock.Object });
        var sqlExpression = string.Empty;
        functionParserMock.Setup(x => x.TryParse(It.IsAny<IExpressionFunction>(), It.IsAny<ISqlExpressionEvaluator>(), out sqlExpression)).Returns(false);
        var queryExpressionFunctionBuilderMock = new Mock<IExpressionFunctionBuilder>();
        var queryExpressionFunctionMock = new Mock<IExpressionFunction>();
        queryExpressionFunctionBuilderMock.Setup(x => x.Build()).Returns(queryExpressionFunctionMock.Object);
        var expression = new FieldExpressionBuilder().WithFieldName("Test")
                                                     .WithFunction(queryExpressionFunctionBuilderMock.Object)
                                                     .Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();

        // Act & Assert
        sut.Invoking(x => x.TryGetSqlExpression(expression, evaluatorMock.Object, out _))
           .Should().ThrowExactly<ArgumentException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported function: IExpressionFunctionProxy");
    }
}
