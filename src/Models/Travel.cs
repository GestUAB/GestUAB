using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;

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
                DepartureDate = DateTimeOffset.Now,
                DepartureTime = DateTimeOffset.Now,
                ReturnDate = DateTimeOffset.Now,
                Vehicle = string.Empty,
                TravelReason = string.Empty,
                Driver = false
            };
        }
        #endregion

        #region IModel implementation
        System.Guid IModel.Id { get ; set ; }
        #endregion
        
        [Display(Name = "Título da viagem",
                 Description= "Título da viagem. Ex.: Excursão para o CSBC.")]
        public string TravelTitle { get; set; }

        [Display(Name = "Nome do professor",
                 Description= "Nome do professor. Ex.: Tony Hild.")]
        public string TeacherName { get; set; }

        [Display(Name = "Data de partida",
                 Description= "Data de partida. Ex.: 13/09/2012.")]
        public DateTimeOffset DepartureDate { get; set; }

        [Display(Name = "Data de retorno",
                 Description= "Data de retorno. Ex.: 14/09/2012.")]
        public DateTimeOffset ReturnDate { get; set; }

        [Display(Name = "Hora de partida",
                 Description= "Hora de partida. Ex.: 15:00")]
        public DateTimeOffset DepartureTime { get; set; }

        [Display(Name = "Observaçao para o veículo",
                 Description= "Observação para veículo. Ex.: Carro com ar condicionado.")]
        public string Vehicle { get; set; }

        [Display(Name = "Motivo da viagem",
                 Description= "Motivo da viagem. Ex.: Aula de campo.")]
        public string TravelReason { get; set; }

        [Display(Name = "Solicitar motorista",
                 Description= "Solicitar motorista. Se necessário, marque a opção.")]
        public bool Driver { get; set; }

        public DateTimeOffset CompareDate { get { return ReturnDate > DepartureDate; } }

    }

    public class TravelValidator : ValidatorBase<Travel>
    {
        public TravelValidator ()
        {
            using (var session = DocumentSession){
                RuleFor (travel => travel.TravelTitle)
                    .NotEmpty().WithMessage("O título da viagem é obrigatório.")
                        .Length(5, 60).WithMessage("O título da viagem deve conter entre 5 e 60 caracteres")
                        .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.")
                        .Must ((travel, title) => !session.Query<Travel> ()
                        .Where (n => n.TravelTitle == title).Any ()
                ).WithMessage (@"Já existe uma viagem cadastrada com este título: ""{0}""", travel => travel.TravelTitle)
                     .Remote ("Viagem já cadastrada.", "/validation/travel/validate-exists-title", "GET", "*");
                RuleFor (travel => travel.TeacherName)
                    .NotEmpty().WithMessage("O nome do professor é obrigatório.")
                        .Length(5, 30).WithMessage("O nome do professor deve conter entre 5 e 30 caracteres")
                        .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
                RuleFor (travel => travel.CompareDate)
                    .Must(true).WithMessage("A data de retorno deve ser maior que a data de partida.");
                RuleFor (travel => travel.TravelReason)
                    .NotEmpty().WithMessage("O motivo da viagem é obrigatório.")
                        .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
            }
            RuleSet ("Update", () => {
                RuleFor (travel => travel.TeacherName)
                    .NotEmpty ().WithMessage ("O nome do professor é obrigatório.")
                        .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                        .Length (5, 30).WithMessage ("O nome deve conter entre 5 e 30 caracteres.");
                RuleFor (travel => travel.CompareDate)
                    .Must(true).WithMessage("A data de retorno deve ser maior que a data de partida.");
                RuleFor (travel => travel.TravelReason)
                    .NotEmpty().WithMessage("O motivo da viagem é obrigatório.")
                        .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
            }
            );
        }
    }
}

