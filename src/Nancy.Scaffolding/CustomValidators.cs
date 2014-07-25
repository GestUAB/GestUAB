//
// CustomValidators.cs
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
namespace Nancy.Scaffolding.Validators
{
    using FluentValidation.Validators;
    using FluentValidation;
    using System.Text.RegularExpressions;

    public class RegularExpressionWithMaskValidator : RegularExpressionValidator
    {

        public RegularExpressionWithMaskValidator(string expression, string mask) 
            : base(expression)
        {
            this.Mask = mask;
        }

        public RegularExpressionWithMaskValidator(string expression, 
                                                   RegexOptions options, 
                                                   string mask) 
            : base(expression, options)
        {
            this.Mask = mask;
        }

        public RegularExpressionWithMaskValidator(Regex regex, string mask) 
            : base(regex)
        {
            this.Mask = mask;
        }

        public string Mask
        {
            get;
            set;
        }
    }
    //Create a validator, that does nothing on the server side.  
    public class RemoteValidator : PropertyValidator
    {  
        public string Action { get; private set; }

        public string HttpMethod { get; private set; }

        public string AdditionalFields { get; private set; }

        public RemoteValidator(string errorMessage,   
                                string action,   
                                string httpMethod = "GET",   
                                string additionalFields = "")  
            : base(errorMessage)
        {  
            Action = action;  
            HttpMethod = httpMethod;  
            AdditionalFields = additionalFields;  
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {  
            //This is not a server side validation rule. So, should not effect at the server side.  
            return true;  
        }
    }

    public static class CustomValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> Remote<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, 
                                                                              string errorMessage,   
                                                                              string action,   
                                                                              string httpMethod = "GET",   
                                                                              string additionalFields = "")
        {
            return ruleBuilder.SetValidator(new RemoteValidator(errorMessage,   
                                                                  action,   
                                                                  httpMethod,   
                                                                  additionalFields));
        }

        public static IRuleBuilderOptions<T, TProperty> MatchesWithMask<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, 
                                                                                      string expression, 
                                                                                      string mask)
        {
            return ruleBuilder.SetValidator(new RegularExpressionWithMaskValidator(expression, mask));
        }

        public static IRuleBuilderOptions<T, TProperty> MatchesWithMask<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, 
                                                                                      string expression, 
                                                                                      RegexOptions options,
                                                                                      string mask)
        {
            return ruleBuilder.SetValidator(new RegularExpressionWithMaskValidator(expression, options, mask));
        }

        public static IRuleBuilderOptions<T, TProperty> MatchesWithMask<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, 
                                                                                      Regex regex, 
                                                                                      string mask)
        {
            return ruleBuilder.SetValidator(new RegularExpressionWithMaskValidator(regex, mask));
        }
    }
}

