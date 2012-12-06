using GestUAB;
using NUnit.Framework;

namespace GestUAB.Tests.Models
{
	[TestFixture]
	public class MemorandumTests
	{
		
		[Test]
		public void StateTest_BankAccount_Assert_NotNullOrEmpty_()
		{
			// Arrange
			var instance = new Memorandum();
			instance.BankAccount = "String with Spaces";

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(instance.BankAccount));
		}
	}
}