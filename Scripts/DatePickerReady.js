if (!Modernizr.inputtypes.date) {
    $(function () {

        $(".datecontrol").datepicker({
            format: 'dd/mm/yyyy'
        });

        $("select").addClass("form-control");

    });
}