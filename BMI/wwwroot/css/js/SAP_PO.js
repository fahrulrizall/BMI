$(function () {
    //----------------------------- MOWI LINE ---------------------------------
    $('.change-refference-plant').on('click', function () {
        var refference = $(this).data("reffrence")
        console.log(refference)
        $('#formModalLabel').html(refference);
        $('.modal-body form').attr('action', '/SAP_PO/Update');
        $.ajax({
            url: '/SAP_PO/GetData',
            data: { refference: refference },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#refference').val(data.refference);
                $('#vendor').val(data.vendor);
                $('#container').val(data.container);
                $('#bl_no').val(data.bl_no);
                $('#sap_po').val(data.sap_po);
                $('#pgi').val(data.pgi);
                $('#pgr').val(data.pgr);
                $('#return_no').val(data.return_no);
                $('#shipping_line').val(data.shipping_line);
                $('#loading_port').val(data.loading_port);
                $('#destination').val(data.destination);

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

                var date = new Date(data.pgr_date);
                yr = date.getFullYear();
                month = date.getMonth() + 1;
                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                pgr_date = yr + '-' + month + '-' + day;
                $('#pgr_date').val(pgr_date);
                if (data.status == 'Plant') {
                    $('#status').val('Plant');
                } else if (data.status == 'Otw') {
                    $('#status').val('On The Water');
                } else {
                    $('#status').val('Closed');
                }


            }
        });
    })


    $('.change-item-detail-plant').on('click', function () {
        var id = $(this).data("id")
        $('.modal-body form').attr('action', '/SAP_PO/UpdateDetail');
        $.ajax({
            url: '/SAP_PO/GetDetail',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#id_rmmowidetail').val(data.id_rmmowidetail)
                $('#sap_code').val(data.sap_code);
                $('#style').val(data.style);
                $('#vessel').val(data.vessel);
                $('#unit_price').val(data.unit_price);
                $('#qty_pl').val(data.qty_pl);
                $('#qty_received').val(data.qty_received);
            }
        })
    })

})