$(function () {

    $('.add-new-role').on('click', function () {
        $('.RoleName').show();
        $('#RoleName').val("");
        $('#Id').val("");
        //$('.user-list').hide();
        $('.modal-body form').attr('action', '/Administration/CreateRole');
    });

    $('.update-role').on('click', function () {
        $('#formModalLabel').html('Update Role');
        var id = $(this).data('id');
        //$('.user-list').hide();
        $('.RoleName').show();
        $('.modal-body form').attr('action', '/Administration/UpdateRole');
        $.ajax({
            url: '/Administration/Getrole',
            data: { id: id },
            method: 'get',
            dataType: 'json',
            success: function (data) {
                $('#Id').val(data.id);
                $('#RoleName').val(data.name);
            }
        });
    });


});
