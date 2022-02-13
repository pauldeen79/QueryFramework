namespace ExpressionFramework.Abstractions;

public interface IValueProvider
{
    object? GetValue(object? context, string fieldName);
}
