namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, default, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_ValueDelegate_Result_When_Expression_Is_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new DelegateExpressionBuilder().WithValue(new Func<object?, object?>(_ => "Test")).Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, default, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("@p0");
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Null_When_Expression_Is_Not_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, default, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetLengthExpression_Returns_ValueDelegate_Result_When_Expression_Is_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new DelegateExpressionBuilder().WithValue(new Func<object?, object?>(_ => "Test")).Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, default, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test".Length.ToString());
    }
}
