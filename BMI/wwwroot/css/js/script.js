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
        $("#example3").DataTable({
            "aaSorting": [],
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


    //$('.user-role').on('click', function () {
    //    $('#formModalLabel').html('Update User Role');
    //    $('.RoleName').hide();
    //    $('#RoleName').hide();
    //    $('.user-list').show();
    //    $('.user-list').html("");
    //    var id = $(this).data('id');
    //    $('.modal-body form').attr('action', '/Administration/UpdateUserRole');
    //    $.ajax({
    //        url: '/Administration/Getuserrole',
    //        data: { id: id },
    //        method: 'get',
    //        dataType: 'json',
    //        success: function (data) {
    //            var num = ((data).length)
    //            for (i = 0; i < num; i++) {
    //                $('.user-list').append("<div class='custom-control custom-checkbox'><input class='form-check-input' type='checkbox' id=" + data[i].userName + "><label for=" + data[i].userName + " class='form-check-label'>" + data[i].userName + "</label></div>");
    //                console.log(i, data[i].userName);
    //            }
    //        }
    //    });
    //});
   









});  