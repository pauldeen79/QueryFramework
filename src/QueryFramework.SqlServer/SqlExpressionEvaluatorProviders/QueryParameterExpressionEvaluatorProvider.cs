namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class QueryParameterExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (expression is not QueryParameterExpression queryParameterExpression)
        {
            result = null;
            return false;
        }

        if (query is not IParameterizedQuery parameterizedQuery)
        {
            result = null;
            return false;
        }

        var parameter = parameterizedQuery.Parameters.FirstOrDefault(x => x.Name == queryParameterExpression.ParameterName);
        if (parameter is null)
        {
            result = null;
            return false;
        }

        result = parameter.Value.ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        if (expression is not QueryParameterExpression queryParameterExpression)
        {
            result = null;
            return false;
        }

        if (query is not IParameterizedQuery parameterizedQuery)
        {
            result = null;
            return false;
        }

        var parameter = parameterizedQuery.Parameters.FirstOrDefault(x => x.Name == queryParameterExpression.ParameterName);
        if (parameter is null)
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(parameter.Value);
        return true;
    }
}
