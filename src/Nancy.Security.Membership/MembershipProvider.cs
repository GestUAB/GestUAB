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


namespace Nancy.Security
{
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    [Serializable]
    public class MembershipException : Exception 
    {
        public MembershipException ()
        {
        }

        public MembershipException (string message) : base(message)
        {
            
        }
    }

    public sealed class ValidatePasswordEventArgs : EventArgs
    {

        ValidatePasswordEventArgs()
        {
        }

        public ValidatePasswordEventArgs(string userName, string password, bool isNewUser)
        {
            UserName = userName;
            Password = password;
            IsNewUser = isNewUser;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the current create-user, change-password, or reset-password action will be canceled.
        /// </summary>
        public bool Cancel { get; set; }

        public Exception FailureInformation { get; set; }

        public bool IsNewUser { get; private set; }

        public string Password { get; private set; }

        public string UserName { get; private set; }
    }

    public delegate void MembershipValidatePasswordEventHandler(object sender,ValidatePasswordEventArgs e);

    public enum MembershipPasswordFormat
    {
        Clear,
        Hashed,
        Encrypted
    }

    //http://msdn.microsoft.com/en-us/library/webmatrix.webdata.simplemembershipprovider%28v=vs.111%29.aspx

    /// <summary>
    /// Specialized MembershipProvider that uses a file (Users.config) to store its data.
    /// Passwords for the users are always stored as a salted hash (see: http://msdn.microsoft.com/msdnmag/issues/03/08/SecurityBriefs/)
    /// </summary>
    public abstract class MembershipProvider : IUserMapper
    {

        public event MembershipValidatePasswordEventHandler ValidatingPassword;

        #region IUserMapper implementation

        public abstract IUserIdentity GetUserFromIdentifier (Guid identifier, NancyContext context);

        #endregion

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public abstract bool EnablePasswordReset { get; protected set; }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public abstract bool EnablePasswordRetrieval { get; protected set; }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <value></value>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        public abstract int MaxInvalidPasswordAttempts { get; protected set; }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <value></value>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public abstract int MinRequiredNonAlphanumericCharacters { get; protected set; }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <value></value>
        /// <returns>The minimum length required for a password. </returns>
        public abstract int MinRequiredPasswordLength { get; protected set; }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <value></value>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        public abstract int PasswordAttemptWindow { get; protected set; }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"></see> values indicating the format for storing passwords in the data store.</returns>
        public abstract MembershipPasswordFormat PasswordFormat { get; protected set; }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <value></value>
        /// <returns>A regular expression used to evaluate a password.</returns>
        public abstract string PasswordStrengthRegularExpression { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <value></value>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public abstract bool RequiresQuestionAndAnswer { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <value></value>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        public abstract bool RequiresUniqueEmail { get; protected set; }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        public abstract bool ChangePassword(string username, string oldPassword, string newPassword);

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
        public abstract bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer);

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
        public abstract MembershipUser CreateUser(string username, string password, string email,
                        string passwordQuestion, string passwordAnswer, bool isApproved,
                        Guid providerUserKey, out MembershipCreateStatus status);



        /// <summary>
        /// Activates a pending membership account.
        /// </summary>
        /// <returns>
        /// true if the user account is confirmed; otherwise, false.
        /// </returns>
        /// <param name='accountConfirmationToken'>
        /// A confirmation token to pass to the authentication provider.
        /// </param>
        public abstract bool ConfirmAccount(string accountConfirmationToken);

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
        public abstract bool ConfirmAccount(string userName, string accountConfirmationToken);

        /// <summary>
        /// Creates a new user account using the specified user name and password.
        /// </summary>
        /// <returns>
        /// The account.
        /// </returns>
        public abstract string CreateAccount(string userName, string password);

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
        public abstract string CreateAccount(string userName, string password, bool requireConfirmationToken = false);

        /// <summary>
        /// Decrypts an encrypted password.
        /// </summary>
        /// <returns>
        /// A byte array that contains the decrypted password.
        /// </returns>
        /// <param name='encodedPassword'>
        /// A byte array that contains the encrypted password to decrypt.
        /// </param>
        protected virtual byte[] DecryptPassword(byte[] encodedPassword) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Encrypts a password.
        /// </summary>
        /// <returns>
        /// A byte array that contains the encrypted password.
        /// </returns>
        /// <param name='password'>
        /// A byte array that contains the password to encrypt.
        /// </param>
        protected virtual byte[] EncryptPassword(byte[] password) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        public abstract bool DeleteUser(string username, bool deleteAllRelatedData);

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
        /// </returns>
        public abstract IEnumerable<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
            

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
        /// </returns>
        public abstract IEnumerable<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
        /// </returns>
        public abstract IEnumerable<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        public abstract int GetNumberOfUsersOnline();

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        public abstract string GetPassword(string username, string answer);

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's information from the data source.
        /// </returns>
        public abstract MembershipUser GetUser(string username, bool userIsOnline);

        /// <summary>
        /// Gets information from the data source for a user based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's information from the data source.
        /// </returns>
        public abstract MembershipUser GetUser(Guid userId, bool userIsOnline);

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        public abstract string GetUserNameByEmail(string email);

        /// <summary>
        /// Raises the ValidatingPassword event if an event handler has been defined.
        /// </summary>
        /// <param name='args'>
        /// The ValidatePasswordEventArgs to pass to the ValidatingPassword event handler.
        /// </param>
        protected virtual void OnValidatingPassword(ValidatePasswordEventArgs args)
        {
            if (ValidatingPassword != null)
            {
                ValidatingPassword(this, args);
            }
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>The new password for the specified user.</returns>
        public abstract string ResetPassword(string username, string answer);

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user to clear the lock status for.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        public abstract bool UnlockUser(string userName);

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"></see> object that represents the user to update and the updated information for the user.</param>
        public abstract void UpdateUser(MembershipUser user);

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public abstract Guid? ValidateUser(string username, string password);

    }

    sealed class SaltedHash
    {
        readonly string _salt;
        readonly string _hash;
        const int SaltLength = 6;

        public string Salt { get { return _salt; } }

        public string Hash { get { return _hash; } }

        public static SaltedHash Create(string password)
        {
            string salt = CreateSalt();
            string hash = CalculateHash(salt, password);
            return new SaltedHash(salt, hash);
        }

        public static SaltedHash Create(string salt, string hash)
        {
            return new SaltedHash(salt, hash);
        }

        public bool Verify(string password)
        {
            string h = CalculateHash(_salt, password);
            return _hash.Equals(h);
        }

        SaltedHash(string s, string h)
        {
            _salt = s;
            _hash = h;
        }

        static string CreateSalt()
        {
            byte[] r = CreateRandomBytes(SaltLength);
            return Convert.ToBase64String(r);
        }

        static byte[] CreateRandomBytes(int len)
        {
            var r = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(r);
            return r;
        }

        static string CalculateHash(string salt, string password)
        {
            byte[] data = ToByteArray(salt + password);
            byte[] hash = CalculateHash(data);
            return Convert.ToBase64String(hash);
        }

        static byte[] CalculateHash(byte[] data)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(data);
        }

        static byte[] ToByteArray(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }
    }
}

