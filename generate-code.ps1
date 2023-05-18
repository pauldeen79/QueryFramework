# dotnet run --project src/QueryFramework.CodeGeneration\QueryFramework.CodeGeneration.csproj

dotnet build -c Debug src/QueryFramework.CodeGeneration/QueryFramework.CodeGeneration.csproj
Invoke-Expression "t4plus assembly -a $(Resolve-Path "src/QueryFramework.CodeGeneration/bin/Debug/net7.0/QueryFramework.CodeGeneration.dll") -p $(Resolve-Path "src") -u $(Resolve-Path "src/QueryFramework.CodeGeneration/bin/Debug/net7.0")"
