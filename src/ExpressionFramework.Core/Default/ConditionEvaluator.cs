namespace ExpressionFramework.Core.Default;

public class ConditionEvaluator : IConditionEvaluator
{
    private readonly IEnumerable<IExpressionEvaluator> _expressionEvaluators;

    public ConditionEvaluator(IEnumerable<IExpressionEvaluator> expressionEvaluators)
        => _expressionEvaluators = expressionEvaluators;

    public bool IsItemValid(object? item, ICondition condition)
    {
        var leftValue = Evaluate(item, condition.LeftExpression, nameof(condition), "left");
        var rightValue = Evaluate(item, condition.RightExpression, nameof(condition), "right");
        
        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }

    private object? Evaluate(object? item, IExpression expression, string paramName, string expressionName)
    {
        foreach (var evaluator in _expressionEvaluators)
        {
            if (evaluator.TryEvaluate(item, expression, out var result))
            {
                return result;
            }
        }

        throw new ArgumentOutOfRangeException(paramName, $"Unsupported {expressionName} expression in condition: [{expression.GetType().Name}]");
    }
}
