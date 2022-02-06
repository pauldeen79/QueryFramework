namespace QueryFramework.FileSystemSearch;

public class FileSystemDataProvider : IDataProvider
{
    private static readonly string[] _fileDataFields = new[]
    {
        nameof(FileData.Contents),
        nameof(FileData.DateCreated),
        nameof(FileData.DateLastModified),
        nameof(FileData.Directory),
        nameof(FileData.Extension),
        nameof(FileData.FileName),
        nameof(FileData.FullPath)
    };

    private static readonly string[] _lineDataFields = new[]
    {
        nameof(LineData.Line),
        nameof(LineData.LineNumber)
    };

    private readonly IConditionEvaluator _conditionEvaluator;

    public FileSystemDataProvider(IConditionEvaluator conditionEvaluator) => _conditionEvaluator = conditionEvaluator;
    
    public bool TryGetData<TResult>(ISingleEntityQuery query, out IEnumerable<TResult>? result) where TResult : class
    {
        var fileSystemQuery = query as IFileSystemQuery;
        if (fileSystemQuery == null)
        {
            result = default;
            return false;
        }

        if (!typeof(FileData).IsAssignableFrom(typeof(TResult))
            && !typeof(LineData).IsAssignableFrom(typeof(TResult)))
        {
            result = default;
            return false;
        }

        var fileDataConditions = query.Conditions.Where(x => _fileDataFields.Contains(x.Field.FieldName)).ToArray();
        var fileData = Directory.GetFiles(fileSystemQuery.Path, fileSystemQuery.SearchPattern, fileSystemQuery.SearchOption)
            .Select(x => new FileData(x))
            .Where(x => _conditionEvaluator.IsItemValid(x, fileDataConditions))
            .ToArray();

        if (typeof(FileData).IsAssignableFrom(typeof(TResult)))
        {
            result = fileData.Cast<TResult>();
            return true;
        }

        var lineDataConditions = query.Conditions.Where(x => _lineDataFields.Contains(x.Field.FieldName)).ToArray();
        result = fileData
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, x)))
            .Where(x => _conditionEvaluator.IsItemValid(x, lineDataConditions))
            .Cast<TResult>();
        return true;
    }
}
