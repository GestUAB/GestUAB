using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Validators;
using GestUAB.Models;

namespace GestUAB
{
    public class Teacher : IModel
    {
        #region Builder
        public Teacher ()
        {

        }

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
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Hidden)] 
        public Guid Id {get; set;}
        #endregion
        
        [Display(Name = "Nome")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public String Name {get; set;}
    }
}

