namespace QueryFramework.InMemory
{
    internal interface IValueProvider
    {
        object GetFieldValue(object item, string fieldName);
    }
}
