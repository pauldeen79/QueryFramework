using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal class ValidatableQueryMock : ISingleEntityQuery, IValidatableObject
    {
        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Limit == null)
            {
                yield return new ValidationResult("Limit is required", new[] { nameof(Limit) });
            }
            if (Offset == null)
            {
                yield return new ValidationResult("Offset is required", new[] { nameof(Offset) });
            }
        }

        public ValidatableQueryMock()
        {
            Conditions = new List<IQueryCondition>();
            OrderByFields = new List<IQuerySortOrder>();
        }
    }
}
