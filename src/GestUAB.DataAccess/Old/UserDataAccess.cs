//
// UserDataAccess.cs
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
using System.Linq;
using GestUAB.Models;

namespace GestUAB.DataAccess
{
    /// <summary>
    /// User data access.
    /// </summary>
    public class UserDataAccess 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.DataAccess.UserDataAccess"/> class.
        /// </summary>
        public UserDataAccess()
        {
        }

        #region ICrud implementation
        /// <summary>
        /// Create the specified model.
        /// </summary>
        /// <param name='model'>
        /// The user model.
        /// </param>
        public User Create(User model)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                session.Store(model);
                session.SaveChanges();
                return model;
            }
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>
        /// Returns all users.
        /// </returns>
        public IQueryable<User> ReadAll()
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                // do not retrieve the password
                return session.Query<User>("UsersByUserName")
                    .Select(n => new User(){
                           Claims = n.Claims,
                           Email = n.Email,
                           FirstName = n.FirstName,
                           LastName = n.LastName,
                           UserName = n.UserName,
                        }
                );
            }
        }

        /// <summary>
        /// Read the specified id.
        /// </summary>
        /// <param name='id'>
        /// Identifier.
        /// </param>
        public User Read(string id)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                return session.Query<User>("UsersByUserName")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.UserName == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Read the specified id.
        /// </summary>
        /// <param name='id'>
        /// Identifier.
        /// </param>
        public User Read(Guid id)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                // do not store the password in memory
                return session.Query<User>("UsersByUserName")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                        .Where(n => n.Id == id).Select(n => new User(){
                           Claims = n.Claims,
                           Email = n.Email,
                           FirstName = n.FirstName,
                           LastName = n.LastName,
                           UserName = n.UserName,
                        }
                ).FirstOrDefault();
            }
        }

        public Guid? Validate(string username, string password)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                var userRecord = session.Query<User>().Where(
                    u => u.UserName == username && u.Password == password
                ).FirstOrDefault();

                if (userRecord == null)
                {
                    return null;
                }

                return userRecord.Id;
            }
        }

        /// <summary>
        /// Update the specified oldT and newT.
        /// </summary>
        /// <param name='oldT'>
        /// If set to <c>true</c> old t.
        /// </param>
        /// <param name='newT'>
        /// If set to <c>true</c> new t.
        /// </param>
        public bool Update(User oldT, User newT)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                var saved = session.Query<User>("UsersByUserName")
                    .Where(n => n.UserName == oldT.UserName)
                    .FirstOrDefault();
                if (saved == null)
                    return false;
                saved.Fill(newT);
                session.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Update the specified id and newT.
        /// </summary>
        /// <param name='id'>
        /// If set to <c>true</c> identifier.
        /// </param>
        /// <param name='newT'>
        /// If set to <c>true</c> new t.
        /// </param>
        public bool Update(string id, User newT)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                var saved = session.Query<User>("UsersByUserName")
                    .Where(n => n.UserName == id)
                    .FirstOrDefault();
                if (saved == null)
                    return false;
                saved.Fill(newT);
                session.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <param name='id'>
        /// If set to <c>true</c> identifier.
        /// </param>
        public bool Delete(string id)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                var user = session.Query<User>("UsersByUserName")
                        .Where(n => n.UserName == id)
                        .FirstOrDefault();
                if (user == null)
                    return false;
                session.Delete(user);
                session.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Delete the specified model.
        /// </summary>
        /// <param name='model'>
        /// If set to <c>true</c> model.
        /// </param>
        public bool Delete(User model)
        {
            using (var session = Database.DocumentStore.OpenSession())
            {
                var user = session.Query<User>("UsersByUserName")
                        .Where(n => n.UserName == model.UserName)
                        .FirstOrDefault();
                if (user == null)
                    return false;
                session.Delete(user);
                session.SaveChanges();
                return true;
            }
        }
        #endregion
        
    }
}

