namespace ExpressionFramework.Abstractions.DomainModel;

public interface IExpression
{
    string FieldName { get; }
    IExpressionFunction? Function { get; }
}
