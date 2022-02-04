using ModelFramework.Objects.Builders;

namespace QueryFramework.Abstractions.Tests.CodeGenerationProviders
{
    public abstract class QueryFrameworkCodeGenerationCSharpClassBase : CSharpClassBase
    {
        protected override bool CreateCodeGenerationHeader => true;
        protected override bool EnableNullableContext => true;
        protected override Type RecordCollectionType => typeof(ValueCollection<>);
        protected override string SetMethodNameFormatString => string.Empty;

        protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate) => string.Empty;
        protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder) { }
    }
}
