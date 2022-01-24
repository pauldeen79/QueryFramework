namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface ISingleEntityQueryBuilder : ISingleEntityQueryBuilderBase
    {
        ISingleEntityQuery Build();
    }
}
