$(function () {
    $('.change-repack').on('click', function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/Repack/Getitemrepack',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
               
                var repack = new Date(data.date);
                yr = repack.getFullYear();
                month = repack.getMonth() + 1;
                day = repack.getDate() < 10 ? '0' + repack.getDate() : repack.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                repack_date = yr + '-' + month + '-' + day;

                var production = new Date(data.production_date);
                yr = production.getFullYear();
                month = production.getMonth() + 1;
                day = production.getDate() < 10 ? '0' + production.getDate() : production.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                production_date = yr + '-' + month + '-' + day;
                    
                $('#id_repack').val(data.id_repack)
                $('#packing_date').val(repack_date)
                $('#po').val(data.po)
                $('#from_po').val(data.fromPOModel.pt)
                $('#production_date').val(production_date)
                $('#from_bmi_code').val(data.from_bmi_code)
                $('#qty').val(data.qty)
                $('#to_po').val(data.toPOModel.pt)
                $('#to_bmi_code').val(data.to_bmi_code)

            }
        })
    })

});