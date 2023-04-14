namespace CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract partial class QueryFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string FileNameSuffix => ".template.generated";
    protected override bool UseLazyInitialization => false; // this needs to be disabled, because extension method-based builders currently don't support this

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance.Namespace == "QueryFramework.Core")
        {
            return forCreate
                ? "QueryFramework.Core." + instance.Name
                : "QueryFramework.Abstractions.I" + instance.Name;
        }

        return string.Empty;
    }

    protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder)
    {
    }
}
