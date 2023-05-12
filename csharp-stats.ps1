$list = New-Object Collections.Generic.List[LineCountInfo]
foreach ($directory in Get-ChildItem -Path ./src -Directory)
{
    [long]$totalLines = 0;
    [long]$generatedLines = 0;
    [long]$notGeneratedLines = 0;
    foreach ($file in Get-ChildItem -Path $directory -File -Filter *.cs -Recurse)
    {
        $lineCount = (Get-Content $file).Length
        $totalLines += $lineCount
        if ($file.FullName.EndsWith(".template.generated.cs"))
        {
            $generatedLines += $lineCount
        }
        else
        {
            $notGeneratedLines += $lineCount
        }
    }
    $item = [LineCountInfo]::new()
    $item.Directory = $directory.Name
    $item.GeneratedLines = $generatedLines
    $item.NotGeneratedLines = $notGeneratedLines
    $item.TotalLines = $totalLines
    $list.Add($item)
}

$list | Format-Table -AutoSize -Property Directory, GeneratedLines, NotGeneratedLines, TotalLines

class LineCountInfo {
    [string]$Directory;
    [long]$GeneratedLines;
    [long]$NotGeneratedLines;
    [long]$TotalLines;
}