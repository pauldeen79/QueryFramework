using QueryFramework.Abstractions.Builders;
using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries.Builders
{
    /// <summary>
    /// Interface for a advanced single data object query builder.
    /// </summary>
    public interface IAdvancedSingleDataObjectQueryBuilder : IFieldSelectionQueryBuilder, IGroupingQueryBuilder
    {
        /// <summary>Gets or sets the name of the data object.</summary>
        /// <value>The name of the data object.</value>
        string DataObjectName { get; set; }

        /// <summary>Gets the parameters.</summary>
        /// <value>The parameters.</value>
        List<IQueryParameterBuilder> Parameters { get; set; }
    }
}
