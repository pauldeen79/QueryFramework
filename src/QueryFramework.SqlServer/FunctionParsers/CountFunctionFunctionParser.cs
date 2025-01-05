namespace QueryFramework.SqlServer.FunctionParsers;

public class CountFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is CountExpression countExpression)
        {
            sqlExpression = "COUNT({0})";
            innerExpression = countExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
