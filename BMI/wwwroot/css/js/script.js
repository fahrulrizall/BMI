$(function () {

    $(function () {
        $("#example1").DataTable({
            "aaSorting": [],
        });
        $('#example2').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": false,
            "ordering": true,
            "aaSorting": [],
            "info": true,
            "autoWidth": false,
        });
    });
    

    //untuk fgs
    $('.add-new-fg').on('click',function () {
        $('#formModalLabel').html('Add New Item');
        $('.modal-body form').attr('action', 'Fg/Create');
        $('#id_fg').val("");
        $('#sap_code').val("");
        $('#price_lbs').val("");
        $('#std_price').val("");
        $('#processing_fee').val("");
    });
    $('.update-fg').on('click',function () {
        $('#formModalLabel').html('Update Item');
        var id = $(this).data('id');
        $('.modal-body form').attr('action', 'Fg/Update');
        $.ajax({
            url: '/Fg/Getdata',
            data: {id : id},
            method:'get',
            dataType : 'json',
            success: function (data) {
                $('#id_fg').val(data.id_fg);
                $('#sap_code').val(data.sap_code);
                $('#price_lbs').val(data.price_lbs);
                $('#std_price').val(data.std_price);
                $('#processing_fee').val(data.processing_fee);
            }
        });
    });

    $('.nav-link').on('click', function(){
        $('.nav-link').removeClass('active');
        $(this).addClass('active');
    });

    //untuk macs
    //$('.tombolTambahDataMacs').on('click',function () {
    //    $('#formModalLabel').html('Add New Item');
    //    $('.modal-footer button[type=submit]').html('Add');
    //    $('.modal-body form').attr('action','/macs');
    //    $('#sap_code').val("");
    //    $('#mac').val("");
    //});
    //$('.tampilModalUbahMacs').on('click',function () {
    //    $('#formModalLabel').html('Update Item');
    //    $('.modal-footer button[type=submit]').html('Update');
    //    var id = $(this).data('id');
    //    $('.modal-body form').attr('action','/macs/'+id+'/update');
    //    $.ajax({
    //        url: '/macs/getubah',
    //        data: {id : id},
    //        method:'get',
    //        dataType : 'json',
    //        success: function (data) {
    //            $('#sap_code').val(data.sap_code);
    //            $('#mac').val(data.mac);
    //        }
    //    });
    //});


    //untuk pts
    //$('.tampilModalUbahPts').on('click',function () {
    //    var id = $(this).data('id');
    //    $('.modal-body form').attr('action','/pts/'+id+'/update');
    //    $.ajax({
    //        url: '/ptspt/getubah',
    //        data: {id : id},
    //        method:'get',
    //        dataType : 'json',
    //        success: function (data) {
    //            $('#lbs').val(data.lbs);
    //            $('.loincode').each(function( index ) {
    //                $(this).attr("checked",(data.loin && data.loin.filter(x=>(x==$(this).val())).length) ? true : false);
    //            });
    //        }
    //    });
    //});
    

     //$("#no_pt, #plant").on("keyup", function(){
     //    $("#validatept").val($("#no_pt").val() + $("#plant").val());
     //});

    //untuk packagings
    //$('.tombolTambahDataPackagings').on('click',function () {
    //    $('#formModalLabel').html('Add New Item');
    //    $('.modal-footer button[type=submit]').html('Add');
    //    $('.modal-body form').attr('action','/packagings');
    //    $('#month').val("");
    //    $('#lab').val("");
    //    $('#ofc').val("");
    //    $('#expenses').val("");
    //    $('#packaging').val("");
    //    $('#lbs').val("");
    //    $('#other').val("");
    //});
    //$('.tampilModalUbahPackaging').on('click',function () {
    //    $('#formModalLabel').html('Update Item');
    //    $('.modal-footer button[type=submit]').html('Update');
    //    var id = $(this).data('id');
    //    $('.modal-body form').attr('action','/packagings/'+id+'/update');
    //    $.ajax({
    //        url: '/packagings/getubah',
    //        data: {id : id},
    //        method:'get',
    //        dataType : 'json',
    //        success: function (data) {
    //            $('#month').val(data.month);
    //            $('#lab').val(data.lab);
    //            $('#ofc').val(data.ofc);
    //            $('#expenses').val(data.expenses);
    //            $('#packaging').val(data.packaging);
    //            $('#lbs').val(data.lbs);
    //            $('#other').val(data.other);
    //        }
    //    });
    //});

    //$('.formProfile').on('click',function () {
    //    $('#formModalLabel').html('Update Item');
    //    $('.modal-footer button[type=submit]').html('Update');
    //    var id = $(this).data('id');
    //    $('.modal-body form').attr('action','/profile/'+id+'/update');
    //    $.ajax({
    //        url: '/profileuser/getuser',
    //        data: {id : id},
    //        method:'get',
    //        dataType : 'json',
    //        success: function (data) {
    //            $('#name').val(data.name);
    //            $('#email').val(data.email);
    //            $('#departement').val(data.departement);
    //            $('#position').val(data.position);
    //            $('#education').val(data.education);
    //            $('#address').val(data.address);
    //            $('#avatar').val(data.avatar);
    //        }
    //    });
    //});

    $(document).ready(function () {
        bsCustomFileInput.init();
    });

      //untuk bmi raws
    $('.change-container').on('click',function () {
        var raw = $(this).data('raw');
        $('#formModalLabel').html(raw);
        $('.modal-body form').attr('action','/Rm/Update');
        $.ajax({
            url: '/Rm/Getdata',
            data: { raw_source: raw },
            method:'get',
            dataType : 'json',
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
                if (data.status == 'in_plant') {
                    $('#status').val('In Plant');
                } else if (data.status == 'otw') {
                    $('#status').val('On The Water');
                } else {
                    $('#status').val('Closed');
                }
            }
        });
    });
    $('.change-item-detail').on('click',function () {
        $('#formModalLabel').html('Update Item');
        var id = $(this).data('id');
        $('.modal-body form').attr('action','/Rm/Updatedetail');
        $.ajax({
            url: '/Rm/Getdetailitem',
            data: {id : id},
            method:'get',
            dataType : 'json',
            success: function (data) {
                $('#id_raw').val(data.id_raw);
                $('#sap_code').val(data.sap_code);
                $('#landing_site').val(data.landing_site);
                $('#usd_price').val(data.usd_price);
                $('#ex_rate').val(data.ex_rate);
                $('#qty_pl').val(data.qty_pl);
                $('#qty_received').val(data.qty_received);
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

    $('.upload-gi').on('click',function () {
        $('#formModalLabel').html('Upload GI');
        $('.modal-body form').attr('action','/Production/ImportGI');
    });
    $('.upload-gr').on('click',function () {
        $('#formModalLabel').html('Upload GR');
        $('.modal-body form').attr('action','/Production/ImportGR');
    });


    // untuk shipment
    //$('.add-shipment').on('click', function () {
    //    $('#formModalLabel').html('New Shipment');
    //    $('.modal-footer button[type=submit]').html('Create');
    //    $('.modal-body form').attr('action', 'Shipment/Create');
    //    $('.shipment-no-label').show();
    //    $('.shipment-no').attr('type', 'number');
    //    $('#id_shipment').val("");
    //    $('#po').val("");
    //    $('#etd').val("");
    //    $('#eta').val("");
    //    $('#document_date').val("");
    //    $('#ocean_carrier').val("");
    //    $('#container').val("");
    //    $('#voyage_no').val("");
    //    $('#house_bol').val("");
    //    $('#vessel_name').val("");
    //    $('#inv_no').val("");
    //    $('#fda_no').val("");
    //    $('#seal_no').val("");
    //    $('#destination').val("");
    //    $('#updated_at').val("");
    //});
    $('.update-shipment').on('click', function () {
        $('.modal-footer button[type=submit]').html('Update');
        var po = $(this).data('po');
        $('#formModalLabel').html('Update Shipment -'+po);
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
            }
        });
    });

    $('.update-item-shipment').on('click', function () {
        $('.modal-footer button[type=submit]').html('Update');
        var id = $(this).data('id');
        $('#formModalLabel').html('Update Item ');
        $('.modal-body form').attr('action', '/Shipment/Updateitem');
        $.ajax({
            url: '/Shipment/Getitem',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#id_shipment_detail').val(data.id_shipment_detail);
                $('#qty').val(data.qty);
                $('#po').val(data.batch);
            }
        });
    });


    //table global pt
    $('.detail-pt-daily').on('click', function () {
        $('.table-production-daily').show();
        $("#master-pt-table").attr('class', 'col-md-7');
        var code = $(this).data('code');
        var sap = $(this).data('sap');
        $('#card-title').html(sap);
        var pt = $(this).data('pt');
        $('#detail-table-daily').html("");
        $.ajax({
            url: '/Production/Detailperitem',
            data: { pt: pt,code:code },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    var date = new Date(e.date);
                    yr = date.getFullYear();
                    month = date.getMonth() + 1;
                    day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                    if (month < 10) {
                        month = "0" + month;
                    }
                    newdate = day + '-' + month + '-' + yr;
                    totaldaily = parseInt(e.qty * 2.204 / e.masterBMIModel.lbs);
                    $('#detail-table-daily').append("<tr><td style='font-size:small'>" + newdate + "</td>" + "<td style='font-size:small'>" + e.raw_source + "</td>" + "<td style='font-size:small'>" + e.landing_site + "</td>" + "<td style='font-size:small'>" + parseFloat(e.qty * 2.204).toFixed(2) + "</td>" + "<td class='case' style='font-size:small'>" + totaldaily + "</td></tr>");
                    calc_case();
                })  
            }
        });
        function calc_case() {
            var sum = 0;
            $('.case').each(function () {
                sum += parseInt($(this).text())
            });
            $('#total').text(sum);
        };
    });

    $('.close-production-daily').on('click', function () {
        $('.table-production-daily').hide();
        $("#master-pt-table").attr('class', 'col-md-12');
    });

    $(function () {
        $('.table-production-daily').hide();
    });

    // table production by daily
    $(function () {
        $('.table-raw-daily').hide();
        $('.table-fg-daily').hide();
    });



    $('.detail-po-daily').on('click', function () {
        $('.table-raw-daily').show();
        $('.table-fg-daily').show();
        var po = $(this).data('po');
        var date = $(this).data('date');
        $('#detail-raw-daily').html("");
        $('#detail-fg-daily').html("");
        $.ajax({
            url: '/Production/RawMaterial',
            data: { po: po, date: date },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    console.log(e);
                    $('#detail-raw-daily').append("<tr><td style='font-size:small'>" + e.raw_source + "</td>" + "<td style='font-size:small'>" + e.sap_code + "</td>" + "<td style='font-size:small'>" + e.masterdatamodel.description + "</td>" + "<td style='font-size:small'>" + e.landing_site + "</td>" + "<td class='qty' style='font-size:small'>" + e.qty + "</td></tr>");
                    total_kg();
                })
            }
        });
        function total_kg() {
            var sum = 0;
            $('.qty').each(function () {
                sum += parseInt($(this).text())
            });
            $('#total-kg-raw').text(sum);
        };

        $.ajax({
            url: '/Production/FinishedGood',
            data: { po: po, date: date },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    total_case = parseInt(e.qty * 2.204 / e.masterBMIModel.lbs);
                    $('#detail-fg-daily').append("<tr><td style='font-size:small'>" + e.masterBMIModel.sap_code + "</td>" + "<td style='font-size:small'>" + e.masterBMIModel.description + "</td>" + "<td class='qty-kg' style='font-size:small'>" + parseFloat(e.qty).toFixed(2) + "</td>" + "<td class='qty-case' style='font-size:small'>" + total_case + "</td></tr>");
                    calc_total_fg();
                    calc_total_case();
                })
            }
        });
        function calc_total_fg() {
            var sum = 0;
            $('.qty-kg').each(function () {
                sum += parseFloat($(this).text())
            });
            sum = parseFloat(sum).toFixed(2);
            $('#total-kg-fg').html(sum);
        };
        function calc_total_case() {
            var sum = 0;
            $('.qty-case').each(function () {
                sum += parseInt($(this).text())
            });
            $('#total-cs-fg').html(sum);
        };


    });

    // adjust dan repack
    $('.adjustment-fg').on('click', function () {
        console.log("dasdasd")
        $('#formModalLabel').html('Adjustment');
        var pt = $(this).data('pt');
        var code = $(this).data('code');
        $('#qty').val("");
        $('.destination-pt').hide();
        $('.destination-code').hide();
        $('.production-date').hide();
        $('.po').hide();
        $('.date').hide();
        $('.raw-source').hide();
        $('.modal-body form').attr('action', '/Production/Adjustment');
        $.ajax({
            url: '/Production/Getitemdata',
            data: { pt: pt, code: code },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    $('#pt').val(pt);
                    $('#bmi_code').val(e.bmi_code)
                })
            }
        });
    });

    $('.repack-fg').on('click', function () {
        $('#formModalLabel').html('Repack Item');
        $('.destination-pt').show();
        $('.destination-code').show();
        $('.raw-source').show();
        $('.production-date').show();
        $('.date').show();
        $('.po').show();
        var pt = $(this).data('pt');
        var code = $(this).data('code');
        $('#qty').val("");
        $('#destination_pt').val("");
        $('#to_bmi_code').val("");
        $('#date').html("");
        $('#production_date').html("");
        $('#raw_source').html("");
        $('.modal-body form').attr('action', '/Production/Repack');
        $.ajax({
            url: '/Production/Getitemdata',
            data: { pt: pt, code: code },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    var date = new Date(e.date);
                    yr = date.getFullYear();
                    month = date.getMonth() + 1;
                    day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                    if (month < 10) {
                        month = "0" + month;
                    }
                    newdate = day + '-' + month + '-' + yr;
                    $('#production_date').append("<option>" + newdate + "</option>");
                    $('#raw_source').append("<option>" + e.raw_source + "</option>");
                    $('#bmi_code').val(e.bmi_code)
                })
            }
        });

    });


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
            data: { id: id,},
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


   


    $('.repack-table-date').on('change', function () {
        $('#repack-table').html("")
        var date = $(this).val()
        $.ajax({
            url: '/Repack/ByDate',
            data: { date : date },
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
                    $('#repack-table').append("<tr><td style='font-size:small'>" + repack_date +"</td><td style='font-size:small'>"+ pdc_date+"</td><td style='font-size:small'>" + e.fromPOModel.pt + "</td><td style='font-size:small'>" + e.fromMasterBMIModel.sap_code + "</td><td style='font-size:small'>" + e.fromMasterBMIModel.description + "</td><td style='font-size:small'>" + e.qty + "</td><td style='font-size:small'>" + e.toPOModel.pt + "</td><td style='font-size:small'>" + e.toMasterBMIModel.sap_code + "</td><td style='font-size:small'>" + e.toMasterBMIModel.description + "</td><td style='font-size:small'>" + e.qty + "</td><td>" +
                        "<form action='/Repack/Delete?id="+e.id_repack+"' method='post'>" +
                            "<a data-toggle='modal' data-target='#modal-default' data-id="+e.id_repack+" class='btn btn-sm btn-warning change-repack-modal' style='font-size:small' >Update</a>" +
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

    $('.change-pt').on('click', function () {
        var po = $(this).data('po');
        $.ajax({
            url: '/Production/Getptdata',
            data: { po:po },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#po').val(data.po);
                $('#plant').val(data.plant);
                $('#batch').val(data.batch);
                $('#status').val(data.status);
            }
        });
    })



    









});  