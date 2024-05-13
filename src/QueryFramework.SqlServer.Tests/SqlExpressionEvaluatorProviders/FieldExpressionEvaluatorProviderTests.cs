﻿namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetSqlExpression_Returns_FieldName_When_Expression_Is_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0).ToUpperInvariant());
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test".ToUpperInvariant());
    }

    [Fact]
    public void TryGetSqlExpression_Throws_On_Unknown_Field()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(default(string));
        var parameterBag = new ParameterBag();

        // Act & Assert
        sut.Invoking(x => x.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Expression contains unknown field [Test]");
    }

    [Fact]
    public void TryGetLengthExpression_Returns_Null_When_Expression_Is_Not_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new EmptyExpressionBuilder().Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();

        // Act
        var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetLengthExpression_Returns_LEN_FieldName_When_Expression_Is_Of_Type_FieldExpression()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));

        // Act
        var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("LEN(Test)");
    }

    [Fact]
    public void TryGetLengthExpression_Throws_On_Unknown_Field()
    {
        // Arrange
        var sut = new FieldExpressionEvaluatorProvider();
        var expression = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Test").Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(default(string));

        // Act & Assert
        sut.Invoking(x => x.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Expression contains unknown field [Test]");
    }
}
