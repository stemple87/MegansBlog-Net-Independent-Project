$(document).ready(function () {
    //$(".dropdown-button").dropdown();

    //Category Create
    $('.new-category').submit(function (event) {
        event.preventDefault();
        $.ajax({
            url: '~/../../Categories/NewCategory',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (result) {
                //var resultMessage = 'You\'ve added a new category to the database!<br>Id: ' + result.Id + '<br>Name: ' + result.Name;
                //$('#result6').html(resultMessage);
                console.log("SUCCCCESSSS");
            }
        });

    });
});