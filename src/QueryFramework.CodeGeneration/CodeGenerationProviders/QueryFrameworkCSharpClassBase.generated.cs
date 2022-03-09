﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
#nullable enable
    public partial class QueryFrameworkCSharpClassBase
    {
        protected static ITypeBase[] GetModels()
        {
            return new[]
            {
                new ClassBuilder()
                    .WithNamespace(@"QueryFramework.Abstractions")
                    .AddProperties(
                        new ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@"Name")
                            .WithTypeName(@"System.String"),
                        new ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@"Value")
                            .WithTypeName(@"System.Object"))
                    .WithName(@"IQueryParameter"),
                new ClassBuilder()
                    .WithNamespace(@"QueryFramework.Abstractions")
                    .AddProperties(
                        new ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@"Name")
                            .WithTypeName(@"System.String"))
                    .WithName(@"IQueryParameterValue"),
                new ClassBuilder()
                    .WithNamespace(@"QueryFramework.Abstractions")
                    .AddProperties(
                        new ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@"Field")
                            .WithTypeName(@"ExpressionFramework.Abstractions.DomainModel.IExpression"),
                        new ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@"Order")
                            .WithTypeName(@"QueryFramework.Abstractions.QuerySortOrderDirection"))
                    .WithName(@"IQuerySortOrder"),
            }.Select(x => x.Build()).ToArray();
        }
    }
#nullable restore
}
