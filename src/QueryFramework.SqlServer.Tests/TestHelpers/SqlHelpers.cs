using System.Collections.Generic;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
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
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            var mapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            mapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            var queryProcessorSettingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(default(IEnumerable<string>));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(connection, mapperMock.Object, queryProcessorSettingsMock.Object, new DatabaseCommandGenerator(), fieldProviderMock.Object);
            var query = new SingleEntityQuery(new[] { new QueryCondition(expression, QueryOperator.Equal, "test") });
            sut.FindMany(query);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
        }
    }
}
