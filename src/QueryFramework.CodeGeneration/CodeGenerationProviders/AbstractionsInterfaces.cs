namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Abstractions";
    public override string DefaultFileName => "Interfaces.generated.cs";

    public override object CreateModel()
        => GetModels().Select(x => x.ToInterfaceBuilder()
                                    .WithPartial()
                                    .Chain(x => x.Properties.ForEach(y => y.HasSetter = false))
                                    .Build());
}
