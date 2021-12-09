//Khởi tạo và get giỏ hàng
var gioHang = [];
var soSP = 0;
var tongTien = 0;
var tienGiam = 0;
var maGiamGia = "";
var maPhieuQuaTang = "";
var thanhToan = 0;
if (window.localStorage) {
    gioHang = JSON.parse(window.localStorage.getItem("gioHang"));
    if (sessionStorage.length > 0) {
        tienGiam = sessionStorage.getItem("tienGiam");
        maGiamGia = sessionStorage.getItem("maGiamGia");
        maPhieuQuaTang = sessionStorage.getItem("maPhieuQuaTang");
    } else {
        tienGiam = 0;
        maGiamGia = "";
    }
}
if (!gioHang)
    gioHang = [];
if (gioHang.length > 0)
    for (var i = 0; i < gioHang.length; i++) {
        soSP += gioHang[i].soLuong;
        if (gioHang[i].buyCheck) {
            tongTien += gioHang[i].giaBan * gioHang[i].soLuong;
            thanhToan = tongTien - tienGiam;
        }
    }
window.onload = function () {
    $(".MainGioHang").html('<i class="bi bi-cart4" style="font-size: 1.2em;color: white;"></i>');
    $(".MainGioHang").append(' Giỏ hàng (' + soSP + ')');
}
// Thêm giỏ hàng
function ThemGioHang(maSP, tenSP, brand, gia, soLuongTon, hinh) {
    var exist = false;
    for (i in gioHang)
        if (gioHang[i].maSanPham == maSP) {
            if (gioHang[i].soLuong < soLuongTon)
                gioHang[i].soLuong++;
            else
                gioHang[i].soLuong = soLuongTon;
            soSP++;
            exist = true;
            AddSuccess();
        }
    if (!exist && soLuongTon > 0) {
        var sanPham = {
            buyCheck: true,
            maSanPham: maSP,
            tenSanPham: tenSP,
            brand: brand,
            hinh: hinh,
            giaBan: gia,
            soLuong: 1,
            soLuongTon: soLuongTon
        };
        gioHang.push(sanPham);
        soSP++;
        //tongTien += sanPham.giaBan;
        AddSuccess();
    }
    else if (soLuongTon <= 0) {
        Swal.fire({
            title: '<strong>Hết hàng</strong>',
            icon: 'info',
            html: 'Không còn sản phầm nào trong kho mời bạn chọn sản phẩm khác!!! ',
            showCloseButton: true,
            showCancelButton: true,
            focusConfirm: false,
        })
    }
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
}
//Success Noti
function AddSuccess() {
    Swal.fire({
        icon: 'success',
        title: 'Thêm giỏ hàng thành công',
        showConfirmButton: false,
        timer: 1500
    })
    $(".MainGioHang").html('<i class="bi bi-cart4" style="font-size: 1.2em;color: white;"></i>');
    $(".MainGioHang").append(' Giỏ hàng (' + soSP + ')');
}

function GioHangPage() {
    if (gioHang.length > 0)
        location.pathname = "/GioHang/HienThiGioHang";
    else
        location.pathname = "/GioHang/GioHangRong";
}
//Xác nhận xóa 1 sản phẩm trong giỏ hàng
function ConfirmXoaGioHang(sanPham) {
    Swal.fire({
        title: 'Xác nhận xóa sản phẩm?',
        text: "Bạn sẽ không thể khôi phục lại sản phẩm!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
            var maSP = sanPham.getAttribute("data-sp-id");
            XoaGioHang(maSP);
        }
    })
}
//Xác nhận xóa toàn bộ sản phẩm trong giỏ hàng
function ConfirmXoaToanBoGioHang() {
    Swal.fire({
        title: 'Xác nhận xóa toàn bộ giỏ hàng không?',
        text: "Bạn sẽ không thể khôi phục lại sản phẩm!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: 'Your file has been deleted.',
                showConfirmButton: false,
                timer: 1500
            })
            XoaToanBoGioHang();
        }
    })
}
//Xóa 1 sản phẩm trong giỏ hàng
function XoaGioHang(maSP) {
    var tmp = gioHang;
    window.localStorage.removeItem("gioHang");
    for (var i = 0; i < tmp.length; i++) {
        if (tmp[i].maSanPham == maSP)
            tmp.splice(i, 1);
    }
    gioHang = tmp;
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}
//Xóa toàn bộ giỏ hàng
function XoaToanBoGioHang() {
    window.localStorage.removeItem("gioHang");
    gioHang = [];
    soSP = 0;
    tongTien = 0;
    thanhToan = 0;
    location.pathname = "/GioHang/GioHangRong";
}
//Tăng số lượng 1 sản phẩm trong giỏ hàng
function increaseValue(num) {
    var maSP = num.getAttribute("data-id");
    for (var i = 0; i < gioHang.length; i++) {
        if (gioHang[i].maSanPham == maSP)
            if (gioHang[i].soLuong + 1 > gioHang[i].soLuongTon) {
                gioHang[i].soLuong = gioHang[i].soLuongTon;
            }
            else {
                gioHang[i].soLuong++;
                soSP++;
                //    tongTien += gioHang[i].giaBan;
            }
    }

    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}
//Giảm số lượng sản phẩm trong giỏ hàng
function decreaseValue(num) {
    var maSP = num.getAttribute("data-id");
    for (var i = 0; i < gioHang.length; i++) {
        if (gioHang[i].maSanPham == maSP)
            if (gioHang[i].soLuong - 1 < 1) {
                gioHang[i].soLuong = 1;
            }
            else {
                gioHang[i].soLuong--;
                soSP--;
                //    tongTien -= gioHang[i].giaBan;
            }
    }

    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}
