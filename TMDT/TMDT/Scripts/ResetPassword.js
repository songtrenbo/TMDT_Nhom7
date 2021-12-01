function ConfirmResetPassword(username, controller, actionName) {
    Swal.fire({
        title: 'Xác nhận khôi phục mật khẩu mặc định của  ' + username + '?',
        text: "Bạn sẽ không thể khôi phục lại hành động này!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, reset it!'
    }).then((result) => {
        if (result.isConfirmed) {
            ResetCall(username, actionName, controller);
        }
    })
}

function ResetCall(username, actionName, controller) {
    $.ajax({
        type: "POST",
        url: '/' + controller + '/' + actionName,
        data: { username: username },
        //dataType: "string",
        success: function () {
            Swal.fire({
                icon: 'success',
                title: 'Đã khôi phục!',
                text: 'Đã khôi phục mật khẩu mặc định!',
                showConfirmButton: false,
                timer: 1500
            })
            location.reload();
        },
        error: function () {
            Swal.fire({
                icon: 'warning',
                title: 'Không thể khôi phục!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    });
}