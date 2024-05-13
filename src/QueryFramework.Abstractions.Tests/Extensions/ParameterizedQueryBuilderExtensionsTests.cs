namespace QueryFramework.Abstractions.Tests.Extensions;

public class ParameterizedQueryBuilderExtensionsTests : TestBase
{
    public class AddParameter : ParameterizedQueryBuilderExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Name()
        {
            // Arrange
            var sut = Fixture.Freeze<IParameterizedQueryBuilder>();

            // Act & Assert
            sut.Invoking(x => x.AddParameter(name: null!, "some value"))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("name");
        }

        [Fact]
        public void Adds_Parameter_On_Filled_Name()
        {
            // Arrange
            var sut = Fixture.Freeze<IParameterizedQueryBuilder>();
            var parameterList = new List<IQueryParameterBuilder>();
            sut.Parameters.ReturnsForAnyArgs(parameterList);

            // Act
            _ = sut.AddParameter("Name", "some value");

            // Assert
            parameterList.Should().HaveCount(1);
            parameterList[0].Name.Should().Be("Name");
            parameterList[0].Value.Should().BeEquivalentTo("some value");
        }

        [Fact]
        public void Can_Call_Build_And_ToBuilder_On_Result()
        {
            // Arrange
            var sut = Fixture.Freeze<IParameterizedQueryBuilder>();
            var parameterList = new List<IQueryParameterBuilder>();
            sut.Parameters.ReturnsForAnyArgs(parameterList);
            _ = sut.AddParameter("Name", "some value");
            parameterList.Should().HaveCount(1);

            // Act
            var parameter = parameterList[0].Build();
            var builder = parameter.ToBuilder();

            // Assert
            builder.Name.Should().Be(parameterList[0].Name);
            builder.Value.Should().BeEquivalentTo(parameterList[0].Value);
        }
    }
}
