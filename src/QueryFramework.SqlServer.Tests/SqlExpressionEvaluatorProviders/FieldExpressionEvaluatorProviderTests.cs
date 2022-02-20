namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_OfType_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, -1, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_FieldName_When_Function_Is_Null()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, -1, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test");
    }
}
