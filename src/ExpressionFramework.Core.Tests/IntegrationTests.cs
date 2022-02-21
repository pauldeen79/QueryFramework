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
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition }, null), null, CreateEvaluator(), out var result);

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
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition }, null), null, CreateEvaluator(), out var result);

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
            .WithLeftExpression(new DelegateExpressionBuilder().WithValueDelegate((_, _, _) => "12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new DelegateExpressionBuilder().WithValueDelegate((_, _, _) => "12345"))
            .Build();

        // Act
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition }, null), null, CreateEvaluator(), out var result);

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
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition }, null), null, CreateEvaluator(), out var result);

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
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition }, null), null, CreateEvaluator(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(false);
    }

    [Fact]
    public void Can_Evaluate_Multiple_Conditions_With_And_Combination()
    {
        // Arrange
        var sut = CreateSut();
        var condition1 = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .Build();
        var condition2 = new ConditionBuilder()
            .WithCombination(Combination.And)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .Build();

        // Act
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition1, condition2 }, null), null, CreateEvaluator(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
    }

    [Fact]
    public void Can_Evaluate_Multiple_Conditions_With_Or_Combination()
    {
        // Arrange
        var sut = CreateSut();
        var condition1 = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .Build();
        var condition2 = new ConditionBuilder()
            .WithCombination(Combination.Or)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("wrong"))
            .Build();

        // Act
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition1, condition2 }, null), null, CreateEvaluator(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
    }

    [Fact]
    public void Can_Evaluate_Multiple_Conditions_With_Group_And_Different_Combinations_1()
    {
        // Arrange
        var sut = CreateSut();
        //This translates to: True&(False|True) -> True
        var condition1 = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .Build();
        var condition2 = new ConditionBuilder()
            .WithStartGroup()
            .WithCombination(Combination.And)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("wrong"))
            .Build();
        var condition3 = new ConditionBuilder()
            .WithEndGroup()
            .WithCombination(Combination.Or)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .Build();

        // Act
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition1, condition2, condition3 }, null), null, CreateEvaluator(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
    }

    [Fact]
    public void Can_Evaluate_Multiple_Conditions_With_Group_And_Different_Combinations_2()
    {
        // Arrange
        var sut = CreateSut();
        //This translates to: False|(True&True) -> True
        var condition1 = new ConditionBuilder()
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("12345"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("wrong"))
            .Build();
        var condition2 = new ConditionBuilder()
            .WithStartGroup()
            .WithCombination(Combination.Or)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .Build();
        var condition3 = new ConditionBuilder()
            .WithEndGroup()
            .WithCombination(Combination.And)
            .WithLeftExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .WithOperator(Operator.Equal)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue("54321"))
            .Build();

        // Act
        var returnValue = sut.TryEvaluate(new ConditionFunction(new[] { condition1, condition2, condition3 }, null), null, CreateEvaluator(), out var result);

        // Assert
        returnValue.Should().BeTrue();
        result.Should().Be(true);
    }

    private ConditionFunctionEvaluator CreateSut() => new ConditionFunctionEvaluator();

    private IExpressionEvaluator CreateEvaluator() => _serviceProvider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose() => _serviceProvider.Dispose();
}
