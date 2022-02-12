namespace ExpressionFramework.Core.Default;

public class ValueProvider : IValueProvider
{
    public object? GetValue(object item, string fieldName)
    {
        var type = item.GetType();
        object? returnValue = null;
        foreach (var part in fieldName.Split('.'))
        {
            var property = type.GetProperty(part);

            if (property == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), $"Fieldname [{fieldName}] is not found on type [{type.FullName}]");
            }

            returnValue = property.GetValue(item) ?? null;
            if (returnValue == null)
            {
                break;
            }
            type = returnValue.GetType();
        }

        return returnValue;
    }
}
