using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Validators;
using GestUAB.Models;

namespace GestUAB.Models
{
    /// <summary>
    /// Desistence Class
    /// </summary>
    /// 
    public class Desistence : IModel
    {
        #region Variables
        [Display(Name = "Código",
            Description = "Código da desistencia.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)]
        public Guid Id { get; set; }

        [Display(Name = "Professor",
            Description = "Professor.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string NameTeacher { get; set; }

        [Display(Name = "Data",
            Description = "Data da desistencia")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string TravelDate { get; set; }

        [Display(Name = "Destino",
            Description = "Destino da viagem")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Destiny { get; set; }

        [Display(Name = "Motivo",
            Description = "Motivo da desistencia")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string ReasonGiveup { get; set; }
        #endregion

        /// <summary>
        /// Class Builder
        /// </summary>
        public Desistence()
        {

        }

        /// <summary>
        /// Static method that creates a default Desistence
        /// </summary>
        /// <returns></returns>
        public static Desistence DefaultDesistence()
        {
            return new Desistence() {
                Id = Guid.NewGuid(),
                NameTeacher = string.Empty,
                TravelDate = DateTime.Now.ToString(),
                Destiny = string.Empty,
                ReasonGiveup = string.Empty
            };
        }
    }

    /// <summary>
    /// Desistence Validator Class
    /// </summary>
    public class DesistenceValidator : ValidatorBase<Desistence>
    {
        public DesistenceValidator()
        {
            #region New Desistence
            using (var session = DocumentSession)
            {
                RuleFor(desistence => desistence.Id).NotEmpty();

                RuleFor(desistence => desistence.NameTeacher)
                    .NotEmpty().WithMessage("Nome do professor é obrigatório.");
                    //.Must((desistence, teacherName) => !session.Query<Teacher>()
                    //    .Where(n => n.Name != teacherName).Any())
                    //    .WithMessage("Nome do professor não cadastrado.");
                
                RuleFor(desistence => desistence.TravelDate)
                    .NotEmpty().WithMessage("Preencha da data.")
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");

                RuleFor(desistence => desistence.Destiny).NotEmpty()
                    .WithMessage("Informe o destino.");

                RuleFor(desistence => desistence.ReasonGiveup).NotEmpty()
                    .WithMessage("Informe o destino.");

            }
            #endregion

            #region Update
            RuleSet("Update", () => {
                RuleFor(desistence => desistence.NameTeacher)
                       .NotEmpty().WithMessage("Nome do professor é obrigatório.");

                RuleFor(desistence => desistence.TravelDate).NotEmpty()
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");

                RuleFor(desistence => desistence.Destiny).NotEmpty()
                   .WithMessage("Informe o destino.");

                RuleFor(desistence => desistence.ReasonGiveup).NotEmpty()
                    .WithMessage("Informe o destino.");

            });
            
            #endregion
        }
    }

}