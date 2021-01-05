$(function () {

    $('.update-shipment').on('click', function () {
        $('.modal-footer button[type=submit]').html('Update');
        var po = $(this).data('po');
        $('#formModalLabel').html('Update Shipment -' + po);
        $('.modal-body form').attr('action', 'Shipment/Update');
        $('#etd').val("");
        $('#eta').val("");
        $('#document_date').val("");
        $.ajax({
            url: '/Shipment/Getshipment',
            data: { po: po },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#shipment_no').val(data.shipment_no);
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
                var date = new Date(data.document_date);
                yr = date.getFullYear();
                month = date.getMonth() + 1;
                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                document_date = yr + '-' + month + '-' + day;
                $('#document_date').val(document_date);
                $('#ocean_carrier').val(data.ocean_carrier);
                $('#container').val(data.container);
                $('#voyage_no').val(data.voyage_no);
                $('#house_bol').val(data.house_bol);
                $('#vessel_name').val(data.vessel_name);
                $('#inv_no').val(data.inv_no);
                $('#fda_no').val(data.fda_no);
                $('#seal_no').val(data.seal_no);
                $('#destination').val(data.destination);
                $('#status').val(data.po_status);
            }
        });
    });


    $('.shipment-item-detail').on('click', function () {
        $('#shipment-table').attr('class', 'col-md-7');
        $('.detail-pdc').show();
        var po = $(this).data('po');
        var code = $(this).data('code');
        var batch = $(this).data('batch');
        $('#table-detail-pdc').html("");
        $.ajax({
            url: '/Shipment/Getpdc',
            data: { po: po, batch: batch, code: code },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    console.log(e);
                    var date = new Date(e.pdc);
                    yr = date.getFullYear();
                    month = date.getMonth() + 1;
                    day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                    if (month < 10) {
                        month = "0" + month;
                    }
                    newdate = day + '-' + month + '-' + yr;
                    $('#table-detail-pdc').append("<tr><td style='font-size:small'>" + newdate + "</td>" + "<td style='font-size:small' >" + e.raw_source + "</td>" + "<td style='font-size:small' class='case'>" + e.qty + "</td></tr>")
                    calc_case();
                })

                function calc_case() {
                    var sum = 0;
                    $('.case').each(function () {
                        sum += parseInt($(this).text())
                    });
                    $('#total').text(sum);
                };
            }
        })
    });

    $(function () {
        $('.detail-pdc').hide();
    });

    $('.close-detail-pdc').on('click', function () {
        $('#shipment-table').attr('class', 'col-md-12');
        $('.detail-pdc').hide();
    })


    //$('.update-item-shipment').on('click', function () {
    //    $('.modal-footer button[type=submit]').html('Update');
    //    var batch = $(this).data('batch');
    //    var id = $(this).data('id');
    //    var code = $(this).data('code');
    //    var lbs = $(this).data('lbs');
    //    $('#formModalLabel').html('Update Item ');
    //    $('.modal-body form').attr('action', '/Shipment/Updateitem');
    //    $('.pdc').html("");
    //    $.ajax({
    //        url: '/Shipment/Getproduction',
    //        data: { batch: batch, code:code , lbs:lbs },
    //        method: 'get',
    //        dataType: 'json',
    //        success: function (data) {
    //            data.forEach(function (e) {
    //                console.log(e)
    //                var date = new Date(e.date);
    //                yr = date.getFullYear();
    //                month = date.getMonth() + 1;
    //                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
    //                if (month < 10) {
    //                    month = "0" + month;
    //                }
    //                pdc = day + '-' + month + '-' + yr;
    //                $('.pdc').append("<div class='custom-control custom-checkbox'><input class='form-check-input' type='checkbox' id=" + pdc + "><label for=" + pdc + " class='form-check-label'>" + pdc +"-"+ e.raw_source +"</label>    <input type='number' class='form - control - sm' id='qty' name='qty' value="+e.cases+">  </div>");
    //                $('#id_shipment').val(id);
    //            })
    //        }
    //    });
    //});
});
