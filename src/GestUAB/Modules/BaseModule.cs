// 
// BaseModule.cs
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
using Nancy;
using Raven.Client;
using System.Diagnostics;
using GestUAB.DataAccess;


namespace GestUAB.Modules
{
    public abstract class BaseModule : NancyModule
    {
        protected BaseModule() {
            if (!Nancy.StaticConfiguration.DisableErrorTraces) {
                Before += ctx => {                    
                    
                    this.Context.Trace.TraceLog.WriteLog(
                        s => s.AppendLine(string.Format ("Executing request {0}", ctx.Request.Url)));
                    return null;
                };
                After += ctx => {
                    Debug.WriteLine ("Executed request {0}", ctx.Request.Url);
                   
                };
            
            }
        }

        protected BaseModule(string modulePath) 
            : base(modulePath) { }


    }
}