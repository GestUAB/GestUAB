using GestUAB.Models;
using NUnit.Framework;
using System;

namespace GestUAB.Tests.Models
{
    [TestFixture]
    public class DriverTests
    {

        [Test]
        public void Parametros_Driver()
        {
            // Arrange
            var instance = new Driver();

            // Assert
            Assert.Fail("No expectation defined");
        }

        [Test]
        public void StateTest_Id_Assert_EqualTo_The_Member_Empty()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreEqual(Guid.Empty, instance.Id);
        }

        [Test]
        public void StateTest_Address_Assert_EqualTo_()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreEqual(instance.Address, "");
        }

        [Test]
        public void StateTest_Complement_Assert_NotEqualTo_()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreNotEqual(instance.Complement, "");
        }

        [Test]
        public void StateTest_Cep_Assert_NotEqualTo_()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreNotEqual(instance.Cep, "");
        }

        [Test]
        public void StateTest_Neighborhood_Assert_NotEqualTo_()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreNotEqual(instance.Neighborhood, "");
        }

        [Test]
        public void StateTest_Obs_Assert_NotEqualTo_()
        {
            // Arrange
            var instance = new Driver();
            instance.Address = "";

            // Assert
            Assert.AreNotEqual(instance.Obs, "");
        }
    }
}