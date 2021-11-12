var gioHang = [];
var soSP = 0;
var tongTien = 0;
if (window.localStorage)
    gioHang = JSON.parse(window.localStorage.getItem("gioHang"));
if (!gioHang)
    gioHang = [];
if (gioHang.length > 0)
    for (var i = 0; i < gioHang.length; i++) {
        soSP += gioHang[i].soLuong;
        if (gioHang[i].buyCheck) {
            tongTien += gioHang[i].giaBan * gioHang[i].soLuong;
        }
    }
window.onload = function () {
    $(".MainGioHang").append(' Giỏ hàng (' + soSP + ')');
}

function ThemGioHang(maSP, tenSP, gia, soLuongTon, hinh) {
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
    if (!exist) {
        var sanPham = {
            buyCheck: true,
            maSanPham: maSP,
            tenSanPham: tenSP,
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
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
}
function AddSuccess() {
    Swal.fire({
        icon: 'success',
        title: 'Thêm giỏ hàng thành công',
        showConfirmButton: false,
        timer: 1500
    })
    $("#gioHangMain").text("Giỏ hàng (" + soSP + ")");
}


function CheckAllCheckbox(source) {
    var checkedValue = $('.checkAllid').is(":checked");
    checkboxes = document.getElementsByName('foo');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
    if (checkedValue) {
        for (var i = 0; i < gioHang.length; i++) {
            gioHang[i].buyCheck = false;
        }
    }
    else {
        for (var i = 0; i < gioHang.length; i++) {
            gioHang[i].buyCheck = true;
        }
    }
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}

function TinhTien(choose) {
    var maSP = choose.getAttribute("data-id");
    for (var i = 0; i < gioHang.length; i++) {
        if (gioHang[i].maSanPham == maSP) {
            gioHang[i].buyCheck = !gioHang[i].buyCheck;
        }
    }
    window.localStorage.setItem("gioHang", JSON.stringify(gioHang));
    location.reload();
}


function GioHangPage() {
    if (gioHang.length > 0)
        location.pathname = "/GioHang/HienThiGioHang";
    else
        location.pathname = "/GioHang/GioHangRong";
}

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

function XoaToanBoGioHang() {
    window.localStorage.removeItem("gioHang");
    gioHang = [];
    soSP = 0;
    tongTien = 0;
    location.pathname = "/GioHang/GioHangRong";
}

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



function decreaseValue(num) {
    var maSP = num.getAttribute("data-id");
    for (var i = 0; i < gioHang.length; i++) {
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




