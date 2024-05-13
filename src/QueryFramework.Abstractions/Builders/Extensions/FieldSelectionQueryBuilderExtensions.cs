namespace QueryFramework.Abstractions.Builders.Extensions;

public static partial class FieldSelectionQueryBuilderExtensions
{
    public static T Select<T>(this T instance, params string[] additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
    {
        instance.GetAllFields = false;
        instance.FieldNames.AddRange(additionalFieldNames);
        return instance;
    }

    public static T SelectAll<T>(this T instance)
        where T : IFieldSelectionQueryBuilder
    {
        instance.GetAllFields = true;
        instance.FieldNames.Clear();
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
            instance.FieldNames.Clear();
        }
        return instance;
    }

    public static T Distinct<T>(this T instance, bool distinct = true)
        where T : IFieldSelectionQueryBuilder
        => instance.With(x => x.Distinct = distinct);
}
