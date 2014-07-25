using System;
using System.Text.RegularExpressions;
using FluentValidation.Validators;
using FluentValidation;
using System.Globalization;

namespace GestUAB.Models
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
//    public class RemoteValidator : PropertyValidator
//    {  
//        public string Action { get; private set; }
//
//        public string HttpMethod { get; private set; }
//
//        public string AdditionalFields { get; private set; }
//
//        public RemoteValidator (string errorMessage,   
//                                string action,   
//                                string httpMethod = "GET",   
//                                string additionalFields = "")  
//                : base(errorMessage)
//        {  
//            Action = action;  
//            HttpMethod = httpMethod;  
//            AdditionalFields = additionalFields;  
//        }
//
//        protected override bool IsValid (PropertyValidatorContext context)
//        {  
//            //This is not a server side validation rule. So, should not effect at the server side.  
//            return true;  
//        }
//    }
//
    public class CpfValidator : PropertyValidator
    {
        public CpfValidator () : base("CPF inválido.")
        {
            
        }

        protected override bool IsValid (PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return true;
            var cpfcnpj = (string)context.PropertyValue;

            if (string.IsNullOrEmpty (cpfcnpj))
                return true;
            else {
                int[] d = new int[14];
                int[] v = new int[2];
                int j, i, soma;
                string sequencia, sonumero;

                sonumero = Regex.Replace (cpfcnpj, "[^0-9]", string.Empty);

                //verificando se todos os numeros são iguais
                if (new string (sonumero [0], sonumero.Length) == sonumero)
                    return false;

                // se a quantidade de dígitos numérios for igual a 11
                // iremos verificar como CPF
                if (sonumero.Length == 11) {
                    for (i = 0; i <= 10; i++)
                        d [i] = Convert.ToInt32 (sonumero.Substring (i, 1));
                    for (i = 0; i <= 1; i++) {
                        soma = 0;
                        for (j = 0; j <= 8 + i; j++)
                            soma += d [j] * (10 + i - j);

                        v [i] = (soma * 10) % 11;
                        if (v [i] == 10)
                            v [i] = 0;
                    }
                    return (v [0] == d [9] & v [1] == d [10]);
                }
                // se a quantidade de dígitos numérios for igual a 14
                // iremos verificar como CNPJ
                else if (sonumero.Length == 14) {
                    sequencia = "6543298765432";
                    for (i = 0; i <= 13; i++)
                        d [i] = Convert.ToInt32 (sonumero.Substring (i, 1));
                    for (i = 0; i <= 1; i++) {
                        soma = 0;
                        for (j = 0; j <= 11 + i; j++)
                            soma += d [j] * Convert.ToInt32 (sequencia.Substring (j + 1 - i, 1));

                        v [i] = (soma * 10) % 11;
                        if (v [i] == 10)
                            v [i] = 0;
                    }
                    return (v [0] == d [12] & v [1] == d [13]);
                }
                // CPF ou CNPJ inválido se
                // a quantidade de dígitos numérios for diferente de 11 e 14
                else
                    return false;
            }
        }
    }

    public class ValidDate : PropertyValidator
    {

        public ValidDate (string format) 
            : base("A data precisa estar no formato dd/mm/aaaa.")
        {
            Format = format;
        }

        public string Format {
            get;
            private set;
        }

        protected override bool IsValid (PropertyValidatorContext context)
        {
            var str = context.PropertyValue.ToString();
            DateTime date;
            return DateTime.TryParseExact (str, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }

    public class ValidNomeConjuge : PropertyValidator
    {

        public ValidNomeConjuge () 
            : base("{ValidationMessage}")
        {
        }

        protected override bool IsValid (PropertyValidatorContext context)
        {
            var colaborador = context.Instance as Colaborador;
            var value = context.PropertyValue as string;
            if(colaborador.EstadoCivil == EstadoCivilType.Casado && value == "") {
                context.MessageFormatter.AppendArgument("ValidationMessage",
                                                        @"O nome do cônjuge é obrigatório se o estado civil for ""casado"".");
                return false;
            }
            else if(colaborador.EstadoCivil != EstadoCivilType.Casado && value != "") {
                context.MessageFormatter.AppendArgument("ValidationMessage",
                                                        @"O nome do cônjuge deve ser informado somente se o estado civil for ""casado"".");
                return false;
            }
            return true;
        }
    }


    public static class CustomValidatorExtensions
    {
//        public static IRuleBuilderOptions<T, TProperty> Remote<T, TProperty> (this IRuleBuilder<T, TProperty> ruleBuilder, 
//                                                                              string errorMessage,   
//                                                                              string action,   
//                                                                              string httpMethod = "GET",   
//                                                                              string additionalFields = "")
//        {
//            return ruleBuilder.SetValidator (new RemoteValidator (errorMessage,   
//                                                                  action,   
//                                                                  httpMethod,   
//                                                                  additionalFields));
//        }

        public static IRuleBuilderOptions<T, TProperty> ValidCpf<T, TProperty> (this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator (new CpfValidator ());
        }

        public static IRuleBuilderOptions<T, TProperty> ValidDate<T, TProperty> (this IRuleBuilder<T, TProperty> ruleBuilder, string format)
        {
            return ruleBuilder.SetValidator (new ValidDate (format));
        }

        public static IRuleBuilderOptions<T, TProperty> ValidNomeConjuge<T, TProperty> (this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator (new ValidNomeConjuge ());
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

