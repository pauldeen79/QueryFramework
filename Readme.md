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
To generate, you have to trigger the code generation tool from either Visual Studio (hit F5) or a command prompt.
This will replace all generated code.

I am using t4plus, which is a dotnet global tool, so it can run on all environments.
The models and code generation are stored in an assembly which is also added to the solution.
Note that there is no project reference to this project, it is only needed at build time.

At this moment, I am not using automatic code generation after changing code generation models or providers.
This can be enabled, but is a bit cumbersome in build pipelines.
First, the file system isn't updated fast enough, so the build will not succeed on a fresh cloned instance.
Second, Sonar runs the post build command in a Docker image, and this image doesn't know how to load the code generation tool.
Because of these problems, I have commented the post build event. You may uncomment it if you find yourself constantly re-generating stuff.

Command to install t4plus:

```bash
dotnet tool install --global pauldeen79.TextTemplateTransformationFramework.T4.Plus.Cmd --version 0.2.3
```

Command to build code generation project (example where you are in the root directory):

```bash
dotnet build -c Debug src/QueryFramework.CodeGeneration/QueryFramework.CodeGeneration.csproj
```

Command to run code generation (example where you are in the root directory):

```powershell
Invoke-Expression "t4plus assembly -a $(Resolve-Path "src/QueryFramework.CodeGeneration/bin/Debug/net7.0/QueryFramework.CodeGeneration.dll") -p $(Resolve-Path "src") -u $(Resolve-Path "src/QueryFramework.CodeGeneration/bin/Debug/net7.0")"
```

Note that the -a argument of the t4plus assembly command currently requires a fully qualified path to the code generation assembly.

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
