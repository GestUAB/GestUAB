using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Models;
using Nancy.Scaffolding;

namespace GestUAB
{
    /// <summary>
    /// Teacher class.
    /// </summary>
    /// 
    public class Teacher : IModel
    {
        #region Builder
        /// <summary>
        /// Teacher class builder.
        /// </summary>
        /// 
        public Teacher ()
        {

        }

        /// <summary>
        /// Static method that creates a default Teacher.
        /// </summary>
        /// <returns> Default Teacher</returns>
        /// 
        public static Teacher DefaultTeacher()
        {
            return new Teacher() { 
                Id = Guid.NewGuid(),
                Name = string.Empty
            };
        }

        #endregion

        #region IModel implementation
        [Display(Name = "Id")]
        [ScaffoldVisibility(all:Visibility.Hidden)] 
        public Guid Id {get; set;}
        #endregion
        
        [Display(Name = "Nome")]
        [ScaffoldVisibility(all:Visibility.Show)] 
        public String Name {get; set;}
    }
}

