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
    MaNguoiDung BIGINT IDENTITY PRIMARY KEY,
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
    MaSanPham BIGINT IDENTITY PRIMARY KEY,
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
    MaSanPhamNhapKho BIGINT IDENTITY PRIMARY KEY,
    MaSanPham BIGINT  NOT NULL,
    SoLuong INT DEFAULT 0,
    GiaNhap FLOAT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE Hinh
(
    MaHinh BIGINT IDENTITY PRIMARY KEY,
    MaSanPham BIGINT NOT NULL,
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
    NoiDung NVARCHAR(255) NOT NULL,
    MaSanPham BIGINT,
    MaNguoiTao BIGINT NOT NULL,
    MaNguoiSua BIGINT,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME,
    IsHide BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE PhieuQuaTang
(
    MaPhieuQuaTang BIGINT IDENTITY PRIMARY KEY,
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

CREATE TABLE NguoiDung_PhieuQuaTang
(
    MaNguoiDung_PhieuQuaTang BIGINT IDENTITY PRIMARY KEY,
    MaPhieuQuaTang BIGINT,
    MaNguoiDung BIGINT,
    FOREIGN KEY (MaPhieuQuaTang) REFERENCES dbo.PhieuQuaTang(MaPhieuQuaTang),
    FOREIGN KEY (MaNguoiDung) REFERENCES dbo.NguoiDung(MaNguoiDung),
);

CREATE TABLE HoaDon
(
    MaHoaDon INT IDENTITY PRIMARY KEY,
    NgayMua DATETIME NOT NULL,
    MaKhachHang BIGINT NOT NULL,
    TenKhach NVARCHAR(255) NOT NULL, 
    DiaChi NVARCHAR(255), 
    SDT VARCHAR(15) NOT NULL, 
    HinhThucThanhToan INT DEFAULT 1,--1: COD, 2: paypal 
    HinhThucGiaoHang INT DEFAULT 1,--1: tại quầy, 2: tại địa chỉ 
    MaNVDuyet BIGINT,
    NgayTao DATETIME DEFAULT GETDATE(),
    MaPhieuQuaTang BIGINT,
    SoTienGiam FLOAT DEFAULT 0,
    PhiGiaoHang INT,
    TinhTrang INT DEFAULT 1,--1: Chờ xác nhận, 2: Chờ lấy hàng, 3: Đang giao, 4: Đã giao, 5: Đã hủy, 6: Trả hàng
    TongThanhToan INT DEFAULT 0,
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaNVDuyet) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaPhieuQuaTang) REFERENCES dbo.PhieuQuaTang(MaPhieuQuaTang),
);

CREATE TABLE CTHoaDon
(
    MaCTHoaDon BIGINT IDENTITY PRIMARY KEY,
    MaHoaDon INT NOT NULL,
    MaSanPham BIGINT NOT NULL,
    DonGia FLOAT NOT NULL,
    SoLuong INT NOT NULL,
    ThanhTien INT NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon),
);

CREATE TABLE DanhGia
(
    MaDanhGia BIGINT IDENTITY PRIMARY KEY,
    MaSanPham BIGINT NOT NULL,
    MaKhachHang BIGINT NOT NULL,
    Diem INT NOT NULL,
    NoiDung NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayChinhSua DATETIME NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
);
CREATE TABLE Banner
(
    MaBanner INT IDENTITY PRIMARY KEY,
    TenBanner NVARCHAR(255) NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,
    MaSanPham BIGINT  NOT NULL,
    IsHide BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgaySua DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);
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
    (N'M1 Pro Max', N'Apple', N'2021-11-05')--9
GO

--GPU
INSERT INTO LoaiGPU(TenLoaiGPU, NgayTao)
VALUES(N'Card Onboard', N'2021-11-05'),--1
    (N'AMD Radeon', N'2021-11-05'),--2
    (N'NVIDIA GeForce GTX', N'2021-11-05'),--3
    (N'NVIDIA GeForce RTX', N'2021-11-05')--4
GO

--SizeManHinh
INSERT INTO SizeManHinh(Size, NgayTao)
VALUES(15.6, N'2021-11-05'),--1
(17.3, N'2021-11-05'),--2
(14, N'2021-11-05'),--3
(13.3, N'2021-11-05'),--4
(16, N'2021-11-05')--5

