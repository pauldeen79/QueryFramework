namespace QueryFramework.SqlServer.FunctionParsers;

public class UpperFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is ToUpperCaseExpression toUpperCaseExpression)
        {
            sqlExpression = "UPPER({0})";
            innerExpression = toUpperCaseExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
