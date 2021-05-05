$(function () {
    //table global pt
    $('.detail-pt-daily').on('click', function () {
        $('.table-production-daily').show();
        $("#master-pt-table").attr('class', 'col-md-7');
        var sap = $(this).data('sap');
        var bmicode = $(this).data('bmicode');
        $('#card-title').html(sap);
        var po = $(this).data('po');
        $('#detail-table-daily').html("");
        $.ajax({
            url: '/Production/Detailperitem',
            data: { po: po, bmicode: bmicode },
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
                    pdc = yr + '-' + month + '-' + day;
                    $('#detail-table-daily').append("<tr><td style='font-size:small'>" + newdate + "</td>" + "<td style='font-size:small' class='btn check-landing-site " + "site" + pdc + "-" + e.raw_source + "'  data-po=" + po + " data-code=" + bmicode + "  data-date=" + pdc + " data-source=" + e.raw_source + " >" + e.raw_source + "</td>" + "<td style='font-size:small' class='casetotal qty" + pdc + "-" + e.raw_source +"'  >" + parseInt(e.cases) + "</td>" + "<td class='caseavailable' style='font-size:small'>" + parseInt( e.available) + "</td></tr>");
                    calc_case_total();
                    calc_case_available();
                })

                $('.check-landing-site').on('click', function () {
                    var pdc = $(this).data('date');
                    var raw_source = $(this).data('source');
                    $('.landing-site').remove();
                    $.ajax({
                        url: '/Production/Detaillandingsite',
                        data: { po: po, bmicode: bmicode, pdc: pdc, raw_source: raw_source },
                        method: 'get',
                        dataType: 'json',
                        success: function (data) {
                            data.forEach(function (e) {
                                $('.site' + pdc + "-" + raw_source).append("<tr class='landing-site'><td class='text text-sm' >" + e.landing_site + "</td></tr>")
                                $('.qty' + pdc + "-" + raw_source).append("<tr class='landing-site'><td class='text text-sm' >" + e.cases + "</td></tr>")
                            })

                        }
                    })
                })

            }
        });
        function calc_case_total() {
            var sum = 0;
            $('.casetotal').each(function () {
                sum += parseInt($(this).text())
            });
            $('#totalcase').text(sum);
        };

        function calc_case_available() {
            var sum = 0;
            $('.caseavailable').each(function () {
                sum += parseInt($(this).text())
            });
            $('#totalavailable').text(sum);
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


    $('.upload-gi').on('click', function () {
        $('#formModalLabel').html('Upload GI');
    });
    $('.upload-gr').on('click', function () {
        $('#formModalLabel').html('Upload GR');
    });


    $('.change-pt').on('click', function () {
        var po = $(this).data('po');
        $.ajax({
            url: '/Production/Getptdata',
            data: { po: po },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#po').val(data.po);
                $('#plant').val(data.plant);
                $('#batch').val(data.batch);
                $('#pt_status').val(data.pt_status);
            }
        });
    })

    $('.detail-po-daily').on('click', function () {
        $('.table-raw-daily').show();
        $('.table-fg-daily').show();
        var po = $(this).data('po');
        var date = $(this).data('date');
        $('#detail-raw-daily').html("");
        $('#detail-fg-daily').html("");
        $.ajax({
            url: '/Production/AllMaterial',
            data: { po: po, date: date },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.productionInputModel.forEach(function (e) {
                    $('#detail-raw-daily').append("<tr><td style='font-size:small'>" + e.raw_source + "</td>" + "<td style='font-size:small'>" + e.sap_code + "</td>" + "<td style='font-size:small'>" + e.masterdatamodel.description + "</td>" + "<td style='font-size:small'>" + e.landing_site + "</td>" + "<td class='qty' style='font-size:small'>" + e.qty + "</td></tr>");
                    total_kg();
                })
                data.productionOutputModel.forEach(function (e) {
                    total_case = parseInt(e.qty * 2.204 / e.masterBMIModel.lbs);
                    $('#detail-fg-daily').append("<tr><td style='font-size:small'>" + e.masterBMIModel.sap_code + "</td>" + "<td style='font-size:small'>" + e.masterBMIModel.description + "</td>" + "<td class='qty-kg' style='font-size:small'>" + parseFloat(e.qty).toFixed(2) + "</td>" + "<td class='qty-case' style='font-size:small'>" + total_case + "</td></tr>");
                    calc_total_fg();
                    calc_total_case();
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
        $('#formModalLabel').html('Adjustment');
        $('#qty_label').html('Qty (CS)');
        var po = $(this).data('po');
        var code = $(this).data('code');
        $('#qty').val("");
        $('.destination-pt').hide();
        $('.destination-code').hide();
        $('.production-date').hide();
        $('.fairtrade-status').hide();
        $('.po').hide();
        $('.date').hide();
        $('.raw-source').hide();
        $('.landing-site-input').hide();
        $('.reason').hide();
        $('.modal-body form').attr('action', '/Production/Adjustment');
        $.ajax({
            url: '/Production/Getitemdata',
            data: { po: po, code: code },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                data.forEach(function (e) {
                    $('#po').val(po);
                    $('#bmi_code').val(e.bmi_code)
                })
            }
        });
    });

    $('.repack-fg').on('click', function () {
        var sap = $(this).data('sap');
        $('#formModalLabel').html('Repack Item ' + sap);
        $('.destination-pt').show();
        $('.destination-code').show();
        $('.raw-source').show();
        $('.production-date').show();
        $('.fairtrade-status').show();
        $('.date').show();
        $('.po').show();
        $('.landing-site-input').show();
        $('.reason').hide();
        $('#qty_label').html('Qty (Kg)');
        var po = $(this).data('po');
        var code = $(this).data('code');
        $('#qty').val("");
        $('#destination_pt').val("");
        $('#to_bmi_code').val("");
        $('#date').html("");
        $('#production_date').html("");
        $('#raw_source').html("");
        //$('#fairtrade_status').html("");
        $('#landing_site').html("");
        $('.modal-body form').attr('action', '/Production/Repack');
        $.ajax({
            url: '/Production/Getitemdata',
            data: { po: po, code: code },
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
                    $('#production_date').append("<option>" + newdate + "</option>");
                    $('#raw_source').append("<option>" + e.raw_source + "</option>");
                    $('#landing_site').append("<option>" + e.landing_site + "</option>");
                    $('#bmi_code').val(e.bmi_code);
                    //$('#fairtrade_status').val(e.fairtrade_status);
                })
            }
        });

    });

    $('.destroy-fg').on('click', function () {
        //var sap = $(this).data('sap');
        $('#formModalLabel').html('Destroy Item ');
        $('#qty_label').html('Qty (CS)');
        $('.destination-pt').hide();
        $('.destination-code').hide();
        $('.raw-source').show();
        $('.production-date').show();
        $('.fairtrade-status').show();
        $('.reason').show();
        $('.date').hide();
        $('.po').hide();
        $('.landing-site-input').show();
        var po = $(this).data('po');
        var code = $(this).data('code');
        $('#qty').val("");
        $('#production_date').html("");
        $('#raw_source').html("");
        $('#landing_site').html("");
        $('#reason').html("");
        $('.modal-body form').attr('action', '/Production/Destroy');
        $.ajax({
            url: '/Production/Getitemdata',
            data: { po: po, code: code },
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
                    $('#production_date').append("<option>" + newdate + "</option>");
                    $('.production_date').on('change', function () {
                    })
                    //$('#raw_source').append("<option>" + e.raw_source + "</option>");
                    //$('#landing_site').append("<option>" + e.landing_site + "</option>");
                    $('#bmi_code').val(e.bmi_code)
                    
                })
                
            }
        });

    });





});