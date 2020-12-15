$(function () {
    $('.add-master-bmi').on('click', function () {
        $('.bmi-code').show();
        $('#bmi_code').val("").show();
        $('#sap_code').val("");
        $('#description').val("");
        $('#lbs').val("");
    });

    $('.update-master-bmi').on('click', function () {
        $('#formModalLabel').html('Update Item');
        var id = $(this).data('id');
        $('.bmi-code').hide();
        $('.modal-body form').attr('action', '/MasterBMI/Update');
        $.ajax({
            url: '/MasterBMI/Getmasterbmi',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#bmi_code').val(data.bmi_code).hide();
                $('#sap_code').val(data.sap_code);
                $('#description').val(data.description);
                $('#lbs').val(data.lbs);
                $('#daily_category').val(data.daily_category);
            }
        });
    });

});
