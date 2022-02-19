namespace QueryFramework.FileSystemSearch.Queries;

public record FileSystemQuery : SingleEntityQuery, IFileSystemQuery
{
    public FileSystemQuery(string path, string searchPattern, SearchOption searchOption)
        : this(path, searchPattern, searchOption, null, null, Enumerable.Empty<ICondition>(), Enumerable.Empty<IQuerySortOrder>())
    {
    }

    public FileSystemQuery(string path, string searchPattern, SearchOption searchOption, ISingleEntityQuery source)
        : this(path, searchPattern, searchOption, source.Limit, source.Offset, source.Conditions, source.OrderByFields)
    {
    }

    public FileSystemQuery(string path,
                           string searchPattern,
                           SearchOption searchOption,
                           int? limit,
                           int? offset,
                           IEnumerable<ICondition> conditions,
                           IEnumerable<IQuerySortOrder> orderByFields)
        : base(limit, offset, conditions, orderByFields)
    {
        Path = path;
        SearchPattern = searchPattern;
        SearchOption = searchOption;
    }

    public string Path { get; }

    public string SearchPattern { get; }

    public SearchOption SearchOption { get; }
}
