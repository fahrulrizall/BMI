$(function () {
    $('.repack-table-date').on('change', function () {
        $('#repack-table').html("")
        var date = $(this).val()
        $.ajax({
            url: '/Repack/ByDate',
            data: { date: date },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    var repack = new Date(e.date);
                    yr = repack.getFullYear();
                    month = repack.getMonth() + 1;
                    day = repack.getDate() < 10 ? '0' + repack.getDate() : repack.getDate();
                    if (month < 10) {
                        month = "0" + month;
                    }
                    repack_date = day + '-' + month + '-' + yr;

                    var pdc = new Date(e.production_date);
                    yr = pdc.getFullYear();
                    month = pdc.getMonth() + 1;
                    day = pdc.getDate() < 10 ? '0' + pdc.getDate() : pdc.getDate();
                    if (month < 10) {
                        month = "0" + month;
                    }
                    pdc_date = day + '-' + month + '-' + yr;
                    $('#repack-table').append("<tr><td style='font-size:small'>" + repack_date + "</td><td style='font-size:small'>" + pdc_date + "</td><td style='font-size:small'>" + e.fromPOModel.pt + "</td><td style='font-size:small'>" + e.fromMasterBMIModel.sap_code + "</td><td style='font-size:small'>" + e.fromMasterBMIModel.description + "</td><td style='font-size:small'>" + e.qty + "</td><td style='font-size:small'>" + e.toPOModel.pt + "</td><td style='font-size:small'>" + e.toMasterBMIModel.sap_code + "</td><td style='font-size:small'>" + e.toMasterBMIModel.description + "</td><td style='font-size:small'>" + e.qty + "</td><td>" +
                        "<form action='/Repack/Delete?id=" + e.id_repack + "' method='post'>" +
                        "<a data-toggle='modal' data-target='#modal-default' data-id=" + e.id_repack + " class='btn btn-sm btn-warning change-repack-modal' style='font-size:small' >Update</a>" +
                        "<button type='submit'  onclick='return confirm('Delete Item Repack?');'   class='btn btn-sm btn-danger'  style='font-size:small'>Delete</button>" +
                        "</form>"
                        + "</td></tr>");
                    $('.change-repack-modal').on('click', function () {
                        var id = $(this).data('id');
                        $.ajax({
                            url: '/Repack/Getitemrepack',
                            data: { id: id },
                            method: 'get',
                            dataType: 'json',
                            success: function (data) {
                                var date = new Date(data.date);
                                yr = date.getFullYear();
                                month = date.getMonth() + 1;
                                day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                                if (month < 10) {
                                    month = "0" + month;
                                }
                                newdate = yr + '-' + month + '-' + day;
                                $('#id_repack').val(data.id_repack);
                                $('#date').val(newdate);
                                $('#po').val(data.po);
                                $('#from_po').val(data.fromPOModel.pt);
                                $('#from_bmi_code').val(data.from_bmi_code);
                                $('#qty').val(data.qty);
                                $('#to_bmi_code').val(data.to_bmi_code);
                                $('#to_po').val(data.toPOModel.pt);
                            }
                        });

                    })
                })
            }
        })
    })

});