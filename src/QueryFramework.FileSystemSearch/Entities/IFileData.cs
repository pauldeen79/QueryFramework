namespace QueryFramework.FileSystemSearch.Entities;

public interface IFileData
{
    IEnumerable<string> Lines { get; }
    string FileName { get; }
    string FullPath { get; }
    string Directory { get; }
    string Extension { get; }
    string Contents { get; }
    DateTime DateCreated { get; }
    DateTime DateLastModified { get; }
}
