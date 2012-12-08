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
    /// Car Class.
    /// </summary>
    /// 
    public class Car : IModel
    {

        /// <summary>
        /// Builder Car Class
        /// </summary>
        /// 
        public Car()
        {

        }

        /// <summary>
        /// Static method that creates a default Car.
        /// </summary>
        /// <returns> Default Car</returns>
        /// 
        public static Car DefaultCar()
        {
            return new Car()
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Year = string.Empty,
                Chassi = string.Empty,
                Plate = string.Empty
            };
        }

        #region IModel implementation
        [Display(Name = "Nome",
                 Description= "Nome do carro.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        #region variables
        /// <summary>
        /// The Car's name.
        /// </summary>
        /// 
        [Display(Name = "Nome",
                 Description= "Nome do carro. Ex.: Gol.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Name { get; set; }

        /// <summary>
        /// The Car's Year/Model
        /// </summary>
        ///
        [Display(Name = "Ano/Modelo",
                 Description= "Ano do Carro. Ex.: 2010.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Year { get; set; }

        /// <summary>
        /// The Car's Chassis number.
        /// </summary>
        ///
        [Display(Name = "Chassi",
                 Description= "Número do Chassi.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Chassi { get; set; }

        /// <summary>
        /// The Car's Plate.
        /// </summary>
        ///
        [Display(Name = "Placa",
                 Description= "Placa do Carro. Ex.: AAA-0000.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Plate { get; set; }
        #endregion
      
        public override string ToString ()
        {
            return Name;
        }

    }

    /// <summary>
    /// Car Validator
    /// </summary>
    /// 
    public class CarValidator : ValidatorBase<Car>
    {
        /// <summary>
        /// Method that validates the creation of or a change in an object.
        /// </summary>
        /// 
        public CarValidator()
        {
            #region New Car
            using (var session = DocumentSession)
            {
                RuleFor(car => car.Id).NotEmpty();
                RuleFor(car => car.Name).NotEmpty().WithMessage("O campo Nome é obrigatório").Length(2, 15).WithMessage("O nome deve conter no mínimo 2 e no máximo 15 caracteres.");
                RuleFor(car => car.Year).NotEmpty().WithMessage("O campo Ano/Modelo é obrigatório").Length(4).WithMessage("O ano deve ter 4 caracteres.").Matches("\\d{4}").WithMessage("O ano deve seguir o seguinte modelo: 2010.");
                RuleFor(car => car.Chassi).NotEmpty().WithMessage("O campo Chassi é obrigatório");
                RuleFor(car => car.Plate).NotEmpty().WithMessage("O campo Placa é obrigatório").Matches("[A-Z]{3}-\\d{4}").WithMessage("A placa deve seguir o seguinte modelo: AAA-0000.");
            }
            #endregion

            #region Update
            RuleSet("Update", () =>
            {
                RuleFor(car => car.Id).NotEmpty();
                RuleFor(car => car.Name).NotEmpty().WithMessage("O campo Nome é obrigatório").Length(2, 15).WithMessage("O nome deve conter no mínimo 2 e no máximo 15 caracteres.");
                RuleFor(car => car.Year).NotEmpty().WithMessage("O campo Ano/Modelo é obrigatório").Length(4).WithMessage("O ano deve ter 4 caracteres.").Matches("\\d{4}").WithMessage("O ano deve seguir o seguinte modelo: 2010.");
                RuleFor(car => car.Chassi).NotEmpty().WithMessage("O campo Chassi é obrigatório");
                RuleFor(car => car.Plate).NotEmpty().WithMessage("O campo Placa é obrigatório").Matches("[A-Z]{3}-\\d{4}").WithMessage("A placa deve seguir o seguinte modelo: AAA-0000.");
                  
            });
            #endregion
        }
    }

}
