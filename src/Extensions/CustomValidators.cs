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
using System;
using FluentValidation.Validators;
using FluentValidation;

namespace GestUAB.Validators
{
//    public class LettersOnly<T> : PropertyValidator
//    {
//
//        public LettersOnly () 
//        : base("O campo {PropertyName} deve conter apenas letras.")
//        {
//        
//        }
//
//        protected override bool IsValid (PropertyValidatorContext context)
//        {
//            var str = context.PropertyValue as string;
//            var regex = new RegularExpressionValidator("^[a-zA-Z]*$");
//            return regex.Validate(context);
//        }
//    }

    //Create a validator, that does nothing on the server side.  
    public class RemoteValidator : PropertyValidator
    {  
        public string Action { get; private set; }

        public string HttpMethod { get; private set; }

        public string AdditionalFields { get; private set; }
      
        public RemoteValidator (string errorMessage,   
                                string action,   
                                string httpMethod = "GET",   
                                string additionalFields = "")  
            : base(errorMessage)
        {  
            Action = action;  
            HttpMethod = httpMethod;  
            AdditionalFields = additionalFields;  
        }
      
        protected override bool IsValid (PropertyValidatorContext context)
        {  
            //This is not a server side validation rule. So, should not effect at the server side.  
            return true;  
        }  
    }

    public static class CustomValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> Remote<T, TProperty> (this IRuleBuilder<T, TProperty> ruleBuilder, 
                                                                              string errorMessage,   
                                                                              string action,   
                                                                              string httpMethod = "GET",   
                                                                              string additionalFields = "")
        {
            return ruleBuilder.SetValidator (new RemoteValidator (errorMessage,   
                                                                  action,   
                                                                  httpMethod,   
                                                                  additionalFields));
        }
    }
//      
//    //Create a MVC wrapper for the validator.  
//    public class RemoteFluentValidationPropertyValidator :   
//                     FluentValidationPropertyValidator
//    {  
//        private RemoteValidator RemoteValidator {  
//            get { return (RemoteValidator)Validator; }  
//        }
//      
//        public RemoteFluentValidationPropertyValidator (ModelMetadata metadata,   
//                                                     ControllerContext controllerContext,   
//                                                     PropertyRule propertyDescription,   
//                                                     IPropertyValidator validator)  
//            : base(metadata, controllerContext, propertyDescription, validator)
//        {  
//            ShouldValidate = false;  
//        }
//      
//        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules ()
//        {  
//            if (!ShouldGenerateClientSideRules ())
//                yield break;  
//      
//            var formatter = new MessageFormatter ().AppendPropertyName (Rule.PropertyDescription);  
//            string message = formatter.BuildMessage (RemoteValidator.ErrorMessageSource.GetString ());  
//      
//            //This is the rule that asp.net mvc 3, uses for Remote attribute.   
//            yield return new ModelClientValidationRemoteRule (message, RemoteValidator.Url, RemoteValidator.HttpMethod, RemoteValidator.AdditionalFields);  
//        }  
//    }  
}

