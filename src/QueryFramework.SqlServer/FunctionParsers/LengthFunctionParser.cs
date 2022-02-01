﻿namespace QueryFramework.SqlServer.FunctionParsers;

public class LengthFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
    {
        if (function is LengthFunction f)
        {
            sqlExpression = "LEN({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
