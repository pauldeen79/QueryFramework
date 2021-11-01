using System;
using System.Linq;
using QueryFramework.Abstractions;

namespace QueryFramework.InMemory
{
    public class ExpressionEvaluator<T> : IExpressionEvaluator<T>
        where T : class
    {
        private readonly IValueProvider _valueProvider;

        public ExpressionEvaluator(IValueProvider valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public object? GetValue(T item, IQueryExpression field)
        {
            if (field.Expression != field.FieldName)
            {
                var functionName = GetFunctionName(field is IExpressionContainer c
                    ? c.SourceExpression ?? string.Empty
                    : field.Expression, out var parameters);
                if (Functions.Items.TryGetValue(functionName, out var function))
                {
                    var split = parameters.Split(',');
                    var fieldName = split[0] == "{0}"
                        ? _valueProvider.GetFieldValue(item, field.FieldName)
                        : _valueProvider.GetFieldValue(item, split[0]);
                    return function.Invoke(fieldName, split.Skip(1));
                }
                throw new ArgumentOutOfRangeException(nameof(field), $"Expression [{field.Expression}] is not supported");
            }

            return _valueProvider.GetFieldValue(item, field.FieldName);
        }

        private static string GetFunctionName(string expression, out string parameter)
        {
            var closeIndex = expression.IndexOf(")");
            parameter = string.Empty;
            if (closeIndex == -1)
            {
                return string.Empty;
            }

            var openIndex = expression.LastIndexOf("(", closeIndex);
            if (openIndex == -1)
            {
                return string.Empty;
            }

            parameter = expression.Substring(openIndex + 1, closeIndex - openIndex - 1);
            return expression.Substring(0, openIndex);
        }
    }
}
