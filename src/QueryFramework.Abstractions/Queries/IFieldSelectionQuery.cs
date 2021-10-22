using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries
{
    /// <summary>
    /// Interace for a query with dynamic field selection. (distinct on/off, field selection)
    /// </summary>
    public interface IFieldSelectionQuery : ISingleEntityQuery
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IFieldSelectionQuery"/> is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        bool Distinct { get; }

        /// <summary>
        /// Gets a value indicating whether to get all fields.
        /// </summary>
        /// <value>
        ///   <c>true</c> if get all fields; otherwise, <c>false</c>.
        /// </value>
        bool GetAllFields { get; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        /// <remarks>
        /// Only useful when GetAllFields is <c>false</c>.
        /// </remarks>
        IReadOnlyCollection<IQueryExpression> Fields { get; }
    }
}
