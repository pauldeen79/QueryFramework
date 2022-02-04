﻿using ModelFramework.Objects.Builders;

namespace QueryFramework.Abstractions.Tests.CodeGenerationProviders
{
    public class AbstractionsInterfacesModels : QueryFrameworkCodeGenerationCSharpClassBase
    {
        public override string Path => "QueryFramework.CodeGeneration\\CodeGenerationProviders";

        public override string DefaultFileName => "QueryFrameworkCSharpClassBase.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => new[]
            {
                new ClassBuilder()
                    .WithNamespace("QueryFramework.CodeGeneration.CodeGenerationProviders")
                    .WithName("QueryFrameworkCSharpClassBase")
                    .WithPartial()
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
            var namespacesToAbbreviate = new[]
            {
                "System.Collections.Generic",
                "ModelFramework.Objects.Builders"
            };
            var models = new[]
            {
                typeof(IQueryCondition),
                typeof(IQueryExpression),
                typeof(IQueryExpressionFunction),
                typeof(IQueryParameter),
                typeof(IQueryParameterValue),
                typeof(IQuerySortOrder)
            }.Select(x => x.ToClassBuilder(new ClassSettings())).ToArray();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddCsharpExpressionDumper
                (
                    x => x.AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
                          .AddSingleton<ITypeNameFormatter>(new SkipNamespacesTypeNameFormatter(namespacesToAbbreviate))
                )
                .BuildServiceProvider();
            var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

            return dumper.Dump(models);
        }
    }
}
