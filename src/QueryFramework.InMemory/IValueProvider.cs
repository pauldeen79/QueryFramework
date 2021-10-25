namespace QueryFramework.InMemory
{
    public interface IValueProvider
    {
        object GetFieldValue(object item, string fieldName);
    }
}
