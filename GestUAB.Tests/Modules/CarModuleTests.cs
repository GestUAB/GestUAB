using GestUAB.Modules;
using Nancy;
using Nancy.Validation;
using NUnit.Framework;

namespace GestUAB.Tests.Modules
{
    [TestFixture]
    public class CarModuleTests
    {

        [Test]
        public void StateTest_Get_Assert_NotEqualTo_Null()
        {
            // Arrange
            var instance = new CarModule();
            ((NancyModule)(instance)).ModelValidationResult = ModelValidationResult.Valid;

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Get);
        }

        [Test]
        public void StateTest_Post_Assert_NotEqualTo_Null()
        {
            // Arrange
            var instance = new CarModule();

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Post);
        }
    }
}