# QueryFramework
Abstraction for executing SELECT queries on Sql Server, built on top of System.Data. (IDbConnection)

Fluent extensions to build queries.

Example using in-memory query provider:
```C#
var items = new[]
{
    new MyClass { Property = "A" },
    new MyClass { Property = "B" }
};
var sut = new QueryProvider<MyClass>(items);
var query = new SingleEntityQueryBuilder()
    .Where(nameof(MyClass.Property).IsEqualTo("B"))
    .Build();

var result = sut.Query(query);
// result only contains the second item because of the WHERE clause.
```

See unit tests for more examples.

# Code generation

I am currently not storing generated files in the code repository.
To generate, you have to trigger the t4plus dotnet tool from either Visual Studio (hit F5) or a command prompt.
This will replace all generated code.

Command to install t4plus:
```bash
dotnet tool install --global pauldeen79.TextTemplateTransformationFramework.T4.Plus.Cmd --version 0.2.3
```

Command to build code generation project (example where you are in the root directory):
```bash
dotnet build ./src/ExpressionFramework.CodeGeneration/ExpressionFramework.CodeGeneration.csproj
```

Command to run code generation (example where you are in the root directory):
```bash
t4plus assembly -a ./src/QueryFramework.CodeGenertion/bin/debug/net7.0/QueryFramework.CodeGeneration.dll -p . -u ./src/QueryFramework.CodeGenertion/bin/debug/net7.0/
```

You can use the following post build event in the code generation project to run it automatically after changing code generation models or providers:

```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
  <Exec Command="t4plus assembly -a $(TargetDir)QueryFramework.CodeGeneration.dll -p $(TargetDir)../../../../ -u $(TargetDir)" />
</Target>
```

Note that if you use a post build event, this will cause SonarCloud to fail because t4plus is not available in the docker image where the code analysis runs. To fix this, you need the following PowerShell script:

```powershell
((Get-Content -path src/QueryFramework.CodeGeneration/QueryFramework.CodeGeneration.csproj -Raw) -replace 't4plus','echo') | Set-Content -Path src/QueryFramework.CodeGeneration/QueryFramework.CodeGeneration.csproj
```