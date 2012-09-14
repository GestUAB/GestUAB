// 
// UtilExtensions.cs
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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GestUAB
{
    public static class UtilExtensions
    {
        public static void DeleteIfExists (string path)
        {
            if (File.Exists (path)) {
                File.Delete (path);
            }
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T> (T source)
        {
            if (!typeof(T).IsSerializable) {
                throw new ArgumentException (
                    "The type must be serializable.",
                    "source"
                );
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals (source, null)) {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter ();
            Stream stream = new MemoryStream ();
            using (stream) {
                formatter.Serialize (stream, source);
                stream.Seek (0, SeekOrigin.Begin);
                return (T)formatter.Deserialize (stream);
            }
        }

        /// <summary>
        /// Fill the dest object with the source object.
        /// </summary>
        /// <param name='dest'>
        /// Destination.
        /// </param>
        /// <param name='source'>
        /// Source.
        /// </param>
        /// <typeparam name='T'>
        /// The type parameter of filled object.
        /// </typeparam>
        public static void Fill<T> (this T dest, T source)
        {
            //We get the array of fields for the new type instance.
            var props = dest.GetType ().GetProperties ();

            int i = -1;

            foreach (var p in source.GetType().GetProperties()) {
                ++i; 
                if (!(p.CanWrite || props[i].CanWrite)) continue;

                //We query if the fiels support the ICloneable interface.
                var cloneType = p.PropertyType.GetInterface ("ICloneable", true);

                if (cloneType != null) {
                    //Getting the ICloneable interface from the object.

                    var clone = (ICloneable)p.GetValue (source, null);
                    //We use the clone method to set the new value to the field.
                    props [i].SetValue (dest, clone == null ? default(T) : clone.Clone (), null);
                } else {
                    // If the field doesn't support the ICloneable 
                    // interface then just set it.
                    props [i].SetValue (dest, p.GetValue (source, null), null);
                }


//                //Now we check if the object support the 
//                //IEnumerable interface, so if it does
//                //we need to enumerate all its items and check if 
//                //they support the ICloneable interface.
//                Type IEnumerableType = fi.FieldType.GetInterface
//                            ("IEnumerable", true);
//                if (IEnumerableType != null) {
//                    //Get the IEnumerable interface from the field.
//                    IEnumerable IEnum = (IEnumerable)fi.GetValue (this);
//
//                    //This version support the IList and the 
//                    //IDictionary interfaces to iterate on collections.
//                    Type IListType = fields [i].FieldType.GetInterface
//                                    ("IList", true);
//                    Type IDicType = fields [i].FieldType.GetInterface
//                                    ("IDictionary", true);
//
//                    int j = 0;
//                    if (IListType != null) {
//                        //Getting the IList interface.
//                        IList list = (IList)fields [i].GetValue (newObject);
//
//                        foreach (object obj in IEnum) {
//                            //Checking to see if the current item 
//                            //support the ICloneable interface.
//                            ICloneType = obj.GetType ().
//                            GetInterface ("ICloneable", true);
//                        
//                            if (ICloneType != null) {
//                                //If it does support the ICloneable interface, 
//                                //we use it to set the clone of
//                                //the object in the list.
//                                ICloneable clone = (ICloneable)obj;
//
//                                list [j] = clone.Clone ();
//                            }
//
//                            //NOTE: If the item in the list is not 
//                            //support the ICloneable interface then in the 
//                            //cloned list this item will be the same 
//                            //item as in the original list
//                            //(as long as this type is a reference type).
//
//                            j++;
//                        }
//                    } else if (IDicType != null) {
//                        //Getting the dictionary interface.
//                        IDictionary dic = (IDictionary)fields [i].
//                                        GetValue (newObject);
//                        j = 0;
//                    
//                        foreach (DictionaryEntry de in IEnum) {
//                            //Checking to see if the item 
//                            //support the ICloneable interface.
//                            ICloneType = de.Value.GetType ().
//                            GetInterface ("ICloneable", true);
//
//                            if (ICloneType != null) {
//                                ICloneable clone = (ICloneable)de.Value;
//
//                                dic [de.Key] = clone.Clone ();
//                            }
//                            j++;
//                        }
//                    }
//                }
//                i++;
            }
        }
    }
    
}

