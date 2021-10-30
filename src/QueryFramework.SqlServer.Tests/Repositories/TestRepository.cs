using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : ITestRepository
    {
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

        public TestEntity FindOne(ITestQuery query)
        {
            return _queryProcessor.FindOne(query);
        }

        public IReadOnlyCollection<TestEntity> FindMany(ITestQuery query)
        {
            return _queryProcessor.FindMany(query);
        }

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
        {
            return _queryProcessor.FindPaged(query);
        }

        public TestEntity FindOne(IDatabaseCommand command)
        {

            return _connection.FindOne(command, _mapper.Map);
        }

        public IReadOnlyCollection<TestEntity> FindMany(IDatabaseCommand command)
        {

            return _connection.FindMany(command, _mapper.Map);
        }

        public TestEntity Find(TestEntityIdentity identity)
        {

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            return FindOne(new Mock<IDatabaseCommand>().Object);
        }

        public TestRepository(IDbConnection connection,
                              IQueryProcessor<ITestQuery, TestEntity> queryProcessor,
                              IDataReaderMapper<TestEntity> mapper)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (queryProcessor == null)
            {
                throw new ArgumentNullException(nameof(queryProcessor));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _connection = connection;
            _queryProcessor = queryProcessor;
            _mapper = mapper;

        }

        private readonly IDbConnection _connection;
        private readonly IQueryProcessor<ITestQuery, TestEntity> _queryProcessor;
        private readonly IDataReaderMapper<TestEntity> _mapper;

        private const string SelectFields = @"[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], [DriveTypeCodeType], [DriveTypeCode], [DriveTypeDescription], [DriveTotalSize], [DriveFreeSpace], [Recursive], [Sorted], [StartDirectory], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16]";

        private const string TableAlias = @"(SELECT c.[Id], [Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], c.[DriveTypeCodeType], c.[DriveTypeCode], c.[DriveTotalSize], c.[DriveFreeSpace], c.[Recursive], c.[Sorted], c.[StartDirectory], c.[ExtraField1], c.[ExtraField2], c.[ExtraField3], c.[ExtraField4], c.[ExtraField5], c.[ExtraField6], c.[ExtraField7], c.[ExtraField8], c.[ExtraField9], c.[ExtraField10], c.[ExtraField11], c.[ExtraField12], c.[ExtraField13], c.[ExtraField14], c.[ExtraField15], c.[ExtraField16], cd.[Description] AS [DriveTypeDescription] FROM [Catalog] c INNER JOIN [Code] cd ON c.[DriveTypeCode] = cd.[Code] AND cd.[CodeType] = 'CDT') AS [CatalogView]";
    }
}
