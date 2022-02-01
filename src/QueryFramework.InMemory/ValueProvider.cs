namespace QueryFramework.InMemory;

internal class ValueProvider : IValueProvider
{
    public object? GetFieldValue(object? item, string fieldName)
    {
        if (item == null)
        {
            return null;
        }

        var type = item.GetType();
        var property = type.GetProperty(fieldName);

        if (property == null)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldName), $"Fieldname [{fieldName}] is not found on type [{type.FullName}]");
        }

        return property.GetValue(item) ?? null;
    }
}
