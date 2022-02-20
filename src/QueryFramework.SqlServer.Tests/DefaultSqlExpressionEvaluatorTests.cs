namespace QueryFramework.SqlServer.Tests;

public class DefaultSqlExpressionEvaluatorTests
{
    private readonly Mock<ISqlExpressionEvaluatorProvider> _expressionEvaluatorProviderMock = new Mock<ISqlExpressionEvaluatorProvider>();
    private readonly Mock<IFunctionParser> _functionParserMock = new Mock<IFunctionParser>();
    private readonly Mock<IExpression> _expressionMock = new Mock<IExpression>();
    private readonly Mock<IQueryFieldInfo> _fieldInfoMock = new Mock<IQueryFieldInfo>();
    private readonly ParameterBag _parameterBag = new ParameterBag();

    [Fact]
    public void GetSqlExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetSqlExpression(_expressionMock.Object, _fieldInfoMock.Object, _parameterBag))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [IExpressionProxy]");
    }

    [Fact]
    public void GetSqlExpression_Throws_On_Unsupported_Function()
    {
        // Arrange
        var functionMock = new Mock<IExpressionFunction>();
        _expressionMock.SetupGet(x => x.Function).Returns(functionMock.Object);
        var result = default(string);
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetSqlExpression(It.IsAny<IExpression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), out result))
                                        .Returns(true);
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetSqlExpression(_expressionMock.Object, _fieldInfoMock.Object, _parameterBag))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported function: [IExpressionFunctionProxy]");
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_Without_Function()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetSqlExpression(It.IsAny<IExpression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), out result))
                                        .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(_expressionMock.Object, _fieldInfoMock.Object, _parameterBag);

        // Assert
        actual.Should().Be(result);
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_With_Function()
    {
        // Arrange
        var functionMock = new Mock<IExpressionFunction>();
        _expressionMock.SetupGet(x => x.Function).Returns(functionMock.Object);
        var expressionResult = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetSqlExpression(It.IsAny<IExpression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), out expressionResult))
                                        .Returns(true);
        var functionResult = "Function({0})";
        _functionParserMock.Setup(x => x.TryParse(It.IsAny<IExpressionFunction>(), It.IsAny<ISqlExpressionEvaluator>(), out functionResult))
                           .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(_expressionMock.Object, _fieldInfoMock.Object, _parameterBag);

        // Assert
        actual.Should().Be("Function(result)");
    }

    [Fact]
    public void GetLengthExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetLengthExpression(_expressionMock.Object, _fieldInfoMock.Object))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [IExpressionProxy]");
    }

    [Fact]
    public void GetLengthExpression_Returns_Correct_Result_On_Supported_Expression()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetLengthExpression(It.IsAny<IExpression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), out result))
                                        .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetLengthExpression(_expressionMock.Object, _fieldInfoMock.Object);

        // Assert
        actual.Should().Be(result);
    }

    private ISqlExpressionEvaluator CreateSut() => new DefaultSqlExpressionEvaluator(new[] { _expressionEvaluatorProviderMock.Object },
                                                                                     new[] { _functionParserMock.Object });
}
