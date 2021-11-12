window.onload = function () {
    if (gioHang.length <= 0)
        location.pathname = "/GioHang/GioHangRong";
    else
        $("#gioHangMain").text("Giỏ hàng (" + soSP + ")");
    for (var i = 0; i < gioHang.length; i++) {

        tableBody = $("table tbody");
        tableBody.append('<tr><td style="text-align: center;vertical-align: middle;">' +
            '<input class="checkMess" type="checkbox" name="foo" onclick="TinhTien(this)" value="bar1" data-id="' + gioHang[i].maSanPham + '"></td><td>' +
            '<figure class="itemside align-items-center">' +
            '<div class="aside"><img src="' + gioHang[i].hinh + '" class="img-sm"></div>' +
            '<figcaption class="info">' +
            '<a href="#" class="title text-dark" data-abc="true">' + gioHang[i].tenSanPham + '</a>' +
            '<p class="text-muted small">Brand:' + gioHang[i].maSanPham + '</p>' +
            '</figcaption></figure>' +
            '</td><td><div class="price-wrap"> <var class="price">' + gioHang[i].giaBan + '</var> </div></td>' +
            '<td><div class="input-group">' +
            '<button type="button" class="value-button form-control" style="text-align:center" id="decrease" data-id="' + gioHang[i].maSanPham + '" onclick="decreaseValue(this)" value="Decrease Value">-</button>' +
            '<input type = "number" class="quantity form-control" data-id="' + gioHang[i].maSanPham + '" min="1" max="' + gioHang[i].soLuongTon + '" value="' + gioHang[i].soLuong + '"/>' +
            '<button type="button" class="value-button form-control" style="text-align:center" id="increase" data-id="' + gioHang[i].maSanPham + '" onclick="increaseValue(this)" value="Increase Value">+</button>' +
            '</div></td>' +
            '<td class="text-right d-none d-md-block"> <a href="#" class="btn btn-light" data-abc="true" onclick="ConfirmXoaGioHang(this)" data-sp-id="' + gioHang[i].maSanPham + '"> Remove</a> </td></tr > "')
        document.getElementsByClassName("checkMess")[i].checked = gioHang[i].buyCheck;
    }
    $(".cart").append('<div class="back-to-shop"><a href="/Home/Index">&leftarrow;</a><span class="text-muted">Back to shop</span></div>');
    $("#TotalPrice").text(tongTien);
    $("#TamTinh").text(tongTien);
    $("#Total").text(tongTien);
};