namespace ExpressionFramework.Core.FunctionEvaluators;

public class ConditionFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        if (!(function is ConditionFunction c))
        {
            result = null;
            return false;
        }

        var fullResult = true;
        foreach (var condition in c.Conditions.Select((item, index) => new { Item = item, Index = index }))
        {
            if ((condition.Item.StartGroup && !condition.Item.EndGroup) || (condition.Item.EndGroup && !condition.Item.StartGroup))
            {
                //TODO: Add support for groups
                throw new NotSupportedException("Grouped conditions are not supported yet");
            }
            var isItemValid = IsItemValid(value, condition.Item, evaluator);
            fullResult = condition.Index == 0 || condition.Item.Combination == Combination.And
                ? fullResult && isItemValid
                : fullResult || isItemValid;
        }
        result = fullResult;
        return true;
    }

    private bool IsItemValid(object? item, ICondition condition, IExpressionEvaluator evaluator)
    {
        var leftValue = evaluator.Evaluate(item, condition.LeftExpression);
        var rightValue = evaluator.Evaluate(item, condition.RightExpression);

        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }
}
