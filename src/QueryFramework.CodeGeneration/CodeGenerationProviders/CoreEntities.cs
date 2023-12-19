namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities : QueryFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.Core;

    public override object CreateModel()
        => GetImmutableClasses(GetCoreModels(), Constants.Namespaces.Core)
            .OfType<IClass>()
            .Select(x => new ClassBuilder(x)
                .AddMethods(new[] { new ClassMethodBuilder()
                    .WithName("ToBuilder")
                    .WithTypeName($"{Constants.Namespaces.CoreBuilders}.{x.Name}Builder")
                    .AddLiteralCodeStatements($"return new {Constants.Namespaces.CoreBuilders}.{x.Name}Builder(this);") }.Where(x => !x.Name.EndsWith("Base"))
                )
                .Chain(x => { if (x.Name.EndsWith("Base")) { x.Methods.RemoveAll(x => x.Name == "ToBuilder"); } })
                .BuildTyped()
            )
            .ToArray();
}
