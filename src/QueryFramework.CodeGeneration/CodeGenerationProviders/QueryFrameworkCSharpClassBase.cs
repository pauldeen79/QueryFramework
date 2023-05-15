namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract partial class QueryFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill ilt

    public override void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
    {
        base.Initialize(true, false, basePath);
    }

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string ProjectName => Constants.ProjectName;
    protected override Type BuilderClassCollectionType => typeof(IEnumerable<>);
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override bool InheritFromInterfaces => true;
    protected override bool UseLazyInitialization => false; // this needs to be disabled, because extension method-based builders currently don't support this

    protected override IEnumerable<KeyValuePair<string, string>> GetCustomBuilderNamespaceMapping()
    {
        yield return new KeyValuePair<string, string>(typeof(Expression).Namespace!, $"{typeof(Expression).Namespace}.Builders");
    }

    protected override void FixImmutableBuilderProperty(ClassPropertyBuilder property, string typeName)
    {
        if (typeName == typeof(Expression).FullName)
        {
            property.ConvertSinglePropertyToBuilderOnBuilder
            (
                !string.IsNullOrEmpty(typeName.GetGenericArguments())
                    ? $"{GetBuilderNamespace(typeName.WithoutProcessedGenerics())}.{typeName.WithoutProcessedGenerics().GetClassName()}Builder<{typeName.GetGenericArguments()}>"
                    : $"{GetBuilderNamespace(typeName)}.{typeName.GetClassName()}Builder",
                GetCustomBuilderConstructorInitializeExpressionForSingleProperty(property, typeName),
                GetCustomBuilderMethodParameterExpression(typeName)
            );

            if (!property.IsNullable)
            {
                property.SetDefaultValueForBuilderClassConstructor(new Literal($"new {typeof(EmptyExpressionBuilder).FullName}()"));
            }
        }
        else
        {
            base.FixImmutableBuilderProperty(property, typeName);
        }
    }

    private string GetCustomBuilderConstructorInitializeExpressionForSingleProperty(ClassPropertyBuilder property, string typeName)
        => property.IsNullable
            ? "{0} = source.{0} == null ? null : " + GetBuilderNamespace(typeName) + ".ExpressionBuilderFactory.Create(source.{0})"
            : "{0} = " + GetBuilderNamespace(typeName) + ".ExpressionBuilderFactory.Create(source.{0})";
    // When lazy initialization has been fixed, use this instead:
        ///=> property.IsNullable
        ///    ? "_{1}Delegate = new (() => source.{0} == null ? null : " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + "BuilderFactory.Create(source.{0}))"
        ///    : "_{1}Delegate = new (() => " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + "BuilderFactory.Create(source.{0}))";
}
