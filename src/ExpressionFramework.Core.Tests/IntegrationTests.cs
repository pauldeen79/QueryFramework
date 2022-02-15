namespace ExpressionFramework.Core.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public IntegrationTests()
        => _serviceProvider = new ServiceCollection()
            .AddExpressionFramework()
            .BuildServiceProvider();

    [Fact]
    public void Can_Evaluate_Condition_With_Constant_Expressions_True()
    {
        // Arrange
        var sut = CreateSut();
        var condition = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .Build();

        // Act
        var actual = sut.IsItemValid(null, condition);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Can_Evaluate_Condition_With_Empty_Expressions_True()
    {
        // Arrange
        var sut = CreateSut();
        var condition = new ConditionBuilder()
            .WithLeftExpression(new EmptyExpressionBuilder())
            .WithOperator(Operator.Equal)
            .WithRightExpression(new EmptyExpressionBuilder())
            .Build();

        // Act
        var actual = sut.IsItemValid(null, condition);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Can_Evaluate_Condition_With_Delegate_Expressions_True()
    {
        // Arrange
        var sut = CreateSut();
        var condition = new ConditionBuilder()
            .WithLeftExpression(new DelegateExpressionBuilder().WithValue(() => "12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new DelegateExpressionBuilder().WithValue(() => "12345"))
            .Build();

        // Act
        var actual = sut.IsItemValid(null, condition);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Can_Evaluate_Condition_With_Different_Expressions_False()
    {
        // Arrange
        var sut = CreateSut();
        var condition = new ConditionBuilder()
            .WithLeftExpression(new EmptyExpressionBuilder())
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .Build();

        // Act
        var actual = sut.IsItemValid(null, condition);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Can_Evaluate_Condition_With_Constant_Expressions_And_Functions_True()
    {
        // Arrange
        var sut = CreateSut();
        var condition = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345").WithFunction(new LeftFunctionBuilder().WithLength(1)))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345").WithFunction(new RightFunctionBuilder().WithLength(1)))
            .Build();

        // Act
        var actual = sut.IsItemValid(null, condition);

        // Assert
        actual.Should().BeFalse();
    }

    private IConditionEvaluator CreateSut() => _serviceProvider.GetRequiredService<IConditionEvaluator>();

    public void Dispose() => _serviceProvider.Dispose();
}
