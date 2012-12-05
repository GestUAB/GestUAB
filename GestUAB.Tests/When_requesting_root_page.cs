using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using NUnit.Framework;
using Nancy.Testing;
using Nancy.Bootstrapper;
using Nancy.ViewEngines.Razor;



namespace GestUAB.Tests
{
    [TestFixture]

    public class When_requesting_root_page
    {
        private BrowserResponse _result;

        [SetUp]
        public void SetUp()
        {
            var bootstrapper = new AppBootstrapper();
            var browser = new Browser(bootstrapper);
            bootstrapper.Initialise();

            _result = browser.Get("/", with => with.HttpRequest());
        }

        [Test]
        public void return_ok_status_code()
        {
            Assert.That(_result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}