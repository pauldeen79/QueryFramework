﻿namespace QueryFramework.SqlServer.Tests.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryGetSqlExpression_Returns_Null_When_Expression_Is_Not_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
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
    public void TryGetSqlExpression_Returns_ValueDelegate_Result_When_Expression_Is_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new DelegateExpressionBuilder().WithValue(new Func<object?, object?>(_ => "Test")).Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));
        var parameterBag = new ParameterBag();

        // Act
        var actual = sut.TryGetSqlExpression(queryMock, expression, evaluatorMock, fieldInfoMock, parameterBag, out var result);

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
    public void TryGetLengthExpression_Returns_ValueDelegate_Result_When_Expression_Is_Of_Type_DelegateExpression()
    {
        // Arrange
        var sut = new DelegateExpressionEvaluatorProvider();
        var expression = new DelegateExpressionBuilder().WithValue(new Func<object?, object?>(_ => "Test")).Build();
        var queryMock = Substitute.For<IQuery>();
        var evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        var fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));

        // Act
        var actual = sut.TryGetLengthExpression(queryMock, expression, evaluatorMock, fieldInfoMock, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be("Test".Length.ToString());
    }
}
