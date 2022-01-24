using System.Linq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class FieldSelectionQueryBuilderExtensions
    {
        public static T Select<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = false;
            instance.Fields.AddRange(additionalFieldNames);
            return instance;
        }

        public static T Select<T>(this T instance, params string[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
            => instance.Select(additionalFieldNames.Select(s => new QueryExpressionBuilder(s)).ToArray());

        public static T SelectAll<T>(this T instance)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = true;
            instance.Fields.Clear();
            return instance;
        }

        public static T SelectDistinct<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = true;
            instance.GetAllFields = false;
            instance.Fields.AddRange(additionalFieldNames);
            return instance;
        }

        public static T SelectDistinct<T>(this T instance, params string[] additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = true;
            return instance.Select(additionalFieldNames.Select(s => new QueryExpressionBuilder(s)).ToArray());
        }

        public static T GetAllFields<T>(this T instance, bool getAllFields = true)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = getAllFields;
            if (getAllFields)
            {
                instance.Fields.Clear();
            }
            return instance;
        }

        public static T Distinct<T>(this T instance, bool distinct = true)
            where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = distinct;
            return instance;
        }
    }
}
