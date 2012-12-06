using GestUAB.Models;
using NUnit.Framework;
using System;

namespace GestUAB.Tests.Models
{
    [TestFixture]
    public class CourseTests
    {

        [Test]
        public void StateTest_Active_Assert_EqualTo_True()
        {
            // Arrange
            var instance = new Course();
            instance.Active = true;

            // Assert
            Assert.IsTrue(instance.Active);
        }

        [Test]
        public void StateTest_Active_Assert_EqualTo_False()
        {
            // Arrange
            var instance = new Course();
            instance.Active = false;

            // Assert
            Assert.IsFalse(instance.Active);
        }

        [Test]
        public void StateTest_Id_Assert_EqualTo_The_Member_Empty()
        {
            // Arrange
            var instance = new Course();
            instance.Id = default(Guid);

            // Assert
            Assert.AreEqual(Guid.Empty, instance.Id);
        }

        [Test]
        public void StateTest_Id_Assert_EqualTo_The_Member_Empty()
        {
            // Arrange
            var instance = new Course();
            instance.Id = new Guid();

            // Assert
            Assert.AreEqual(Guid.Empty, instance.Id);
        }

        [Test]
        public void StateTest_Name_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Course();
            instance.Name = "NonEmptyString";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Name));
        }

        [Test]
        public void StateTest_Name_Assert_NotNullOrEmpty_()
        {
            // Arrange
            var instance = new Course();
            instance.Name = "String with Spaces";

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(instance.Name));
        }
    }
}