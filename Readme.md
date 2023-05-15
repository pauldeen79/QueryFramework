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

Command to run code generation:
```bash
dotnet build ./src/ExpressionFramework.CodeGeneration/ExpressionFramework.CodeGeneration.csproj
```
