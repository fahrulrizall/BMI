$('.otw').hide();
$('.plant').hide();
$('.fg').hide();
hide();

function hide() {
    $('.otw-show').on('click', function () {
        $('.otw').show();
        $('.otw-show').attr('id', 'otw-hide');
        $('#otw-hide').on('click', function () {
            $('.otw').hide();
            hide();
        })
    })

    $('.plant-show').on('click', function () {
        $('.plant').show();
        $('.plant-show').attr('id', 'plant-hide');
        $('#plant-hide').on('click', function () {
            $('.plant').hide();
            hide();
        })
    })

    $('.fg-show').on('click', function () {
        $('.fg').show();
        $('.fg-show').attr('id', 'fg-hide');
        $('#fg-hide').on('click', function () {
            $('.fg').hide();
            hide();
        })
    })



}



