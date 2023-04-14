namespace QueryFramework.SqlServer.Tests;

public class DefaultSqlExpressionEvaluatorTests
{
    private readonly Mock<ISqlExpressionEvaluatorProvider> _expressionEvaluatorProviderMock = new();
    private readonly Mock<IFunctionParser> _functionParserMock = new();
    private readonly Mock<IQueryFieldInfo> _fieldInfoMock = new();
    private readonly ParameterBag _parameterBag = new();

    [Fact]
    public void GetSqlExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetSqlExpression(new EmptyExpression(), _fieldInfoMock.Object, _parameterBag, null))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [EmptyExpression]");
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_Without_Function()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetSqlExpression(It.IsAny<Expression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), It.IsAny<object?>(), out result))
                                        .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(new ConstantExpression(result), _fieldInfoMock.Object, _parameterBag, null);

        // Assert
        actual.Should().Be(result);
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_With_Function()
    {
        // Arrange
        var expressionResult = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetSqlExpression(It.IsAny<ConstantExpression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), It.IsAny<object?>(), out expressionResult))
                                        .Returns(true);
        var functionResult = "Function({0})";
        _functionParserMock.Setup(x => x.TryParse(It.IsAny<StringLengthExpression>(), It.IsAny<ISqlExpressionEvaluator>(), out functionResult))
                           .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(new StringLengthExpression(new ConstantExpression(expressionResult)), _fieldInfoMock.Object, _parameterBag, null);

        // Assert
        actual.Should().Be("Function(result)");
    }

    [Fact]
    public void GetLengthExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetLengthExpression(new EmptyExpression(), _fieldInfoMock.Object, null))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [EmptyExpression]");
    }

    [Fact]
    public void GetLengthExpression_Returns_Correct_Result_On_Supported_Expression()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.Setup(x => x.TryGetLengthExpression(It.IsAny<Expression>(), It.IsAny<ISqlExpressionEvaluator>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<object?>(), out result))
                                        .Returns(true);
        var sut = CreateSut();

        // Act
        var actual = sut.GetLengthExpression(new ConstantExpression(result), _fieldInfoMock.Object, null);

        // Assert
        actual.Should().Be(result);
    }

    private ISqlExpressionEvaluator CreateSut() => new DefaultSqlExpressionEvaluator(new[] { _expressionEvaluatorProviderMock.Object },
                                                                                     new[] { _functionParserMock.Object });
}
