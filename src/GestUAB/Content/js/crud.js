//
// crud.js
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

$(function() {

    $("#btn-edit").click(function(event) {
        if(!$("#edit-form").valid()) {
            var validate = $("#edit-form").validate()
            $(document).scrollTop(0);
            validate.focusInvalid();
            $(document).scrollTop($(document).scrollTop() + 70);
            
            var ul = $("<ul></ul>");
            $.each(validate.invalid, function (i, e) {
                ul.append($("<li>" + e + "</li>"));
            });
            //err.html(ul);
            err = ul[0].outerHTML;
            
            $.pnotify({
                title: "Erro",
                text: err,
                type: 'error',
                hide: true
            });
            
            return false;
        }
        $.ajax({
           url: window.location.href,
           async: false,
           type: "PUT",
           data: $("#edit-form").serialize(),
           dataType: "jsonp",
           error: function(jqXHR, textStatus, errorThrown){
                $.pnotify({
                    title: textStatus,
                    text: errorThrown,
                    type: 'error',
                    hide: true
                });
                console.log(textStatus);
                console.log(errorThrown);
           },
           complete: function(jqXHR, textStatus){
           
                console.log(textStatus);
           },
            statusCode: {
                400: function(jqXHR, textStatus, errorThrown) {
                    //var err = $("#errors");
                    var err;
                    if(jqXHR.responseJSON[0].ErrorMessage) {
                        var ul = $("<ul></ul>");
                        $.each(jqXHR.responseJSON, function (i, e) {
                            ul.append($("<li>" + e.ErrorMessage + "</li>"));
                        });
                        //err.html(ul);
                        err = ul[0].outerHTML;
                    }
                    else {
                        //err.html(jqXHR.responseJSON);
                        err = jqXHR.responseJSON;
                    }
                    //$('html,body').animate({scrollTop:$('[name="errors"]').offset().top - 50}, 500);
                    var reason = jqXHR.getResponseHeader('X-Status-Reason');
                    $.pnotify({
                        title: reason ? reason : "Erro",
                        text: err,
                        type: 'error',
                        hide: true
                    });
                },                
                202: function(data, textStatus, jqXHR) {
                    if (jqXHR.getResponseHeader('Location')) {
                        window.location.href = jqXHR.getResponseHeader('Location');
                    }
                }
              }                  
        });
        return false;
    });
    
    $("#btn-new").click(function(event) {
        if(!$("#new-form").valid()) {
            var validate = $("#edit-form").validate()
            $(document).scrollTop(0);
            validate.focusInvalid();
            $(document).scrollTop($(document).scrollTop() - 60);
            
            var ul = $("<ul></ul>");
            $.each(validate.invalid, function (i, e) {
                ul.append($("<li>" + e + "</li>"));
            });
            //err.html(ul);
            err = ul[0].outerHTML;
            
            $.pnotify({
                title: "Erro",
                text: err,
                type: 'error',
                hide: true
            });
            
            return false;
        }

        $.ajax({
           url: window.location.href,
           async: false,
           type: "POST",
           data: $("#new-form").serialize(),
           dataType: "jsonp",
           error: function(jqXHR, textStatus, errorThrown){
                $.pnotify({
                    title: textStatus,
                    text: errorThrown,
                    type: 'error',
                    hide: true
                });
                console.log(textStatus);
                console.log(errorThrown);
           },
           complete: function(jqXHR, textStatus){
                console.log(textStatus);
           },
            statusCode: {
                400: function(jqXHR, textStatus, errorThrown) {
                    //var err = $("#errors");
                    var err;
                    if(jqXHR.responseJSON[0].ErrorMessage) {
                        var ul = $("<ul></ul>");
                        $.each(jqXHR.responseJSON, function (i, e) {
                            ul.append($("<li>" + e.ErrorMessage + "</li>"));
                        });
                        //err.html(ul);
                        err = ul[0].outerHTML;
                    }
                    else {
                        //err.html(jqXHR.responseJSON);
                        err = jqXHR.responseJSON;
                    }
                    //$('html,body').animate({scrollTop:$('[name="errors"]').offset().top - 50}, 500);
                    var reason = jqXHR.getResponseHeader('X-Status-Reason');
                    $.pnotify({
                        title: reason ? reason : "Erro",
                        text: err,
                        type: 'error',
                        hide: true
                    });
                },                
                201: function(data, textStatus, jqXHR) {
                    if (jqXHR.getResponseHeader('Location')) {
                        window.location.href = jqXHR.getResponseHeader('Location');
                    }
                }
              }                  
        });
        return false;
    });    
    
    $(".btn-delete").click(function(event) {
                var link = $(this);
                bootbox.confirm("Tem certeza que deseja excluir o item?", "Não", "Sim", function(result) {
                    if (result) {
                        $.ajax({
                           url: link.attr("href"),
                           async: false,
                           type: "DELETE",
                           dataType: "jsonp",
                           error: function(jqXHR, textStatus, errorThrown){
                                console.log(errorThrown);
                           },
                           complete: function(jqXHR, textStatus){
                           },
                            statusCode: {
                                204: function(data, textStatus, jqXHR) {
                                    if (jqXHR.getResponseHeader('Location')) {
                                        window.location.href = jqXHR.getResponseHeader('Location');
                                    }
                                    else {
                                        link.parents("tr").slideUp();
                                    }                    
                                }
                            }
                        });
                    }
                }); 
                return false;
            })
                
});


