namespace QueryFramework.SqlServer.FunctionParsers;

public class LeftFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is LeftExpression leftExpression)
        {
            sqlExpression = $"LEFT({{0}}, {leftExpression.LengthExpression.EvaluateTyped().Value})";
            innerExpression = leftExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
