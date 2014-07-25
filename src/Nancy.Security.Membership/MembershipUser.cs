//
// System.Web.Security.MembershipUser
//
// Authors:
//  Ben Maurer (bmaurer@users.sourceforge.net)
//
// (C) 2003 Ben Maurer
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Nancy.Security
{
    using Nancy.Security;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MembershipUser : IUserIdentity
    {
        DateTime _creationDate;
        DateTime _lastLoginDate;
        DateTime _lastActivityDate;
        DateTime _lastPasswordChangedDate;
        DateTime _lastLockoutDate;

        protected MembershipUser()
        {
        }
        
        public MembershipUser(string name, object providerUserKey, string email,
            string passwordQuestion, string comment, bool isApproved, bool isLockedOut,
            DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate,
            DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
        {
            UserName = name;
            ProviderUserKey = providerUserKey;
            Email = email;
            PasswordQuestion = passwordQuestion;
            Comment = comment;
            IsApproved = isApproved;
            IsLockedOut = isLockedOut;
            CreationDate = creationDate.ToUniversalTime();
            LastLoginDate = lastLoginDate.ToUniversalTime();
            LastActivityDate = lastActivityDate.ToUniversalTime();
            LastPasswordChangedDate = lastPasswordChangedDate.ToUniversalTime();
            LastLockoutDate = lastLockoutDate.ToUniversalTime();
        }
        
        void UpdateSelf(MembershipUser fromUser)
        {
            try
            {
                Comment = fromUser.Comment;
            } catch (NotSupportedException)
            {
            }
            try
            {
                CreationDate = fromUser.CreationDate;
            } catch (NotSupportedException)
            {
            }
            try
            {
                Email = fromUser.Email;
            } catch (NotSupportedException)
            {
            }
            try
            {
                IsApproved = fromUser.IsApproved;
            } catch (NotSupportedException)
            {
            }
            try
            {
                IsLockedOut = fromUser.IsLockedOut;
            } catch (NotSupportedException)
            {
            }
            try
            {
                LastActivityDate = fromUser.LastActivityDate;
            } catch (NotSupportedException)
            {
            }
            try
            {
                LastLockoutDate = fromUser.LastLockoutDate;
            } catch (NotSupportedException)
            {
            }
            try
            {
                LastLoginDate = fromUser.LastLoginDate;
            } catch (NotSupportedException)
            {
            }
            try
            {
                LastPasswordChangedDate = fromUser.LastPasswordChangedDate;
            } catch (NotSupportedException)
            {
            }
            try
            {
                PasswordQuestion = fromUser.PasswordQuestion;
            } catch (NotSupportedException)
            {
            }
            try
            {
                ProviderUserKey = fromUser.ProviderUserKey;
            } catch (NotSupportedException)
            {
            }
        }

        internal void UpdateUser()
        {
            MembershipUser newUser = Provider.GetUser(UserName, false);
            UpdateSelf(newUser);
        }

        public virtual bool ChangePassword(string oldPassword, string newPassword)
        {
            bool success = Provider.ChangePassword(UserName, oldPassword, newPassword);

            UpdateUser();
            
            return success;
        }
        
        public virtual bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, 
                                                            string newPasswordAnswer)
        {
            bool success = Provider.ChangePasswordQuestionAndAnswer(UserName, password, newPasswordQuestion, 
                                                                    newPasswordAnswer);

            UpdateUser();
            
            return success;
        }
        
        public virtual string GetPassword()
        {
            return GetPassword(null);
        }
        
        public virtual string GetPassword(string answer)
        {
            return Provider.GetPassword(UserName, answer);
        }
        
        public virtual string ResetPassword()
        {
            return ResetPassword(null);
        }
        
        public virtual string ResetPassword(string answer)
        {
            string newPass = Provider.ResetPassword(UserName, answer);

            UpdateUser();
            
            return newPass;
        }
        
        public virtual string Comment { get; set; }
        
        public virtual DateTime CreationDate
        {
            get { return _creationDate.ToLocalTime(); }
            protected set { _creationDate = value.ToLocalTime(); }
        }
        
        public virtual string Email { get; set; }
        
        public virtual bool IsApproved { get; set; }

        public virtual bool IsLockedOut { get; set; }

        public virtual
        bool IsOnline
        {
            get
            {
                int minutes = Membership.UserIsOnlineTimeWindow;
                return LastActivityDate > DateTime.Now - TimeSpan.FromMinutes(minutes);
            }
        }
        
        public virtual DateTime LastActivityDate
        {
            get { return _lastActivityDate.ToLocalTime(); }
            set { _lastActivityDate = value.ToUniversalTime(); }
        }
        
        public virtual DateTime LastLoginDate
        {
            get { return _lastLoginDate.ToLocalTime(); }
            set { _lastLoginDate = value.ToUniversalTime(); }
        }
        
        public virtual DateTime LastPasswordChangedDate
        {
            get { return _lastPasswordChangedDate.ToLocalTime(); }
            set { _lastPasswordChangedDate = value.ToLocalTime(); }
        }
        
        public virtual DateTime LastLockoutDate
        {
            get { return _lastLockoutDate.ToLocalTime(); }
            set { _lastLockoutDate = value.ToLocalTime(); }
        }
        
        public virtual string PasswordQuestion { get; protected set; }

        public virtual string UserName { get; set; }

        public virtual Guid UserId { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public virtual object ProviderUserKey { get; protected set; }

        public override string ToString()
        {
            return UserName;
        }
        
        public virtual bool UnlockUser()
        {
            bool retval = Provider.UnlockUser(UserName);

            UpdateUser();

            return retval;
        }
        
        MembershipProvider Provider
        {
            get
            {
                return Membership.Provider;
            }
        }
    }
}


