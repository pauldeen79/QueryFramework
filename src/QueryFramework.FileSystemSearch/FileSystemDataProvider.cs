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
    
    public IEnumerable<TResult>? GetData<TResult>(ISingleEntityQuery query) where TResult : class
    {
        var fileSystemQuery = query as IFileSystemQuery;
        if (fileSystemQuery == null)
        {
            return null;
        }

        if (!typeof(FileData).IsAssignableFrom(typeof(TResult))
            && !typeof(LineData).IsAssignableFrom(typeof(TResult)))
        {
            return null;
        }

        var fileDataConditions = query.Conditions.Where(x => _fileDataFields.Contains(x.Field.FieldName)).ToArray();
        var fileData = Directory.GetFiles(fileSystemQuery.Path, fileSystemQuery.SearchPattern, fileSystemQuery.SearchOption)
            .Select(x => new FileData(x))
            .Where(x => _conditionEvaluator.IsItemValid(x, fileDataConditions))
            .ToArray();

        if (typeof(FileData).IsAssignableFrom(typeof(TResult)))
        {
            return fileData.Cast<TResult>();
        }

        var lineDataConditions = query.Conditions.Where(x => _lineDataFields.Contains(x.Field.FieldName)).ToArray();
        return fileData
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, x)))
            .Where(x => _conditionEvaluator.IsItemValid(x, lineDataConditions))
            .Cast<TResult>();
    }
}
