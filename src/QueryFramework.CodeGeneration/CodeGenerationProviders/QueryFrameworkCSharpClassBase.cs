namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract partial class QueryFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill ilt

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string FileNameSuffix => ".template.generated";
    protected override string ProjectName => Constants.ProjectName;
    protected override Type BuilderClassCollectionType => typeof(IEnumerable<>);
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override bool InheritFromInterfaces => true;
    protected override string RootNamespace => Constants.Namespaces.Abstractions;
    protected override bool UseLazyInitialization => false; // this needs to be disabled, because extension method-based builders currently don't support this

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance.Namespace == Constants.Namespaces.Abstractions || instance.Namespace == Constants.Namespaces.Core)
        {
            return forCreate
                ? $"{Constants.Namespaces.Core}.{instance.Name}"
                : $"{Constants.Namespaces.Abstractions}.I{instance.Name}";
        }

        return string.Empty;
    }
}
