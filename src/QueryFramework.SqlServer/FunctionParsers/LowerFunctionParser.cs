namespace QueryFramework.SqlServer.FunctionParsers;

public class LowerFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is ToLowerCaseExpression toLowerCaseExpression)
        {
            sqlExpression = "LOWER({0})";
            innerExpression = toLowerCaseExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        innerExpression = new EmptyExpression();
        return false;
    }
}
