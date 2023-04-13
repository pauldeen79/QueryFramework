namespace QueryFramework.Core.Extensions;

public static class FieldSelectionQueryBuilderExtensions
{
    public static T Select<T>(this T instance, params ExpressionBuilder[] additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
    {
        instance.GetAllFields = false;
        instance.Fields.AddRange(additionalFieldNames);
        return instance;
    }

    public static T Select<T>(this T instance, IEnumerable<ExpressionBuilder> additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
        => instance.Select(additionalFieldNames.ToArray());

    public static T Select<T>(this T instance, params string[] additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
        => instance.Select(additionalFieldNames.Select(s => new FieldExpressionBuilder().WithFieldName(s)));

    public static T SelectAll<T>(this T instance)
        where T : IFieldSelectionQueryBuilder
    {
        instance.GetAllFields = true;
        instance.Fields.Clear();
        return instance;
    }

    public static T SelectDistinct<T>(this T instance, params ExpressionBuilder[] additionalFieldNames)
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
        return instance.Select(additionalFieldNames);
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
        => instance.With(x => x.Distinct = distinct);
}
