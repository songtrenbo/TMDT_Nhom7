﻿function ConfirmXoaThuongHieu(id) {
    Swal.fire({
        title: 'Xác nhận xóa thương hiệu?',
        text: "Bạn sẽ không thể khôi phục lại hành động này!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteCall(id);
        }
    })
}

function DeleteCall(id) {
    $.ajax({
        type: "POST",
        url: '/QuanLy/ThuongHieuDelete',
        data: { id: id },
        //dataType: "string",
        success: function () {
            Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: 'Your file has been deleted.',
                showConfirmButton: false,
                timer: 1500
            })
            location.reload();
        },
        error: function () {
            console.log(error.toString());
            Swal.fire({
                icon: 'warning',
                title: 'Xóa không thành công',
                showConfirmButton: false,
                timer: 3000
            })
        }
    });
}