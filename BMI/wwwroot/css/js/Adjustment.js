$(function () {

    $('.add-destroy-raw').on('click', function () {
        $('#id_adjustmentRaw').val("");
        $('#raw_source').val("");
        $('#sap_code').val("");
        $('#reason').val("");
        $('#qty').val("");
        $('.modal-body form').attr('action', '/Adjustment/AddDestroyRaw');
    })

    $('.update-destroy-raw').on('click', function () {
        $('#formModalLabel').html('Update Destory');
        var id = $(this).data('id');
        $('.modal-body form').attr('action', '/Adjustment/UpdateRaw');
        $.ajax({
            url: '/Adjustment/Getitemraw',
            data: { id: id, },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#id_adjustmentRaw').val(data.id_adjustmentRaw);
                $('#raw_source').val(data.raw_source);
                $('#sap_code').val(data.sap_code);
                $('#reason').val(data.reason);
                $('#qty').val(data.qty);
            }
        });

    })

});