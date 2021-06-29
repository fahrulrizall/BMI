$(function () {

    $('.add-master-data').on('click', function () {
        $('.sap-code').show();
        $('#sap_code').val("").show();
        $('#description').val("");
        $('#lbs').val("");
        $('#PF3770').val("");
    });

    $('.update-master-data').on('click', function () {
      
        var id = $(this).data('id');
        $('#formModalLabel').html('Update Item ' + id );
        $('.sap-code').hide();
        $('.modal-body form').attr('action', '/MasterData/Update');
        $.ajax({
            url: '/MasterData/Getmasterbmi',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#sap_code').val(data.sap_code).hide();
                $('#hers_code').val(data.hers_code);
                $('#description').val(data.description);
                $('#lbs').val(data.lbs);
                $('#standard_price').val(data.standard_price);
                $('#PF3770').val(data.pF3770);
                $('#PF3710').val(data.pF3710);
                $('#category').val(data.category);
            }
        });
    });


})
