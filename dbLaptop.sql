use master
if exists (select *
from sysdatabases
where name='DBLaptop')
drop database DBLaptop
CREATE DATABASE DBLaptop
go
USE DBLaptop
go
CREATE TABLE Quyen
(
    MaQuyen INT IDENTITY PRIMARY KEY,
    TenQuyen NVARCHAR(50) NOT NULL
);
CREATE TABLE NguoiDung
(
    MaNguoiDung INT IDENTITY PRIMARY KEY,
    Ten NVARCHAR(50) NOT NULL,
    SDT VARCHAR(15) NULL,
    DiaChi NVARCHAR(100) NULL,
    Username VARCHAR(100) NOT NULL,
    Password VARCHAR(1000) NOT NULL DEFAULT 0,
    Email NVARCHAR(100) NULL,
    MaQuyen INT NOT NULL DEFAULT 4,-- 1: admin && 2: quản lý && 3: nhân viên && 4: khách
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    Status INT DEFAULT 1,--1: active, 2: locked, 3: deleted
    FOREIGN KEY (MaQuyen) REFERENCES dbo.Quyen(MaQuyen)
);

CREATE TABLE DanhMuc
(
    MaDanhMuc INT IDENTITY PRIMARY KEY,
    TenDanhMuc NVARCHAR(255) NOT NULL,
    SeoTitle NVARCHAR(255),
    Hinh NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsShowHome BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE ThuongHieu
(
    MaThuongHieu INT IDENTITY PRIMARY KEY,
    TenThuongHieu NVARCHAR(255) NOT NULL,
    Hinh NVARCHAR(255) NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE CPU
(
    MaCPU INT IDENTITY PRIMARY KEY,
    TenCPU NVARCHAR(255) NOT NULL,
    HangCPU NVARCHAR(255) NOT NULL,--Intel, AMD, Apple
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE LoaiGPU
(
    MaLoaiGPU INT IDENTITY PRIMARY KEY,
    TenLoaiGPU NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE SizeManHinh
(
    MaSizeManHinh INT IDENTITY PRIMARY KEY,
    Size FLOAT NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE SanPham
(
    MaSanPham INT IDENTITY PRIMARY KEY,
    TenSanPham NVARCHAR(255) NOT NULL,
    MaDanhMuc INT NOT NULL,
    SeoTitle NVARCHAR(255),
    MaThuongHieu INT NOT NULL,
    SoLuongTon INT DEFAULT 0,
    SoLuongBan INT DEFAULT 0,
    GiaBan FLOAT DEFAULT 0,
    GiaGiam FLOAT DEFAULT 0,
    DiemRate INT DEFAULT 0,
    SoLuotRate INT DEFAULT 0,
    LuotXem INT DEFAULT 0,
    BaoHanh INT NOT NULL,
    MaLoaiCPU INT NOT NULL,
    CPUInfo NVARCHAR(255),
    RAM NVARCHAR(255) NOT NULL,--4, 8, 16, 32
    MaLoaiGPU INT NOT NULL,
    GPUInfo NVARCHAR(255),
    SizeManHinh INT NOT NULL,
    ManHinh NVARCHAR(255),
    Pin NVARCHAR(255) NOT NULL,
    LuuTru NVARCHAR(255) NOT NULL,
    KhoiLuong FLOAT,
    Hinh NVARCHAR(255) NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsHide BIT DEFAULT 0,
    FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DanhMuc(MaDanhMuc),
    FOREIGN KEY (MaThuongHieu) REFERENCES dbo.ThuongHieu(MaThuongHieu),
    FOREIGN KEY (MaLoaiCPU) REFERENCES dbo.CPU(MaCPU),
    FOREIGN KEY (MaLoaiGPU) REFERENCES dbo.LoaiGPU(MaLoaiGPU),
    FOREIGN KEY (SizeManHinh) REFERENCES dbo.SizeManHinh(MaSizeManHinh),
);

CREATE TABLE SanPhamNhapKho
(
    MaSanPhamNhapKho INT IDENTITY PRIMARY KEY,
    MaSanPham INT  NOT NULL,
    SoLuong INT DEFAULT 0,
    GiaNhap FLOAT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE Hinh
(
    MaHinh INT IDENTITY PRIMARY KEY,
    MaSanPham INT NOT NULL,
    TenHinh NVARCHAR(255) NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE TinTuc
(
    MaTinTuc INT IDENTITY PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    Thumbnail NVARCHAR(255) NOT NULL,
    NoiDung NTEXT NOT NULL,
    MaSanPham INT,
    MaNguoiTao INT NOT NULL,
    MaNguoiSua INT,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsHide BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE PhieuQuaTang
(
    MaPhieuQuaTang INT IDENTITY PRIMARY KEY,
    TenPhieuQuaTang NVARCHAR(255) NOT NULL,
    MaGiamGia VARCHAR(20),
    LoaiPhamVi INT NOT NULL,--1: all, theo thương hiệu
    GiaTri FLOAT DEFAULT 0,
    GiaTriDonHangToiThieu FLOAT DEFAULT 0,
    SoLuong INT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayKichHoat DATETIME NOT NULL,
    NgayKetThuc DATETIME NOT NULL,
    Status INT DEFAULT 1,--1: inactive, 2: active, 3: end
);

CREATE TABLE NguoiDung_PhieuQuaTang
(
    MaNguoiDung_PhieuQuaTang INT IDENTITY PRIMARY KEY,
    MaPhieuQuaTang INT,
    MaNguoiDung INT,
    Status INT,--1: Đã sử dụng, 2: Chưa sử dụng, 3: Đã hết hạn
    FOREIGN KEY (MaPhieuQuaTang) REFERENCES dbo.PhieuQuaTang(MaPhieuQuaTang),
    FOREIGN KEY (MaNguoiDung) REFERENCES dbo.NguoiDung(MaNguoiDung),
);

CREATE TABLE HoaDon
(
    MaHoaDon INT IDENTITY PRIMARY KEY,
    NgayMua DATETIME NOT NULL,
    MaKhachHang INT NOT NULL,
    TenKhach NVARCHAR(255) NOT NULL, 
    DiaChi NVARCHAR(255), 
    SDT VARCHAR(15) NOT NULL, 
    HinhThucThanhToan INT DEFAULT 1,--1: COD, 2: paypal 
    HinhThucGiaoHang INT DEFAULT 1,--1: tại quầy, 2: tại địa chỉ 
    MaNVDuyet INT,
    NgayTao DATETIME DEFAULT GETDATE(),
    MaPhieuQuaTang INT,
    SoTienGiam FLOAT DEFAULT 0,
    PhiGiaoHang INT,
    TinhTrang INT DEFAULT 1,--1: Chờ xác nhận, 2: Chờ lấy hàng, 3: Đang giao, 4: Đã giao, 5: Đã hủy, 6: Trả hàng
    TongThanhToan INT DEFAULT 0,
    TinhTrangThanhToan BIT DEFAULT 0,--0: Chưa thanh toán, 1: Đã thanh toán
    NgayNhan DATETIME,
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaNVDuyet) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaPhieuQuaTang) REFERENCES dbo.PhieuQuaTang(MaPhieuQuaTang),
);

CREATE TABLE CTHoaDon
(
    MaCTHoaDon INT IDENTITY PRIMARY KEY,
    MaHoaDon INT NOT NULL,
    MaSanPham INT NOT NULL,
    DonGia FLOAT NOT NULL,
    SoLuong INT NOT NULL,
    ThanhTien INT NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon),
);

CREATE TABLE DanhGia
(
    MaDanhGia INT IDENTITY PRIMARY KEY,
    MaSanPham INT NOT NULL,
    MaKhachHang INT NOT NULL,
    MaHoaDon INT NOT NULL,
    Diem INT NOT NULL,
    NoiDung NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME NOT NULL,
    IsApproved BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon),
);
CREATE TABLE Banner
(
    MaBanner INT IDENTITY PRIMARY KEY,
    TenBanner NVARCHAR(255) NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,
    MaSanPham INT  NOT NULL,
    IsHide BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgaySua DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);
GO

 CREATE TRIGGER UTG_UpdateSLTonSP ON SanPhamNhapKho
 AFTER INSERT
 AS
 BEGIN
     UPDATE SanPham
     SET SoLuongTon = SoLuongTon + (SELECT SoLuong FROM INSERTED WHERE MaSanPham = SanPham.MaSanPham)
     FROM SanPham, INSERTED
	 WHERE SanPham.MaSanPham = INSERTED.MaSanPham
 END
GO

 CREATE TRIGGER UTG_UpdateRateSP ON DanhGia
 AFTER INSERT
 AS
 BEGIN
     UPDATE SanPham
     SET DiemRate = DiemRate + (SELECT Diem FROM INSERTED WHERE MaSanPham = SanPham.MaSanPham),
     SoLuotRate = SoLuotRate + 1
     FROM SanPham, INSERTED
	 WHERE SanPham.MaSanPham = INSERTED.MaSanPham
 END
GO

--Quyền
INSERT INTO Quyen
    (TenQuyen)
VALUES(N'admin'),
    (N'quản lý'),
    (N'nhân viên'),
    (N'khách hàng')
GO

--Danh mục
INSERT INTO DanhMuc
    (TenDanhMuc, Hinh, NgayTao, IsShowHome)
VALUES(N'Gaming đồ họa', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352691284513627_Group%2073@3x.png', N'2021-11-05', 1),--1
    (N'Doanh nhân', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352693282588825_Group%2073@3x.png', N'2021-11-05', 1),--2
    (N'Sinh viên', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/10/637353308751863585_Img@3x.png', N'2021-11-05', 0),--3
    (N'Mỏng nhẹ', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352694501010620_Group%2073@3x.png', N'2021-11-05', 1),--4
    (N'Văn phòng', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352695747985641_Group%2073@3x.png', N'2021-11-05', 0)--5
GO

--Thương hiệu
INSERT INTO ThuongHieu
    (TenThuongHieu, Hinh, NgayTao)
VALUES(N'Macbook', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494668267616_Macbook-Apple@2x.jpg', N'2021-11-05'),--1
    (N'Asus', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666861275_Asus.jpg', N'2021-11-05'),--2
    (N'Acer nhẹ', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666704995_Acer@2x.jpg', N'2021-11-05'),--3
    (N'Dell', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666861275_Dell@2x.jpg', N'2021-11-05'),--4
    (N'HP', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/7/15/637619564183327279_HP@2x.png', N'2021-11-05'),--5
    (N'Lenovo', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494668267616_Lenovo@2x.jpg', N'2021-11-05'),--6
    (N'Microsoft', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/1/13/637461259692529909_Microsoft@2x.png', N'2021-11-05'),--7
    (N'MSI', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340493755614653_MSI@2x.jpg', N'2021-11-05')--8
GO

--CPU
INSERT INTO CPU
    (TenCPU, HangCPU, NgayTao)
VALUES(N'i3', N'Intel', N'2021-11-05'),--1
    (N'i5', N'Intel', N'2021-11-05'),--2
    (N'i7', N'Intel', N'2021-11-05'),--3
    (N'i9', N'Intel', N'2021-11-05'),--4
    (N'Ryzen 5', N'AMD', N'2021-11-05'),--5
    (N'Ryzen 7', N'AMD', N'2021-11-05'),--6
    (N'M1', N'Apple', N'2021-11-05'),--7
    (N'M1 Pro', N'Apple', N'2021-11-05'),--8
    (N'M1 Pro Max', N'Apple', N'2021-11-05'),--9
    (N'Ryzen 9', N'AMD', N'2021-11-05')--10
GO

--GPU
INSERT INTO LoaiGPU(TenLoaiGPU, NgayTao)
VALUES(N'Card Onboard', N'2021-11-05'),--1
    (N'AMD Radeon', N'2021-11-05'),--2
    (N'NVIDIA GeForce GTX', N'2021-11-05'),--3
    (N'NVIDIA GeForce RTX', N'2021-11-05'),--4
    (N'NVIDIA GeForce MX350', N'2021-11-05')--5
GO

--SizeManHinh
INSERT INTO SizeManHinh(Size, NgayTao)
VALUES(15.6, N'2021-11-05'),--1
(17.3, N'2021-11-05'),--2
(14, N'2021-11-05'),--3
(13.3, N'2021-11-05'),--4
(16, N'2021-11-05')--5

--Sản phẩm
INSERT INTO SanPham(TenSanPham, MaDanhMuc, SeoTitle, MaThuongHieu, GiaBan, GiaGiam, BaoHanh,  MaLoaiCPU, CPUInfo, RAM, MaLoaiGPU, GPUInfo, SizeManHinh, ManHinh, Pin, LuuTru, KhoiLuong, Hinh, NgayTao)
VALUES(N'Laptop Gaming Acer Aspire 7 A715 42G R1SB', 1, 'laptop-gaming-acer-aspire-7-a715-42g-r1sb', 3, 19990000, 0, 12, 5, N'5500U', 8, 3, N'1650 4GB GDDR6', 1, N'FHD (1920 x 1080) IPS, Anti-Glare, 144Hz', N'4 Cell 48Whr', N'256GB PCIe® NVMe™ M.2 SSD', 2.1, N'https://product.hstatic.net/1000026716/product/laptop_gaming_acer_aspire_7_a715_42g_r1sb_9520016e22274791bb4e1697764c57c4.jpg', N'2021-11-05 05:35:21'),
(N'Laptop Apple MacBook Pro M1 2020 8GB/256GB (MYD82SA/A)', 4, 'apple-macbook-pro-2020-m1-myd82saa', 1, 33490000, 0, 12, 7, null, 8, 1, null, 4, null, N'Khoảng 10 tiếng', N'256 GB SSD', 1.4, N'https://lh3.googleusercontent.com/6iW6tc0lHp4paCYznq-gC5mEXEGMSBmrSq2I4MaXdPne5XWQI4l8m-bGRVCRFH94d4PEqtUIdH3FERr-VNDWaT2k9qcZ5Ey_PQ=w1000-rw', N'2021-11-05 06:35:21'),
(N'Laptop gaming Lenovo Legion 5 Pro 16ACH6H 82JQ005YVN', 1, 'laptop-gaming-lenovo-legion-5-pro-16ach6h-82jq005yvn', 6, 49990000, 40000000, 24, 6, N'5800H 3.2GHz upto 4.4GHz, 8 cores 16 threads', 16, 4, N'3070 8GB GDDR6', 5, N'WQXGA (2560x1600) IPS 500nits Anti-glare, 165Hz, 100% sRGB, Dolby Vision, HDR 400, Free-Sync, G-Sync, DC dimmer', N'4 Cell 80 WHr', N'1TB SSD M.2 2280 PCIe 3.0x4 NVMe', 2.45, N'https://product.hstatic.net/1000026716/product/laptop_gaming_lenovo_legion_5_pro_16ach6h_82jq005yvn_fea32779d7ae4bfd8c9536fd44358354.jpg', N'2021-12-05 15:35:21'),
(N'Microsoft Surface Pro 7 i5/8G/256Gb (Black)', 2, 'microsoft-surface-pro-7-i5-8g-256gb-black-256gb', 7, 24490000, 0, 14, 2, null, 8, 1, null, 5, null, N'3 Cell', N'ROM 256Gb', 1.2, N'https://phucanhcdn.com/media/product/37623_microsoft_surface_pro_7_black__1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Microsoft Surface Laptop Go 12.4" Touchscreen', 5, 'laptop-microsoft-surface-laptop-go-12.4quot-touchscreen', 7, 18990000, 17990000, 12, 2, null, 8, 1, null, 4, null, N'3 Cell', N'128Gb SSD', 1.9, N'https://phucanhcdn.com/media/product/42437_surface_laptop_go_ice_blue_h1.png', N'2021-11-05 06:35:21'),
(N'Macbook Air 2020 M1 7GPU 16GB 256GB Z124000DE - Grey', 4, 'macbook-air-2020-m1-7gpu-16gb-256gb-z124000de-grey', 1, 31990000, 30990000, 24, 8, null, 16, 2, null, 5, null, N'30W USB-C Power Adapter', N'256GB SSD', 1.2, N'https://bachlongmobile.com/media/catalog/product/cache/2/image/040ec09b1e35df139433887a97daa66f/m/a/macbook-air-space-gray-config-201810_4_2_1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Lenovo Yoga Duet 7 13ITL6', 4, 'laptop-lenovo-yoga-duet-7-13itl6-i5-82ma000pvn', 6, 26990000,25990000, 12, 2, null, 8, 2, null, 4, null, N'41 Wh', N'512 GB SSD NVMe PCIe', 1.4, N'https://cdn.ankhang.vn/media/product/20321_laptop_lenovo_yoga_duet_7_13itl6_82ma003xvn_1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Lenovo Yoga Duet 7 13IML05', 4, 'laptop-lenovo-yoga-duet-7-13iml05-i7-82as007cvn', 6, 27990000, 25990000, 12, 3, null, 8, 3, null, 4, null, N'Li-Polymer, 42 Wh', N'512 GB SSD NVMe PCIe', 1.1, N'https://phucanhcdn.com/media/product/41229_yoga_duet_7_tim_ha1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Lenovo YOGA Slim 7 Carbon', 4, 'laptop-lenovo-yoga-slim-7-carbon-13itl5-i7-82ev0017vn', 6, 32990000, 0, 24, 3, null, 16, 3, null, 5, null, N'Li-Polymer, 50 Wh', N'1 TB SSD M.2 PCIe', 1.2, N'https://lumen.thinkpro.vn//backend/uploads/product/color_images/2020/12/18/yslim7c-01.jpg', N'2021-11-05 06:35:21'),
(N'Laptop MSI Gaming GF63 Thin 10SC', 1, 'laptop-msi-gaming-gf63-thin-10sc-i7-480vn', 8, 21840000, 21340000, 16, 3, null, 8, 3, null, 2, null, N'62WHrs, 4S1P, 4-cell Li-ion', N'512 GB SSD NVMe PCIe', 1.7, N'https://images.fpt.shop/unsafe/fit-in/585x390/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/8/7/637639340541330187_msi-gaming-gf63-den-1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop MSI Modern 14 B11SB', 5, 'laptop-msi-modern-14-b11sb-075vn-vang-hong', 8, 22219000, 0, 12, 2, null, 8, 1, null, 4, null, N'3 cell, 52Wh', N'512GB NVMe PCle SSD', 1.5, N'https://hanoicomputercdn.com/media/product/58131_morden14_h___ng__5_.png', N'2021-11-05 06:35:21'),
(N'Laptop MSI Modern 14 B10MW', 5, 'laptop-msi-modern-14-b10mw-482vn-xanh', 8, 14129000, 0, 12, 1, null, 8, 1, null, 3, null, N'3 Cell 52Whr', N'256GB NVMe PCIe SSD', 1.3, N'https://hanoicomputercdn.com/media/product/57914_morden_14__4_.jpg', N'2021-11-05 06:35:21'),
(N'Laptop MSI Modern 14 B11MOU 851VN', 4, 'laptop-msi-modern-14-b11mou-851vn', 8, 14990000, 14490000, 0, 1, null, 8, 1, null, 4, null, N'	3 Cell 39Whr', N'256GB PCIe NVMe™ M.2 SSD', 1.3, N'https://bizweb.dktcdn.net/100/329/122/products/laptop-msi-modern-14-b11mou-851vn-1.png?v=1630307399600', N'2021-11-05 06:35:21'),
(N'Laptop Gaming Acer Predator Helios 300 PH315 54 75YD', 1, 'laptop-gaming-acer-predator-helios-300-ph315-54-75yd', 3, 39990000, 0, 12, 3, null, 16, 3, null, 4, null, N'4 Cell 59 WHr', N'512GB SSD M.2 PCIE', 1.6, N'https://anphat.com.vn/media/product/39035_60433_laptop_acer_gaming_predator_helios_300_ph315_54_75yd_nhqc2sv002_den_2021_4.png', N'2021-11-05 06:35:21'),
(N'Laptop Acer Aspire 3 A315 56 37DV', 5, 'laptop-acer-aspire-3-a315-56-37dv', 3, 11190000, 0, 0, 1, null, 4, 1, null, 4, null, N'3 Cell 36.7 Whr', N'256GB SSD M.2 PCIE, 1x slot SATA3 2.5"', 1.1, N'https://images.fpt.shop/unsafe/fit-in/585x390/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/4/13/637223737030598859_acer-aspire-3-a315-56-den-1.png', N'2021-11-05 06:35:21'),
(N'Laptop Acer Swift 3 SF314-511-56G1', 2, 'laptop-acer-swift-3-sf314-511-56g1-nx-ablsv-002', 3, 22990000, 0, 24, 2, null, 16, 2, null, 4, null, N'3 Cell', N'512GB PCIe NVMe SSD', 1.4, N'https://hc.com.vn/i/ecommerce/media/GS.008130_FEATURE_87528.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Acer Swift 3 Evo SF314 511 59LV', 2, 'laptop-acer-swift-3-evo-sf314-511-59lv', 3, 21999000, 20999000, 12, 2, null, 16, 3, null, 4, null, N'48 Wh Li-ion', N'512GB PCIe NVMe SSD', 1.4, N'https://cdn.nguyenkimmall.com/images/detailed/770/10050665-laptop-acer-swift-3-sf314-43-r4x3-r5-5500u-nxab1sv004-1.jpg', N'2021-11-05 06:35:21'),
(N'aptop Acer Swift 3 SF314 43 R4X3', 2, 'laptop-acer-swift-3-sf314-43-r4x3', 3, 20990000, 20490000, 17, 2, null, 16, 1, null, 4, null, N'3 Cell 48Whr', N'512GB SSD M.2 PCIE Gen3x4', 1.6, N'https://anphat.com.vn/media/product/36592_thumb650_acer_swift_3_sf314_silver_1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop HP 240 G8 342A3PA', 4, 'laptop-hp-240-g8-342a3pa', 5, 11990000, 0, 12, 1, null, 4, 1, null, 3, null, N'3 Cell 41Whr', N'256GB SSD M.2 NVMe', 1.7, N'https://hc.com.vn/i/ecommerce/media/GS.008101_FEATURE_86164.jpg', N'2021-11-05 06:35:21'),
(N'Laptop HP ProBook 440 G8 i3 1115G4', 3, 'may-tinh-xach-tay-hp-probook-440-g8-i3-1115g4', 5, 14199000, 0, 12, 1, null, 4, 1, null, 4, null, N'3 Cell', N'SSD M2.PCIe 256 GB', 1.6, N'https://fptshop.com.vn/Uploads/images/2015/Tin-Tuc/QuanLNH2/hp-probook-440-g8-1.jpg', N'2021-11-05 06:35:21'),
(N'Hp Spectre X360 13-Aw0204tu', 3, 'hp-spectre-x360-13-aw0204tu', 5, 39990000, 0, 24, 3, null, 8, 1, null, 4, null, N'3 cell', N'256Gb SSD', 1.4, N'https://product.hstatic.net/1000238589/product/hp_spectre_x360_13-aw0204tu_710dfb27512f4feebce9c24b727b2216.jpg', N'2021-11-05 06:35:21'),
(N'Laptop HP Envy 13 ba1535TU i7 1165G7', 3, 'may-tinh-xach-tay-hp-envy-13-ba1535tu-i7-1165g7', 5, 29899000, 28899000, 12, 3, null, 8, 2, null, 4, null, N'PIN liền, 42Wh Li-ion', N'SSD 512 GB', 1.4, N'https://images.fpt.shop/unsafe/fit-in/585x390/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/18/637360388031305676_637320742814213728_hp-envy-13-ba0046tu-vang-1.png', N'2021-11-05 06:35:21'),
(N'Laptop Gaming HP Pavilion 15 - CX0179TX (5EF42PA)', 3, 'laptop-gaming-hp-pavilion-15-cx0179tx-5ef42pa', 5, 24490000, 0, 12, 2, null, 8, 2, null, 3, null, N'3-Cell 52.5Whr', N'256GB SSD', 1.4, N'https://images.fpt.shop/unsafe/fit-in/585x390/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/7/28/637630622749883418_hp-pavilion-15-eg-bac-1.jpg', N'2021-11-05 06:35:21'),
(N'Laptop Dell Vostro 3400 70253900', 2, 'laptop-dell-vostro-3400-70253900', 4, 17990000, 0, 12, 2, null, 4, 1, null, 3, null, N'PIN liền, 42Wh Li-ion', N'SSD 256GB NVMe PCIe', 1.4, N'https://www.tnc.com.vn/uploads/product/sp2021/laptop-dell-vostro-3400-70253900-den.jpg', N'2021-10-05 06:35:21'),
(N'Laptop Dell Inspiron 14 5415 70262929', 4, 'may-tinh-xach-tay-dell-inspiron-14-5415-70262929', 4, 19999000, 18999000, 10, 5, null, 8, 1, null, 4, null, N'4Cell 54Whrs', N'256GB SSD', 1.0, N'https://cdn.nguyenkimmall.com/images/thumbnails/696/522/detailed/765/10050712-laptop-dell-inspiron-14-5415-r5-5500u-70262929-1.jpg', N'2021-10-15 06:35:21'),
(N'Laptop Dell Inspiron 3511 P112F001BBL (Black)', 3, 'laptop-dell-inspiron-3511-p112f001bbl-black', 4, 19290000, 0, 12, 2, null, 4, 1, null, 3, null, N'41WHrs', N'512GB M.2 PCIe Gen3 x4 NVMe SSD', 1.2, N'https://cdn.mediamart.vn/images/product/laptop-dell-inspiron-3511-p112f001bbl-wvwy51.png', N'2020-11-15 06:35:21'),
(N'Laptop Dell Vostro 3510 (P112F002ABL)', 4, 'laptop-dell-vostro-3510-p112f002abl', 4, 22790000, 0, 12, 2, null, 8, 5, null, 4, null, N'PIN liền', N'SSD 512 GB M.2 PCIe', 1.4, N'https://hanoicomputercdn.com/media/product/60643_laptop_dell_vostro_3510_p112f002abl_den_6.png', N'2021-07-07 06:35:21'),
(N'Laptop Gaming Asus ROG Zephyrus G15 GA503QM HQ158T', 1, 'laptop-gaming-asus-rog-zephyrus-g15-ga503qm-hq158t', 2, 43490000, 0, 13, 10, null, 16, 4, null, 4, null, N'90WHrs, 4S1P, 4-cell Li-ion', N'512GB M.2 NVMe™ PCIe® 3.0 SSD', 1.4, N'https://laptop88.vn/media/product/6615_asus_rog_zephyrus_g15_ga503qm_hq158t_3.jpg', N'2021-01-01 06:35:21'),
(N'Laptop Gaming Asus ROG Flow X13 GV301QC K6029T', 1, 'laptop-gaming-asus-rog-flow-x13-gv301qc-k6029t', 2, 79990000, 0, 17, 10, null, 32, 4, null, 5, null, N'62WHrs, 4S1P, 4-cell Li-ion', N'1TB M.2 2230 NVMe™ PCIe® 3.0 SSD', 1.7, N'https://hanoicomputercdn.com/media/product/60755_laptop_asus_gaming_rog_flow_13_gv301qc_k6029t_win10_but_tui_den_6.png', N'2021-01-15 07:15:21'),
(N'Laptop Gaming Asus ROG Zephyrus G15 GA503QC HN074T', 1, 'laptop-gaming-asus-rog-zephyrus-g15-ga503qc-hn074t', 2, 32790000, 0, 15, 10, null, 16, 4, null, 3, null, N'90WHrs, 4S1P, 4-cell Li-ion', N'512GB M.2 NVMe™ PCIe® 3.0', 1.7, N'https://product.hstatic.net/1000026716/product/g15-gray-02_e48d2db494d34fb0aa39cc331c3a2079.jpg', N'2020-12-04 16:35:21'),
(N'Laptop Gaming Asus TUF Dash F15 FX516PC HN002T', 1, 'laptop-gaming-asus-tuf-dash-f15-fx516pc-hn002t', 2, 25990000, 25190000, 13, 2, null, 8, 4, null, 5, null, N'4 Cell 76Whr', N'	512GB M.2 PCIe NVMe 3.0', 1.4, N'https://product.hstatic.net/1000026716/product/gnbps2d5a11_2decb521ed074efe9ede5cfaf9e7c23a.jpg', N'2021-8-11 12:15:2'),
(N'Laptop ASUS TUF Gaming F15 FX506HCB HN139T', 1, 'laptop-asus-tuf-gaming-f15-fx506hcb-hn139t', 2, 24390000, 0, 12, 2, null, 8, 4, null, 4, null, N'	3 Cell 48WHr', N'512GB SSD M.2 PCIE G3X2, 1x slot M.2', 1.4, N'https://product.hstatic.net/1000026716/product/gearvn-laptop-asus-tuf-gaming-f15-fx506hcb-hn139t-4_ba5e261b78d24f468429cb63b7aebb58.jpg', N'2021-10-12 08:35:21')
GO


--Sản phẩm nhập kho 32
INSERT INTO SanPhamNhapKho(MaSanPham, SoLuong, GiaNhap, NgayTao)
VALUES(1,10, 18500000,N'2021-11-05 05:35:21')
INSERT INTO SanPhamNhapKho(MaSanPham, SoLuong, GiaNhap, NgayTao)
VALUES(1,5, 18000000,N'2021-11-05 05:35:25')
INSERT INTO SanPhamNhapKho(MaSanPham, SoLuong, GiaNhap, NgayTao)
VALUES(2,5, 30490000,N'2021-11-05 05:35:50')
INSERT INTO SanPhamNhapKho(MaSanPham, SoLuong, GiaNhap, NgayTao)
VALUES(3,2, 40000000,N'2021-11-05 05:35:51'),
(4,9, 20500000,N'2021-11-05 05:35:51'),
(5,27, 15000000,N'2021-11-05 05:35:51'),
(6,34, 28990000,N'2021-11-05 05:35:51'),
(7,28, 23000000,N'2021-11-05 05:35:51'),
(8,27, 23000000,N'2021-11-05 05:35:51'),
(9,19, 29000000,N'2021-11-05 05:35:51'),
(10,6, 18000000,N'2021-11-05 05:35:51'),
(11,18, 20000000,N'2021-11-05 05:35:51'),
(12,31, 12000000,N'2021-11-05 05:35:51'),
(13,7, 12000000,N'2021-11-05 05:35:51'),
(14,10, 34000000,N'2021-11-05 05:35:51'),
(15,19, 9000000,N'2021-11-05 05:35:51'),
(16,42, 20000000,N'2021-11-05 05:35:51'),
(17,22, 19000000,N'2021-11-05 05:35:51'),
(18,12, 18000000,N'2021-11-05 05:35:51'),
(19,32, 9000000,N'2021-11-05 05:35:51'),
(20,60, 10000000,N'2021-11-05 05:35:51'),
(21,50, 36000000,N'2021-11-05 05:35:51'),
(22,31, 27000000,N'2021-11-05 05:35:51'),
(23,37, 22000000,N'2021-11-05 05:35:51'),
(24,49, 15000000,N'2021-11-05 05:35:51'),
(25,42, 17000000,N'2021-11-05 05:35:51'),
(26,26, 17090000,N'2021-11-05 05:35:51'),
(27,15, 20090000,N'2021-11-05 05:35:51'),
(28,40, 39490000,N'2021-11-05 05:35:51'),
(29,27, 75000000,N'2021-11-05 05:35:51'),
(30,34, 29790000,N'2021-11-05 05:35:51'),
(31,14, 23990000,N'2021-11-05 05:35:51'),
(32,20, 22000000,N'2021-11-05 05:35:51') 
GO


--Người dùng
INSERT INTO NguoiDung(Ten, Username, Password, SDT, DiaChi, MaQuyen, NgayTao)
VALUES(N'Trần Văn Chuyên',N'admin',N'202cb962ac59075b964b07152d234b70','0123456789',N'958 Phố Hằng, Xã Uyên Trưởng, Quận 53 Hồ Chí Minh',1,N'2021-11-05 05:35:21'),
(N'Trần Trùi Trụi',N'quanly',N'202cb962ac59075b964b07152d234b70','0111222333',N'28 Mac Dinh Chi Street, Dakao Ward, District 1',2,N'2021-11-05'),
(N'Nguyễn Thị Nở',N'nhanvien',N'202cb962ac59075b964b07152d234b70','0555222444',N'21 Huynh Tinh Cua StreetWard 8, District 3',3,N'2021-11-05'),
(N'Lâm Văn Hến',N'khach1',N'202cb962ac59075b964b07152d234b70','0987456321',N' 905 Lac Long Quan',4,N'2021-11-05'),
(N'Nguyễn Văn Ướt',N'khach2',N'202cb962ac59075b964b07152d234b70','0123456789',N'258 Ba Trieu',4,N'2021-11-05'),
(N'Lăng Ra Ngủ',N'khach3',N'202cb962ac59075b964b07152d234b70','0123456789',N'40B Street 3/2, Ninh Kieu District',4,N'2021-11-05')
GO
--Voucher
INSERT INTO PhieuQuaTang(TenPhieuQuaTang, MaGiamGia, LoaiPhamVi, GiaTri, GiaTriDonHangToiThieu, SoLuong, NgayTao, NgayKichHoat, NgayKetThuc, Status)
VALUES
(N'DELL Black friday','DELLBGD154',4,2000000,10000000,10,N'2021-12-15 05:35:51',N'2021-12-15 05:35:51',N'2022-01-01 05:35:51',2),
(N'APDPS Ưu đãi lớn','APDPSBS543',0,2500000,13000000,17,N'2021-12-15 05:35:51',N'2021-12-15 05:35:51',N'2022-02-01 05:35:51',2),
(N'APDPS Tri ân khách hàng','APDPS65742',0,1000000,8000000,20,N'2021-12-15 05:35:51',N'2021-11-13 05:35:51',N'2021-12-30 05:35:51',2),
(N'Sinh nhật MSI','MSIBD12345',8,1200000,13000000,2,N'2021-12-15 05:35:51',N'2021-12-15 05:35:51',N'2021-12-16 05:35:51',3),
(N'HP Ưu đãi khủng','HPS15348',5,2000000,10000000,21,N'2021-12-15 05:35:51',N'2021-12-15 05:35:51',N'2022-01-01 05:35:51',2),
(N'Lenovo sản phẩm HOT','LENOVOHOT156',6,1600000,17000000,31,N'2021-12-15 05:35:51',N'2021-12-01 05:35:51',N'2022-01-13 05:35:51',2),
(N'Asus 7 ngày ưu đãi','ASUS7D4521',2,1000000,10000000,5,N'2021-12-15 05:35:51',N'2021-12-15 05:35:51',N'2021-12-22 05:35:51',2)

GO
--Tin tức


select *from NguoiDung
select *from Quyen
select *from SanPham

SELECT * FROM CTHoaDon
SELECT * FROM HoaDon
SELECT * FROM DanhGia

UPDATE HoaDon
     SET TinhTrang = 4
	 WHERE MaHoaDon = 2

   