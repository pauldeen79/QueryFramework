﻿global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Data;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using AutoFixture;
global using AutoFixture.AutoNSubstitute;
global using CrossCutting.Common;
global using CrossCutting.Common.Results;
global using CrossCutting.Data.Abstractions;
global using CrossCutting.Data.Core;
global using CrossCutting.Data.Core.Builders;
global using CrossCutting.Data.Core.Commands;
global using CrossCutting.Data.Sql.Builders;
global using CrossCutting.Data.Sql.Extensions;
global using ExpressionFramework.Domain;
global using ExpressionFramework.Domain.Builders;
global using ExpressionFramework.Domain.Builders.Evaluatables;
global using ExpressionFramework.Domain.Builders.Expressions;
global using ExpressionFramework.Domain.Builders.Operators;
global using ExpressionFramework.Domain.Contracts;
global using ExpressionFramework.Domain.Domains;
global using ExpressionFramework.Domain.Evaluatables;
global using ExpressionFramework.Domain.Expressions;
global using FluentAssertions;
global using Microsoft.Extensions.DependencyInjection;
global using NSubstitute;
global using QueryFramework.Abstractions;
global using QueryFramework.Abstractions.Builders;
global using QueryFramework.Abstractions.Builders.Extensions;
global using QueryFramework.Abstractions.Domains;
global using QueryFramework.Abstractions.Extensions;
global using QueryFramework.Core;
global using QueryFramework.Core.Builders;
global using QueryFramework.Core.Builders.Queries;
global using QueryFramework.Core.Queries;
global using QueryFramework.SqlServer.Abstractions;
global using QueryFramework.SqlServer.CrossCutting.Data;
global using QueryFramework.SqlServer.Extensions;
global using QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;
global using QueryFramework.SqlServer.Tests.Repositories;
global using QueryFramework.SqlServer.Tests.TestHelpers;
global using Xunit;
