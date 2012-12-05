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
    public class Travel : IModel
    {
        #region Builder
        /// <summary>
        /// Builder Class Travel
        /// </summary>
        /// 
        public Travel ()
        {
            
        }
        
        /// <summary>
        /// Static method that creates a default Travel.
        /// </summary>
        /// <returns> Default Travel </returns>
        /// 
        public static Travel DefaultTravel()
        {
            return new Travel(){
                Id = Guid.NewGuid(),
                TravelTitle = string.Empty,
                TeacherName = string.Empty,
                DepartureDate = string.Empty,
                ReturnDate = string.Empty,
                Vehicle = string.Empty,
                TravelReason = string.Empty,
                Driver = false
            };
        }
        #endregion

        #region IModel implementation
        [Display(Name = "Código",
                 Description = "Código da viagem.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Hidden)]
        public System.Guid Id { get ; set ; }
        #endregion

        #region Variables
        [Display(Name = "Título da viagem",
                 Description= "Título da viagem. Ex.: Excursão para o CSBC.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string TravelTitle { get; set; }

        [Display(Name = "Nome do professor",
                 Description= "Nome do professor. Ex.: Tony Hild.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string TeacherName { get; set; }

        [Display(Name = "Data e hora de partida",
                 Description = "Data de partida. Ex.: 13/09/2012.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string DepartureDate { get; set; }

        [Display(Name = "Data de retorno",
                 Description= "Data de retorno. Ex.: 14/09/2012.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string ReturnDate { get; set; }

        [Display(Name = "Observação para o veículo",
                 Description= "Observação para veículo. Ex.: Carro com ar condicionado.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Vehicle { get; set; }

        [Display(Name = "Motivo da viagem",
                 Description= "Motivo da viagem. Ex.: Aula prática.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string TravelReason { get; set; }

        [Display(Name = "Solicitar motorista",
                 Description= "Solicitar motorista. Se necessário, marque a opção.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public bool Driver { get; set; }
        #endregion
    }

    public class TravelValidator : ValidatorBase<Travel>
    {
        public TravelValidator ()
        {
            using (var session = DocumentSession){
                RuleFor (travel => travel.TravelTitle)
                    .NotEmpty().WithMessage("O título da viagem é obrigatório.")
                        .Length(10, 60).WithMessage("O título da viagem deve conter entre 10 e 60 caracteres")
                        .Matches("^[A-Za-z ]+$").WithMessage("Insira somente letras e espaços.")
                        .Must ((travel, title) => !session.Query<Travel> ()
                        .Where (n => n.TravelTitle == title).Any ()
                ).WithMessage (@"Já existe uma viagem cadastrada com este título: ""{0}""", travel => travel.TravelTitle)
                     .Remote ("Viagem já cadastrada.", "/validation/travel/validate-exists-title", "GET", "*");
                RuleFor (travel => travel.TeacherName)
                    .NotEmpty().WithMessage("O nome do professor é obrigatório.")
                        .Length(5, 30).WithMessage("O nome do professor deve conter entre 5 e 30 caracteres")
                        .Matches("^[A-Za-z ]+$").WithMessage("Insira somente letras.");
                RuleFor(travel => travel.DepartureDate)
                    .NotEmpty().WithMessage("A data de partida deve ser preenchida.")
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");
                RuleFor(travel => travel.ReturnDate)
                    .NotEmpty().WithMessage("A data de retorno deve ser preenchida.")
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");
                RuleFor (travel => travel.TravelReason)
                    .NotEmpty().WithMessage("O motivo da viagem é obrigatório.")
                        .Length(10, 60).WithMessage("O motivo da viagem deve conter entre 10 e 60 caracteres")
                        .Matches("^[A-Za-z ]+$").WithMessage("Insira somente letras e espaços.");
            }
            RuleSet ("Update", () => {
                RuleFor (travel => travel.TeacherName)
                    .NotEmpty ().WithMessage ("O nome do professor é obrigatório.")
                        .Matches("^[A-Za-z ]+$").WithMessage("Insira somente letras.")
                        .Length (5, 30).WithMessage ("O nome deve conter entre 5 e 30 caracteres.");
               
            }
            );
        }
    }
}

