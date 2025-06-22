namespace QueryFramework.CodeGeneration.Models.Expressions;

internal interface ILiteralExpression : Abstractions.IExpression
{
    object? Value { get; }
}
