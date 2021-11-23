using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Core.Queries
{
    public record FieldSelectionQuery : IFieldSelectionQuery
    {
        public FieldSelectionQuery() : this(null,
                                            null,
                                            false,
                                            false,
                                            Enumerable.Empty<IQueryCondition>(),
                                            Enumerable.Empty<IQuerySortOrder>(),
                                            Enumerable.Empty<IQueryExpression>())
        {
        }

        public FieldSelectionQuery(int? limit,
                                   int? offset,
                                   bool distinct,
                                   bool getAllFields,
                                   IEnumerable<IQueryCondition> conditions,
                                   IEnumerable<IQuerySortOrder> orderByFields,
                                   IEnumerable<IQueryExpression> fields)
        {
            Limit = limit;
            Offset = offset;
            Distinct = distinct;
            GetAllFields = getAllFields;
            Fields = new ValueCollection<IQueryExpression>(fields);
            Conditions = new ValueCollection<IQueryCondition>(conditions);
            OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields);
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
