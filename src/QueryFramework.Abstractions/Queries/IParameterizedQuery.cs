using CrossCutting.Common;

namespace QueryFramework.Abstractions.Queries
{
    /// <summary>
    /// Interface for a query with parameters.
    /// </summary>
    public interface IParameterizedQuery : ISingleEntityQuery
    {
        /// <summary>
        /// Gets the parameters for this query.
        /// </summary>
        ValueCollection<IQueryParameter> Parameters { get; }
    }
}
