namespace ExpressionFramework.Core.Tests.Default;

public class ConditionEvaluatorTests
{
    private readonly Mock<IExpressionEvaluator> _evaluatorMock = new Mock<IExpressionEvaluator>();
    private ConditionEvaluator CreateSut() => new ConditionEvaluator(new[] { _evaluatorMock.Object });


}
