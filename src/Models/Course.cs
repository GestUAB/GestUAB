using FluentValidation;

namespace GestUAB.Models
{
	public class Course : IModel
	{
        public int Number { get; set; }
        public string Name { get; set; }
	}
 
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(course => course.Number).NotEmpty();
            RuleFor(course => course.Name).NotEmpty();
        }
    }
}
