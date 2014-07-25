//
// ScaffoldVisibilityConfig.cs
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
namespace Nancy.Scaffolding
{
    /// <summary>
    /// Scaffold visibility config.
    /// </summary>
    public class VisibilityConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nancy.Scaffolding.ScaffoldVisibilityConfig"/> class.
        /// </summary>
        public VisibilityConfig()
        {
            this.Create = Visibility.Show;
            this.Delete = Visibility.Show;
            this.Read = Visibility.Show;
            this.Update = Visibility.Show;
        }

        /// <summary>
        /// Gets or sets the create.
        /// </summary>
        /// <value>The create.</value>
        public Visibility Create { get; set; }

        /// <summary>
        /// Gets or sets the read.
        /// </summary>
        /// <value>The read.</value>
        public Visibility Read { get; set; }

        /// <summary>
        /// Gets or sets the update.
        /// </summary>
        /// <value>The update.</value>
        public Visibility Update { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>The delete.</value>
        public Visibility Delete { get; set; }

        /// <summary>
        /// Gets from form action.
        /// </summary>
        /// <returns>The from form action.</returns>
        /// <param name="formAction">Form action.</param>
        public Visibility GetFromFormAction(FormAction formAction)
        {
            switch (formAction)
            {
                case FormAction.Create:
                    return Create;
                case FormAction.Delete:
                    return Delete;
                case FormAction.Read:
                    return Read;
                case FormAction.Update:
                    return Update;
            }
            return Visibility.None;
        }
    }
}