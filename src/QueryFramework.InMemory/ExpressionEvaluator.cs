using System;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions;

namespace QueryFramework.InMemory
{
    public class ExpressionEvaluator<T> : IExpressionEvaluator<T>
        where T : class
    {
        private IValueProvider ValueProvider { get; }

        public ExpressionEvaluator(IValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }

        public object? GetValue(T item, IQueryExpression field)
        {
            if (field.Function != null)
            {
                var functionName = GetFunctionName(field.Function.Expression, out var parameters);
                if (Functions.Items.TryGetValue(functionName, out var function))
                {
                    var split = parameters.Split(',');
                    var fieldName = split[0] == "{0}"
                        ? ValueProvider.GetFieldValue(item, field.FieldName)
                        : ValueProvider.GetFieldValue(item, split[0]);
                    return function.Invoke(fieldName, split.Skip(1));
                }
                throw new ArgumentOutOfRangeException(nameof(field), $"Function [{field.GetExpression()}] is not supported");
            }

            return ValueProvider.GetFieldValue(item, field.FieldName);
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
