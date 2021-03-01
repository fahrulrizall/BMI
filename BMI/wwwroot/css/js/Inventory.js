
$('#detail-fg-inventory').hide();
$('.detail-fg').on('click', function () {
    $('#fg-detail-batch').html("");
    var code = $(this).data('code');
    var sap_code = $(this).data('sap');
    $("#list-fg-inventory").attr('class', 'col-md-7');
    $('#detail-fg-inventory').show();
    $('#card-title-detail').html(sap_code);
    $.ajax({
        url: '/Inventory/EachDetailFG',
        data: { bmi_code: code },
        method: 'get',
        dataType: 'json',
        success: function (data) {
            data.forEach(function (e) {
                console.log(e)
                $('#fg-detail-batch').append("<tr><td class='text-sm'>" + sap_code + "</td><td class='text-sm'>" + e.poModel.batch + "</td><td class='text-sm'>" + e.total + "</td></tr>")
            })
        }
    });
})

$('.close-detail-fg').on('click', function () {
    $('#list-fg-inventory').attr('class', 'col-md-12');
    $('#detail-fg-inventory').hide();
})




$('#detail-raw-material').hide();
$('.detail-raw-material').on('click', function () {
    var raw_source = $(this).data('raw');
    $('#detail-raw-table').attr('class', 'col-md-6');
    $('#detail-raw-material').show();
    $('#card-title-detail').html(raw_source);
    $('.data-raw-material').html("");
    $.ajax({
        url: '/Inventory/EachDetailRaw',
        data: { raw_source: raw_source, },
        method: 'get',
        dataType: 'json',
        success: function (data) {
            data.forEach(function (e) {
                console.log(e)
                $('.data-raw-material').append("<tr><td class='text-sm'>" + e.sap_code + "</td><td class='text-sm'>" + e.masterdatamodel.description + "</td><td class='text-sm'>" + e.landing_site +"</td><td class='text-sm'>"+parseFloat(e.total).toFixed(2)+"</td> </tr>")
            })
        }
    });

})

$('.close-detail-raw-material').on('click', function () {
    $('#detail-raw-table').attr('class', 'col-md-12');
    $('#detail-raw-material').hide();
})