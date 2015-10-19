//var $j = jQuery.noConflict();

function clearForm() {
    $(':input').not(':button, :submit, :reset, :hidden, :checkbox, :radio').val('');
    $(':checkbox, :radio').prop('checked', false);
}

$(document).ready(function () {
    //var url = "@(Html.Raw(Url.Action("ReOrderLevelStatus", "Home")))";
    //$("#ReOrderStatus").load('/Home/ReOrderLevelStatus');
    //$("#menu").add($("li:last").load('/Home/SiteMenuList'));
    $.get('/Home/SiteMenuList', function (data) {
        $("#menu").append(data);
    });

    $.ajax({
        url: "/Home/ReOrderLevelStatus/",
        type: 'GET',
        dataType: 'html', // <-- to expect an html response
        success: function (data) {
            $("div#ReOrderStatus").html(data);
        }
    });

    $.ajax({
        url: "/Home/SiteMenuList/",
        type: 'GET',
        dataType: 'html', // <-- to expect an html response
        success: function (data) {
            //alert(data.responseText);
            $("div#menu").html(data);
        },
        error: function (xht, status, err) {
            alert(xht.responseText);
        }
    });

    setInterval(function () {
        //var url = "@(Html.Raw(Url.Action("ReOrderLevelStatus", "Home")))";
        //$("#ReOrderStatus").load('/Home/ReOrderLevelStatus');
        $.ajax({
            url: "/Home/ReOrderLevelStatus/",
            type: 'GET',
            dataType: 'html', // <-- to expect an html response
            success: function (data) {
                $("div#ReOrderStatus").html(data);
            }
        });
    }, 30000); //Refreshes every 30 seconds

    $.ajaxSetup({ cache: false });  //Turn off caching
});
//var jQuery = jQuery.noConflict();