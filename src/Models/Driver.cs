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
    public class Driver : IModel
    {
        #region IModel implementation
        [Display(Name = "Código",
            Description = "Código do motorista.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Hidden)]
        public System.Guid Id { get; set; }
        #endregion

        #region Variables
        /// <summary>
        /// Name of driver.
        /// </summary>
        /// 
        [Display(Name = "Nome",
                 Description = "Nome do usuário. Ex.: João.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Name { get; set; }

        /// <summary>
        /// Date of birth of the driver.
        /// </summary>
        /// 
        [Display(Name = "DtNascimento",
         Description = "Data de Nascimento. Ex.: 01/01/1967.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string BirthDate { get; set; }

        /// <summary>
        /// Registration of natural person's driver (CPF).
        /// </summary>
        /// 
        [Display(Name = "Cpf Motorista",
         Description = "CPF do motorista. Ex.: xxx.xxx.xxx-xx.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Cpf { get; set; }

        /// <summary>
        /// Registrar General of the driver (RG).
        /// </summary>
        /// 
        [Display(Name = "Rg Motorista",
         Description = "RG do motorista.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Rg { get; set; }

        /// <summary>
        /// Telephone number of the driver.
        /// </summary>
        /// 
        [Display(Name = "Telefone",
         Description = "Telefone de contato. Ex.: (xx) xxxx-xxxx")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Phone { get; set; }

        /// <summary>
        /// Cell phone number of the driver.
        /// </summary>
        /// 
        [Display(Name = "Celular",
         Description = "Telefone celular. Ex.: (xx) xxxx-xxxx.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string CellPhone { get; set; }

        /// <summary>
        /// Address driver
        /// </summary>
        /// 
        [Display(Name = "Endereco",
         Description = "Endereço residencial.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Address { get; set; }

        /// <summary>
        /// Number of home driver.
        /// </summary>
        /// 
        [Display(Name = "Numero Casa",
         Description = "Número.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string AddressNumber { get; set; }

        /// <summary>
        /// Complement
        /// </summary>
        /// 
        [Display(Name = "Complemento",
         Description = "Complemento.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Complement { get; set; }

        /// <summary>
        /// Zip code (CEP).
        /// </summary>
        /// 
        [Display(Name = "Cep",
         Description = "Código de endereçamento postal (CEP). Ex.: xxxxx-xxx")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Cep { get; set; }

        /// <summary>
        /// Neighborhood.
        /// </summary>
        /// 
        [Display(Name = "Bairro",
         Description = "Bairro.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Neighborhood { get; set; }

        /// <summary>
        /// Workplace.
        /// </summary>
        /// 
        [Display(Name = "Local Trabalho",
         Description = "Local de trabalho.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Workplace { get; set; }

        /// <summary>
        /// Bout to work.
        /// </summary>
        /// 
        [Display(Name = "Turno",
         Description = "Turno de trabalho.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Rotation { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        /// 
        [Display(Name = "Oservacao",
         Description = "Observação.")]
        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
        public string Obs { get; set; }
        #endregion

        #region Class Builder
        public Driver()
        {

        }

        public static Driver DefaultDriver()
        {
            return new Driver() {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                BirthDate = string.Empty,
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


    public class DriverValidator : ValidatorBase<Driver>
    {
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

                RuleFor(driver => driver.BirthDate).NotEmpty()
                    .Length(10)
                    .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                    .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");

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

                    RuleFor(driver => driver.BirthDate).NotEmpty()
                        .Length(10)
                        .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$")
                        .WithMessage("O formato deve seguir o seguinte padrão: 01/01/0001 ");

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