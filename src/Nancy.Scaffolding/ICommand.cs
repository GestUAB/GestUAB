using System;
using Mono.Addins;
using HtmlTags;
using FluentValidation.Validators;


namespace Nancy.Scaffolding
{
    [TypeExtensionPoint]
    public interface ICommand
    {
        void Run (IPropertyValidator validator, HtmlTag tag, string formatedMessage);
    };
}

