namespace ExpressionFramework.Core.ExpressionEvaluators;

public class FieldExpressionEvaluator : IExpressionEvaluator
{
    private readonly IValueProvider _valueProvider;
    private readonly IEnumerable<IFunctionEvaluator> _functionEvaluators;

    public FieldExpressionEvaluator(IValueProvider valueProvider, IEnumerable<IFunctionEvaluator> functionEvaluators)
    {
        _valueProvider = valueProvider;
        _functionEvaluators = functionEvaluators;
    }

    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (expression is IFieldExpression fieldExpression)
        {
            if (expression.Function != null)
            {
                foreach (var evaluator in _functionEvaluators)
                {
                    if (evaluator.TryEvaluate(expression.Function,
                                              _valueProvider.GetValue(item, fieldExpression.FieldName),
                                              callback,
                                              out var functionResult))
                    {
                        result = functionResult;
                        return true;
                    }
                }
                throw new ArgumentOutOfRangeException(nameof(expression), $"No evaluator found for function [{expression.Function.GetType().Name}]");
            }

            result = _valueProvider.GetValue(item, fieldExpression.FieldName);
            return true;
        }

        result = default;
        return false;
    }
}
