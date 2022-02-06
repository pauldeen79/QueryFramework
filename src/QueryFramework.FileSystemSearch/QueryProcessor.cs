﻿namespace QueryFramework.FileSystemSearch;

public class QueryProcessor : IQueryProcessor
{
    private readonly IConditionEvaluator _conditionEvaluator;
    private readonly IPaginator _paginator;

    public QueryProcessor(IConditionEvaluator conditionEvaluator, IPaginator paginator)
    {
        _conditionEvaluator = conditionEvaluator;
        _paginator = paginator;
    }

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

    public IReadOnlyCollection<TResult> FindMany<TResult>(ISingleEntityQuery query) where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Conditions, query.OrderByFields),
            GetData<TResult>(query)
        ).ToList();

    public TResult? FindOne<TResult>(ISingleEntityQuery query) where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Conditions, query.OrderByFields),
            GetData<TResult>(query)
        ).FirstOrDefault();

    public IPagedResult<TResult> FindPaged<TResult>(ISingleEntityQuery query) where TResult : class
    {
        var filteredRecords = GetData<TResult>(query).ToArray();
        return new PagedResult<TResult>
        (
            _paginator.GetPagedData(query, filteredRecords),
            filteredRecords.Length,
            query.Offset.GetValueOrDefault(),
            query.Limit.GetValueOrDefault()
        );
    }

    private IEnumerable<TResult> GetData<TResult>(ISingleEntityQuery query) where TResult : class
    {
        var fileSystemQuery = query as IFileSystemQuery;
        if (fileSystemQuery == null)
        {
            throw new ArgumentException($"Query is not of type {nameof(IFileSystemQuery)}", nameof(query));
        }

        if (!typeof(FileData).IsAssignableFrom(typeof(TResult))
            && !typeof(LineData).IsAssignableFrom(typeof(TResult)))
        {
            throw new InvalidOperationException($"Result type should be {nameof(FileData)} or {nameof(LineData)}");
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
