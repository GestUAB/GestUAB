$(function(){
    $('.carousel').carousel();
    $("#back_button, #cancel_button").click(function () {
    	history.go(-1);
    });

    $("#remove_button").click(function () {
    	return confirm("Deseja Remover?");
    });
});
