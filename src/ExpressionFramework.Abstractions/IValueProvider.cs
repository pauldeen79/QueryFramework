namespace ExpressionFramework.Abstractions;

public interface IValueProvider
{
    object? GetValue(object item, string fieldName);
}
