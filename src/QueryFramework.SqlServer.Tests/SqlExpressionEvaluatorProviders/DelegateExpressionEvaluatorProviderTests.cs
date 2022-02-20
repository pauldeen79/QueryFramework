namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_DelegateExpression()
    {
        // Arrange
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();
        var sut = new DelegateExpressionEvaluatorProvider(expressionEvaluatorMock.Object);
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_FieldName_When_Expression_Is_Of_Type_DelegateExpression()
    {
        // Arrange
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();
        var sut = new DelegateExpressionEvaluatorProvider(expressionEvaluatorMock.Object);
        var expression = new DelegateExpressionBuilder().WithValueDelegate(new Func<object?, IExpression, IExpressionEvaluator, object?>((_, _, _) => "Test")).Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("@p0");
    }
}
