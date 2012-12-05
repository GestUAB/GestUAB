using System;
using Nancy;

namespace GestUAB.Tests
{
    class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = o => { return "<p>Hello, world</p><div>foo</div>"; };
        }
    }  
}
