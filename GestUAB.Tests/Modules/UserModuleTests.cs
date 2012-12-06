using GestUAB.Modules;
using Nancy;
using Nancy.Validation;
using NUnit.Framework;
using System;
using QuickUnit;

namespace GestUAB.Tests.Modules
{
    [TestFixture]
    public class UserModuleTests
    {

        [Test]
        public void StateTest_ModelValidationResult_Assert_EqualTo_Null()
        {
            // Arrange
            var instance = new UserModule();
            ((NancyModule)(instance)).ValidatorLocator = new DefaultValidatorLocator(null);

            // Assert
            Assert.IsNull(((NancyModule)(instance)).ModelValidationResult);
        }

        [Test]
        public void StateTest_Routes_Assert_NotEqualTo_Null()
        {
            // Arrange
            var instance = new UserModule();
            ((NancyModule)(instance)).Get.InvokeNonPublicX("AddRoute", "/", ((Func<NancyContext, bool>)(a1 => default(bool))), ((Func<object, object>)(a1 => default(object))));

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Routes);
        }

        [Test]
        public void StateTest_Delete_Assert_NotEqualTo_Null()
        {
            // Arrange
            var instance = new UserModule();
            ((IHideObjectMembers)(((NancyModule)(instance)).Delete)).GetType();

            // Assert
            Assert.IsNotNull(((NancyModule)(instance)).Delete);
        }
    }
}