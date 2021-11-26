using CrossCutting.Common;

namespace QueryFramework.Abstractions.Queries
{
    public interface IParameterizedQuery : ISingleEntityQuery
    {
        ValueCollection<IQueryParameter> Parameters { get; }
    }
}
