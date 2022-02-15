namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class CoreEntities : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Core\\DomainModel";

    public override string DefaultFileName => "Entities.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses
        (
            GetCoreModels(),
            "ExpressionFramework.Core.DomainModel"
        ).Select
        (
            x => new ClassBuilder(x)
                .Chain(y => y.Methods.RemoveAll(z => z.Static))
                .Chain(y =>
                {
                    if (y.Interfaces[0].EndsWith("Expression"))
                    {
                        y.Methods.Add(new ClassMethodBuilder()
                            .WithName("ToBuilder")
                            .WithTypeName("ExpressionFramework.Abstractions.DomainModel.Builders.IExpressionBuilder")
                            .AddLiteralCodeStatements($"return new ExpressionFramework.Core.DomainModel.Builders.{y.Name}Builder(this);")
                        );
                    }
                })
                .Build()
        )
        .ToArray();
}
