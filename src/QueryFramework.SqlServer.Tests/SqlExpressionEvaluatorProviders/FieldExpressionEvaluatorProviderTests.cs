namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
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
    public void TryGetSqlExpression_Returns_FieldName_When_Expression_Is_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x.ToUpperInvariant());
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test".ToUpperInvariant());
    }

    [Fact]
    public void TryGetSqlExpression_Throws_On_Unknown_Field()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act & Assert
        sut.Invoking(x => x.TryGetSqlExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, parameterBag, out var result))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Expression contains unknown field [Test]");
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Null_When_Expression_Is_Not_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetLengthExpression_Returns_LEN_FieldName_When_Expression_Is_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();
        fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

        // Act
        var actual = sut.TryGetLengthExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("LEN(Test)");
    }

    [Fact]
    public void TryGetLengthExpression_Throws_On_Unknown_Field()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithFieldName("Test").Build();
        var evaluatorMock = new Mock<ISqlExpressionEvaluator>();
        var fieldInfoMock = new Mock<IQueryFieldInfo>();

        // Act & Assert
        sut.Invoking(x => x.TryGetLengthExpression(expression, evaluatorMock.Object, fieldInfoMock.Object, out var result))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Expression contains unknown field [Test]");

    }
}
