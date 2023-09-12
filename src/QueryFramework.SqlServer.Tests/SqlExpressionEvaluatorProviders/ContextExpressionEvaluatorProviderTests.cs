namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class ContextExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_ContextExpression()
    {
        // Arrange
        var sut = new ContextExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock, fieldInfoMock, parameterBag, default, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_Value_When_Expression_Is_Of_Type_ContextExpression()
    {
        // Arrange
        var sut = new ContextExpressionEvaluatorProvider();
        var expression = new ContextExpressionBuilder().Build();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock, fieldInfoMock, parameterBag, "Test", out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("@p0");
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Null_When_Expression_Is_Not_Of_Type_ContextExpression()
    {
        // Arrange
        var sut = new ContextExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock, fieldInfoMock, default, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Length_Of_Value_When_Expression_Is_Of_Type_ContextExpression()
    {
        // Arrange
        var sut = new ContextExpressionEvaluatorProvider();
        var expression = new ContextExpressionBuilder().Build();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock, fieldInfoMock, "Test", out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test".Length.ToString());
    }
}
