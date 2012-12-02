using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace GestUAB.Models
{
    /// <summary>
    /// Scholarship.
    /// </summary>
    public class Scholarship : IModel
    {
        #region class builder
        public Scholarship ()
        {


        }
        
        public static Scholarship DefaultScholarship ()
        {
            return new Scholarship {Id = Guid.NewGuid(),
            CPF = string.Empty,
            Name = string.Empty,
            Function = string.Empty,
            Lots = string.Empty,
            State = string.Empty,
            Value = string.Empty
            };
            
        }
        #endregion

        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código da bolsa.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public System.Guid Id { get ; set ; }


        [Display(Name = "CPF",
                 Description= "CPF.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string CPF { get ; set ; }
        
        [Display(Name = "Nome",
                 Description= "Nome.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Name { get ; set ; }

        [Display(Name = "Funçao",
                 Description= "Funçao.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public string Function { get ; set ; }

        [Display(Name = "Lote",
                 Description= "Lote.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public string Lots { get ; set ; }

        [Display(Name = "Estado",
                 Description= "Estado.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public string State { get ; set ; }

        [Display(Name = "Valor",
                 Description= "Valor.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public string Value { get ; set ; }
        #endregion
    }

    public class ScholarshipValidator : ValidatorBase<Scholarship>
    {
        public ScholarshipValidator ()
        {
            using (var session = DocumentSession) {
                RuleFor (scholarship => scholarship.Name)
                    .NotEmpty ().WithMessage ("O nome do proprietário é obrigatório.")
                        .Length (5, 50).WithMessage ("O nome do proprietário deve conter entre 5 e 50 caracteres.")
                        .Matches (@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
               /**
                RuleFor (scholarship => scholarship.CPF)
                    .NotEmpty ().WithMessage ("A data de entrada é obrigatório.");
                RuleFor (scholarship => scholarship.FinishDate)
                    .NotEmpty ().WithMessage ("A data de saída é obrigatório.");
                    */
            }

            RuleSet ("Update", () => {
                RuleFor (scholarship => scholarship.CPF)
                    .NotEmpty ().WithMessage ("O nome do proprietário é obrigatório.")
                        .Length (5, 50).WithMessage ("O nome do proprietário deve conter entre 5 e 50 caracteres.")
                        .Matches (@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
                /**
                RuleFor (scholarship => scholarship.StartDate)
                    .NotEmpty ().WithMessage ("A data de entrada é obrigatório.");
                RuleFor (scholarship => scholarship.FinishDate)
                    .NotEmpty ().WithMessage ("A data de saída é obrigatório.");
                    */
            }
            );
        }
    }
}

