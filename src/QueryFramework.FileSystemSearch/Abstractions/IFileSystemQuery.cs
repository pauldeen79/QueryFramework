namespace QueryFramework.FileSystemSearch.Abstractions;

public interface IFileSystemQuery : IQuery
{
    string Path { get; }
    string SearchPattern { get; }
    SearchOption SearchOption { get; }
}
