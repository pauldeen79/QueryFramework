namespace QueryFramework.Core.Extensions;

public static class SingleEntityQueryExtensions
{
    public static IEnumerable<ValidationResult> ValidateQuery(this ISingleEntityQuery instance)
    {
        var bracketCount = 0;
        var bracketErrorShown = false;
        foreach (var condition in instance.Conditions)
        {
            if (!bracketErrorShown)
            {
                if (condition.OpenBracket)
                {
                    bracketCount++;
                }
                if (condition.CloseBracket)
                {
                    bracketCount--;
                }
                if (bracketCount < 0)
                {
                    yield return new ValidationResult("Too many brackets closed at condition: " + condition.Field.FieldName, new[] { nameof(ISingleEntityQuery.Conditions), nameof(ISingleEntityQuery.Conditions) });
                    bracketErrorShown = true;
                }
            }
        }
        if (bracketCount > 0)
        {
            yield return new ValidationResult("Missing close brackets, braket count should be 0 but remaining " + bracketCount, new[] { nameof(ISingleEntityQuery.Conditions), nameof(ISingleEntityQuery.Conditions) });
        }
    }
}
