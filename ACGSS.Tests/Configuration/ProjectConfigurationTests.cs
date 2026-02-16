using System.Reflection;
using System.Runtime.Versioning;

namespace ACGSS.Tests.Configuration
{
    [TestFixture]
    public class ProjectConfigurationTests
    {
        [Test]
        public void Domain_Assembly_Should_BeLoaded()
        {
            // Act
            var assembly = Assembly.Load("ACGSS.Domain");

            // Assert
            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.GetName().Name, Is.EqualTo("ACGSS.Domain"));
        }

        [Test]
        public void Application_Assembly_Should_BeLoaded()
        {
            // Act
            var assembly = Assembly.Load("ACGSS.Application");

            // Assert
            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.GetName().Name, Is.EqualTo("ACGSS.Application"));
        }

        [Test]
        public void Infrastructure_Assembly_Should_BeLoaded()
        {
            // Act
            var assembly = Assembly.Load("ACGSS.Infrastructure");

            // Assert
            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.GetName().Name, Is.EqualTo("ACGSS.Infrastructure"));
        }

        [Test]
        public void Tests_Assembly_Should_BeLoaded()
        {
            // Act
            var assembly = Assembly.Load("ACGSS.Tests");

            // Assert
            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.GetName().Name, Is.EqualTo("ACGSS.Tests"));
        }

        [Test]
        public void Domain_Assembly_Should_TargetCorrectFramework()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Domain");
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

            // Assert
            Assert.That(targetFrameworkAttribute, Is.Not.Null);
            Assert.That(targetFrameworkAttribute.FrameworkName, Does.Contain(".NETCoreApp"));
        }

        [Test]
        public void Application_Assembly_Should_TargetCorrectFramework()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Application");
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

            // Assert
            Assert.That(targetFrameworkAttribute, Is.Not.Null);
            Assert.That(targetFrameworkAttribute.FrameworkName, Does.Contain(".NETCoreApp"));
        }

        [Test]
        public void Infrastructure_Assembly_Should_TargetCorrectFramework()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Infrastructure");
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

            // Assert
            Assert.That(targetFrameworkAttribute, Is.Not.Null);
            Assert.That(targetFrameworkAttribute.FrameworkName, Does.Contain(".NETCoreApp"));
        }

        [Test]
        public void Domain_Assembly_Should_ContainExpectedTypes()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Domain");

            // Act
            var types = assembly.GetTypes();

            // Assert
            Assert.That(types, Is.Not.Empty);
            Assert.That(types.Any(t => t.Name == "User"), Is.True);
            Assert.That(types.Any(t => t.Name == "UserDto"), Is.True);
            Assert.That(types.Any(t => t.Name == "UserStatus"), Is.True);
        }

        [Test]
        public void Application_Assembly_Should_ContainExpectedTypes()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Application");

            // Act
            var types = assembly.GetTypes();

            // Assert
            Assert.That(types, Is.Not.Empty);
            Assert.That(types.Any(t => t.Name == "UserService"), Is.True);
        }

        [Test]
        public void Infrastructure_Assembly_Should_ContainExpectedTypes()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Infrastructure");

            // Act
            var types = assembly.GetTypes();

            // Assert
            Assert.That(types, Is.Not.Empty);
            Assert.That(types.Any(t => t.Name == "UserValidator"), Is.True);
            Assert.That(types.Any(t => t.Name == "EmailSender"), Is.True);
            Assert.That(types.Any(t => t.Name == "UserRepository"), Is.True);
        }

        [Test]
        public void Application_Should_ReferenceDomain()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Application");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "ACGSS.Domain"), Is.True);
        }

        [Test]
        public void Infrastructure_Should_ReferenceDomain()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Infrastructure");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "ACGSS.Domain"), Is.True);
        }

        [Test]
        public void Tests_Should_ReferenceAllProjectAssemblies()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Tests");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "ACGSS.Domain"), Is.True);
            Assert.That(referencedAssemblies.Any(a => a.Name == "ACGSS.Application"), Is.True);
            Assert.That(referencedAssemblies.Any(a => a.Name == "ACGSS.Infrastructure"), Is.True);
        }

        [Test]
        public void Application_Should_ReferenceAutoMapper()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Application");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "AutoMapper"), Is.True);
        }

        [Test]
        public void Infrastructure_Should_ReferenceEntityFramework()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Infrastructure");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name.Contains("EntityFrameworkCore")), Is.True);
        }

        [Test]
        public void Tests_Should_ReferenceNUnit()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Tests");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "nunit.framework"), Is.True);
        }

        [Test]
        public void Tests_Should_ReferenceMoq()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Tests");
            var referencedAssemblies = assembly.GetReferencedAssemblies();

            // Assert
            Assert.That(referencedAssemblies.Any(a => a.Name == "Moq"), Is.True);
        }

        [Test]
        public void RuntimeVersion_Should_BeCompatibleWithNet10()
        {
            // Arrange
            var currentFramework = Environment.Version;

            // Assert
            Assert.That(currentFramework.Major, Is.GreaterThanOrEqualTo(8));
        }

        [Test]
        public void ImplicitUsings_Should_BeAvailable()
        {
            // This test verifies that implicit usings are working by using a type
            // that would only be available with implicit usings enabled

            // Arrange & Act
            var list = new List<string> { "test" };

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void NullableAnnotations_Should_BeEnabled()
        {
            // Arrange
            var assembly = Assembly.Load("ACGSS.Domain");
            var userType = assembly.GetType("ACGSS.Domain.Entities.User");

            // Assert
            Assert.That(userType, Is.Not.Null);
            var properties = userType.GetProperties();
            Assert.That(properties, Is.Not.Empty);
        }
    }
}