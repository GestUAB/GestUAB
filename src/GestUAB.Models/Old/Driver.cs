using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GestUAB.Models;
using Nancy.Scaffolding;
using Nancy.Scaffolding.Validators;

namespace GestUAB.Models
{
    /// <summary>
    /// Driver class.
    /// </summary>
    /// 
    public class Driver : IModel
    {
        #region IModel implementation
        [Display(Name = "Código",
            Description = "Código do motorista.")]
        [ScaffoldVisibility(all: Visibility.Hidden)]
        public System.Guid Id { get; set; }
        #endregion

        #region Variables
        /// <summary>
        /// Driver's name.
        /// </summary>
        /// 
        [Display(Name = "Nome",
                 Description = "Nome do usuário. Ex.: João.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Name { get; set; }

        /// <summary>
        /// Driver's birthdate.
        /// </summary>
        /// 
        [Display(Name = "DtNascimento",
         Description = "Data de Nascimento. Ex.: 01/01/1967.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Driver's Natural Person Register (CPF).
        /// </summary>
        /// 
        [Display(Name = "Cpf Motorista",
         Description = "CPF do motorista. Ex.: xxx.xxx.xxx-xx.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Cpf { get; set; }

        /// <summary>
        /// Driver's  ID card (RG).
        /// </summary>
        /// 
        [Display(Name = "Rg Motorista",
         Description = "RG do motorista.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Rg { get; set; }

        /// <summary>
        /// Driver's telephone number.
        /// </summary>
        /// 
        [Display(Name = "Telefone",
         Description = "Telefone de contato. Ex.: (xx) xxxx-xxxx")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Phone { get; set; }

        /// <summary>
        /// Driver cellphone number.
        /// </summary>
        /// 
        [Display(Name = "Celular",
         Description = "Telefone celular. Ex.: (xx) xxxx-xxxx.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string CellPhone { get; set; }

        /// <summary>
        /// Driver's address.
        /// </summary>
        /// 
        [Display(Name = "Endereco",
         Description = "Endereço residencial.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Address { get; set; }

        /// <summary>
        /// Driver's house number.
        /// </summary>
        /// 
        [Display(Name = "Numero Casa",
         Description = "Número.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string AddressNumber { get; set; }

        /// <summary>
        /// Complement
        /// </summary>
        /// 
        [Display(Name = "Complemento",
         Description = "Complemento.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Complement { get; set; }

        /// <summary>
        /// Zip code (CEP).
        /// </summary>
        /// 
        [Display(Name = "Cep",
         Description = "Código de endereçamento postal (CEP). Ex.: xxxxx-xxx")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Cep { get; set; }

        /// <summary>
        /// Neighborhood.
        /// </summary>
        /// 
        [Display(Name = "Bairro",
         Description = "Bairro.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Neighborhood { get; set; }

        /// <summary>
        /// Workplace.
        /// </summary>
        /// 
        [Display(Name = "Local Trabalho",
         Description = "Local de trabalho.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Workplace { get; set; }

        /// <summary>
        /// Work shift.
        /// </summary>
        /// 
        [Display(Name = "Turno",
         Description = "Turno de trabalho.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Rotation { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        /// 
        [Display(Name = "Oservacao",
         Description = "Observação.")]
        [ScaffoldVisibility(all: Visibility.Show)]
        public string Obs { get; set; }
        #endregion

        #region Class Builder
        
        /// <summary>
        /// Driver class builder.
        /// </summary>
        /// 
        public Driver()
        {

        }

        /// <summary>
        /// Static method that creates a default Driver.
        /// </summary>
        /// <returns> Default Driver</returns>
        /// 
        public static Driver DefaultDriver()
        {
            return new Driver() {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                BirthDate = DateTime.Today,
                Cpf = string.Empty,
                Rg = string.Empty,
                Phone = string.Empty,
                CellPhone = string.Empty,
                Address = string.Empty,
                AddressNumber = string.Empty,
                Complement = string.Empty,
                Cep = string.Empty,
                Neighborhood = string.Empty,
                Workplace = string.Empty,
                Rotation = string.Empty,
                Obs = string.Empty
            };
        }
        #endregion
    }

    /// <summary>
    /// Driver validator
    /// </summary>
    /// 
    public class DriverValidator : ValidatorBase<Driver>
    {
        /// <summary>
        /// Method that validates the creation of or a change in an object.
        /// </summary>
        /// 
        public DriverValidator()
        {
            #region New Driver
            using (var session = DocumentSession)
            {
                RuleFor(driver => driver.Id).NotEmpty();

                RuleFor(driver => driver.Name)
                    .NotEmpty().WithMessage("O nome do motorista é obrigatório.")
                    .Length(10, 30).WithMessage("o nome do motorista deve conter entre 10 e 30 caracteres.")
                    .Matches(@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage("Insira somente letras.")
                    .Must((driver, drivername) => !session.Query<Driver>()
                        .Where(n => n.Name == drivername).Any())
                        .WithMessage(@"Motorista já cadastrado ""{0}""", driver => driver.Name)
                        .Remote("Motorista já existe.", "/validation/user/validate-exists-username", "GET", "*");//verificar

                RuleFor(driver => driver.BirthDate)
                    .InclusiveBetween(DateTime.Today.AddYears(-80), DateTime.Today);

                RuleFor(cpfDriver => cpfDriver.Cpf)
                    .Length(14).WithMessage("O campo CPF deve conter 14 caracteres.")
                    .Matches("^(\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2})|(\\d{11})$").WithMessage("Formato do CPF incorreto.")
                    .Must((cpf, cpfdriver) => !session.Query<Driver>()
                        .Where(n => n.Cpf == cpfdriver).Any())
                        .WithMessage(@"O cpf informado já está cadastrado ""{0}""", cpfDriver => cpfDriver.Cpf)
                        .Remote("CPF já cadastrado.", "/validation/user/validate-exists-username", "GET", "*");//verificar

                RuleFor(driver => driver.Rg).NotEmpty().WithMessage("Informe o RG.");

                RuleFor(driver => driver.Phone).NotEmpty()
                    .Length(14).WithMessage("O campo telefone deve conter 14 caracteres.")
                    .Matches("^\\([0-9]{2}\\)\\s[0-9]{4}-[0-9]{4}$").WithMessage("O telefone deve seguir o seguinte padrão: (xx) xxxx-xxxx.");

                RuleFor(driver => driver.CellPhone).NotEmpty()
                    .Length(14).WithMessage("O campo telefone deve conter 14 caracteres.")
                    .Matches("^\\([0-9]{2}\\)\\s[0-9]{4}-[0-9]{4}$").WithMessage("O telefone deve seguir o seguinte padrão: (xx) xxxx-xxxx.");

                RuleFor(driver => driver.Address).NotEmpty().WithMessage("Preencha o endereço.");

                RuleFor(driver => driver.AddressNumber).NotEmpty().WithMessage("Preencha o número da casa.");

                RuleFor(driver => driver.Complement).NotEmpty().WithMessage("Preencha o complemento.");

                RuleFor(driver => driver.Cep).NotEmpty().WithMessage("Preencha o CEP");

                RuleFor(driver => driver.Neighborhood).NotEmpty().WithMessage("Preencha o bairro.");

                RuleFor(driver => driver.Workplace).NotEmpty().WithMessage("Preencha o local de trabalho.");

                RuleFor(driver => driver.Rotation).NotEmpty().WithMessage("Preencha o turno que deseja trabalhar.");

                RuleFor(driver => driver.Obs).NotEmpty().WithMessage("Preencha o campo observação.");
            }
            #endregion

            #region Update
            RuleSet("Update", () =>
                {
                    RuleFor(driver => driver.Id).NotEmpty();

                    RuleFor(driver => driver.Name)
                        .NotEmpty().WithMessage("O nome do motorista é obrigatório.")
                        .Length(10, 30).WithMessage("o nome do motorista deve conter entre 10 e 30 caracteres.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage("Insira somente letras.");

                    RuleFor(driver => driver.BirthDate)
                        .InclusiveBetween(DateTime.Today.AddYears(-80), DateTime.Today);

                    RuleFor(cpfDriver => cpfDriver.Cpf)
                        .Length(14).WithMessage("O campo CPF deve conter 14 caracteres.")
                        .Matches("^(\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2})|(\\d{11})$").WithMessage("Formato do CPF incorreto.");

                    RuleFor(driver => driver.Rg).NotEmpty().WithMessage("Informe o RG.");

                    RuleFor(driver => driver.Phone).NotEmpty()
                        .Length(14).WithMessage("O campo telefone deve conter 14 caracteres.")
                        .Matches("^\\([0-9]{2}\\)\\s[0-9]{4}-[0-9]{4}$").WithMessage("O telefone deve seguir o seguinte padrão: (xx) xxxx-xxxx.");

                    RuleFor(driver => driver.CellPhone).NotEmpty()
                        .Length(14).WithMessage("O campo telefone deve conter 14 caracteres.")
                        .Matches("^\\([0-9]{2}\\)\\s[0-9]{4}-[0-9]{4}$").WithMessage("O telefone deve seguir o seguinte padrão: (xx) xxxx-xxxx.");

                    RuleFor(driver => driver.Address).NotEmpty().WithMessage("Preencha o endereço.");

                    RuleFor(driver => driver.AddressNumber).NotEmpty().WithMessage("Preencha o número da casa.");

                    RuleFor(driver => driver.Complement).NotEmpty().WithMessage("Preencha o complemento.");

                    RuleFor(driver => driver.Cep).NotEmpty().WithMessage("Preencha o CEP");

                    RuleFor(driver => driver.Neighborhood).NotEmpty().WithMessage("Preencha o bairro.");

                    RuleFor(driver => driver.Workplace).NotEmpty().WithMessage("Preencha o local de trabalho.");

                    RuleFor(driver => driver.Rotation).NotEmpty().WithMessage("Preencha o turno que deseja trabalhar.");

                    RuleFor(driver => driver.Obs).NotEmpty().WithMessage("Preencha o campo observação.");
                });
            #endregion
        }
    }

}
