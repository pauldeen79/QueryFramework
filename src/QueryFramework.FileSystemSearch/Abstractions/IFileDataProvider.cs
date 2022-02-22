namespace QueryFramework.FileSystemSearch.Abstractions;

public interface IFileDataProvider
{
    IEnumerable<IFileData> Get(IFileSystemQuery fileSystemQuery);
}
