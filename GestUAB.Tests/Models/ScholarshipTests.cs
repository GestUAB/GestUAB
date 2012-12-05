using GestUAB.Models;
using NUnit.Framework;
using System;


namespace GestUAB.Tests.Models
{
	[TestFixture]
	public class ScholarshipTests
	{
        	
		[Test]
		public void TestarOwnerNaoNull()
		{
			// Arrange
			var instance = new Scholarship();
			instance.Owner = "NonEmptyString";

			// Act
			instance.ToString();

			// Assert
			Assert.AreNotEqual(instance.Owner, "");
		}

		[Test]
		public void TestarIdsNewGuild()
		{
			// Arrange
			var instance = new Scholarship();
			instance.Id = default(Guid);
		
			// Assert
			Assert.AreEqual(Guid.Empty, instance.Id);
		}
}

	}
}