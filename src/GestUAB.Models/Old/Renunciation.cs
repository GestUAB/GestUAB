using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Models;
using Nancy.Scaffolding;

namespace GestUAB.Models
{
    /// <summary>
    /// Renunciation Class
    /// </summary>
    ///
    public class Renunciation : IModel
    {
        #region Variables
        [Display(Name = "Código",
        Description = "Código da desistencia da viagem.")]
        [ScaffoldVisibility(all: Visibility.Hidden)]
        public Guid Id { get; set; }

        [Display(Name = "Professor",
        Description = "O Professor que desiste da viagem.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string NameTeacher { get; set; }

        [Display(Name = "Data",
        Description = "Data da desistencia")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string TravelDate { get; set; }

        [Display(Name = "Destino",
        Description = "Destino da viagem")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Destiny { get; set; }

        [Display(Name = "Motivo",
        Description = "Motivo da desistencia")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string ReasonGiveup { get; set; }
        #endregion

        /// <summary>
        /// Class Builder
        /// </summary>
        public Renunciation()
        {

        }

        /// <summary>
        /// Static method that creates a default Renunciation
        /// </summary>
        /// <returns></returns>
        public static Renunciation DefaultRenunciation()
        {
            return new Renunciation()
            {
                Id = Guid.NewGuid(),
                NameTeacher = string.Empty,
                TravelDate = DateTime.Now.ToString(),
                Destiny = string.Empty,
                ReasonGiveup = string.Empty
            };
        }
    }

    /// <summary>
    /// Renunciation Validator Class
    /// </summary>
    public class RenunciationValidator : ValidatorBase<Renunciation>
    {
        public RenunciationValidator()
        {
            #region New Renunciation
            using (var session = DocumentSession)
            {
                RuleFor(renunciation => renunciation.Id).NotEmpty();

                RuleFor(renunciation => renunciation.NameTeacher)
                    .NotEmpty().WithMessage("O Nome do professor não pode estar em branco.");
               
                RuleFor(renunciation => renunciation.TravelDate)
                    .NotEmpty().WithMessage("Preencha da data.")
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("Data deve ser no formato: 99/99/9999");

                RuleFor(renunciation => renunciation.Destiny).NotEmpty()
                    .WithMessage("O destino não pode estar em branco.");

                RuleFor(renunciation => renunciation.ReasonGiveup).NotEmpty()
                    .WithMessage("O destino não pode estar em branco.");

            }
            #endregion

            #region Update
            RuleSet("Update", () =>
            {
                RuleFor(renunciation => renunciation.NameTeacher)
                       .NotEmpty().WithMessage("Nome do professor não pode estar em branco.");

                RuleFor(renunciation => renunciation.TravelDate).NotEmpty()
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("Data deve ser no formato: 99/99/9999");

                RuleFor(renunciation => renunciation.Destiny).NotEmpty()
                   .WithMessage("O destino não pode estar em branco.");

                RuleFor(desistence => desistence.ReasonGiveup).NotEmpty()
                    .WithMessage("O destino não pode estar em branco.");
            });

            #endregion
        }
    }
}
