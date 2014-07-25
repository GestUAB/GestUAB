namespace GestUAB.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation;
    using Nancy.Scaffolding;

    /// <summary>
    /// Course class
    /// </summary>
    public class Course : IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.Models.Course"/> class.
        /// </summary>
        public Course()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Display(Name = "Código",
                 Description = "Código do curso.")]
        [ScaffoldVisibility(all: Visibility.Hidden)] 
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Display(Name = "Nome",
                 Description = "Nome do curso. Ex.: Matemática.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GestUAB.Models.Course"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [Display(Name = "Ativo",
                 Description = "O curso está ativo?")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public bool Active { get; set; }
    }

    /// <summary>
    /// Course validator.
    /// </summary>
    public class CourseValidator: ValidatorBase<Course>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.Models.CourseValidator"/> class.
        /// </summary>
        public CourseValidator()
        {
            RuleFor(course => course.Id).NotEmpty();

            this.RuleSet("Update", () => 
            {
                RuleFor(user => user.Name)
                    .NotEmpty().WithMessage("O campo nome é obrigatório.")
                    .Matches(@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage("Insira somente letras.")
                        .Length(2, 30).WithMessage("O nome deve conter entre 2 e 30 caracteres.");
            });
        }
    }
}
