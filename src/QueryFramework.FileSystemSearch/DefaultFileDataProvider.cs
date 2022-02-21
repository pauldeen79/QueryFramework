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

    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly IFileDataProvider _fileDataProvider;

    public DefaultFileDataProvider(IExpressionEvaluator expressionEvaluator, IFileDataProvider fileDataProvider)
    {
        _expressionEvaluator = expressionEvaluator;
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
                LeftFieldName = x.LeftExpression.GetFieldName(),
                RightFieldName = x.RightExpression.GetFieldName()
            });

        var noDataExpressions = conditions
            .Where
            (
                x => !IsValidForFields(x.LeftFieldName, _fileDataFields.Concat(_lineDataFields))
                && !IsValidForFields(x.RightFieldName, _fileDataFields.Concat(_lineDataFields))
            )
            .Select(x => new ConstantExpressionBuilder().WithValue(true)
                                                        .WithFunction(new ConditionFunctionBuilder().AddConditions(new ConditionBuilder(x.Condition)))
                                                        .Build())
            .ToArray();

        if (noDataExpressions.Length > 0 && !noDataExpressions.All(x => Convert.ToBoolean(_expressionEvaluator.Evaluate(null, x))))
        {
            result = Enumerable.Empty<TResult>();
            return true;
        }

        var fileDataExpressions = conditions
            .Where
            (
                x => IsValidForFields(x.LeftFieldName, _fileDataFields)
                || IsValidForFields(x.RightFieldName, _fileDataFields)
            )
            .Select(x => new DelegateExpressionBuilder()
            .WithValueDelegate((item, _, _) => item)
            .WithFunction(new ConditionFunctionBuilder().AddConditions(new ConditionBuilder(x.Condition)))
            .Build());
        var fileData = _fileDataProvider.Get(fileSystemQuery)
            .Where(x => fileDataExpressions.All(y => Convert.ToBoolean(_expressionEvaluator.Evaluate(x, y))))
            .ToArray();

        if (typeof(IFileData).IsAssignableFrom(typeof(TResult)))
        {
            result = fileData.Cast<TResult>();
            return true;
        }

        var lineDataExpressions = conditions
            .Where
            (
                x => IsValidForFields(x.LeftFieldName, _lineDataFields)
                || IsValidForFields(x.RightFieldName, _lineDataFields)
            )
            .Select(x => new DelegateExpressionBuilder()
            .WithValueDelegate((item, _, _) => item)
            .WithFunction(new ConditionFunctionBuilder().AddConditions(new ConditionBuilder(x.Condition)))
            .Build());
        result = fileData
            .SelectMany(x => x.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, x)))
            .Where(x => lineDataExpressions.All(y => Convert.ToBoolean(_expressionEvaluator.Evaluate(x, y))))
            .Cast<TResult>();
        return true;
    }

    private static bool IsValidForFields(string? fieldName, IEnumerable<string> validFieldNames)
        => fieldName != null && validFieldNames.Contains(fieldName);
}
