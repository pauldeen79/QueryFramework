namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (!(expression is IFieldExpression fieldExpression))
        {
            result = null;
            return false;
        }

        var fieldName = fieldInfo.GetDatabaseFieldName(fieldExpression.FieldName);
        if (fieldName == null)
        {
            throw new InvalidOperationException($"Expression contains unknown field [{fieldExpression.FieldName}]");
        }
        result = $"LEN({fieldName})";
        return true;
    }

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, int paramCounter, out string? result)
    {
        if (!(expression is IFieldExpression fieldExpression))
        {
            result = null;
            return false;
        }

        result = fieldInfo.GetDatabaseFieldName(fieldExpression.FieldName);
        if (result == null)
        {
            throw new InvalidOperationException($"Expression contains unknown field [{fieldExpression.FieldName}]");
        }

        return true;
    }
}
