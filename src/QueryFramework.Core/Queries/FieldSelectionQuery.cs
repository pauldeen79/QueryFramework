using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Core.Queries
{
    public record FieldSelectionQuery : IFieldSelectionQuery
    {
        public FieldSelectionQuery(IEnumerable<IQueryCondition> conditions = null,
                                   IEnumerable<IQuerySortOrder> orderByFields = null,
                                   int? limit = null,
                                   int? offset = null,
                                   bool distinct = false,
                                   bool getAllFields = false,
                                   IEnumerable<IQueryExpression> fields = null)
        {
            Limit = limit;
            Offset = offset;
            Distinct = distinct;
            GetAllFields = getAllFields;
            Fields = new List<IQueryExpression>(fields ?? Enumerable.Empty<IQueryExpression>()).AsReadOnly();
            Conditions = new List<IQueryCondition>(conditions ?? Enumerable.Empty<IQueryCondition>()).AsReadOnly();
            OrderByFields = new List<IQuerySortOrder>(orderByFields ?? Enumerable.Empty<IQuerySortOrder>()).AsReadOnly();
        }

        public int? Limit { get; }

        public int? Offset { get; }

        public bool Distinct { get; }

        public bool GetAllFields { get; }

        public IReadOnlyCollection<IQueryExpression> Fields { get; }

        public IReadOnlyCollection<IQueryCondition> Conditions { get; }

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
    }
}
