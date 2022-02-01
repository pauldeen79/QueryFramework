namespace QueryFramework.InMemory;

public class ExpressionEvaluator<T> : IExpressionEvaluator<T>
    where T : class
{
    private IValueProvider ValueProvider { get; }

    public ExpressionEvaluator(IValueProvider valueProvider) => ValueProvider = valueProvider;

    public object? GetValue(T item, IQueryExpression field)
    {
        if (field.Function != null)
        {
            var functionName = GetFunctionName(field.Function, out var parameters);
            if (Functions.Items.TryGetValue(functionName, out var function))
            {
                var split = parameters.Split(',');
                var fieldName = split[0] == "{0}"
                    ? ValueProvider.GetFieldValue(item, field.FieldName)
                    : ValueProvider.GetFieldValue(item, split[0]);
                return function.Invoke(fieldName, split.Skip(1));
            }
            throw new ArgumentOutOfRangeException(nameof(field), $"Function [{field.Function.GetType().Name}] is not supported");
        }

        return ValueProvider.GetFieldValue(item, field.FieldName);
    }

    private static string GetFunctionName(IQueryExpressionFunction function, out string parameter)
    {
        parameter = "{0}";

        if (function is LengthFunction)
        {
            return "LEN";
        }

        if (function is LeftFunction l)
        {
            parameter = "{0}," + l.Length.ToString(CultureInfo.InvariantCulture);
            return "LEFT";
        }

        if (function is RightFunction r)
        {
            parameter = "{0}," + r.Length.ToString(CultureInfo.InvariantCulture);
            return "RIGHT";
        }

        if (function is UpperFunction)
        {
            return "UPPER";
        }

        if (function is LowerFunction)
        {
            return "LOWER";
        }

        if (function is TrimFunction)
        {
            return "TRIM";
        }

        // unknown, let the calling function handle this
        return string.Empty;
    }
}
