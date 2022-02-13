namespace ExpressionFramework.Abstractions.DomainModel;

public interface IFieldExpression : IExpression
{
    string FieldName { get; }
}
