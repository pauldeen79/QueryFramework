using QueryFramework.Abstractions.Queries.Builders;

namespace QueryFramework.Abstractions
{
    public interface IQueryParser<T> where T : ISingleEntityQueryBuilder
    {
        T Parse(T builder, string queryString);
    }
}
