﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using CrossCutting.Common.Extensions;
global using CrossCutting.Data.Abstractions;
global using CrossCutting.Data.Core;
global using CrossCutting.Data.Sql.Builders;
global using CrossCutting.Data.Sql.Extensions;
global using ExpressionFramework.Domain;
global using ExpressionFramework.Domain.Contracts;
global using ExpressionFramework.Domain.Domains;
global using ExpressionFramework.Domain.Evaluatables;
global using ExpressionFramework.Domain.Expressions;
global using ExpressionFramework.Domain.Operators;
global using Microsoft.Extensions.DependencyInjection;
global using QueryFramework.Abstractions;
global using QueryFramework.Abstractions.Domains;
global using QueryFramework.Abstractions.Extensions;
global using QueryFramework.SqlServer.Abstractions;
global using QueryFramework.SqlServer.CrossCutting.Data;
global using QueryFramework.SqlServer.Extensions;
global using QueryFramework.SqlServer.FunctionParsers;
global using QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;
