namespace ExpressionFramework.Core.Default;

public class ExpressionEvaluatorCallback : IExpressionEvaluatorCallback
{
    private readonly IEnumerable<IExpressionEvaluator> _expressionEvaluators;
    private readonly IEnumerable<IFunctionEvaluator> _functionEvaluators;

    public ExpressionEvaluatorCallback(IEnumerable<IExpressionEvaluator> expressionEvaluators, IEnumerable<IFunctionEvaluator> functionEvaluators)
    {
        _expressionEvaluators = expressionEvaluators;
        _functionEvaluators = functionEvaluators;
    }

    public object? Evaluate(object? item, IExpression expression)
    {
        object? result = null;
        var handled = false;
        foreach (var evaluator in _expressionEvaluators)
        {
            if (evaluator.TryEvaluate(item, expression, this, out var evaluatorResult))
            {
                result = evaluatorResult;
                handled = true;
                break;
            }
        }

        if (!handled)
        {
            throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
        }

        if (expression.Function != null)
        {
            foreach (var evaluator in _functionEvaluators)
            {
                if (evaluator.TryEvaluate(expression.Function,
                                          result,
                                          this,
                                          out var functionResult))
                {
                    return functionResult;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported function: [{expression.Function.GetType().Name}]");
        }

        return result;
    }
}
