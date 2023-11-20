# dotnet run --project src/QueryFramework.CodeGeneration\QueryFramework.CodeGeneration.csproj

dotnet build -c Debug src/QueryFramework.CodeGeneration/QueryFramework.CodeGeneration.csproj
t4plus assembly -a src/QueryFramework.CodeGeneration/bin/Debug/net8.0/QueryFramework.CodeGeneration.dll -p src/
