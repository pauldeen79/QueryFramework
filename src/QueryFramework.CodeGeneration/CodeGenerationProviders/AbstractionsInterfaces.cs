namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => Constants.Namespaces.Abstractions;

    public override object CreateModel()
        => GetCoreModels().Select(x => x.ToInterfaceBuilder()
                                        .WithPartial()
                                        .WithName($"I{x.Name}") //TODO: Review how to fix this
                                        .Build());
}
