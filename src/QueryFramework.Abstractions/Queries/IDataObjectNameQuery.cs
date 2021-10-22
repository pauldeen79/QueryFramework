namespace QueryFramework.Abstractions.Queries
{
    /// <summary>
    /// Interface for a query with data object selection.
    /// </summary>
    public interface IDataObjectNameQuery : ISingleEntityQuery
    {
        /// <summary>
        /// Gets the name of the data object.
        /// </summary>
        /// <value>
        /// The name of the data object.
        /// </value>
        string DataObjectName { get; }
    }
}
