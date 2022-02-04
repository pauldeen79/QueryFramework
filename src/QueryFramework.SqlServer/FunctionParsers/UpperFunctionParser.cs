﻿namespace QueryFramework.SqlServer.FunctionParsers;

public class UpperFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is UpperFunction f)
        {
            sqlExpression = "UPPER({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
