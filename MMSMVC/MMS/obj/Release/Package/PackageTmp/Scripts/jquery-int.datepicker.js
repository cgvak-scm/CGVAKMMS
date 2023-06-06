jQuery(document).on('ready page:change', function () {
    var datetimefield = jQuery('.datetimepicker');
    datetimefield.datetimepicker({
        format: 'd/m/Y h:i:s A',
        closeOnDateSelect: true,
        lang: datetimefield.attr('lang') || datetimefield.parents('[lang]').attr('lang')
    });
});

jQuery(document).on('ready page:change', function () {
    var datetimefield = jQuery('.datepicker');
    datetimefield.datetimepicker({
        timepicker: false,
        format: 'd/m/Y',
        closeOnDateSelect: true,
        lang: datetimefield.attr('lang') || datetimefield.parents('[lang]').attr('lang')
    });
});

jQuery(document).on('ready page:change', function () {
    var datetimefield = jQuery('.timepicker');
    datetimefield.datetimepicker({
        datepicker: false,
        format: 'h:i:s A',
        closeOnDateSelect: true,
        lang: datetimefield.attr('lang') || datetimefield.parents('[lang]').attr('lang')
    });
});

jQuery(document).on('ready page:change', function () {
    var datetimefield = jQuery('.durationpicker');
    datetimefield.datetimepicker({
        datepicker: false,
        format: 'h:i',
        closeOnDateSelect: true,
        lang: datetimefield.attr('lang') || datetimefield.parents('[lang]').attr('lang')
    });
});