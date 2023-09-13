namespace QueryFramework.FileSystemSearch.Queries;

public record FileSystemQuery : SingleEntityQuery, IFileSystemQuery
{
    public FileSystemQuery(string path, string searchPattern, SearchOption searchOption)
        : this(path, searchPattern, searchOption, null, null, new ComposedEvaluatable(Enumerable.Empty<ComposableEvaluatable>()), Enumerable.Empty<IQuerySortOrder>())
    {
    }

    public FileSystemQuery(string path, string searchPattern, SearchOption searchOption, IQuery source)
        : this(path, searchPattern, searchOption, source.Limit, source.Offset, source.Filter, source.OrderByFields)
    {
    }

    public FileSystemQuery(string path,
                           string searchPattern,
                           SearchOption searchOption,
                           int? limit,
                           int? offset,
                           ComposedEvaluatable filter,
                           IEnumerable<IQuerySortOrder> orderByFields)
        : base(limit, offset, filter, orderByFields)
    {
        ArgumentGuard.IsNotNull(path, nameof(path));
        ArgumentGuard.IsNotNull(searchOption, nameof(searchOption));

        Path = path;
        SearchPattern = searchPattern;
        SearchOption = searchOption;
    }

    public string Path { get; }

    public string SearchPattern { get; }

    public SearchOption SearchOption { get; }
}
