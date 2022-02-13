namespace ExpressionFramework.Core.Default;

public class ConditionEvaluator : IConditionEvaluator
{
    private readonly IEnumerable<IExpressionEvaluator> _expressionEvaluators;

    public ConditionEvaluator(IEnumerable<IExpressionEvaluator> expressionEvaluators)
        => _expressionEvaluators = expressionEvaluators;

    public bool IsItemValid(object item, IReadOnlyCollection<ICondition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(condition.Combination == Combination.And ? "&" : "|");
            }

            var leftValue = Evaluate(item, condition.LeftExpression);
            var rightValue = Evaluate(item, condition.RightExpression);
            var prefix = condition.OpenBracket ? "(" : string.Empty;
            var suffix = condition.CloseBracket ? ")" : string.Empty;
            var result = Evaluate(condition, leftValue, rightValue);
            builder.Append(prefix)
                   .Append(result ? "T" : "F")
                   .Append(suffix);
        }

        return EvaluateBooleanExpression(builder.ToString());
    }

    private object? Evaluate(object item, IExpression expression)
    {
        foreach (var evaluator in _expressionEvaluators)
        {
            if (evaluator.TryEvaluate(item, expression, out var result))
            {
                return result;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
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

    private static bool Evaluate(ICondition condition, object? leftValue, object? rightValue)
    {
        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }
}
