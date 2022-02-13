namespace ExpressionFramework.Core.Default;

public class ConditionEvaluator : IConditionEvaluator
{
    private readonly IEnumerable<IExpressionEvaluator> _expressionEvaluators;

    public ConditionEvaluator(IEnumerable<IExpressionEvaluator> expressionEvaluators)
        => _expressionEvaluators = expressionEvaluators;

    public bool IsItemValid(object item, ICondition condition)
        => EvaluateBooleanExpression(new StringBuilder().Chain(x => IsItemValid(item, condition, x, default, default, default)).ToString());

    public bool AreItemsValid(object item, IReadOnlyCollection<ICondition> conditions, Combination combination)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            IsItemValid(item, condition, builder, combination, false, false);
        }

        return EvaluateBooleanExpression(builder.ToString());
    }

    private void IsItemValid(object item, ICondition condition, StringBuilder builder, Combination combination, bool openBracket, bool closeBracket)
    {
        if (builder.Length > 0)
        {
            builder.Append(GetCombination(combination));
        }

        var leftValue = Evaluate(item, condition.LeftExpression);
        var rightValue = Evaluate(item, condition.RightExpression);
        var prefix = openBracket ? "(" : string.Empty;
        var suffix = closeBracket ? ")" : string.Empty;
        var result = IsValid(condition, leftValue, rightValue);
        builder.Append(prefix)
               .Append(result ? "T" : "F")
               .Append(suffix);
    }

    private static string GetCombination(Combination combination)
        => combination switch
        {
            Combination.And => "&",
            Combination.Or => "|",
            _ => throw new ArgumentOutOfRangeException()
        };

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

    private static bool IsValid(ICondition condition, object? leftValue, object? rightValue)
    {
        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }
}
