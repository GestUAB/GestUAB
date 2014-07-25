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
    public enum MembershipCreateStatus
    {
        Success, // The user was successfully created.
        InvalidUserName, // The user name was not found in the database.
        InvalidPassword, // The password is not formatted correctly.
        InvalidQuestion, // The password question is not formatted correctly.
        InvalidAnswer, // The password answer is not formatted correctly.
        InvalidEmail, // The e-mail address is not formatted correctly.
        DuplicateUserName, // The user name already exists in the database for the application.
        DuplicateEmail, // The e-mail address already exists in the database for the application.
        UserRejected, // The user was not created, for a reason defined by the provider.
        InvalidProviderUserKey, // The provider user key is of an invalid type or format.
        DuplicateProviderUserKey, // The provider user key already exists in the database for the application.
        ProviderError // The provider returned an error that is not described by other MembershipCreateStatus enumeration values.
    }
}

