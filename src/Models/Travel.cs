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

        public Travel()
        {
            (this as IModel).Id = Guid.NewGuid();
            ProfessorName = string.Empty;
            DepartureDate = DateTimeOffset.Now;
            DepartureTime = DateTimeOffset.Now;
            ReturnDate = DateTimeOffset.Now;
            Vehicle = string.Empty;
            TravelReason = string.Empty;
            Driver = false;
        }

        #region IModel implementation
        System.Guid IModel.Id { get ; set ; }
        #endregion

        [Display(Name = "Nome do professor",
                 Description= "Nome do professor. Ex.: Tony Hild.")]
        public string ProfessorName { get; set; }

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
                RuleFor (travel => travel.ProfessorName)
                    .NotEmpty().WithMessage("O nome do professor é obrigatório.")
                        .Length(5, 30).WithMessage("O nome deve conter entre 5 e 15 caracteres")
                        .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.");
                RuleFor (travel => travel.CompareDate)
                    .Must(true).WithMessage("A data de retorno deve ser maior que a data de partida.");
            }
        }
    }
}

