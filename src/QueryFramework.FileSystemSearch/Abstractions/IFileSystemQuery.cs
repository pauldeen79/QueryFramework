namespace QueryFramework.FileSystemSearch.Abstractions;

public interface IFileSystemQuery : ISingleEntityQuery
{
    string Path { get; }
    string SearchPattern { get; }
    SearchOption SearchOption { get; }
}
