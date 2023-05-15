namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.Abstractions;

    public override object CreateModel()
        => GetCoreModels().Select(x => x.ToInterfaceBuilder()
                                        .WithPartial()
                                        .WithName($"I{x.Name}")
                                        .Build());
}
