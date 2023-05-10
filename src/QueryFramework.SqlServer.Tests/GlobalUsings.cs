﻿global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Data;
global using System.Linq;
global using System.Text;
global using AutoFixture;
global using AutoFixture.AutoMoq;
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
global using Moq;
global using QueryFramework.Abstractions;
global using QueryFramework.Abstractions.Domains;
global using QueryFramework.Abstractions.Extensions;
global using QueryFramework.Abstractions.Queries;
global using QueryFramework.Core;
global using QueryFramework.Core.Builders;
global using QueryFramework.Core.Extensions;
global using QueryFramework.Core.Queries;
global using QueryFramework.Core.Queries.Builders;
global using QueryFramework.SqlServer.Abstractions;
global using QueryFramework.SqlServer.CrossCutting.Data;
global using QueryFramework.SqlServer.Extensions;
global using QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;
global using QueryFramework.SqlServer.Tests.Repositories;
global using QueryFramework.SqlServer.Tests.TestHelpers;
global using QueryFramework.SqlServer.Tests.TestHelpers.Expressions;
global using Xunit;
