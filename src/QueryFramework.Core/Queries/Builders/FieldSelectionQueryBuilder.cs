using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Abstractions.Queries.Builders;

namespace QueryFramework.Core.Queries.Builders
{
    public sealed class FieldSelectionQueryBuilder : IFieldSelectionQueryBuilder
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public bool Distinct { get; set; }
        public bool GetAllFields { get; set; }
        public List<IQueryExpressionBuilder> Fields { get; set; }
        public List<IQueryConditionBuilder> Conditions { get; set; }
        public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

        public FieldSelectionQueryBuilder()
        {
            Fields = new List<IQueryExpressionBuilder>();
            Conditions = new List<IQueryConditionBuilder>();
            OrderByFields = new List<IQuerySortOrderBuilder>();
            GetAllFields = false;
        }

        public IFieldSelectionQuery Build()
            => new FieldSelectionQuery(Limit,
                                       Offset,
                                       Distinct,
                                       GetAllFields,
                                       Conditions.Select(x => x.Build()),
                                       OrderByFields.Select(x => x.Build()),
                                       Fields.Select(x => x.Build()));
    }
}
