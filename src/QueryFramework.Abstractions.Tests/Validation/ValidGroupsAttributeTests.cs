namespace QueryFramework.Abstractions.Tests.Validation;

public class ValidGroupsAttributeTests : TestBase
{
    public class IsValid : ValidGroupsAttributeTests
    {
        [Fact]
        public void Returns_ValidationError_When_Used_On_Class_Without_StartGroupProperty()
        {
            // Arrange
            var sut = new ValidGroupsAttribute();
            var instance = new ContainerForEntityWithoutStartGroupProperty();

            // Act
            var result = sut.GetValidationResult(instance, new ValidationContext(instance));

            // Assert
            result.ShouldNotBeNull();
            result.ErrorMessage.ShouldBe($"Properties 'StartGroup' or 'EndGroup' not found on ContainerForEntityWithoutStartGroupProperty");
        }

        [Fact]
        public void Returns_ValidationError_When_Used_On_Class_Without_EndGroupProperty()
        {
            // Arrange
            var sut = new ValidGroupsAttribute();
            var instance = new ContainerForEntityWithoutEndGroupProperty();

            // Act
            var result = sut.GetValidationResult(instance, new ValidationContext(instance));

            // Assert
            result.ShouldNotBeNull();
            result.ErrorMessage.ShouldBe($"Properties 'StartGroup' or 'EndGroup' not found on ContainerForEntityWithoutEndGroupProperty");
        }

        [Fact]
        public void Returns_ValidationError_When_Used_On_Class_Without_StartGroupProperty_And_EndGroupProperty()
        {
            // Arrange
            var sut = new ValidGroupsAttribute();
            var instance = new ContainerForEntityWithoutStartGroupPropertyAndEndgroupProperty();

            // Act
            var result = sut.GetValidationResult(instance, new ValidationContext(instance));

            // Assert
            result.ShouldNotBeNull();
            result.ErrorMessage.ShouldBe($"Properties 'StartGroup' or 'EndGroup' not found on ContainerForEntityWithoutStartGroupPropertyAndEndGroupProperty");
        }

        private sealed class EntityWithoutStartGroupProperty
        {
            public bool EndGroup { get; set; }
        }

        private sealed class EntityWithoutEndGroupProperty
        {
            public bool StartGroup { get; set; }
        }

        private sealed class EntityWithoutStartGroupPropertyAndEndGroupProperty
        {
        }

        private sealed class ContainerForEntityWithoutStartGroupProperty
        {
            [ValidGroups]
            public IReadOnlyCollection<EntityWithoutStartGroupProperty>? Filter
            {
                get;
            }
        }

        private sealed class ContainerForEntityWithoutEndGroupProperty
        {
            [ValidGroups]
            public IReadOnlyCollection<EntityWithoutEndGroupProperty>? Filter
            {
                get;
            }
        }

        private sealed class ContainerForEntityWithoutStartGroupPropertyAndEndgroupProperty
        {
            [ValidGroups]
            public IReadOnlyCollection<EntityWithoutStartGroupPropertyAndEndGroupProperty>? Filter
            {
                get;
            }
        }
    }
}
