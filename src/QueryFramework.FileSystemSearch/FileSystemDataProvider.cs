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

    private readonly IExpressionEvaluator _expressionEvaluator;

    public FileSystemDataProvider(IExpressionEvaluator expressionEvaluator) => _expressionEvaluator = expressionEvaluator;
    
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

        var conditions = query.Conditions
            .Select(x =>
            new
            {
                Condition = x,
                LeftFieldName = x.LeftExpression.GetFieldName(),
                RightFieldName = x.RightExpression.GetFieldName()
            });

        var noDataExpressions = conditions
            .Where
            (
                x => (x.LeftFieldName == null || (!_fileDataFields.Contains(x.LeftFieldName) && !_lineDataFields.Contains(x.LeftFieldName)))
                && (x.RightFieldName == null || (!_fileDataFields.Contains(x.RightFieldName) && !_lineDataFields.Contains(x.RightFieldName)))
            )
            .Select(x => new ConstantExpressionBuilder().WithValue(true).WithFunction(new ConditionFunctionBuilder().WithCondition(new ConditionBuilder(x.Condition))).Build())
            .ToArray();

        if (noDataExpressions.Length > 0 && !noDataExpressions.All(x => Convert.ToBoolean(_expressionEvaluator.Evaluate(null, x))))
        {
            result = Enumerable.Empty<TResult>();
            return true;
        }

        var fileDataExpressions = conditions
            .Where
            (
                x => (x.LeftFieldName != null && _fileDataFields.Contains(x.LeftFieldName))
                || (x.RightFieldName != null && _fileDataFields.Contains(x.RightFieldName))
            )
            .Select(x => new DelegateExpressionBuilder()
            .WithValueDelegate((item, _, _) => item)
            .WithFunction(new ConditionFunctionBuilder().WithCondition(new ConditionBuilder(x.Condition)))
            .Build());
        var fileData = Directory.GetFiles(fileSystemQuery.Path, fileSystemQuery.SearchPattern, fileSystemQuery.SearchOption)
            .Select(x => new FileData(x))
            .Where(x => fileDataExpressions.All(y => Convert.ToBoolean(_expressionEvaluator.Evaluate(x, y))))
            .ToArray();

        if (typeof(FileData).IsAssignableFrom(typeof(TResult)))
        {
            result = fileData.Cast<TResult>();
            return true;
        }

        var lineDataExpressions = conditions
            .Where
            (
                x => (x.LeftFieldName != null && _lineDataFields.Contains(x.LeftFieldName))
                || (x.RightFieldName != null && _lineDataFields.Contains(x.RightFieldName))
            )
            .Select(x => new DelegateExpressionBuilder()
            .WithValueDelegate((item, _, _) => item)
            .WithFunction(new ConditionFunctionBuilder().WithCondition(new ConditionBuilder(x.Condition)))
            .Build());
        result = fileData
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, x)))
            .Where(x => lineDataExpressions.All(y => Convert.ToBoolean(_expressionEvaluator.Evaluate(x, y))))
            .Cast<TResult>();
        return true;
    }
}
