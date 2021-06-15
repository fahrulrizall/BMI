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

    $(document).on('keyup', '.select2-search__field', function (e) {
        $('.dropdown-wrapper').html("");
        var material = $('.select2-search__field').val();
        $.ajax({
            url: '/PO/GetMaterial',
            data: { material: material, },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    $('.show-material').append("<option>" + e + "</option>");
                })
            }
        });
    })


    $(document).on('click', '.select-material', function () {
        $('.show-material').html("");
        $('#Material').val("").select2();
        var po = $(this).data('po');
        $.ajax({
            url: '/PO/SelectedMaterial',
            data: { po: po },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    $('.show-material').append("<option>" + e + "</option>");
                })
                $('#Material').val(data).select2()
            }
        });
    })

    $('.select2').select2();

    $('.update-material-ca').on('click', function () {
        var Id = $(this).data('id');
        var Code = $(this).data('code');
        $('.modal-title').html("Update Item " + Code);
        $('.modal-body form').attr('action', '/PO/UpdateTargetLBS');
        $.ajax({
            url: '/PO/GetTargetLBS',
            data: { id: Id  },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                console.log(data)
                $('#Target_Lbs').val(data)
            }
        });
    })

});