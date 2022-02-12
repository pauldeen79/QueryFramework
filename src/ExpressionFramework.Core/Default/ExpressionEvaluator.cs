namespace ExpressionFramework.Core.Default;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IValueProvider _valueProvider;
    private readonly IEnumerable<IFunctionEvaluator> _functionEvaluators;

    public ExpressionEvaluator(IValueProvider valueProvider, IEnumerable<IFunctionEvaluator> functionEvaluators)
    {
        _valueProvider = valueProvider;
        _functionEvaluators = functionEvaluators;
    }

    public object? GetValue(object item, IExpression field)
    {
        if (field.Function != null)
        {
            foreach (var evaluator in _functionEvaluators)
            {
                if (evaluator.TryEvaluate(field.Function,
                                          _valueProvider.GetValue(item, field.FieldName),
                                          field.FieldName,
                                          out var result))
                {
                    return result;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(field), $"No evaluator foudn for function [{field.Function.GetType().Name}]");
        }

        return _valueProvider.GetValue(item, field.FieldName);
    }
}
