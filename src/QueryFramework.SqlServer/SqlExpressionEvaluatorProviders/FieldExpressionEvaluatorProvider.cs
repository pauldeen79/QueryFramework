namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public FieldExpressionEvaluatorProvider(IEnumerable<IFunctionParser> functionParsers)
        => _functionParsers = functionParsers;

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, out string? result)
    {
        if (!(expression is IFieldExpression fieldExpression))
        {
            result = null;
            return false;
        }

        result = expression.Function == null
            ? fieldExpression.FieldName
            : expression.Function.GetSqlExpression(evaluator,
                                                   _functionParsers,
                                                   nameof(expression)).Replace("{0}", fieldExpression.FieldName);

        return true;
    }
}
