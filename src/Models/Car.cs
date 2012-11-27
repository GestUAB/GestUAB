using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace GestUAB.Models
{
    public class Car : IModel
    {
        public Car ()
        {
            Id = Guid.NewGuid ();
            Name = string.Empty;
            Year = string.Empty;
            Chassi = string.Empty;
            Plate = string.Empty;
        }

        #region IModel implementation
        [Display(Name = "Nome",
                 Description= "Nome do carro.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        [Display(Name = "Nome",
                 Description= "Nome do carro. Ex.: Gol.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Name { get; set; }

        [Display(Name = "Ano/Modelo",
                 Description= "Ano do Carro. Ex.: 2010.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Year { get; set; }

        [Display(Name = "Chassi",
                 Description= "Número do Chassi.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Chassi { get; set; }

        [Display(Name = "Placa",
                 Description= "Placa do Carro. Ex.: AAA 0000.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Plate { get; set; }
      
        public override string ToString ()
        {
            return Name;
        }

    }
 
    public class CarValidator: ValidatorBase<Car>
    {
        public CarValidator ()
        {
            RuleFor (car => car.Id).NotEmpty ();

            RuleSet ("Update", () => {
                RuleFor (car => car.Name).NotEmpty().WithMessage ("O campo nome é obrigatório.").Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");        
            }
            );

         
        }
    }
}
