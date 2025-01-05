namespace QueryFramework.SqlServer.FunctionParsers;

public class RightFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is RightExpression rightExpression)
        {
            sqlExpression = $"RIGHT({{0}}, {rightExpression.LengthExpression.EvaluateTyped().Value})";
            innerExpression = rightExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