--Sản phẩm
INSERT INTO SanPham(TenSanPham, MaDanhMuc, SeoTitle, MaThuongHieu, SoLuongTon, GiaBan, GiaGiam, BaoHanh,  MaLoaiCPU, CPUInfo, RAM, MaLoaiGPU, GPUInfo, SizeManHinh, ManHinh, Pin, LuuTru, KhoiLuong, Hinh, NgayTao)
VALUES(N'Laptop Gaming Acer Aspire 7 A715 42G R1SB', 1, 'laptop-gaming-acer-aspire-7-a715-42g-r1sb', 3, 15, 19990000, 0, 12, 5, N'5500U', 8, 3, N'1650 4GB GDDR6', 1, N'FHD (1920 x 1080) IPS, Anti-Glare, 144Hz', N'4 Cell 48Whr', N'256GB PCIe® NVMe™ M.2 SSD', 2.1, N'https://product.hstatic.net/1000026716/product/laptop_gaming_acer_aspire_7_a715_42g_r1sb_9520016e22274791bb4e1697764c57c4.jpg', N'2021-11-05'),
(N'Laptop Apple MacBook Pro M1 2020 8GB/256GB (MYD82SA/A)', 4, 'apple-macbook-pro-2020-m1-myd82saa', 1, 5, 33490000, 0, 12, 7, null, 8, 1, null, 4, null, N'Khoảng 10 tiếng', N'256 GB SSD', 1.4, N'https://lh3.googleusercontent.com/6iW6tc0lHp4paCYznq-gC5mEXEGMSBmrSq2I4MaXdPne5XWQI4l8m-bGRVCRFH94d4PEqtUIdH3FERr-VNDWaT2k9qcZ5Ey_PQ=w1000-rw', N'2021-11-05'),
(N'Laptop gaming Lenovo Legion 5 Pro 16ACH6H 82JQ005YVN', 1, 'laptop-gaming-lenovo-legion-5-pro-16ach6h-82jq005yvn', 6, 2, 49990000, 40000000, 24, 6, N'5800H 3.2GHz upto 4.4GHz, 8 cores 16 threads', 16, 4, N'3070 8GB GDDR6', 5, N'WQXGA (2560x1600) IPS 500nits Anti-glare, 165Hz, 100% sRGB, Dolby Vision, HDR 400, Free-Sync, G-Sync, DC dimmer', N'4 Cell 80 WHr', N'1TB SSD M.2 2280 PCIe 3.0x4 NVMe', 2.45, N'https://product.hstatic.net/1000026716/product/laptop_gaming_lenovo_legion_5_pro_16ach6h_82jq005yvn_fea32779d7ae4bfd8c9536fd44358354.jpg', N'2021-11-05')
GO


--Sản phẩm nhập kho
INSERT INTO SanPhamNhapKho(MaSanPham, SoLuong, GiaNhap, NgayTao)
VALUES(1,10, 18500000,N'2021-11-05 05:35:21'),
(1,5, 18000000,N'2021-11-05 05:35:25'),
(2,5, 29000000,N'2021-11-05 05:35:50'),
(3,2, 39000000,N'2021-11-05 05:35:51')
GO


--Người dùng
INSERT INTO NguoiDung(Ten, Username, Password, MaQuyen, NgayTao)
VALUES(N'admin',N'admin',N'202cb962ac59075b964b07152d234b70',1,N'2021-11-05 05:35:21'),
(N'quanly',N'quanly',N'202cb962ac59075b964b07152d234b70',2,N'2021-11-05'),
(N'nhanvien',N'nhanvien',N'202cb962ac59075b964b07152d234b70',3,N'2021-11-05'),
(N'khach1',N'khach1',N'202cb962ac59075b964b07152d234b70',4,N'2021-11-05'),
(N'khach2',N'khach2',N'202cb962ac59075b964b07152d234b70',4,N'2021-11-05'),
(N'khach3',N'khach3',N'202cb962ac59075b964b07152d234b70',4,N'2021-11-05')
GO

select *from NguoiDung
select *from SanPham

SELECT * FROM CTHoaDon