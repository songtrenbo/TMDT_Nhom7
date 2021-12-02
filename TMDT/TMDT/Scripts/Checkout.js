window.onload = function () {
    //Kiểm tra số lượng sản phẩm trong giỏ hàng
    if (gioHang.length <= 0)
        location.pathname = "/GioHang/GioHangRong";
    else {
        $(".MainGioHang").html('<i class="bi bi-cart4" style="font-size: 1.2em;color: white;"></i>');
        $(".MainGioHang").append(' Giỏ hàng (' + soSP + ')');
    }

    //Push ds sản phẩm vào giỏ hàng
    for (var i = 0; i < gioHang.length; i++) {
        if (gioHang[i].buyCheck) {
            tableBody = $("#table-shopping-checkout");
            tableBody.append('<tr><td style="text-align: center;vertical-align: middle;">' +
                '<figure class="itemside align-items-center">' +
                '<div class="aside"><img src="' + gioHang[i].hinh + '" class="img-sm" width="80px" height="80px"></div>' +
                '</td><td style="padding:5px"><a href="#" class="title text-dark" data-abc="true">' + gioHang[i].tenSanPham + '</a>' +
                '<figcaption class="info">' +
                '<div class="price-wrap"> <var class="price">' + gioHang[i].giaBan + '</var> </div>' +
                '<p class="text-muted small" style="margin-bottom:2px">Số lượng: ' + gioHang[i].soLuong + '</p>' +
                '<p class="text-muted small">Mã sản phẩm: ' + gioHang[i].maSanPham + '</p>' +
                '</figcaption></figure>' +
                '</td></tr > "')

            $.ajax({
                type: "POST",
                url: '/GioHang/AddToCart/',
                data: {
                    id: gioHang[i].maSanPham,
                    quantity: gioHang[i].soLuong
                },
                //dataType: "string",
                success: function () {

                },
                error: function () {

                }
            });
        }
    }
    //Load thông tin summary
    $(".cart").append('<div class="back-to-shop"><a href="/Home/Index">&leftarrow;</a><span class="text-muted">Back to shop</span></div>');
    $("#TotalPrice").text(tongTien);
    $("#PhiGiaoHang").val(0);
    $("#Discount").text(0);
    $("#SoTienGiam").val(0);
    $("#Total").text(tongTien);
    $("#TongThanhToan").val(tongTien);
};

function CheckoutSanPham() {
    var tenKhach = $("#TenKhach").val();
    var sdt = $("#SDT").val();
    var diaChi = $("#DiaChi").val();

    if (tenKhach != "" && sdt != "" && diaChi != "") {
        var tmp = gioHang;
        window.localStorage.removeItem("gioHang");
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].buyCheck) {
                tmp.splice(i, 1);
            }
        }
        gioHang = tmp;
        window.localStorage.setItem("gioHang", JSON.stringify(gioHang));

        Swal.fire({
            icon: 'success',
            title: 'Đặt hàng thành công',
            showConfirmButton: false,
            timer: 4000
        })
    }
}
