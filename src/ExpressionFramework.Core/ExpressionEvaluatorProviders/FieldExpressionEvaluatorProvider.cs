namespace ExpressionFramework.Core.ExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProvider : IExpressionEvaluatorProvider
{
    private readonly IValueProvider _valueProvider;

    public FieldExpressionEvaluatorProvider(IValueProvider valueProvider) => _valueProvider = valueProvider;

    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result)
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
