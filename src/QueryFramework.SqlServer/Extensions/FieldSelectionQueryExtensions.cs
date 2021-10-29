using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class FieldSelectionQueryExtensions
    {
        public static IEnumerable<IQueryExpression> GetSelectFields(this IFieldSelectionQuery fieldSelectionQuery,
                                                                    IQueryFieldProvider fieldProvider)
        {
            var fields = fieldProvider.GetSelectFields(fieldSelectionQuery.Fields.Select(x => x.FieldName));
            return fieldSelectionQuery
                .Fields
                .Where(expression => fields.Contains(expression.FieldName));
        }
    }
}
