using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CrossCutting.Common;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Queries
{
    public record SingleEntityQuery : ISingleEntityQuery, IValidatableObject
    {
        public SingleEntityQuery() : this(null,
                                          null,
                                          Enumerable.Empty<IQueryCondition>(),
                                          Enumerable.Empty<IQuerySortOrder>())
        {
        }

        public SingleEntityQuery(int? limit,
                                 int? offset,
                                 IEnumerable<IQueryCondition> conditions,
                                 IEnumerable<IQuerySortOrder> orderByFields)
        {
            Limit = limit;
            Offset = offset;
            Conditions = new ValueCollection<IQueryCondition>(conditions);
            OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields);
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }

        public int? Limit { get; }
        public int? Offset { get; }
        public ValueCollection<IQueryCondition> Conditions { get; }
        public ValueCollection<IQuerySortOrder> OrderByFields { get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => this.ValidateQuery();
    }
}
