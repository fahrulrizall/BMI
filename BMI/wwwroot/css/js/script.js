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
        var id = $(this).data('id');
        $('.modal-body form').attr('action','/Rm/Update');
        $.ajax({
            url: '/Rm/Getdata',
            data: { id: id },
            method:'get',
            dataType : 'json',
            success: function (data) {
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
                if (data.status== 'in_plant'){
                    $('#status').val('In Plant');
                }else{
                    $('#status').val('On The Water');
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
                $('#sap_code').val(data.sap_code);
                $('#landing_site').val(data.landing_site);
                $('#usd_price').val(data.usd_price);
                $('#ex_rate').val(data.ex_rate);
                $('#qty_pl').val(data.qty_pl);
                $('#qty_received').val(data.qty_received);
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
    $('.add-shipment').on('click', function () {
        $('#formModalLabel').html('New Shipment');
        $('.modal-footer button[type=submit]').html('Create');
        $('.modal-body form').attr('action', 'Shipment/Create');
        $('.shipment-id-label').show();
        $('.shipment-id').attr('type', 'number');
        $('#id_ship').val("");
        $('#etd').val("");
        $('#eta').val("");
        $('#po').val("");
        $('#destination').val("");
    });
    $('.update-shipment').on('click', function () {
        $('.modal-footer button[type=submit]').html('Update');
        var id = $(this).data('id');
        $('#formModalLabel').html('Update Shipment '+id);
        $('.modal-body form').attr('action', 'Shipment/Update');
        $.ajax({
            url: '/Shipment/Getshipment',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('.shipment-id-label').hide();
                $('.shipment-id').attr('type', 'hidden');
                $('#id_ship').val(data.id_ship);
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
                $('#id_ship').val(data.id_ship);
                $('#qty').val(data.qty);
                $('#po').val(data.batch);
            }
        });
    });




    //table global pt
    $('.detail-pt-daily').on('click', function () {
        $('.table-production-daily').show();
        $("#master-pt-table").attr('class', 'col-md-8');
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
                    newdate = yr + '-' + month + '-' + day;
                    totaldaily = parseInt(e.total * 2.204 / e.masterBMIModel.lbs);
                    $('#detail-table-daily').append("<tr><td>" + newdate + "</td>" + "<td>" +  parseFloat(e.total).toFixed(2) + "</td>" + "<td class='case'>" + totaldaily + "</td></tr>");
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
                    $('#detail-raw-daily').append("<tr><td>"+e.raw_source+"</td>"+"<td>" + e.masterBMIModel.sap_code + "</td>" + "<td>" + e.masterBMIModel.description + "</td>" + "<td>" + e.landing_site + "</td>" + "<td class='qty'>" + e.qty + "</td></tr>");
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
                    $('#detail-fg-daily').append("<tr><td>" + e.masterBMIModel.sap_code + "</td>" + "<td>" + e.masterBMIModel.description + "</td>" + "<td class='qty-kg'>" + parseFloat(e.qty).toFixed(2) + "</td>" + "<td class='qty-case'>" + total_case + "</td></tr>");
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

    // untuk inventory
    //$(function () {
    //    $('.inventory-fg').hide();
    //    $('.inventory-raw').hide();
    //});

    //$('.select-inventory').change(function () {
    //    data = $(this).val();
    //    if (data == 'fg') {
    //        $('.inventory-fg').show();
    //        $.ajax({
    //            url: '/Inventory/GetInventoryFG',
    //            method: 'get',
    //            dataType: 'json',
    //            success: function (data) {
    //                data.forEach(function (e) {
    //                    each_case = parseInt(e.total);
    //                    $('#inventory-fg').append("<tr><td>" + e.ptModel.pt + "</td>" + "<td>" + e.masterBMIModel.sap_code + "</td>" + "<td>" + e.masterBMIModel.description + "</td>" + "<td>" + e.ptModel.batch + "</td>"+"<td>"+each_case+ "</td></tr>");
    //                })
    //            }
    //        });
    //    }
    //})


});  