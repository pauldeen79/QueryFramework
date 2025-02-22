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
            Action a = () => sut.AddParameter(name: null!, "some value");
            a.ShouldThrow<ArgumentNullException>()
             .ParamName.ShouldBe("name");
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
            parameterList.Count.ShouldBe(1);
            parameterList[0].Name.ShouldBe("Name");
            parameterList[0].Value.ShouldBeEquivalentTo("some value");
        }

        [Fact]
        public void Can_Call_Build_And_ToBuilder_On_Result()
        {
            // Arrange
            var sut = Fixture.Freeze<IParameterizedQueryBuilder>();
            var parameterList = new List<IQueryParameterBuilder>();
            sut.Parameters.ReturnsForAnyArgs(parameterList);
            _ = sut.AddParameter("Name", "some value");
            parameterList.Count.ShouldBe(1);

            // Act
            var parameter = parameterList[0].Build();
            var builder = parameter.ToBuilder();

            // Assert
            builder.Name.ShouldBe(parameterList[0].Name);
            builder.Value.ShouldBeEquivalentTo(parameterList[0].Value);
        }
    }
}
