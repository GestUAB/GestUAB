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
/*
$(function() {
    $("#update_button").click(function(event) {
        if(!$("#update_button").valid()) return false;
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
*/    
    $(".delete").click(function(event) {
                var link = $(this);
                $.ajax({
                   url: link.attr("href"),
                   async: false,
                   type: "DELETE",
                   dataType: "jsonp",
                   error: function(jqXHR, textStatus, errorThrown){
                            alert("Error!");
                   },
                   complete: function(jqXHR, textStatus){
                   },
                    statusCode: {
                    200: function(data, textStatus, jqXHR) {
                            link.parents("tr").slideUp();
                        }
                    }
                });
                return false;
            });
                
});

