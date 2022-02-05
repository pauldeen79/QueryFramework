namespace QueryFramework.Abstractions.Tests.CodeGenerationProviders;

public abstract class QueryFrameworkCodeGenerationCSharpClassBase : CSharpClassBase
{
    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(ValueCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate) => string.Empty;
    protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder) { }

    protected abstract string[] NamespacesToAbbreviate { get; }
    protected abstract Type[] Models { get; }

    public override object CreateModel()
        => new[]
        {
            new ClassBuilder()
                .WithNamespace("QueryFramework.CodeGeneration.CodeGenerationProviders")
                .WithName("QueryFrameworkCSharpClassBase")
                .WithPartial()
                .AddUsings(NamespacesToAbbreviate)
                .AddMethods(new ClassMethodBuilder()
                    .WithName("GetModels")
                    .WithProtected()
                    .WithStatic()
                    .WithType(typeof(ITypeBase[]))
                    .AddLiteralCodeStatements($"return {CreateCode()}.Select(x => x.Build()).ToArray();"))
        }
        .Select(x => x.Build())
        .ToArray();

    private string CreateCode()
    {
        var models = Models.Select(x => x.ToClassBuilder(new ClassSettings())).ToArray();
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection
            .AddCsharpExpressionDumper
            (
                x => x.AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
                      .AddSingleton<ITypeNameFormatter>(new SkipNamespacesTypeNameFormatter(NamespacesToAbbreviate))
            )
            .BuildServiceProvider();
        var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

        return dumper.Dump(models);
    }
}
