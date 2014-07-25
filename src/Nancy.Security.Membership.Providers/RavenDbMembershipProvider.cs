//
// MembershipProvider.cs
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


namespace GestUAB.Security
{
  using System;
    using System.Security.Cryptography;
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Client;
    using Nancy.Security;
    using System.Text.RegularExpressions;
    using System.Text;
    using Nancy;

    /// <summary>
    /// Specialized MembershipProvider that uses a file (Users.config) to store its data.
    /// Passwords for the users are always stored as a salted hash (see: http://msdn.microsoft.com/msdnmag/issues/03/08/SecurityBriefs/)
    /// </summary>
    public class RavenDbMembershipProvider : MembershipProvider
    {

        private class UserSecurityInfo
        {
            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            public string Password;

            /// <summary>
            /// Gets the password question for the membership user.
            /// </summary>
            /// <value>
            /// The password question for the membership user.
            /// </value>
            public string PasswordQuestion;

            /// <summary>
            /// Gets the password question for the membership user.
            /// </summary>
            /// <value>
            /// The password question for the membership user.
            /// </value>
            public string PasswordAnswer;

            /// <summary>
            /// Gets or sets the password salt.
            /// </summary>
            /// <value>
            /// The password salt.
            /// </value>
            public string PasswordSalt;

            /// <summary>
            /// Gets or sets the password verification token.
            /// </summary>
            /// <value>
            /// The password verification token.
            /// </value>
            public string PasswordVerificationToken;
            public string UserName;
        }

        public RavenDbMembershipProvider (IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }

        public IDocumentStore DocumentStore { get; private set; }

        #region IUserMapper implementation

