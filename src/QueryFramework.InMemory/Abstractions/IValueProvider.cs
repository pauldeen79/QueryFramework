namespace QueryFramework.InMemory.Abstractions;

public interface IValueProvider
{
    object? GetFieldValue(object? item, string fieldName);
}
