namespace QueryFramework.FileSystemSearch;

public class FileSystemFileDataProvider : IFileDataProvider
{
    public IEnumerable<IFileData> Get(IFileSystemQuery fileSystemQuery)
        => Directory.GetFiles(fileSystemQuery.Path, fileSystemQuery.SearchPattern, fileSystemQuery.SearchOption)
                    .Select(x => new FileData(x));
}
