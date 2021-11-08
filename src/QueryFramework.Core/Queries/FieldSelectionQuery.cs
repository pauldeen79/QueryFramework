using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Core.Queries
{
    public record FieldSelectionQuery : IFieldSelectionQuery
    {
        public FieldSelectionQuery(IEnumerable<IQueryCondition>? conditions = null,
                                   IEnumerable<IQuerySortOrder>? orderByFields = null,
                                   int? limit = null,
                                   int? offset = null,
                                   bool distinct = false,
                                   bool getAllFields = false,
                                   IEnumerable<IQueryExpression>? fields = null)
        {
            Limit = limit;
            Offset = offset;
            Distinct = distinct;
            GetAllFields = getAllFields;
            Fields = new ValueCollection<IQueryExpression>(fields ?? Enumerable.Empty<IQueryExpression>());
            Conditions = new ValueCollection<IQueryCondition>(conditions ?? Enumerable.Empty<IQueryCondition>());
            OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields ?? Enumerable.Empty<IQuerySortOrder>());
        }

        public int? Limit { get; }
        public int? Offset { get; }
        public bool Distinct { get; }
        public bool GetAllFields { get; }
        public ValueCollection<IQueryExpression> Fields { get; }
        public ValueCollection<IQueryCondition> Conditions { get; }
        public ValueCollection<IQuerySortOrder> OrderByFields { get; }
    }
}
