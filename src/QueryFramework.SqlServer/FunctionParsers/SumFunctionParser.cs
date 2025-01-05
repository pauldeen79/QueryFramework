namespace QueryFramework.SqlServer.FunctionParsers;

public class SumFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is SumExpression sumExpression)
        {
            sqlExpression = "SUM({0})";
            innerExpression = sumExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
