namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => Constants.Namespaces.Abstractions;

    public override object CreateModel()
        => GetAbstractModels().Select(x => x.ToInterfaceBuilder()
                                            .WithPartial()
                                            .WithName($"I{x.Name}")
                                            .Build());
}
