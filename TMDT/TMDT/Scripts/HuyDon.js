$('.modal-Cancel').click(function () {
    $('#hidCancel').val($(this).data('id'));
    $('#CancelError').text("");
})

function HuyDon() {
    var maHoaDon = $('#hidCancel').val();
    var urlstr = $('#hidUrl').val();
    var reason = $('#CancelContent').val();
    if (reason == "") {
        $('#CancelError').text("Bạn chưa nhập lý do hủy!");
    }
    else {
        $.ajax({
            type: "POST",
            url: "/NhanVien/HuyDonHang",
            data: {
                maDonHang: maHoaDon,
                reason: reason,
                urlstr: urlstr,
            },
            success: function () {
                location.reload();
            }
        })
    }
}

$('.modal-Reason').click(function () {
    var maHoaDon=$(this).data('id');
    $.ajax({
        type: "POST",
        url: "/NhanVien/GetReason",
        data: {
            maDonHang: maHoaDon,
        },
        success: function (data) {
            var dateValue = parseInt(data.NgayHuyDon.replace(/\/Date\((\d+)\)\//g, "$1"));
            data.NgayHuyDon = new Date(dateValue);
            $('#CancelDate').text(data.NgayHuyDon.toLocaleString());
            $('#CancelStaff').text(data.NguoiHuyDon);
            $('#ContentShow').val(data.LyDoHuy);
        }
    })
})