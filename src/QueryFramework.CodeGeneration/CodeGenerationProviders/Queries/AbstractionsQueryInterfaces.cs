namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class AbstractionsQueryInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Queries";

    public override object CreateModel()
        => GetOverrideModels(typeof(IQuery)).Select(x => x.ToInterfaceBuilder()
                                            .WithPartial()
                                            .WithName($"I{x.Name}")
                                            .WithNamespace(Constants.Namespaces.AbstractionsQueries)
                                            .Chain(x => x.Interfaces[0] = Constants.TypeNames.IQuery) // hacking here... code generation doesn't work out of the box :(
                                            .Chain(x => x.Properties.RemoveAll(x => typeof(IQuery).GetProperties().Select(y => y.Name).Contains(x.Name.ToString()))) // hacking here... code generation doesn't work out of the box :(
                                            .Build());
}