        public override IUserIdentity GetUserFromIdentifier (Guid identifier, NancyContext context)
        {
            using (var session = DocumentStore.OpenSession()) {
                return session.Query<MembershipUser> ().Where (x => x.UserId == identifier).FirstOrDefault ();
            }   
        }
        #endregion

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public override bool EnablePasswordReset { get; protected set; }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public override bool EnablePasswordRetrieval { get; protected set; }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <value></value>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        public override int MaxInvalidPasswordAttempts { get; protected set; }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <value></value>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public override int MinRequiredNonAlphanumericCharacters { get; protected set; }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <value></value>
        /// <returns>The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength { get; protected set; }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <value></value>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        public override int PasswordAttemptWindow { get; protected set; }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"></see> values indicating the format for storing passwords in the data store.</returns>
        public override MembershipPasswordFormat PasswordFormat { get; protected set; }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <value></value>
        /// <returns>A regular expression used to evaluate a password.</returns>
        public override string PasswordStrengthRegularExpression { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <value></value>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public override bool RequiresQuestionAndAnswer { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        public override bool RequiresUniqueEmail { get; protected set; }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        public override bool ChangePassword (string username, string oldPassword, string newPassword)
        {
            using (var session = DocumentStore.OpenSession()) {
                var secInfo = session.Query<UserSecurityInfo> ().Where (x => x.UserName == username).FirstOrDefault ();
                if (secInfo == null)
                    throw new MembershipException ("User does not exist.");

                if (ValidateUserInternal (secInfo, oldPassword)) {
                    var args = new ValidatePasswordEventArgs (username, newPassword, false);
                    OnValidatingPassword (args);
                    if (args.Cancel) {
                        if (args.FailureInformation != null)
                            throw args.FailureInformation;
                        else
                            throw new MembershipException ("Change password canceled due to new password validation failure.");
                    }
                    if (!ValidatePassword (newPassword))
                        throw new ArgumentException ("Password does not meet password strength requirements.");
                    ///
                    string salt = "";
                    secInfo.Password = TransformPassword (newPassword, ref salt);
                    secInfo.PasswordSalt = salt;
                    var user = session.Query<MembershipUser> ().Where (x => x.UserName == username).FirstOrDefault ();
                    var md = new MembershipUser (user.UserName,
                                                user.ProviderUserKey, user.Email, user.PasswordQuestion,
                                                user.Comment, user.IsApproved, user.IsLockedOut, user.CreationDate,
                                                user.LastLoginDate, user.LastActivityDate,
                                                DateTime.Now, user.LastLockoutDate);
                    session.Store (md, md.UserName);
                    session.SaveChanges ();
                    return true;
                }
                return false;
            }
                
        }

        /// <summary>
        /// Activates a pending membership account.
        /// </summary>
        /// <returns>
        /// true if the user account is confirmed; otherwise, false.
        /// </returns>
        /// <param name='accountConfirmationToken'>
        /// A confirmation token to pass to the authentication provider.
        /// </param>
        public override bool ConfirmAccount (string accountConfirmationToken)
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Indicates whether the user account is confirmed.
        /// </summary>
        /// <returns>
        /// true if the user account is confirmed; otherwise, false.
        /// </returns>
        /// <param name='userName'>
        /// The username.
        /// </param>
        /// <param name='accountConfirmationToken'>
        /// The account confirmation.
        /// </param>
        public override bool ConfirmAccount (string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Creates a new user account using the specified user name and password.
        /// </summary>
        /// <returns>
        /// The account.
        /// </returns>
        public override string CreateAccount (string userName, string password)
        {
            return CreateAccount (userName, password, false);
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <returns>
        /// A token that can be sent to the user to confirm the user account.
        /// </returns>
        /// <param name='userName'>
        /// The user name.
        /// </param>
        /// <param name='password'>
        /// The password.
        /// </param>
        /// <param name='requireConfirmationToken'>
        /// (Optional) true to specify that the user account must be confirmed; otherwise, false. The default is false.
        /// </param>
        public override string CreateAccount (string userName, string password, bool requireConfirmationToken = false)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    bool isUserNameOk = true;
                    bool isEmailOk = true;

                    ValidateUserNameAndEmail (userName, "", ref isUserNameOk, ref isEmailOk, Guid.Empty);

                    // Validate the username
                    if (!isUserNameOk) {
                        throw new MembershipException (string.Format ("The username {0} already exists.", userName));
                    }

                    // Validate the password
                    if (!ValidatePassword (password)) {
                        throw new MembershipException ("The password is not valid.");
                    }

                    // Everything is valid, create the user
                    var user = new MembershipUser (userName,
                                                Guid.NewGuid (), "", "",
                                                "", true, false, DateTime.Today,
                                                DateTime.MinValue, DateTime.MinValue,
                                                DateTime.MinValue, DateTime.MinValue);
                    var secInfo = new UserSecurityInfo ();
                    var salt = "";
                    secInfo.Password = TransformPassword (password, ref salt);
                    secInfo.PasswordSalt = salt;
                    secInfo.PasswordAnswer = "";
                    secInfo.PasswordVerificationToken = requireConfirmationToken == false ? "" : Guid.NewGuid ().ToString ();
                    secInfo.UserName = userName;
                    session.Store (user);
                    session.Store (secInfo);
                    return secInfo.PasswordVerificationToken;
                }
                    
            } catch {
                throw;
            }
            
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        public override bool ChangePasswordQuestionAndAnswer (string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            using (var session = DocumentStore.OpenSession()) {
                var secInfo = session.Query<UserSecurityInfo> ().Where (x => x.UserName == username).FirstOrDefault ();
                if (secInfo != null && ValidateUserInternal (secInfo, password)) {
                    secInfo.PasswordQuestion = newPasswordQuestion;
                    secInfo.PasswordAnswer = newPasswordAnswer;
                    session.SaveChanges ();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"></see> enumeration value indicating whether the user was created successfully.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the information for the newly created user.
        /// </returns>
        public override MembershipUser CreateUser (string username, string password, string email,
                        string passwordQuestion, string passwordAnswer, bool isApproved,
                        Guid providerUserKey, out MembershipCreateStatus status)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {

                    bool isUserNameOk = true;
                    bool isEmailOk = true;
                    
                    ValidateUserNameAndEmail (username, email, ref isUserNameOk, ref isEmailOk, Guid.Empty);

                    // Validate the username 
                    if (!isUserNameOk) {
                        status = MembershipCreateStatus.InvalidUserName;
                        return null;
                    }
                    
                    // Validate the email
                    if (!isEmailOk) {
                        status = MembershipCreateStatus.InvalidEmail;
                        return null;
                    }
                    
                    // Raise the event before validating the password
                    OnValidatingPassword (
                                    new ValidatePasswordEventArgs (
                                            username, password, true)
                    );

                    // Validate the password
                    if (!ValidatePassword (password)) {
                        status = MembershipCreateStatus.InvalidPassword;
                        return null;
                    }

                    // Everything is valid, create the user
                    var user = new MembershipUser (username,
                                                providerUserKey, email, passwordQuestion,
                                                "", isApproved, false, DateTime.Today,
                                                DateTime.MinValue, DateTime.MinValue,
                                                DateTime.MinValue, DateTime.MinValue);
                    var secInfo = new UserSecurityInfo ();
                    var salt = "";
                    secInfo.Password = TransformPassword (password, ref salt);
                    secInfo.PasswordSalt = salt;
                    secInfo.PasswordAnswer = passwordAnswer;
                    secInfo.PasswordVerificationToken = new Guid ().ToString ();
                    secInfo.UserName = username;
                    session.Store (user);
                    session.Store (secInfo);

                    status = MembershipCreateStatus.Success;
                    return CreateMembershipFromInternalUser (user);
                }
                    
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        public override  bool DeleteUser (string username, bool deleteAllRelatedData)
        {

            try {
                using (var session = DocumentStore.OpenSession()) {
                    var user = session.Query<MembershipUser> ().Where (x => x.UserName == username).FirstOrDefault ();
                    var secInfo = session.Query<UserSecurityInfo> ().Where (x => x.UserName == username).FirstOrDefault ();
                    if (user != null) {
                        if (deleteAllRelatedData) {
                            session.Delete (user);
                            session.Delete (secInfo);
                            // FIX: http://www.codeplex.com/aspnetxmlproviders/WorkItem/View.aspx?WorkItemId=7186
//                        string[] roles = Roles.GetRolesForUser(username);
//                        if (roles.Length != 0)
//                            Roles.RemoveUserFromRoles(username, roles);
//                        ProfileManager.DeleteProfile(username);
                        } else {
                            user.IsApproved = false;
                        }
                        session.SaveChanges ();
                        return true;
                    }
                    return false;
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of 
        /// pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by 
        /// pageIndex.
        /// </returns>
        public override IEnumerable<MembershipUser> FindUsersByEmail (string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var users = session.Query<MembershipUser> ().Where (x => x.Email == emailToMatch);
                    totalRecords = users.Count ();
                    users = users.Skip (pageIndex * pageSize).Take (pageSize);
                    return users;
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of 
        /// pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by 
        /// pageIndex.
        /// </returns>
        public override IEnumerable<MembershipUser> FindUsersByName (string usernameToMatch, int pageIndex, 
                                                                    int pageSize, out int totalRecords)
        {

            try {
                using (var session = DocumentStore.OpenSession()) {
                    var users = session.Query<MembershipUser> ().Where (x => x.Email == usernameToMatch);
                    totalRecords = users.Count ();
                    users = users.Skip (pageIndex * pageSize).Take (pageSize);
                    return users;
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of 
        /// pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by 
        /// pageIndex.
        /// </returns>
        public override  IEnumerable<MembershipUser> GetAllUsers (int pageIndex, int pageSize, out int totalRecords)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var users = session.Query<MembershipUser> ();
                    totalRecords = users.Count ();
                    return users.Skip (pageIndex * pageSize).Take (pageSize);
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        public override int GetNumberOfUsersOnline ()
        {
            using (var session = DocumentStore.OpenSession()) {
                return session.Query<MembershipUser> ().Where (x => x.IsOnline == true).Count ();
            }
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        public override string GetPassword (string username, string answer)
        {

            try {
                using (var session = DocumentStore.OpenSession()) {
                    if (EnablePasswordRetrieval) {
                        var user = session.Query<UserSecurityInfo> ()
                            .Where (x => x.UserName == username).FirstOrDefault ();
                        if (user != null) {
                            if (answer.Equals (user.PasswordAnswer, StringComparison.OrdinalIgnoreCase)) {
                                return user.Password;
                            } else {
                                throw new MembershipException ();
                            }
                        }
                        return null;
                    } else {
                        throw new MembershipException ("Password retrieval is not enabled.");
                    }
                }
                
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time 
        /// stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return 
        /// user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's 
        /// information from the data source.
        /// </returns>
        public override MembershipUser GetUser (string username, bool userIsOnline)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var user = session.Query<MembershipUser> ().Where (x => x.UserName == username).FirstOrDefault ();
                    if (user != null) {
                        if (userIsOnline) {
                            user.LastActivityDate = DateTime.Now;
                            session.SaveChanges (); 
                        }
                        return CreateMembershipFromInternalUser (user);
                    } else {
                        return null;
                    }
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets information from the data source for a user based on the unique identifier for the membership user. 
        /// Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return 
        /// user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's 
        /// information from the data source.
        /// </returns>
        public override MembershipUser GetUser (Guid userId, bool userIsOnline)
        {

            try {
                using (var session = DocumentStore.OpenSession()) {
                    var user = session.Query<MembershipUser> ().Where (x => x.UserId == userId).FirstOrDefault ();
                    if (user != null) {
                        if (userIsOnline) {
                            user.LastActivityDate = DateTime.Now;
                            session.SaveChanges ();
                        }
                        return user;
                    } 
                }
                return null;
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        public override string GetUserNameByEmail (string email)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    return session.Query<MembershipUser> ()
                        .Where (x => x.Email == email).FirstOrDefault ().UserName;
                }
            } catch {
                throw;
            }
            
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>The new password for the specified user.</returns>
        public override string ResetPassword (string userName, string answer)
        {
            if (!EnablePasswordReset)
                throw new NotSupportedException ();
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var secInfo = session.Query<UserSecurityInfo> ()
                        .Where (x => x.UserName == userName).FirstOrDefault ();
                    if (secInfo != null) {
                        if (RequiresQuestionAndAnswer && 
                            !secInfo.PasswordAnswer.Equals (answer, StringComparison.OrdinalIgnoreCase)) {
                            throw new MembershipException ("Invalid answer.");
                        } else {
                            string newPasswordString = Membership.GeneratePassword (MinRequiredPasswordLength,
                                                                                   MinRequiredNonAlphanumericCharacters);
                            secInfo.Password = TransformPassword (newPasswordString, ref secInfo.PasswordSalt);
                            var user = session.Query<MembershipUser> ()
                                .Where (x => x.UserName == userName).FirstOrDefault ();
                            user.LastPasswordChangedDate = DateTime.Now;
                            session.SaveChanges ();
                            return newPasswordString;
                        }
                    }
                    return null;
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user to clear the lock status for.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        public override bool UnlockUser (string userName)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var user = session.Query<MembershipUser> ()
                                .Where (x => x.UserName == userName).FirstOrDefault ();
                    user.IsLockedOut = false;
                    session.SaveChanges ();
                }    
                return true;
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"></see> object that represents the 
        /// user to update and the updated information for the user.</param>
        public override void UpdateUser (MembershipUser user)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var suser = session.Query<MembershipUser> ()
                                .Where (x => x.UserName == user.UserName).FirstOrDefault ();
                    if (suser == null) {
                        throw new MembershipException ("User does not exist!");
                    } 

                    bool isUserNameOk = true;
                    bool isEmailOk = true;
                    
                    ValidateUserNameAndEmail (suser.UserName, suser.Email, ref isUserNameOk, ref isEmailOk, Guid.Empty);

                    if (!isUserNameOk)
                        throw new MembershipException ("Username are not unique.");
                    
                    if (!isEmailOk)
                        throw new MembershipException ("Email are not unique.");
                    
                    suser.Email = user.Email;
                    suser.LastActivityDate = user.LastActivityDate;
                    suser.LastLoginDate = user.LastLoginDate;
                    suser.Comment = user.Comment;
                    suser.IsApproved = user.IsApproved;
                    session.SaveChanges ();
                }
            } catch {
                throw;
            }
            
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override Guid? ValidateUser (string username, string password)
        {
            try {
                using (var session = DocumentStore.OpenSession()) {
                    var user = session.Query<MembershipUser> ()
                                .Where (x => x.UserName == username).FirstOrDefault ();
                    var secInfo = session.Query<UserSecurityInfo> ()
                                .Where (x => x.UserName == username).FirstOrDefault ();
                    if (user == null || secInfo == default(UserSecurityInfo))
                        return null;

                    if (ValidateUserInternal (secInfo, password)) {
                        DateTime now = DateTime.Now;
                        user.LastLoginDate = now;
                        user.LastActivityDate = now;
                        session.SaveChanges ();
                        return user.UserId;
                    } 
                    return null;
                }
            } catch {
                throw;
            }
        }


        /// <summary>
        /// Transforms the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        string TransformPassword (string password, ref string salt)
        {

            string ret = "";
            switch (PasswordFormat) {
            case MembershipPasswordFormat.Clear:
                ret = password;
                break;
            case MembershipPasswordFormat.Hashed:
                    // Generate the salt if not passed in
                if (string.IsNullOrEmpty (salt)) {
                    var saltBytes = new byte[16];
                    RandomNumberGenerator rng = RandomNumberGenerator.Create ();
                    rng.GetBytes (saltBytes);
                    salt = Convert.ToBase64String (saltBytes);
                }
                ret = HashPassword ((salt + password), "SHA1");
                break;
            case MembershipPasswordFormat.Encrypted:
                byte[] clearText = Encoding.UTF8.GetBytes (password);
                byte[] encryptedText = base.EncryptPassword (clearText);
                ret = Convert.ToBase64String (encryptedText);
                break;
            }
            return ret;
        }

        static string HashPassword (string password, string passwordFormat)
        {
            if (password == null)
                throw new ArgumentNullException ("password");

            byte [] bytes;
            switch (passwordFormat) {
            case "MD5":
                bytes = MD5.Create ().ComputeHash (Encoding.UTF8.GetBytes (password));
                break;

            case "SHA1":
                bytes = SHA1.Create ().ComputeHash (Encoding.UTF8.GetBytes (password));
                break;

            default:
                throw new ArgumentException ("The format must be either MD5 or SHA1", "passwordFormat");
            }
            return Convert.ToBase64String (bytes);
        }

        /// <summary>
        /// Validates the username.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="excludeKey">The exclude key.</param>
        /// <returns></returns>
        void ValidateUserNameAndEmail (string userName, string email, ref bool userNameIsOk, ref bool emailIsOk, Guid excludeKey)
        {
            userNameIsOk = true;
            emailIsOk = true;
            using (var session = DocumentStore.OpenSession()) {
                userNameIsOk = session.Query<MembershipUser> ()
                    .Where (x => x.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase) &&
                    x.UserId != excludeKey
                ) == null;
                if (RequiresUniqueEmail) {
                    emailIsOk = session.Query<MembershipUser> ()
                    .Where (x => x.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase) && 
                        x.Email.Equals (email, StringComparison.OrdinalIgnoreCase) &&
                        x.UserId != excludeKey
                    ) == null;
                }
            }
        }

        /// <summary>
        /// Validates the username.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="excludeKey">The exclude key.</param>
        /// <returns></returns>
        bool ValidateUserName1 (string userName, string email, Guid excludeKey)
        {
            using (var session = DocumentStore.OpenSession()) {
                if (email == "") {
                    return session.Query<MembershipUser> ()
                        .Where (x => x.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase) &&
                        x.UserId != excludeKey
                    ) == null;
                }
                return session.Query<MembershipUser> ()
                    .Where (x => x.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase) && 
                    x.Email.Equals (email, StringComparison.OrdinalIgnoreCase) &&
                    x.UserId != excludeKey
                ) == null;
            }
        }
        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        bool ValidatePassword (string password)
        {

            bool isValid = true;
            // Validate simple properties
            isValid = isValid && (password.Length >= MinRequiredPasswordLength);
            // Validate non-alphanumeric characters
            var regex = new Regex (@"\W");
            isValid = isValid && (regex.Matches (password).Count >= MinRequiredNonAlphanumericCharacters);
            // Validate regular expression
            regex = new Regex (PasswordStrengthRegularExpression);
            isValid &= (regex.Matches (password).Count > 0);
            return isValid;
        }

        /// <summary>
        /// Validates the user internal.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        bool ValidateUserInternal (UserSecurityInfo user, string password)
        {

            if (user != null) {
                string pass = TransformPassword (password, ref user.PasswordSalt);
                if (string.Equals (pass, user.Password, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates the membership from internal user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        MembershipUser CreateMembershipFromInternalUser (MembershipUser user)
        {

            return new MembershipUser (user.UserName, user.ProviderUserKey, 
                                      user.Email, user.PasswordQuestion,
                                      user.Comment, user.IsApproved, 
                                      user.IsLockedOut, user.CreationDate, 
                                      user.LastLoginDate,
                                      user.LastActivityDate, 
                                      user.LastPasswordChangedDate, 
                                      user.LastLockoutDate);
        }
    }
}

