using QueryFramework.Abstractions.Builders;
using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries.Builders
{
    /// <summary>
    /// Interface for a field selection query builder.
    /// </summary>
    public interface IFieldSelectionQueryBuilder : ISingleEntityQueryBuilder
    {
        /// <summary>Gets or sets a value indicating whether this <see cref="IFieldSelectionQueryBuilder" /> is distinct.</summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.</value>
        bool Distinct { get; set; }

        /// <summary>Gets or sets a value indicating whether [get all fields].</summary>
        /// <value>
        ///   <c>true</c> if [get all fields]; otherwise, <c>false</c>.</value>
        bool GetAllFields { get; set; }

        /// <summary>Gets the fields.</summary>
        /// <value>The fields.</value>
        List<IQueryExpressionBuilder> Fields { get; set; }

    }
}
