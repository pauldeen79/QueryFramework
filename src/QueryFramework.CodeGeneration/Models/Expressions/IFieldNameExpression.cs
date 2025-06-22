namespace QueryFramework.CodeGeneration.Models.Expressions;

internal interface IFieldNameExpression : Abstractions.IExpression
{
    [Required] string FieldName { get; }
}
