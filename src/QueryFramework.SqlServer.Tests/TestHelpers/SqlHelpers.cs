using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal static class SqlHelpers
    {
        internal static void ExpressionSqlShouldBe(IQueryExpression expression, string expectedSqlForExpression)
        {
            // Arrange
            var settingsMock = new Mock<IQueryProcessorSettings>();
            settingsMock.SetupGet(x => x.TableName)
                        .Returns(nameof(MyEntity));
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(Enumerable.Empty<string>());
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            var query = new SingleEntityQuery(new[] { new QueryCondition(expression, QueryOperator.Equal, "test") });

            // Act
            var actual = new DatabaseCommandGenerator().Generate(query,
                                                                 settingsMock.Object,
                                                                 fieldProviderMock.Object,
                                                                 false).CommandText;

            // Assert
            actual.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
