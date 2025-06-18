using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

namespace QueryFramework.Abstractions.Expressions;

public record QueryParameterExpression : IExpressionComponent
{
    [Required]
    public string ParameterName { get; }

    public int Order => 50;

    public QueryParameterExpression(string parameterName) : base()
    {
        ParameterName = parameterName;

        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        if (await context.State["context"] is not IParameterizedQuery parameterizedQuery)
        {
            //return Result.Invalid<object?>("Context should be of type IParameterizedQuery");
            return Result.Continue<object?>();
        }

        var parameter = parameterizedQuery.Parameters.FirstOrDefault(x => x.Name == ParameterName);
        if (parameter is null)
        {
            return Result.Invalid<object?>($"Parameter with name [{ParameterName}] could not be found");
        }

        return Result.Success(parameter.Value);
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(GetType())
            .WithSourceExpression(context.Expression);

        if (await context.State["context"] is not IParameterizedQuery parameterizedQuery)
        {
            //return result.WithStatus(ResultStatus.Invalid).WithErrorMessage("Context should be of type IParameterizedQuery");
            return result.WithStatus(ResultStatus.Continue);
        }

        var parameter = parameterizedQuery.Parameters.FirstOrDefault(x => x.Name == ParameterName);
        if (parameter is null)
        {
            return result.WithStatus(ResultStatus.Invalid).WithErrorMessage($"Parameter with name [{ParameterName}] could not be found");
        }

        return result.WithStatus(ResultStatus.Ok);
    }
}
