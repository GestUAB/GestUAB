//
// crud.cs
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

var page = function () {
    $.validator.addMethod('alpha', function (value, element, params) {
        return (/^[a-zA-Z]+$/.test(value));
    });
    
    $.validator.unobtrusive.adapters.add("alpha", {}, function(options) {
        options.rules['alpha'] = true;
        options.messages['alpha'] = options.message;
    });
    //Update that validator
    $.validator.setDefaults({
        //errorClass:'error',
        //validClass:'success',
        highlight: function (element) {
            //console.log($(element));
            $(element).closest(".control-group").addClass("error");
        },
        unhighlight: function (element) {
            $(element).closest(".control-group").removeClass("error");
        }
    });
} ();


$(function() {
/*
    $("#update_form").validate({
        errorClass:'error',
        validClass:'success',
        errorElement:'span',
        showErrors: function(errorMap, errorList) {
            $("#summary").html("Your form contains "
                                           + this.numberOfInvalids() 
                                           + " errors, see details below.");
            this.defaultShowErrors();
          },        
        highlight: function (element, errorClass, validClass) { 
            $(element).parents("div[class='clearfix']").addClass(errorClass).removeClass(validClass); 
        }, 
        unhighlight: function (element, errorClass, validClass) { 
            $(element).parents(".error").removeClass(errorClass).addClass(validClass); 
        }
    });
    $.validator.setDefaults({
        errorClass:'error',
        validClass:'success',
        errorElement:'span',
        onkeyup:false,
        showErrors: function(errorMap, errorList) {
            $("#summary").html("Your form contains "
                                           + this.numberOfInvalids() 
                                           + " errors, see details below.");
            this.defaultShowErrors();
          }        
    });
*/
    
    
    
    //var settings = $("#update_form").data('validator').settings;
    //settings.errorClass = 'error';
    //settings.validClass = 'success';
    
    // Handler for .ready() called.
    $("#update_button").click(function(event) {
        alert("Handler for .click() called.");
        //event.preventDefault();
        $.ajax({
           url: window.location.href,
           async: false,
           type: "PUT",
           data: $("#update_form").serialize(),
           dataType: "jsonp",
           error: function(jqXHR, textStatus, errorThrown){
                console.log(textStatus);
                console.log(errorThrown);
           },
           complete: function(jqXHR, textStatus){
                console.log(textStatus);
           },
            statusCode: {
                200: function(data, textStatus, jqXHR) {
                            $("#errors").html(data);
                },                
                201: function(data, textStatus, jqXHR) {
                    if (jqXHR.getResponseHeader('Location')) {
                        window.location.href = jqXHR.getResponseHeader('Location');
                    }
                    else {
                        alert("Ooops!");
                    }
                }
              }                  
        });
        return false;
    });
});