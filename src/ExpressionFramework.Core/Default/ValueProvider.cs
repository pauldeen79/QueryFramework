namespace ExpressionFramework.Core.Default;

public class ValueProvider : IValueProvider
{
    public object? GetValue(object? context, string fieldName)
    {
        if (context == null)
        {
            return null;
        }

        var type = context.GetType();
        object? returnValue = null;
        foreach (var part in fieldName.Split('.'))
        {
            var property = type.GetProperty(part);

            if (property == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), $"Fieldname [{fieldName}] is not found on type [{type.FullName}]");
            }

            returnValue = property.GetValue(context);
            if (returnValue == null)
            {
                break;
            }
            context = returnValue;
            type = returnValue.GetType();
        }

        return returnValue;
    }
}
