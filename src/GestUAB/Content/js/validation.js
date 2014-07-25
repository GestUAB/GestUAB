//
// validation.js
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2012 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

$(document).ready(function() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        language: 'pt-BR',
        autoclose: true
    });
    $('[data-mask]').inputmask();
});    

String.prototype.repeat = function(times) {
   return (new Array(times + 1)).join(this);
};

$('input[data-val=true]').on('blur', function() {
    $(this).valid();
});

var page = function () {
/*
    jQuery.extend(jQuery.validator.messages, {
        required: "This field is required.",
        remote: "Please fix this field.",
        email: "Please enter a valid email address.",
        url: "Please enter a valid URL.",
        date: "Please enter a valid date.",
        dateISO: "Please enter a valid date (ISO).",
        number: "Please enter a valid number.",
        digits: "Please enter only digits.",
        creditcard: "Please enter a valid credit card number.",
        equalTo: "Please enter the same value again.",
        accept: "Please enter a value with a valid extension.",
        maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
        minlength: jQuery.validator.format("Please enter at least {0} characters."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });
*/

    $.validator.addMethod('alpha', function (value, element, params) {
        return (/^[a-zA-Z]+$/.test(value));
    });
    
    
    $.validator.unobtrusive.adapters.add("alpha", {}, function(options) {
        options.rules['alpha'] = true;
        options.messages['alpha'] = options.message;
    });
    
    $.validator.addMethod("cpf", function(value, element, params) {
        var cpf = value;
        var exp = /[^0-9]/g;
        cpf = cpf.toString().replace (exp, ""); 
        if (cpf == "") return true;
        if (cpf.charAt(0).repeat(cpf.length) == cpf) return false;
        var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
        var soma1=0, soma2=0;
        var vlr =11;
        
        for(i=0;i<9;i++){
            soma1+=eval(cpf.charAt(i)*(vlr-1));
            soma2+=eval(cpf.charAt(i)*vlr);
            vlr--;
        }   
        soma1 = (((soma1*10)%11)==10 ? 0:((soma1*10)%11));
        soma2=(((soma2+(2*soma1))*10)%11);
        
        var digitoGerado=(soma1*10)+soma2;
        if(digitoGerado!=digitoDigitado) {
            return false;
        }
        return true;
    });    

    $.validator.unobtrusive.adapters.add("cpf", {}, function(options) {
        options.rules['cpf'] = function() { return options.element.value; };
        options.messages['cpf'] = options.message;
    });
    
    $.validator.addMethod("dateBR", function(value, element) {
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if( re.test(value)) {
            var adata = value.split('/');
            var gg = parseInt(adata[0],10);
            var mm = parseInt(adata[1],10);
            var aaaa = parseInt(adata[2],10);
            var xdata = new Date(aaaa,mm-1,gg);
            if ( ( xdata.getFullYear() === aaaa ) && ( xdata.getMonth() === mm - 1 ) && ( xdata.getDate() === gg ) ){
                check = true;
            } else {
                check = false;
            }
        } else {
            check = false;
        }
        return this.optional(element) || check;
    }, "Data inválida");

    $.validator.unobtrusive.adapters.add("dateBR", {}, function(options) {
        options.rules['dateBR'] = true;
        options.messages['dateBR'] = options.message;
    });
        
        
    $.validator.addMethod('daterange', function(value, element, arg) {
     // Same as above

     var startDate = Date.parse(arg[0]),
         endDate = Date.parse(arg[1]),
         enteredDate = Date.parse(value);       
     // Same as below

    if(isNaN(enteredDate)) return false;

    return ((startDate <= enteredDate) && (enteredDate <= endDate));
    
    }, $.validator.format("A data deve estar entre {0} e {1}."))
 
                  
    $.validator.unobtrusive.adapters.addMinMax("daterange", '', '', 'daterange','min', 'max');
            
                 
    $.validator.addMethod("nomeConjuge", function(value, element) {
        if ($('select[name="EstadoCivil"]').val() == "Casado" && value == "") {
            return false;
        }
        else if ($('select[name="EstadoCivil"]').val() != "Casado" && value != "") {
            return false;
        }
        return true;
        //return  && !($('select[name="EstadoCivil"]').val() != "Casado" && value != "");
    });

    $.validator.unobtrusive.adapters.add("nomeConjuge", {}, function(options) {
        options.rules['nomeConjuge'] = true;
        options.messages['nomeConjuge'] = function () {
            if ($('select[name="EstadoCivil"]').val() == "Casado" && $('input[name="NomeConjuge"]').val() == "") {
                return "O nome do cônjuge é obrigatório se o estado civil for \"casado\".";
            }
            else if ($('select[name="EstadoCivil"]').val() != "Casado" && $('input[name="NomeConjuge"]').val() != "") {
                return "O nome do cônjuge deve ser informado somente se o estado civil for \"casado\".";
            }
            return "";
        };
    });
                     
    $.validator.unobtrusive.adapters.add("lettersonly", {}, function(options) {
        options.rules['lettersonly'] = true;
        options.messages['lettersonly'] = options.message;
    });
            
            
    $.validator.addMethod("regexwithmask", function (value, element, params) {
        var match;
        if (this.optional(element)) {
            return true;
        }
        //var exp = /[9a\?\*]/g;
        //var striped = params["mask"].replace (exp, ""); 
        //value = value.toString().replace (new RegExp(striped, "g"), ""); 
        match = new RegExp(params["pattern"]).exec(value);
        return (match && (match.index === 0) && (match[0].length === value.length));
    });
    
    $.validator.unobtrusive.adapters.add("regexwithmask", ["pattern", "mask"], function(options) {
        options.rules['regexwithmask'] = options.params;
        options.messages['regexwithmask'] = options.message;
    });    
    
    /*
    $.validator.addMethod('matches', function (value, element, params) {
        return ((new RegExp(params.regex)).test(value));
    });
    
    $.validator.unobtrusive.adapters.add("matches", ["regex"], function(options) {
        var params = {
            regex: options.params.regex.split(',')
        };
        options.rules['matches'] = params;
        options.messages['matches'] = options.message;
    });
    */
        
    //Update that validator
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".control-group").addClass("alert alert-error");
            //$(element).next().addClass("alert alert-error");
        },
        unhighlight: function (element) {
            $(element).closest(".control-group").removeClass("alert alert-error");
            //$(element).next().removeClass("alert alert-error");
        }
    });
        
    /*
    $.validator.setDefaults({
        errorPlacement: function(error,element) {
           return true;
        },
        highlight: function (element, errorClass, validClass) {
            var $element;
            if (element.type === 'radio') {
                $element = this.findByName(element.name);
            } else {
                $element = $(element);
            }
            $element.addClass(errorClass).removeClass(validClass);
            $element.parents("div.control-group").addClass("error");
        },
        unhighlight: function (element, errorClass, validClass) {
            var $element;
            if (element.type === 'radio') {
                $element = this.findByName(element.name);
            } else {
                $element = $(element);
            }
            $element.removeClass(errorClass).addClass(validClass);
            $element.parents("div.control-group").removeClass("error");
        },
        showErrors: function (errorMap, errorList) {

            $.each(this.successList, function (index, value) {
                $(value).popover('hide');
            });

            $.each(errorList, function (index, value) {
                var pop = $(value.element).popover({
                    trigger: 'manual',
                    content: value.message,
                    template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>'
                });

                pop.data('popover').options.content = value.message;

                $(value.element).popover('show');

            });

            this.defaultShowErrors();
        }
    });
    */
    
} ();

(function($){
    $.fn.toJSON = function(options){

        options = $.extend({}, options);

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key":      /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push":     /^$/,
                "fixed":    /^\d+$/,
                "named":    /^[a-zA-Z0-9_]+$/
            };


        this.build = function(base, key, value){
            base[key] = value;
            return base;
        };

        this.push_counter = function(key){
            if(push_counters[key] === undefined){
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function(){

            // skip invalid keys
            if(!patterns.validate.test(this.name)){
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while((k = keys.pop()) !== undefined){

                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if(k.match(patterns.push)){
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                // fixed
                else if(k.match(patterns.fixed)){
                    merge = self.build([], k, merge);
                }

                // named
                else if(k.match(patterns.named)){
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };
})(jQuery);
