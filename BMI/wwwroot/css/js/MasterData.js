$(function () {

    $('.add-master-data').on('click', function () {
        $('.sap-code').show();
        $('#sap_code').val("").show();
        $('#description').val("");
        $('#lbs').val("");
    });

    $('.update-master-data').on('click', function () {
        $('#formModalLabel').html('Update Item');
        var id = $(this).data('id');
        $('.sap-code').hide();
        $('.modal-body form').attr('action', '/MasterData/Update');
        $.ajax({
            url: '/MasterData/Getmasterbmi',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#sap_code').val(data.sap_code).hide();
                $('#description').val(data.description);
                $('#lbs').val(data.lbs);
                $('#category').val(data.category);
            }
        });
    });


})
