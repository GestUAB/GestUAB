using GestUAB.Modules;
using Nancy;
using System;
using QuickUnit;
using NUnit.Framework;

namespace GestUAB.Tests.Modules
{
	[TestFixture]
	public class HomeModuleTests
	{
		
		[Test]
		public void TestesHomeModule()
		{
			// Arrange
			var instance = new HomeModule();
			((NancyModule)(instance)).Get.InvokeNonPublicX("AddRoute", "/", ((Func<NancyContext, bool>)(a1 => default(bool))), ((Func<object, object>)(a1 => default(object))));

			// Assert
			Assert.IsNotNull(((NancyModule)(instance)).View);
		}

		[Test]
		public void TestesHomeModuleAbout()
		{
			// Arrange
			var instance = new HomeModule();
			((NancyModule)(instance)).Get.InvokeNonPublicX("AddRoute", "/about", ((Func<NancyContext, bool>)(a1 => default(bool))), ((Func<object, object>)(a1 => default(object))));
		
			// Assert
			Assert.IsNotNull(((NancyModule)(instance)).View);
		}
	}
}