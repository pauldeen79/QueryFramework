﻿namespace QueryFramework.SqlServer.FunctionParsers;

public class CoalesceFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is CoalesceFunction f)
        {
            sqlExpression = $"COALESCE({FieldNameAsString(f)}{InnerExpressionsAsString(f, evaluator)})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }

    private string InnerExpressionsAsString(CoalesceFunction instance, IQueryExpressionEvaluator evaluator)
        => string.Join(", ", instance.InnerExpressions.Select(x => evaluator.GetSqlExpression(x)));

    private static string FieldNameAsString(CoalesceFunction instance)
        => string.IsNullOrWhiteSpace(instance.FieldName)
            ? string.Empty
            : "{0}, ";
}
