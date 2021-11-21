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
        tableBody = $("table tbody");
        tableBody.append('<tr><td style="text-align: center;vertical-align: middle;">' +
            '<input type="checkbox" class="div1 show" data-id="' + gioHang[i].maSanPham + '" checked></td><td>' +
            '<figure class="itemside align-items-center">' +
            '<div class="aside"><img src="' + gioHang[i].hinh + '" class="img-sm"></div>' +
            '<figcaption class="info">' +
            '<a href="#" class="title text-dark" data-abc="true">' + gioHang[i].tenSanPham + '</a>' +
            '<p class="text-muted small">Brand:' + gioHang[i].maSanPham + '</p>' +
            '</figcaption></figure>' +
            '</td><td><div class="price-wrap"> <var class="price">' + gioHang[i].giaBan + '</var> </div></td>' +
            '<td><div class="input-group">' +
            '<button type="button" class="value-button form-control" style="text-align:center" id="decrease" data-id="' + gioHang[i].maSanPham + '" onclick="decreaseValue(this)" value="Decrease Value">-</button>' +
            '<input type = "text" onkeypress="return onlyNumberKey(event)" onkeyup="InputChange(this)" style="text-align:center;" class="quantity form-control" data-id="' + gioHang[i].maSanPham + '" min="1" max="' + gioHang[i].soLuongTon + '" value="' + gioHang[i].soLuong + '"/>' +
            '<button type="button" class="value-button form-control" style="text-align:center" id="increase" data-id="' + gioHang[i].maSanPham + '" onclick="increaseValue(this)" value="Increase Value">+</button>' +
            '</div></td>' +
            '<td class="text-right d-none d-md-block"> <a href="#" class="btn btn-light" data-abc="true" onclick="ConfirmXoaGioHang(this)" data-sp-id="' + gioHang[i].maSanPham + '"> Remove</a> </td></tr > "')
        document.getElementsByClassName("div1")[i].checked = gioHang[i].buyCheck;
    }

    //Thay đổi check mua hàng
    $('input[type = checkbox].show').change(function () {
        var maSP = $(this).data("id");
        var ischecked = $(this).is(":checked");

        for (var i = 0; i < gioHang.length; i++) {
            if (gioHang[i].maSanPham == maSP) {
                if (ischecked) gioHang[i].buyCheck = true;
                else gioHang[i].buyCheck = false;
            }
        }
        window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    });

    //Kiểm tra Check All
    var checkboxes = document.querySelectorAll('input.div1');
    var checkdiv1 = document.getElementById('checkdiv1');

    var checkedCount = document.querySelectorAll('input.div1:checked').length;

    checkdiv1.checked = checkedCount > 0;
    checkdiv1.indeterminate = checkedCount > 0 && checkedCount < checkboxes.length;

    //Onclick change checkbox 1 sản phẩm
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].onclick = function () {
            location.reload();
        }
    }
    //Onclick change checkbox all sản phẩm
    checkdiv1.onclick = function () {
        var checkboxes = document.querySelectorAll('input.div1');
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = this.checked;
            $(checkboxes[i]).trigger('change');
        }
        location.reload();
    }
    //Load thông tin summary
    $(".cart").append('<div class="back-to-shop"><a href="/Home/Index">&leftarrow;</a><span class="text-muted">Back to shop</span></div>');
    $("#TotalPrice").text(tongTien);
    $("#TamTinh").text(tongTien);
    $("#Total").text(tongTien);
};
