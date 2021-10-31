namespace QueryFramework.Abstractions
{
    public interface IQueryParameter
    {
        string Name { get; }
        object Value { get; }
    }
}
