namespace QueryFramework.FileSystemSearch.Entities;

public class LineData : ILineData
{
    public LineData(string line, int lineNumber, IFileData fileData)
    {
        Line = line;
        LineNumber = lineNumber;
        FileData = fileData;
    }

    public string Line { get; }
    public int LineNumber { get; }
    public IFileData FileData { get; }
}
