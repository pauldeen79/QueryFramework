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

    private readonly IFileDataProvider _fileDataProvider;

    public DefaultFileDataProvider(IFileDataProvider fileDataProvider)
    {
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

        var conditions = query.Filter.Conditions
            .Select(x =>
            new
            {
                Condition = x,
                LeftFieldName = x.LeftExpression.GetFieldName(),
                RightFieldName = x.RightExpression?.TryGetFieldName()
            });

        var noDataConditions = conditions
            .Where
            (
                x => !IsValidForFields(x.LeftFieldName, _fileDataFields.Concat(_lineDataFields))
                && !IsValidForFields(x.RightFieldName, _fileDataFields.Concat(_lineDataFields))
            )
            .Select(x => x.Condition)
            .ToArray();

        if (noDataConditions.Length > 0 && !new ComposedEvaluatable(noDataConditions).Evaluate(null).Value)
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
            .Where(x => new ComposedEvaluatable(fileDataConditions).Evaluate(x).Value)
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
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber + 1, x)))
            .Where(x => new ComposedEvaluatable(lineDataConditions).Evaluate(x).Value)
            .Cast<TResult>();
        return true;
    }

    private static bool IsValidForFields(string? fieldName, IEnumerable<string> validFieldNames)
        => fieldName != null && validFieldNames.Contains(fieldName);
}
