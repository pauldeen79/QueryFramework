namespace ExpressionFramework.Core.FunctionEvaluators;

public class CoalesceFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (!(function is CoalesceFunction c))
        {
            result = null;
            return false;
        }

        result = null;
        foreach (var expression in c.InnerExpressions)
        {
            var expressionResult = callback.Evaluate(value, expression);
            if (expressionResult != null)
            {
                result = expressionResult;
                break;
            }
        }
        return true;
    }
}
