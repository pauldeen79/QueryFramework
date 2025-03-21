namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class ConstantExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

        // Assert
        actual.ShouldBeFalse();
        result.ShouldBeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_Value_When_Expression_Is_Of_Type_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expression = new ConstantExpressionBuilder().WithValue("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

        // Assert
        actual.ShouldBeTrue();
        result.ShouldBe("@p0");
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Null_When_Expression_Is_Not_Of_Type_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

        // Assert
        actual.ShouldBeFalse();
        result.ShouldBeNull();
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Length_Of_Value_When_Expression_Is_Of_Type_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expression = new ConstantExpressionBuilder().WithValue("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));

        // Act
        var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

        // Assert
        actual.ShouldBeTrue();
        result.ShouldBe("Test".Length.ToString());
    }
}
