using QueryFramework.Abstractions.Builders;
using System.Collections.Generic;

namespace QueryFramework.Abstractions.Queries.Builders
{
    public interface IFieldSelectionQueryBuilder : ISingleEntityQueryBuilder
    {
        bool Distinct { get; set; }
        bool GetAllFields { get; set; }
        List<IQueryExpressionBuilder> Fields { get; set; }
    }
}
