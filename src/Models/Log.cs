using System;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace GestUAB.Models
{
    public class Log : IModel
    {
        public Log ()
        {
            Id = Guid.NewGuid ();
            User = string.Empty;
            Acao = string.Empty;
        }
    }
}
