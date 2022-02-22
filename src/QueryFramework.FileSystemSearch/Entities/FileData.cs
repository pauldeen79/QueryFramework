namespace QueryFramework.FileSystemSearch.Entities;

public class FileData : IFileData
{
    private readonly FileInfo _fileInfo;

    public FileData(string fileName) => _fileInfo = new FileInfo(fileName);

    private string[]? _lines;
    public IEnumerable<string> Lines
    {
        get
        {
            if (_lines == null)
            {
                _lines = File.ReadAllLines(FullPath);
            }
            return _lines;
        }
    }

    public string FileName => _fileInfo.Name;
    public string FullPath => _fileInfo.FullName;
    public string Directory => _fileInfo.DirectoryName;
    public string Extension => _fileInfo.Extension;
    public string Contents => string.Join(Environment.NewLine, Lines);
    public DateTime DateCreated => _fileInfo.CreationTime;
    public DateTime DateLastModified => _fileInfo.LastWriteTime;
}
