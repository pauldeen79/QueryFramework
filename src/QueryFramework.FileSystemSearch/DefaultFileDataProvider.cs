namespace QueryFramework.FileSystemSearch;

public class DefaultFileDataProvider : IDataProvider
{
    private static readonly string[] _fileDataFields = new[]
    {
        nameof(IFileData.Contents),
        nameof(IFileData.DateCreated),
        nameof(IFileData.DateLastModified),
        nameof(IFileData.Directory),
        nameof(IFileData.Extension),
        nameof(IFileData.FileName),
        nameof(IFileData.FullPath)
    };

    private static readonly string[] _lineDataFields = new[]
    {
        nameof(ILineData.Line),
        nameof(ILineData.LineNumber)
    };

    private readonly IConditionEvaluator _conditionEvaluator;
    private readonly IFileDataProvider _fileDataProvider;

    public DefaultFileDataProvider(IConditionEvaluator conditionEvaluator,
                                   IFileDataProvider fileDataProvider)
    {
        _conditionEvaluator = conditionEvaluator;
        _fileDataProvider = fileDataProvider;
    }

    public bool TryGetData<TResult>(ISingleEntityQuery query, out IEnumerable<TResult>? result) where TResult : class
    {
        var fileSystemQuery = query as IFileSystemQuery;
        if (fileSystemQuery == null)
        {
            result = default;
            return false;
        }

        if (!typeof(IFileData).IsAssignableFrom(typeof(TResult))
            && !typeof(ILineData).IsAssignableFrom(typeof(TResult)))
        {
            result = default;
            return false;
        }

        var conditions = query.Conditions
            .Select(x =>
            new
            {
                Condition = x,
                LeftFieldName = (x.LeftExpression as IFieldExpression)?.FieldName,
                RightFieldName = (x.RightExpression as IFieldExpression)?.FieldName
            });

        var noDataConditions = conditions
            .Where
            (
                x => !IsValidForFields(x.LeftFieldName, _fileDataFields.Concat(_lineDataFields))
                && !IsValidForFields(x.RightFieldName, _fileDataFields.Concat(_lineDataFields))
            )
            .Select(x => x.Condition)
            .ToArray();

        if (noDataConditions.Length > 0 && !_conditionEvaluator.Evaluate(null, noDataConditions))
        {
            result = Enumerable.Empty<TResult>();
            return true;
        }

        var fileDataConditions = conditions
            .Where
            (
                x => IsValidForFields(x.LeftFieldName, _fileDataFields)
                || IsValidForFields(x.RightFieldName, _fileDataFields)
            )
            .Select(x => x.Condition);
        var fileData = _fileDataProvider.Get(fileSystemQuery)
            .Where(x => _conditionEvaluator.Evaluate(x, fileDataConditions))
            .ToArray();

        if (typeof(IFileData).IsAssignableFrom(typeof(TResult)))
        {
            result = fileData.Cast<TResult>();
            return true;
        }

        var lineDataConditions = conditions
            .Where
            (
                x => IsValidForFields(x.LeftFieldName, _lineDataFields)
                || IsValidForFields(x.RightFieldName, _lineDataFields)
            )
            .Select(x => x.Condition);
        result = fileData
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, x)))
            .Where(x => _conditionEvaluator.Evaluate(x, lineDataConditions))
            .Cast<TResult>();
        return true;
    }

    private static bool IsValidForFields(string? fieldName, IEnumerable<string> validFieldNames)
        => fieldName != null && validFieldNames.Contains(fieldName);
}
