namespace QueryFramework.SqlServer.Tests;

public class DefaultSqlExpressionEvaluatorTests
{
    private readonly ISqlExpressionEvaluatorProvider _expressionEvaluatorProviderMock = Substitute.For<ISqlExpressionEvaluatorProvider>();
    private readonly IFunctionParser _functionParserMock = Substitute.For<IFunctionParser>();
    private readonly IQueryFieldInfo _fieldInfoMock = Substitute.For<IQueryFieldInfo>();
    private readonly IQuery _queryMock = Substitute.For<IQuery>();
    private readonly ParameterBag _parameterBag = new();

    [Fact]
    public void GetSqlExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetSqlExpression(_queryMock, new EmptyExpression(), _fieldInfoMock, _parameterBag))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [EmptyExpression]");
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_Without_Function()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.TryGetSqlExpression(Arg.Any<IQuery>(), Arg.Any<Expression>(), Arg.Any<ISqlExpressionEvaluator>(), Arg.Any<IQueryFieldInfo>(), Arg.Any<ParameterBag>(), out Arg.Any<string?>())
                                        .Returns(x => { x[5] = result; return true; });
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(_queryMock, new ConstantExpression(result), _fieldInfoMock, _parameterBag);

        // Assert
        actual.Should().Be(result);
    }

    [Fact]
    public void GetSqlExpression_Returns_Correct_Result_On_Supported_Expression_With_Function()
    {
        // Arrange
        var expressionResult = "result";
        _expressionEvaluatorProviderMock.TryGetSqlExpression(Arg.Any<IQuery>(), Arg.Any<ConstantExpression>(), Arg.Any<ISqlExpressionEvaluator>(), Arg.Any<IQueryFieldInfo>(), Arg.Any<ParameterBag>(), out Arg.Any<string?>())
                                        .Returns(x => { x[5] = expressionResult; return true; });
        var functionResult = "Function({0})";
        _functionParserMock.TryParse(Arg.Any<StringLengthExpression>(), Arg.Any<ISqlExpressionEvaluator>(), out Arg.Any<string?>(), out Arg.Any<Expression>())
                           .Returns(x => { x[2] = functionResult; x[3] = x.ArgAt<StringLengthExpression>(0).Expression; return true; });
        var sut = CreateSut();

        // Act
        var actual = sut.GetSqlExpression(_queryMock, new StringLengthExpression(new TypedConstantExpression<string>(expressionResult)), _fieldInfoMock, _parameterBag);

        // Assert
        actual.Should().Be("Function(result)");
    }

    [Fact]
    public void GetLengthExpression_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        sut.Invoking(x => x.GetLengthExpression(_queryMock, new EmptyExpression(), _fieldInfoMock))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported expression: [EmptyExpression]");
    }

    [Fact]
    public void GetLengthExpression_Returns_Correct_Result_On_Supported_Expression()
    {
        // Arrange
        var result = "result";
        _expressionEvaluatorProviderMock.TryGetLengthExpression(Arg.Any<IQuery>(), Arg.Any<Expression>(), Arg.Any<ISqlExpressionEvaluator>(), Arg.Any<IQueryFieldInfo>(), out Arg.Any<string?>())
                                        .Returns(x => { x[4] = result; return true; });
        var sut = CreateSut();

        // Act
        var actual = sut.GetLengthExpression(_queryMock, new ConstantExpression(result), _fieldInfoMock);

        // Assert
        actual.Should().Be(result);
    }

    private DefaultSqlExpressionEvaluator CreateSut()
        => new DefaultSqlExpressionEvaluator(
            [_expressionEvaluatorProviderMock],
            [_functionParserMock]);
}
