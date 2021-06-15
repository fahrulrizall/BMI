$('.add-new-cutting').on('click', function () {
    $('#Date').val("");
    $('#Vessel').html("");
    var reff = $(this).data('reff')
    $.ajax({
        url: '/Processing/GetVessel',
        data: { reff : reff },
        method: 'get',
        dataType: 'json',
        success: function (data) {
            data.forEach(function (e) {
                $('#Vessel').append("<option>" + e + "</option>");
            })

        }
    })
})

$('.update-date-vessel').on('click', function () {
    var reff = $(this).data('reff');
    var id = $(this).data('id');
    $('#Vessel').html("");
    $('.modal-body form').attr('action', '/Processing/UpdateDateVessel');
    $.ajax({
        url: '/Processing/GetVessel',
        data: { reff: reff },
        method: 'get',
        dataType: 'json',
        success: function (data) {
            $('#Id').val(id)
            data.forEach(function (e) {
                $('#Vessel').append("<option>" + e + "</option>");
            })
            
        }

    })

})



$('.add-output-line1').on('click', function () {
    $('#id').val("");
    $('#sap_code').val("");
    $('#qty').val("");
})

$('.update-output-item').on('click', function () {
    var id = $(this).data('id');
    $('.modal-body form').attr('action', '/Processing/UpdateItem');
    $.ajax({
        url: '/Processing/GetDetailOutput',
        data: { id: id },
        method: 'get',
        dataType: 'json',
        success: function (data) {
            console.log(data.id)
            $('#id').val(data.id)
            $('#sap_code').val(data.sap_code);
            $('#qty').val(data.qty)
        }

    })

})