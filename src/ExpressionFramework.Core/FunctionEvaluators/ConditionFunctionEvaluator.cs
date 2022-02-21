namespace ExpressionFramework.Core.FunctionEvaluators;

public class ConditionFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        if (!(function is ConditionFunction c))
        {
            result = null;
            return false;
        }

        var builder = new StringBuilder();
        foreach (var condition in c.Conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(condition.Combination == Combination.And ? "&" : "|");
            }

            var prefix = condition.StartGroup ? "(" : string.Empty;
            var suffix = condition.EndGroup ? ")" : string.Empty;
            var itemResult = IsItemValid(value, condition, evaluator);
            builder.Append(prefix)
                   .Append(itemResult ? "T" : "F")
                   .Append(suffix);
        }

        result = EvaluateBooleanExpression(builder.ToString());
        return true;
    }

    private bool IsItemValid(object? item, ICondition condition, IExpressionEvaluator evaluator)
    {
        var leftValue = evaluator.Evaluate(item, condition.LeftExpression);
        var rightValue = evaluator.Evaluate(item, condition.RightExpression);

        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
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
            : expression.Substring(0, openIndex - 2);

    private static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    private static string GetSuffix(string expression, int closeIndex)
        => closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);
}
