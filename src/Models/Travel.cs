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
            DepartureDate = DateTime.Now;
            ReturnDate = DateTime.Now;
            Vehicle = string.Empty;
            TravelReason = string.Empty;
            Driver = false;
        }

        #region IModel implementation
        System.Guid IModel.Id { get ; set ; }
        #endregion

        [Display(Name = "Nome do Professor",
                 Description= "Nome do professor. Ex.: Tony Hild.")]
        [ScaffoldVisibility(create:ScaffoldVisibilityType.Show)] 
        public string ProfessorName { get; set; }

        [Display(Name = "Data de partida",
                 Description= "Data de partida. Ex.: 13/09/2012.")]
        [ScaffoldVisibility(read:ScaffoldVisibilityType.Show)] 
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Data de retorno",
                 Description= "Data de retorno. Ex.: 14/09/2012.")]
        public DateTimeOffset ReturnDate { get; set; }

        [Display(Name = "Hora de partida",
                 Description= "Hora de partida. Ex.: 15:00")]
        public DateTimeOffset DepartureTime { get; set; }

        [Display(Name = "Veículo",
                 Description= "Observação para veículo. Ex.: Carro com ar condicionado.")]
        public string Vehicle { get; set; }

        [Display(Name = "Motivo da viagem",
                 Description= "Motivo da viagem. Ex.: Aula de campo.")]
        public string TravelReason { get; set; }

        [Display(Name = "Motorista",
                 Description= "Solicitar motorista. Se necessário, marque a opção.")]
        public bool Driver { get; set; }


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
            }

            /*
             * using (var session = DocumentSession) {
                RuleFor (user => user.Username)
                    .NotEmpty ().WithMessage ("O nome de usuário é obrigatório.")
                    .Length (5, 30).WithMessage ("O nome de usuário deve conter entre 5 e 15 caracteres.")
                    .Matches (@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.")
                    .Must ((user, username) => !session.Query<User> ()
                        .Where (n => n.Username == username).Any ()
                ).WithMessage (@"Já existe um usuário com o nome de usuário ""{0}""", user => user.Username)
                     .Remote ("Usuário já existe.", "/validation/user/validate-exists-username", "GET", "*");
                RuleFor (user => user.Email)
                    .NotEmpty ().WithMessage ("O e-mail é obrigatório.")
                    .EmailAddress ().WithMessage ("E-mail inválido.")
                    .Must ((user, email) => !session.Query<User> ()
                        .Where (n => n.Username != user.Username && n.Email == email).Any ()
                ).WithMessage (@"Já existe um usuário com o e-mail ""{0}""", user => user.Email)
                    .Remote ("E-mail já cadastrado.", "/validation/user/validate-exists-email", "GET", "*");
            }
            RuleSet ("Update", () => {
                RuleFor (user => user.FirstName)
                    .NotEmpty ().WithMessage ("O campo nome é obrigatório.")
                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                        .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
                RuleFor (user => user.LastName)
                    .NotEmpty ().WithMessage ("O campo sobrenome é obrigatório.")
                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                    .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
            }
            );*/
        }
    }
}

