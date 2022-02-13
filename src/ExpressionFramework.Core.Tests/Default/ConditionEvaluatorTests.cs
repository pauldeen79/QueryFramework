namespace ExpressionFramework.Core.Tests.Default;

public class ConditionEvaluatorTests
{
    private readonly Mock<IExpressionEvaluator> _evaluatorMock = new Mock<IExpressionEvaluator>();
    private ConditionEvaluator CreateSut() => new ConditionEvaluator(new[] { _evaluatorMock.Object });

    [Fact]
    public void AreItemsValid_Returns_True_When_No_Conditions_Are_Present()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var actual = sut.AreItemsValid(this, new List<ICondition>().AsReadOnly(), Combination.And);

        // Assert
        actual.Should().BeTrue();
    }
}
