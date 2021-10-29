using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : ITestRepository
    {
        private TestEntity Map(IDataReader reader)
        {
            var instance = new TestEntity
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name")
            };

            return instance;
        }

        public TestEntity Add(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new Mock<IDatabaseCommand>().Object,
                "Test entity was not added",
                AddResultEntity,
                AddAfterRead,
                AddFinalize
            );
        }

        private TestEntity AddResultEntity(TestEntity resultEntity)
        {
            return resultEntity;
        }

        private TestEntity AddFinalize(TestEntity resultEntity, Exception exception)
        {
            return resultEntity;
        }

        private TestEntity AddAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            resultEntity.Id = reader.GetInt32("Id");
            resultEntity.Name = reader.GetString("Name", default(string));

            return resultEntity;
        }

        public TestEntity Update(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new Mock<IDatabaseCommand>().Object,
                "Catalog entity was not updated",
                UpdateResultEntity,
                UpdateAfterRead
            );
        }

        private TestEntity UpdateResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

#pragma warning disable S4144 // Methods should not have identical implementations
        private TestEntity UpdateAfterRead(TestEntity resultEntity, IDataReader reader)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            resultEntity.Id = reader.GetInt32("Id");
            resultEntity.Name = reader.GetString("Name", default(string));

            return resultEntity;
        }

        public TestEntity Delete(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _connection.InvokeCommand
            (
                instance,
                x => new Mock<IDatabaseCommand>().Object,
                "Test entity was not deleted",
                DeleteResultEntity
            );
        }

        private TestEntity DeleteResultEntity(TestEntity resultEntity)
        {
            resultEntity.Id = 2; //for test purposes.
            return resultEntity;
        }

        public IQueryResult<TestEntity> Execute(ITestQuery query)
        {

            return _connection.Query
            (
                query,
                Map,
                DefaultOverrideLimit,
                TableAlias,
                SelectFields,
                GetDefaultOrderByClause(query),
                GetDefaultWhereClause(query)
            );
        }

#pragma warning disable S1172 // Unused method parameters should be removed
        private string GetDefaultWhereClause(ISingleEntityQuery query)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            var builder = new StringBuilder();

            return builder.ToString();
        }

#pragma warning disable S1172 // Unused method parameters should be removed
        private string GetDefaultOrderByClause(ISingleEntityQuery query)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            var builder = new StringBuilder();
            builder.Append("[Name]");
            return builder.ToString();
        }

        public TestEntity FindOne(IDatabaseCommand command)
        {

            return _connection.FindOne(command, Map);
        }

        public IReadOnlyCollection<TestEntity> FindMany(IDatabaseCommand command)
        {

            return _connection.FindMany(command, Map);
        }

        public TestEntity Find(TestEntityIdentity identity)
        {

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            return FindOne(new Mock<IDatabaseCommand>().Object);
        }

        public TestRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connection = connection;

        }

        private readonly IDbConnection _connection;

        private const string SelectFields = @"[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], [DriveTypeCodeType], [DriveTypeCode], [DriveTypeDescription], [DriveTotalSize], [DriveFreeSpace], [Recursive], [Sorted], [StartDirectory], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16]";

        private const int DefaultOverrideLimit = -1;

        private const string TableAlias = @"(SELECT c.[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], c.[DriveTypeCodeType], c.[DriveTypeCode], c.[DriveTotalSize], c.[DriveFreeSpace], c.[Recursive], c.[Sorted], c.[StartDirectory], c.[ExtraField1], c.[ExtraField2], c.[ExtraField3], c.[ExtraField4], c.[ExtraField5], c.[ExtraField6], c.[ExtraField7], c.[ExtraField8], c.[ExtraField9], c.[ExtraField10], c.[ExtraField11], c.[ExtraField12], c.[ExtraField13], c.[ExtraField14], c.[ExtraField15], c.[ExtraField16], cd.[Description] AS [DriveTypeDescription] FROM [Catalog] c INNER JOIN [Code] cd ON c.[DriveTypeCode] = cd.[Code] AND cd.[CodeType] = 'CDT') AS [CatalogView]";
    }
}
