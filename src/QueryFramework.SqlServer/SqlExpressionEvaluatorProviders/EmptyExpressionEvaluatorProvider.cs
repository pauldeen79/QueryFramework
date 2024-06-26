﻿namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class EmptyExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (expression is not EmptyExpression)
        {
            result = null;
            return false;
        }

        result = "0";
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        result = null;
        return expression is EmptyExpression;
    }
}