// Cập nhật số lượng sản phẩm bằng cách nhập
function InputChange(sanPham) {
    var maSP = sanPham.getAttribute("data-id");
    var quantity = sanPham.value;
    for (var i = 0; i < gioHang.length; i++) {
        if (gioHang[i].maSanPham == maSP) {
            if (quantity.length === 0) {
                QuantityError(gioHang[i].soLuongTon);
                gioHang[i].soLuong = 1;
            }
            else if (quantity == 0) {
                QuantityError(gioHang[i].soLuongTon);
                quantity = 1;
                gioHang[i].soLuong = 1;
            }
            else if (quantity > gioHang[i].soLuongTon) {
                QuantityError(gioHang[i].soLuongTon);
                gioHang[i].soLuong = gioHang[i].soLuongTon;
            }
            else {
                gioHang[i].soLuong = quantity;
            }
        }
    }
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}
// Kiểm tra nhập số
function onlyNumberKey(evt, num) {
    // Only ASCII character in that range allowed
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}
// Thông báo nhập sai số lượng
function QuantityError(max) {
    Swal.fire({
        icon: 'info',
        title: 'Chỉ được nhập từ 1 đến ' + max,
        showConfirmButton: false,
        timer: 2000
    })
}


function ApplyVoucher() {
    if (maGiamGia == 0) {
        var voucher = $("#VoucherInput").val();
        $.ajax({
            type: 'POST',
            url: '/GioHang/ApplyVoucher',
            dataType: 'json',
            data: { maVoucher: voucher },
            success: function (data) {
                var voucherObj = JSON.stringify(data);
                //var arrObj = JSON.parse(voucherObj);
                //var obj = arrObj[0];
                var obj = JSON.parse(voucherObj);
                if (obj.Status == 2) {
                    if (obj.NgayKetThuc.substring(6, 19) > Date.now()) {
                        if (obj.SoLuong > 0) {
                            if (obj.LoaiPhamVi == 0) {
                                if (tongTien >= obj.GiaTriDonHangToiThieu) {
                                    $.ajax({
                                        type: 'POST',
                                        url: '/GioHang/CheckVoucher',
                                        dataType: 'json',
                                        data: { maVoucher: voucher },
                                        success: function (data) {
                                            if (data == true) {
                                                tienGiam = obj.GiaTri;
                                                maGiamGia = obj.MaGiamGia;
                                                maPhieuQuaTang = obj.MaPhieuQuaTang;
                                                sessionStorage.setItem("tienGiam", tienGiam);
                                                sessionStorage.setItem("maGiamGia", maGiamGia);
                                                sessionStorage.setItem("maPhieuQuaTang", maPhieuQuaTang);
                                                location.reload();
                                            }
                                            else {
                                                tienGiam = 0;
                                                VoucherIssue("Tài khoản chưa có voucher");
                                            }
                                        }
                                    })
                                }
                                else {
                                    tienGiam = 0;
                                    VoucherIssue("Đơn hàng chưa đạt giá trị tối thiểu!");
                                }
                            }
                            else {
                                var stt = false;
                                for (var i = 0; i < gioHang.length; i++) {
                                    if (obj.LoaiPhamVi == gioHang[i].brand && gioHang[i].buyCheck) {
                                        stt = true;
                                        if ((gioHang[i].soLuong * gioHang[i].giaBan) >= obj.GiaTriDonHangToiThieu) {
                                            $.ajax({
                                                type: 'POST',
                                                url: '/GioHang/CheckVoucher',
                                                dataType: 'json',
                                                data: { maVoucher: voucher },
                                                success: function (data) {
                                                    if (data == true) {
                                                        tienGiam = obj.GiaTri;
                                                        maGiamGia = obj.MaGiamGia;
                                                        maPhieuQuaTang = obj.MaPhieuQuaTang;
                                                        sessionStorage.setItem("tienGiam", tienGiam);
                                                        sessionStorage.setItem("maGiamGia", maGiamGia);
                                                        sessionStorage.setItem("maPhieuQuaTang", maPhieuQuaTang);
                                                        location.reload();
                                                    }
                                                    else {
                                                        tienGiam = 0;
                                                        VoucherIssue("Tài khoản chưa có voucher");
                                                    }
                                                }
                                            })
                                        }
                                        else {
                                            tienGiam = 0;
                                            VoucherIssue("Đơn hàng chưa đạt giá trị tối thiểu!");
                                        }
                                    }
                                }
                                if (stt == false) {
                                    tienGiam = 0;
                                    VoucherIssue("Không có sản phẩm phù hợp");
                                }
                            }
                        }
                        else {
                            tienGiam = 0;
                            VoucherIssue("Voucher đã hết");
                        }
                    }
                    else {
                        tienGiam = 0;
                        VoucherIssue("Voucher không còn hiệu lực hoặc không tồn tại");
                    }
                }
                else {
                    tienGiam = 0;
                    VoucherIssue("Voucher không còn hiệu lực hoặc không tồn tại");
                }
            },
            error: function (ex) {

            }
        });
    }
    else {
        tienGiam = 0;
        maGiamGia = "";
        maPhieuQuaTang = "";
        sessionStorage.setItem("tienGiam", tienGiam);
        sessionStorage.setItem("maGiamGia", maGiamGia);
        sessionStorage.setItem("maPhieuQuaTang", maPhieuQuaTang);
        location.reload();
    }
}

function VoucherIssue(title) {
    Swal.fire({
        icon: 'info',
        title: title,
        showConfirmButton: false,
        timer: 1000
    })
}