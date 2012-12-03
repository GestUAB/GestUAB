.using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Validators;
using GestUAB.Models;

namespace GestUAB
{
    //TODO: Maps this class to an requeriment.
    /// <summary>
    /// Function.
    /// This class maps to what requeriment?
    /// </summary>
	public class Function : IModel 
	{
		public Function ()
		{
            Id = Guid.NewGuid();
            Name = string.Empty;
		}
        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código do memorando.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public Guid Id { get ; set ; }
        #endregion
        
        [Display(Name = "Referente",
                 Description= "Texto \"Referente\" a.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public String Name { get ; set ; }
	}

    public class FunctionValidator : ValidatorBase<Function>
    {
        public FunctionValidator ()
        {
            using (var session = DocumentSession) {
                RuleFor (function => function.Name)
                    .NotEmpty ().WithMessage ("O nome da funçao é obrigatório.");
            }
            RuleSet ("Update", () => {
                RuleFor (function => function.Name)
                    .NotEmpty ().WithMessage ("O nome do funçao é obrigatório.");                       
            }
            );
        }
    }
}

