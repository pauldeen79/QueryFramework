namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (expression is not FieldExpression fieldExpression)
        {
            result = null;
            return false;
        }

        var fieldName = fieldExpression.GetFieldName(context);
        var databaseFieldName = fieldInfo.GetDatabaseFieldName(fieldName)
            ?? throw new InvalidOperationException($"Expression contains unknown field [{fieldName}]");

        result = $"LEN({databaseFieldName})";
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        if (expression is not FieldExpression fieldExpression)
        {
            result = null;
            return false;
        }

        var fieldName = fieldExpression.GetFieldName(context);
        result = fieldInfo.GetDatabaseFieldName(fieldName);
        if (result is null)
        {
            throw new InvalidOperationException($"Expression contains unknown field [{fieldName}]");
        }

        return true;
    }
}
