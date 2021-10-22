using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Core.Queries
{
    public record AdvancedSingleDataObjectQuery : IAdvancedSingleDataObjectQuery
    {
        public AdvancedSingleDataObjectQuery(string dataObjectName = null,
                                             IEnumerable<IQueryCondition> conditions = null,
                                             IEnumerable<IQuerySortOrder> orderByFields = null,
                                             int? limit = null,
                                             int? offset = null,
                                             bool distinct = false,
                                             bool getAllFields = false,
                                             IEnumerable<IQueryExpression> fields = null,
                                             IEnumerable<IQueryExpression> groupByFields = null,
                                             IEnumerable<IQueryCondition> havingFields = null,
                                             IEnumerable<IQueryParameter> parameters = null)
        {
            Limit = limit;
            Offset = offset;
            Distinct = distinct;
            GetAllFields = getAllFields;
            DataObjectName = dataObjectName;
            Fields = new List<IQueryExpression>(fields ?? Enumerable.Empty<IQueryExpression>()).AsReadOnly();
            GroupByFields = new List<IQueryExpression>(groupByFields ?? Enumerable.Empty<IQueryExpression>()).AsReadOnly();
            HavingFields = new List<IQueryCondition>(havingFields ?? Enumerable.Empty<IQueryCondition>()).AsReadOnly();
            Conditions = new List<IQueryCondition>(conditions ?? Enumerable.Empty<IQueryCondition>()).AsReadOnly();
            OrderByFields = new List<IQuerySortOrder>(orderByFields ?? Enumerable.Empty<IQuerySortOrder>()).AsReadOnly();
            Parameters = new List<IQueryParameter>(parameters ?? Enumerable.Empty<IQueryParameter>()).AsReadOnly();
        }

        public int? Limit { get; }

        public int? Offset { get; }

        public bool Distinct { get; }

        public bool GetAllFields { get; }

        public string DataObjectName { get; }

        public IReadOnlyCollection<IQueryExpression> Fields { get; }

        public IReadOnlyCollection<IQueryCondition> Conditions { get; }

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }

        public IReadOnlyCollection<IQueryExpression> GroupByFields { get; }

        public IReadOnlyCollection<IQueryCondition> HavingFields { get; }

        public IReadOnlyCollection<IQueryParameter> Parameters { get; }
    }
}
