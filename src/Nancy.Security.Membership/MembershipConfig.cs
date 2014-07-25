//
// System.Web.Security.Membership
//
// Authors:
// Ben Maurer (bmaurer@users.sourceforge.net)
// Lluis Sanchez Gual (lluis@novell.com)
//
// (C) 2003 Ben Maurer
// (C) 2005 Novell, inc.
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
    public class MembershipConfig
    {

        public MembershipConfig()
        {
            HashAlgorithmType = "SHA1";
        }

        public MembershipProvider Provider { get; set; }

        public int OnlineTimeWindow { get; set; }

        public string HashAlgorithmType { get; set; }

        public bool EnablePasswordReset{ get; set; }

        public bool EnablePasswordRetrieval{ get; set; }

        public int MaxInvalidPasswordAttempts{ get; set; }

        public int MinRequiredNonAlphanumericCharacters{ get; set; }

        public int MinRequiredPasswordLength{ get; set; }

        public int PasswordAttemptWindow{ get; set; }

        public MembershipPasswordFormat PasswordFormat{ get; set; }

        public string PasswordStrengthRegularExpression{ get; set; }

        public bool RequiresQuestionAndAnswer{ get; set; }

        public bool RequiresUniqueEmail{ get; set; }

        public bool RequireConfirmationToken{ get; set; }
    }
}