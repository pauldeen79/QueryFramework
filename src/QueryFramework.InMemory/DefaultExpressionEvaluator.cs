namespace QueryFramework.InMemory;

public class DefaultExpressionEvaluator : IExpressionEvaluator
{
    private readonly IValueProvider _valueProvider;
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public DefaultExpressionEvaluator(IValueProvider valueProvider, IEnumerable<IFunctionParser> functionParsers)
    {
        _valueProvider = valueProvider;
        _functionParsers = functionParsers;
    }

    public object? GetValue(object item, IQueryExpression field)
    {
        if (field.Function != null)
        {
            foreach (var parser in _functionParsers)
            {
                if (parser.TryParse(field.Function,
                                    _valueProvider.GetFieldValue(item, field.FieldName),
                                    field.FieldName,
                                    out var result))
                {
                    return result;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(field), $"Function [{field.Function.GetType().Name}] is not supported");
        }

        return _valueProvider.GetFieldValue(item, field.FieldName);
    }
}
