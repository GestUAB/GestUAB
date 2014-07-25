using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Nancy.Scaffolding;

namespace GestUAB.Models
{
    /// <summary>
    /// Scholarship.
    /// </summary>
    public class Scholarship : IModel
    {
        #region Builder
        public Scholarship ()
        {
            
        }

        public static Scholarship DefaultScholarship()
        {
            return new Scholarship() { 
                Id = Guid.NewGuid(),
                Owner = string.Empty
            };
        }

        #endregion

        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código da bolsa.")]
        [ScaffoldVisibility(all:Visibility.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        //TODO: Must this be a person?
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public string Owner { get; set; }

        public override string ToString ()
        {
            return Owner;
        }
		
    }
}

