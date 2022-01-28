namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryParameterValueBuilder
    {
        string Name { get; set; }
        IQueryParameterValue Build();
    }
}
