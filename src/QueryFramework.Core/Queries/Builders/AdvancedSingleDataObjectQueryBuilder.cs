using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Abstractions.Queries.Builders;

namespace QueryFramework.Core.Queries.Builders
{
    public sealed class AdvancedSingleDataObjectQueryBuilder : IAdvancedSingleDataObjectQueryBuilder
    {
        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public bool Distinct { get; set; }

        public bool GetAllFields { get; set; }

        public string DataObjectName { get; set;  }

        public List<IQueryExpressionBuilder> Fields { get; set; }

        public List<IQueryConditionBuilder> Conditions { get; set; }

        public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

        public List<IQueryExpressionBuilder> GroupByFields { get; set; }

        public List<IQueryConditionBuilder> HavingFields { get; set; }

        public List<IQueryParameterBuilder> Parameters { get; set; }

        public AdvancedSingleDataObjectQueryBuilder()
        {
            Fields = new List<IQueryExpressionBuilder>();
            Conditions = new List<IQueryConditionBuilder>();
            OrderByFields = new List<IQuerySortOrderBuilder>();
            GroupByFields = new List<IQueryExpressionBuilder>();
            HavingFields = new List<IQueryConditionBuilder>();
            Parameters = new List<IQueryParameterBuilder>();
            GetAllFields = false;
        }

        public IAdvancedSingleDataObjectQuery Build()
            => new AdvancedSingleDataObjectQuery(DataObjectName,
                                                 Conditions.Select(x => x.Build()),
                                                 OrderByFields.Select(x => x.Build()),
                                                 Limit,
                                                 Offset,
                                                 Distinct,
                                                 GetAllFields,
                                                 Fields.Select(x => x.Build()),
                                                 GroupByFields.Select(x => x.Build()),
                                                 HavingFields.Select(x => x.Build()),
                                                 Parameters.Select(x => x.Build()));
    }
}
