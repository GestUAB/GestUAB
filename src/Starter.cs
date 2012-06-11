// 
// KayakStarter.cs
//  
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
// 
// Copyright (c) 2012 
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
using System;
using Kayak;
using System.Net;
using Nancy;
using System.IO;

namespace GestUAB
{
    public class Starter : ISchedulerDelegate
    {
        public static void Start (int port, bool debug)
        {
            StaticConfiguration.DisableErrorTraces = !debug;
         
            Gate.Hosts.Kayak.KayakGate.Start (
                new Starter (),
                new IPEndPoint (IPAddress.Any, port),
                Gate.Adapters.Nancy.NancyAdapter.App ());
        }

        public void OnException (IScheduler scheduler, Exception e)
        {
            Console.WriteLine ("Exception on scheduler");
            Console.Out.WriteStackTrace (e);
        }

        public void OnStop (IScheduler scheduler)
        {
            // called when Kayak's run loop is about to exit.
            // this is a good place for doing clean-up or other chores.
            Console.WriteLine ("Scheduler is stopping.");
        }

    }
}

