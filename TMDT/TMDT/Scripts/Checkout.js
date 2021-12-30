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
    if (maGiamGia.length != 0) {
        $("#ApplyButton").text("Hủy áp dụng");
        $("#VoucherInput").attr("readonly", true);
    }
    else {
        $("#ApplyButton").text("Áp dụng");
        $("#VoucherInput").removeAttr("readonly");
    }
    $("#PhiGiaoHang").val(0);
    $("#Discount").text(tienGiam);
    $("#VoucherInput").val(maGiamGia);
    $("#SoTienGiam").val(tienGiam);
    $("#MaPhieuQuaTang").val(maPhieuQuaTang);
    $("#Total").text(thanhToan);
    $("#TongThanhToan").val(thanhToan);
};

function CheckoutSanPham() {
    var tenKhach = $("#TenKhach").val();
    var sdt = $("#SDT").val();
    var diaChi = $("#DiaChi").val();

    if (tenKhach != "" && sdt != "" && diaChi != "") {
        var tmp = [];
        for (var i = 0; i < gioHang.length; i++) {
            let j = 0;
            if (gioHang[i].buyCheck == false) {
                tmp[j] = gioHang[i];
                j++;
            }
        }
        if (tmp.length > 0) {
            gioHang = tmp;
            window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
        }
    }

}
