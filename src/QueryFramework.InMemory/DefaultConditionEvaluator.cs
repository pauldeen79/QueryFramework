namespace QueryFramework.InMemory;

public class DefaultConditionEvaluator : IConditionEvaluator
{
    private readonly IExpressionEvaluator _valueRetriever;

    public DefaultConditionEvaluator(IExpressionEvaluator valueRetriever) => _valueRetriever = valueRetriever;

    public bool IsItemValid(object item, IReadOnlyCollection<IQueryCondition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(condition.Combination == QueryCombination.And ? "&" : "|");
            }

            var value = _valueRetriever.GetValue(item, condition.Field);
            var prefix = condition.OpenBracket ? "(" : string.Empty;
            var suffix = condition.CloseBracket ? ")" : string.Empty;
            var result = Evaluate(condition, value);
            builder.Append(prefix)
                   .Append(result ? "T" : "F")
                   .Append(suffix);
        }

        return EvaluateBooleanExpression(builder.ToString());
    }

    private static bool EvaluateBooleanExpression(string expression)
    {
        var result = ProcessRecursive(ref expression);

        var @operator = "&";
        foreach (var character in expression)
        {
            bool currentResult;
            switch (character)
            {
                case '&':
                    @operator = "&";
                    break;
                case '|':
                    @operator = "|";
                    break;
                case 'T':
                case 'F':
                    currentResult = character == 'T';
                    result = @operator == "&"
                        ? result && currentResult
                        : result || currentResult;
                    break;
            }
        }

        return result;
    }

    private static bool ProcessRecursive(ref string expression)
    {
        var result = true;
        var openIndex = -1;
        int closeIndex;
        do
        {
            closeIndex = expression.IndexOf(")");
            if (closeIndex > -1)
            {
                openIndex = expression.LastIndexOf("(", closeIndex);
                if (openIndex > -1)
                {
                    result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                    expression = string.Concat(GetPrefix(expression, openIndex),
                                               GetCurrent(result),
                                               GetSuffix(expression, closeIndex));
                }
            }
        } while (closeIndex > -1 && openIndex > -1);
        return result;
    }

    private static string GetPrefix(string expression, int openIndex)
        => openIndex == 0
            ? string.Empty
            : expression.Substring(0, openIndex - 1);

    private static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    private static string GetSuffix(string expression, int closeIndex)
        => closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);

    private static bool Evaluate(IQueryCondition condition, object? value)
    {
        var conditionValueString = condition.Value == null
            ? null
            : condition.Value.ToString();

        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(value, conditionValueString, condition.Value));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported query operator: {condition.Operator}");
    }
}
