using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace GestUAB.Models
{
    public class Course : IModel
    {
        public Course ()
        {
            Id = Guid.NewGuid ();
            Name = string.Empty;
        }

        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código do curso.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        [Display(Name = "Nome",
                 Description= "Nome do curso. Ex.: Matemática.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Name { get; set; }

        [Display(Name = "Ativo",
                 Description= "O curso está ativo?")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public bool Active { get; set; }

        public override string ToString ()
        {
            return Name;
        }

    }
 
    public class CourseValidator: ValidatorBase<Course>
    {
        public CourseValidator ()
        {
            RuleFor (course => course.Id).NotEmpty ();

            RuleSet ("Update", () => {
                RuleFor (user => user.Name)
                    .NotEmpty ().WithMessage ("O campo nome é obrigatório.")
                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                        .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
            }
            );
        }
    }
}
