using GestUAB.Modules;
using NUnit.Framework;
using Nancy;
using Nancy.Validation;

namespace GestUAB.Tests.Modules
{
    [TestFixture]
    public class DriverModuleTests
    {

        [Test]
        public void StateTest()
        {
            // Arrange
            var instance = new DriverModule();

            // Assert
            Assert.Fail("No expectation defined");
        }

        [Test]
        public void Editing_Driver_NotEqualTo_Null()
        {
            // Arrange
            var instance = new DriverModule();
            ((NancyModule)(instance)).ModelValidationResult = ModelValidationResult.Valid;

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Post);
        }

        [Test]
        public void Delete_Driver()
        {
            // Arrange
            var instance = new DriverModule();

            // Assert
            Assert.Fail("No expectation defined");
        }

        [Test]
        public void StateTest_Get_Assert_NotEqualTo_Null()
        {
            // Arrange
            var instance = new DriverModule();
            ((NancyModule)(instance)).ModelValidationResult = ModelValidationResult.Valid;
            ((NancyModule)(instance)).Response = null;

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Get);
        }

        [Test]
        public void Display_Driver()
        {
            // Arrange
            var instance = new DriverModule();

            // Assert
            Assert.Fail("No expectation defined");
        }

        [Test]
        public void Driver_Display()
        {
            // Arrange
            var instance = new DriverModule();
            ((NancyModule)(instance)).ModelValidationResult = ModelValidationResult.Valid;

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Get);
        }

        [Test]
        public void Create_Default()
        {
            // Arrange
            var instance = new DriverModule();

            // Assert
            Assert.Fail("No expectation defined");
        }

        [Test]
        public void Default_driver()
        {
            // Arrange
            var instance = new DriverModule();
            ((NancyModule)(instance)).ModelValidationResult = ModelValidationResult.Valid;

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Get);
        }

        [Test]
        public void StateTest_Get_Has_Members_Expectations()
        {
            // Arrange
            var instance = new DriverModule();
            ((NancyModule)(instance)).ModelBinderLocator = null;
        }
    }
}
