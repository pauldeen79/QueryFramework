namespace QueryFramework.SqlServer.FunctionParsers;

public class YearFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is YearExpression yearExpression)
        {
            sqlExpression = "YEAR({0})";
            innerExpression = yearExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
