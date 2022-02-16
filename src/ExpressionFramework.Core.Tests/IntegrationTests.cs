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
        var returnValue = sut.TryEvaluate(new ConditionFunction(condition, null), null, CreateCallback(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
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
        var returnValue = sut.TryEvaluate(new ConditionFunction(condition, null), null, CreateCallback(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
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
        var returnValue = sut.TryEvaluate(new ConditionFunction(condition, null), null, CreateCallback(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
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
        var returnValue = sut.TryEvaluate(new ConditionFunction(condition, null), null, CreateCallback(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(false);
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
        var returnValue = sut.TryEvaluate(new ConditionFunction(condition, null), null, CreateCallback(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(false);
    }

    private ConditionFunctionEvaluator CreateSut() => new ConditionFunctionEvaluator();

    private IExpressionEvaluatorCallback CreateCallback() => _serviceProvider.GetRequiredService<IExpressionEvaluatorCallback>();

    public void Dispose() => _serviceProvider.Dispose();
}
