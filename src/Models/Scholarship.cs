using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace GestUAB.Models
{
    public class Scholarship : IModel
    {
 
        public Scholarship ()
        {
            Id = Guid.NewGuid();
            Owner = string.Empty;
        }

        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código do curso.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        public string Owner { get; set; }
		
    }
}

