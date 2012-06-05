using System;
using System.Diagnostics;
using System.Net;
using Nancy.ViewEngines.Razor;
//using Raven.Client.Embedded;
//using Raven.Client;
using Nancy;
using Nancy.Bootstrapper;
using System.Collections.Generic;

namespace Gestuab
{
    class Program
    {
        static void Main (string[] args)
        {
            Console.WriteLine ("Listening on port 8889");
            Console.WriteLine ("Press CTRL+C to quit :-)");
            Process.Start ("http://127.0.0.1:8889/");
            GestuabStarter.Start (8889, true);
        }
    }
}
