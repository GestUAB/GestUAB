//
// ScaffoldingTests.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2013 Tony Alexander Hild
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Runtime.InteropServices;
using Nancy.Scaffolding;

namespace GestUAB.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ScaffoldingTests
    {
        public ScaffoldingTests()
        {

        }

        [Test]
        public void SimpleTest()
        {

            var container = new ConfigContainer();
            container.Register((Foo x) => x.Bar1)
                .Named("Bar1")
                .Described("Bar1 description")
                .WithInputType(InputType.Auto)
                .WithVisibilityConfig(all: Visibility.Show)
                .WithObjectListConfig(new ObjectListConfig(() => {
                return new GestUAB.DataAccess.DataFacade().ReadAllCursos();
            }));


            var config = container.GetConfig((Foo x) => x.Bar1);
            Assert.AreEqual(config.Name, "Bar1");
            Assert.AreEqual(config.Description, "Bar1 description");
            Assert.AreEqual(config.InputType, InputType.Auto);
            Assert.AreEqual(config.VisibilityConfig.Create, Visibility.Show);
            Assert.AreEqual(config.VisibilityConfig.Delete, Visibility.Show);
            Assert.AreEqual(config.VisibilityConfig.Read, Visibility.Show);
            Assert.AreEqual(config.VisibilityConfig.Update, Visibility.Show);
            Assert.NotNull(config.ObjectListConfig.Objects);

            container.Register((Foo x) => x.Bar2)
                .Named("Bar2")
                    .Described("Bar2 description")
                    .WithInputType(InputType.Checkbox)
                    .WithVisibilityConfig(create: Visibility.Hidden);

            config = container.GetConfig((Foo x) => x.Bar2);
            Assert.AreNotEqual(config.Name, "Bar1");
            Assert.AreNotEqual(config.Description, "Bar1 description");
            Assert.AreNotEqual(config.InputType, InputType.Auto);
            Assert.AreEqual(config.VisibilityConfig.Create, Visibility.Hidden);
           
        }

        public class Foo
        {

            public string Bar1 { get; set; }

            public string Bar2 { get; set; }
        }
    }
}

