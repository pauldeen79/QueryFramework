namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryParameterBuilder
    {
        string Name { get; set; }
        object Value { get; set; }

        IQueryParameter Build();
    }
}