$(function () {
    //untuk bmi raws
    $('.change-container').on('click', function () {
        var raw = $(this).data('raw');
        $('#formModalLabel').html(raw);
        $('.modal-body form').attr('action', '/Rm/Update');
        $.ajax({
            url: '/Rm/Getdata',
            data: { raw_source: raw },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#raw_source').val(data.raw_source);
                $('#po').val(data.po);
                var date = new Date(data.etd);
                yr = date.getFullYear();
                month = date.getMonth() + 1;
                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                etd = yr + '-' + month + '-' + day;
                $('#etd').val(etd);
                var date = new Date(data.eta);
                yr = date.getFullYear();
                month = date.getMonth() + 1;
                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                eta = yr + '-' + month + '-' + day;
                $('#eta').val(eta);
                $('#container').val(data.container);
                $('#reff').val(data.reff);
                if (data.status == 'Plant') {
                    $('#status').val('Plant');
                } else if (data.status == 'Otw') {
                    $('#status').val('On The Water');
                } else {
                    $('#status').val('Closed');
                }
            }
        });
    });
    $('.change-item-detail').on('click', function () {
        $('#formModalLabel').html('Update Item');
        var id = $(this).data('id');
        console.log(id)
        $('.modal-body form').attr('action', '/Rm/Updatedetail');
        $.ajax({
            url: '/Rm/Getdetailitem',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#id_raw').val(data.id_raw);
                $('#sap_code').val(data.sap_code);
                $('#landing_site').val(data.landing_site);
                $('#usd_price').val(data.usd_price);
                $('#ex_rate').val(data.ex_rate);
                $('#qty_pl').val(data.qty_pl);
                $('#qty_received').val(data.qty_received);
                $('#landing_site_received').val(data.landing_site_received);
                $('#raw_source').val(data.raw_source);
            }
        });
    });

    $('.destroy-raw').on('click', function () {
        var id = $(this).data('id');
        $('.modal-body form').attr('action', '/Rm/Adddestroy');
        $.ajax({
            url: '/Rm/Getdetailitem',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#sap_code').val(data.sap_code);
                $('#raw_source').val(data.raw_source);
            }
        });
    });

    $('.duplicate-item').on('click', function () {
        $('#id_raw').val("");
        $('.modal-body form').attr('action', '/Rm/Duplicateitem');
    });


    

});
