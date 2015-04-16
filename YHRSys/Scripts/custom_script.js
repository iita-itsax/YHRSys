function clearForm() {
    $(':input').not(':button, :submit, :reset, :hidden, :checkbox, :radio').val('');
    $(':checkbox, :radio').prop('checked', false);
}

$(document).ready(function () {
    //var url = "@(Html.Raw(Url.Action("ReOrderLevelStatus", "Home")))";
    $("#ReOrderStatus").load('/Home/ReOrderLevelStatus');
    setInterval(function () {
        //var url = "@(Html.Raw(Url.Action("ReOrderLevelStatus", "Home")))";
        $("#ReOrderStatus").load('/Home/ReOrderLevelStatus');
    }, 30000); //Refreshes every 30 seconds

    $.ajaxSetup({ cache: false });  //Turn off caching
});