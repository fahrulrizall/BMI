$(function () {


    $('.add-po').on('click', function () {
        $('.purchase-order').show();
        $('#po').val("");
        $('#pt').val("");
        $('#formModalLabel').html('Add New Purchase Order');
        $('.modal-body form').attr('action', '/PO/Create');
    });

    $('.update-po').on('click', function () {
        $('.purchase-order').hide();
        var po = $(this).data('po');
        $('#formModalLabel').html('Update PO ' + po);
        $('.modal-body form').attr('action', '/PO/Update');
        $.ajax({
            url: '/PO/GetPO',
            data: { po: po },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#po').val(data.po);
                $('#pt').val(data.pt);
            }
        });
    });


});