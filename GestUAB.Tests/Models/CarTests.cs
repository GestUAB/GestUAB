using GestUAB.Models;
using NUnit.Framework;
using System;

namespace GestUAB.Tests.Models
{
    [TestFixture]
    public class CarTests
    {

        [Test]
        public void StateTest_Chassi_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Car();
            instance.Chassi = "String with Spaces";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Chassi));
        }

        [Test]
        public void StateTest_Id_Assert_EqualTo_The_Member_Empty()
        {
            // Arrange
            var instance = new Car();
            instance.Id = Guid.Empty;

            // Assert
            Assert.AreEqual(Guid.Empty, instance.Id);
        }

        [Test]
        public void StateTest_Name_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Car();
            instance.Name = "String with Spaces";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Name));
        }

        [Test]
        public void StateTest_Plate_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Car();
            instance.Plate = "String with Spaces";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Plate));
        }

        [Test]
        public void StateTest_Year_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Car();
            instance.Year = "String with Spaces";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Year));
        }
    }
}