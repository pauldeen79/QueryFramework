namespace QueryFramework.FileSystemSearch.Entities;

public interface ILineData
{
    string Line { get; }
    int LineNumber { get; }
    IFileData FileData { get; }
}
