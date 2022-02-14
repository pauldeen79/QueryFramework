namespace ExpressionFramework.Core.Default;

public class ExpressionEvaluatorCallback : IExpressionEvaluatorCallback
{
    private readonly IEnumerable<IExpressionEvaluator> _expressionEvaluators;

    public ExpressionEvaluatorCallback(IEnumerable<IExpressionEvaluator> expressionEvaluators)
        => _expressionEvaluators = expressionEvaluators;

    public object? Evaluate(object? item, IExpression expression)
    {
        foreach (var evaluator in _expressionEvaluators)
        {
            if (evaluator.TryEvaluate(item, expression, this, out var result))
            {
                return result;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
    }
}
