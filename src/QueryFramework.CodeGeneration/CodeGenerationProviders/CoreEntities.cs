namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => Constants.Namespaces.Core;

    public override object CreateModel()
        => GetImmutableClasses(GetCoreModels(), Constants.Namespaces.Core);
}
