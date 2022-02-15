namespace ExpressionFramework.Core.ExpressionEvaluators;

public class FieldExpressionEvaluator : IExpressionEvaluator
{
    private readonly IValueProvider _valueProvider;

    public FieldExpressionEvaluator(IValueProvider valueProvider) => _valueProvider = valueProvider;

    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (expression is IFieldExpression fieldExpression)
        {
            result = _valueProvider.GetValue(item, fieldExpression.FieldName);
            return true;
        }

        result = default;
        return false;
    }
}
