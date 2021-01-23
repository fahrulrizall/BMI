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
    console.log("close")
    $('#detail-raw-table').attr('class', 'col-md-12');
    $('#detail-raw-material').hide();
})