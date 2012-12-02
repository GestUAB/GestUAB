using System;
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
    /// <summary>
    /// Teacher Class
    /// </summary>
    /// 
    public class Teacher : IModel
    {
        #region Builder
        /// <summary>
        /// Builder Class Teacher
        /// </summary>
        /// 
        public Teacher ()
        {

        }

        /// <summary>
        /// Static method that creates a default Teacher.
        /// </summary>
        /// <returns> Default Teacher</returns>
        /// 
        public static Teacher DefaultTeacher()
        {
            return new Teacher() { 
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Course = null
            };
        }

        #endregion

        #region IModel implementation
        [Display(Name = "Id", Description = "Código do Professor")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Hidden)]
        public Guid Id { get; set; }
        #endregion

        [Display(Name = "Nome", Description = "Nome do Professor")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public String Name { get; set; }

        [Display(Name = "Curso", Description = "Curso que o Professor leciona")]
        [ScaffoldVisibility(read: ScaffoldVisibilityType.Show)]
        public Course Course { get ; set; }
    }

    public class TeacherValidator : ValidatorBase<Teacher>
    {
        public TeacherValidator()
        {
            RuleFor(course => course.Course.Name).NotEmpty();

            RuleSet("Update", () =>
            {
                RuleFor(teacher => teacher.Name)
                    .NotEmpty().WithMessage("O campo nome é obrigatório.")
                    .Matches(@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage("Insira somente letras.")
                        .Length(2, 30).WithMessage("O nome deve conter entre 2 e 30 caracteres.");
            }
            );
        }
    }
}

