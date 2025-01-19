namespace QueryFramework.Abstractions.Expressions;

public record QueryParameterExpression : Expression
{
    [Required]
    public string ParameterName { get; }

    public QueryParameterExpression(string parameterName) : base()
    {
        ParameterName = parameterName;

        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public override Result<object?> Evaluate(object? context)
    {
        if (context is not IParameterizedQuery parameterizedQuery)
        {
            return Result.Invalid<object?>("Context should be of type IParameterizedQuery");
        }

        var parameter = parameterizedQuery.Parameters.FirstOrDefault(x => x.Name == ParameterName);
        if (parameter is null)
        {
            return Result.Invalid<object?>($"Parameter with name [{ParameterName}] could not be found");
        }

        return Result.Success(parameter.Value);
    }

    public override ExpressionBuilder ToBuilder()
        => new QueryParameterExpressionBuilder(this);
}
