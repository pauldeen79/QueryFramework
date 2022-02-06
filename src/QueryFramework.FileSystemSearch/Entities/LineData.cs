namespace QueryFramework.FileSystemSearch.Entities;

public class LineData
{
    public LineData(string line, int lineNumber, FileData fileData)
    {
        Line = line;
        LineNumber = lineNumber;
        FileData = fileData;
    }

    public string Line { get; }
    public int LineNumber { get; }
    public FileData FileData { get; }
}
