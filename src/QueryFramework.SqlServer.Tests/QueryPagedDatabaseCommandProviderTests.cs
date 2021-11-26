﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Abstractions;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public class QueryPagedDatabaseCommandProviderTests : TestBase<QueryPagedDatabaseCommandProvider<ISingleEntityQuery>>
    {
        [Theory]
        [InlineData(DatabaseOperation.Delete)]
        [InlineData(DatabaseOperation.Insert)]
        [InlineData(DatabaseOperation.Unspecified)]
        [InlineData(DatabaseOperation.Update)]
        public void Creat_Generates_Correct_Command_When_DatabaseOperation_Is_Not_Select(DatabaseOperation operation)
        {
            // Act
            Sut.Invoking(x => x.Create(new Mock<ISingleEntityQuery>().Object, operation))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("operation");
        }

        [Fact]
        public void Create_With_Source_Argument_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
        {
            // Arrange
            var settingsMock = Fixture.Freeze<Mock<IQueryProcessorSettings>>();
            settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
            var fieldProviderMock = Fixture.Freeze<Mock<IQueryFieldProvider>>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.Is<IQueryExpression>(x => x.FieldName == "Field"))).Returns(true);

            // Act
            var actual = Sut.Create(new SingleEntityQuery(null, null, new[] { new QueryCondition("Field", QueryOperator.Equal, "Value") }, Enumerable.Empty<IQuerySortOrder>()), DatabaseOperation.Select);

            // Assert
            actual.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
            actual.CommandParameters.Should().NotBeNull();
            var parameters = actual.CommandParameters as IEnumerable<KeyValuePair<string, object>>;
            parameters.Should().NotBeNull();
            if (parameters != null)
            {
                parameters.Should().ContainSingle();
                parameters.First().Key.Should().Be("p0");
                parameters.First().Value.Should().Be("Value");
            }
        }

        [Fact]
        public void CreatePaged_With_Source_Argument_Generates_Correct_Command_When_DatabaseOperation_Is_Select()
        {
            // Arrange
            const int pageSize = 10;
            var settingsMock = Fixture.Freeze<Mock<IQueryProcessorSettings>>();
            settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
            settingsMock.SetupGet(x => x.OverridePageSize).Returns(pageSize);
            var fieldProviderMock = Fixture.Freeze<Mock<IQueryFieldProvider>>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.Is<IQueryExpression>(x => x.FieldName == "Field"))).Returns(true);

            // Act
            var actual = Sut.CreatePaged(new SingleEntityQuery(null, null, new[] { new QueryCondition("Field", QueryOperator.Equal, "Value") }, Enumerable.Empty<IQuerySortOrder>()), DatabaseOperation.Select, 0, pageSize);

            // Assert
            actual.DataCommand.CommandText.Should().Be("SELECT TOP 10 * FROM MyTable WHERE Field = @p0");
            actual.DataCommand.CommandParameters.Should().NotBeNull();
            var parameters = actual.DataCommand.CommandParameters as IEnumerable<KeyValuePair<string, object>>;
            parameters.Should().NotBeNull();
            if (parameters != null)
            {
                parameters.Should().ContainSingle();
                parameters.First().Key.Should().Be("p0");
                parameters.First().Value.Should().Be("Value");
            }
        }
    }
}