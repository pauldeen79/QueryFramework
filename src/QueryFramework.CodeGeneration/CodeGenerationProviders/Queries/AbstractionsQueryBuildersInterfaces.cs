namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class AbstractionsQueryBuildersInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders/Queries";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetOverrideModels(typeof(IQuery)),
            Constants.Namespaces.AbstractionsQueries,
            Constants.Namespaces.AbstractionsBuildersQueries
        )
        .Select
        (
            x => x.ToInterfaceBuilder()
                  .WithPartial()
                  .WithNamespace(Constants.Namespaces.AbstractionsBuildersQueries)
                  .WithName($"I{x.Name}")
                  .Chain(x => x.Methods.RemoveAll(y => y.Name.ToString() == "Validate"))
                  .Chain(x => x.Interfaces[0] = Constants.TypeNames.IQueryBuilder) // hacking here... code generation doesn't work out of the box :(
                  .Chain(x => x.Properties.RemoveAll(x => typeof(IQuery).GetProperties().Select(y => y.Name).Contains(x.Name.ToString()))) // hacking here... code generation doesn't work out of the box :(
                  .Chain(x => x.Methods.First(x => x.Name.ToString() == "Build").WithTypeName($"{Constants.Namespaces.AbstractionsQueries}.{x.Name.ToString().ReplaceSuffix("Builder", string.Empty, StringComparison.InvariantCulture)}").WithName("BuildTyped"))
                  .Build()
        )
        .ToArray();
}
