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
    TenQuyen NVARCHAR(50) NOT NULL,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
    IsLockEdit BIT DEFAULT 0,
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
    DiemThuong INT DEFAULT 0,
    MaQuyen INT NOT NULL DEFAULT 4,-- 1: admin && 2: quản lý && 3: nhân viên && 4: khách
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    Status BIT DEFAULT 1,--1: active, 2: lock, 3: deleted
    FOREIGN KEY (MaQuyen) REFERENCES dbo.Quyen(MaQuyen),
);

CREATE TABLE DanhMuc
(
    MaDanhMuc INT IDENTITY PRIMARY KEY,
    TenDanhMuc NVARCHAR(255) NOT NULL,
    Hinh NVARCHAR(255),
    SoLuong INT DEFAULT 0,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
    IsLockEdit BIT DEFAULT 0,
);

CREATE TABLE ThuongHieu
(
    MaThuongHieu INT IDENTITY PRIMARY KEY,
    TenThuongHieu NVARCHAR(255) NOT NULL,
    Hinh NVARCHAR(255) NOT NULL,
    SoLuong INT DEFAULT 0,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
    IsLockEdit BIT DEFAULT 0,
);

CREATE TABLE CPU
(
    MaCPU INT IDENTITY PRIMARY KEY,
    BrandModifier NVARCHAR(255),
    TenCPU NVARCHAR(255) NOT NULL,
    HangCPU NVARCHAR(255) NOT NULL,--Intel, AMD, Apple
    SoLuong INT DEFAULT 0,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE GPU
(
    MaGPU INT IDENTITY PRIMARY KEY,
    TenGPU NVARCHAR(255),
    LoaiGPU NVARCHAR(255) NOT NULL,--NVIDIA GeForce, AMD Radeon, Card Onboard
    SoLuong INT DEFAULT 0,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE OCung
(
    MaOCung INT IDENTITY PRIMARY KEY,
    LoaiOCung INT DEFAULT 1,--1: SSD, 2: HDD
    DungLuong INT DEFAULT 128,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
);

CREATE TABLE SanPham
(
    MaSanPham INT IDENTITY PRIMARY KEY,
    TenSanPham NVARCHAR(255) NOT NULL,
    MaDanhMuc INT NOT NULL,
    MaThuongHieu INT NOT NULL,
    SoLuong INT DEFAULT 0,
    GiaNhap INT DEFAULT 0,
    GiaBan INT DEFAULT 0,
    GiaGiam INT DEFAULT 0,
    PhanTramGiamGia INT DEFAULT 0,
    DiemRate INT DEFAULT 0,
    SoLuotRate INT DEFAULT 0,
    BaoHanh INT NOT NULL,
    MaCPU INT NOT NULL,
    RAM NVARCHAR(255) NOT NULL,--4, 8, 16, 32
    MaGPU INT NOT NULL,
    ManHinh NVARCHAR(255) NOT NULL,
    Pin NVARCHAR(255) NOT NULL,
    MaOCung INT NOT NULL,
    Hinh NVARCHAR(255) NOT NULL,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsHide BIT DEFAULT 0,
    FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DanhMuc(MaDanhMuc),
    FOREIGN KEY (MaThuongHieu) REFERENCES dbo.ThuongHieu(MaThuongHieu),
    FOREIGN KEY (MaCPU) REFERENCES dbo.CPU(MaCPU),
    FOREIGN KEY (MaGPU) REFERENCES dbo.GPU(MaGPU),
    FOREIGN KEY (MaOCung) REFERENCES dbo.OCung(MaOCung),
);


CREATE TABLE Hinh
(
    MaHinh INT IDENTITY PRIMARY KEY,
    MaSanPham INT NOT NULL,
    TenHinh NVARCHAR(255) NOT NULL,
    [Path] NVARCHAR(255) NOT NULL,
    NgayTao DATETIME NOT NUll,
    NgayChinhSua DATETIME,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
);

CREATE TABLE PhieuQuaTang
(
    MaPhieuQuaTang INT IDENTITY PRIMARY KEY,
    GiaTri INT DEFAULT 0,
    TongSoLuong INT DEFAULT 0,
    SoLuongConLai INT DEFAULT 0,
    NgayTao DATETIME NOT NULL,
    NgayKichHoat DATETIME NOT NULL,
    NgayKetThuc DATETIME NOT NULL,
    IsActive BIT DEFAULT 0,
    DiemThuongDoi INT DEFAULT 0,
);

CREATE TABLE HoaDon
(
    MaHoaDon INT IDENTITY PRIMARY KEY,
    NgayMua DATETIME NOT NULL,
    MaKhachHang INT NOT NULL,
    MaPhieuQuaTang INT,
    SoTienGiam INT DEFAULT 0,
    PhiGiaoHang INT,
    TinhTrang INT DEFAULT 1,--1: Chờ xác nhận, 2: Chờ lấy hàng, 3: Đang giao, 4: Đã giao, 5: Đã hủy, 6: Trả hàng
    TongThanhToan INT DEFAULT 0,
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaPhieuQuaTang) REFERENCES dbo.PhieuQuaTang(MaPhieuQuaTang),
);

CREATE TABLE CTHoaDon
(
    MaCTHoaDon INT IDENTITY PRIMARY KEY,
    MaHoaDon INT NOT NULL,
    MaSanPham INT NOT NULL,
    DonGia INT NOT NULL,
    SoLuong INT NOT NULL,
    ThanhTien INT NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon),
);

CREATE TABLE Comment
(
    MaComment INT IDENTITY PRIMARY KEY,
    MaSanPham INT NOT NULL,
    MaKhachHang INT NOT NULL,
    NoiDung NVARCHAR(255) NOT NULL,
    NgayTao DATETIME NOT NULL,
    NgayChinhSua DATETIME NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
);

CREATE TABLE Rate
(
    MaRate INT IDENTITY PRIMARY KEY,
    MaSanPham INT NOT NULL,
    MaKhachHang INT NOT NULL,
    Diem INT NOT NULL,
    NgayTao DATETIME NOT NULL,
    NgayChinhSua DATETIME NOT NULL,
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham),
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.NguoiDung(MaNguoiDung),
);

GO


CREATE PROC USP_Login
@username VARCHAR(100), @password VARCHAR(100)
AS
BEGIN
	SELECT * FROM dbo.NguoiDung WHERE Username = @username AND Password = @password AND Status = 1
END
GO

CREATE PROC USP_Register
@ten NVARCHAR(100), @username VARCHAR(100), @password VARCHAR(100), @diaChi NVARCHAR(100), @sdt VARCHAR(100), @email VARCHAR(100)
AS
BEGIN
	IF @diaChi ='' SET @diaChi = null
	IF @sdt ='' SET @sdt = null
	IF @email ='' SET @email = null
	INSERT NguoiDung(Ten, Username, Password, DiaChi, SDT, Email, NgayTao)
	VALUES(@ten, @username, @password, @diaChi, @sdt, @email, GETDATE())
END
GO



--Quyền
INSERT INTO Quyen
    (TenQuyen, NgayTao, IsLockEdit)
VALUES(N'admin', '2021-11-05', 1),
    (N'quản lý', '2021-11-05', 1),
    (N'nhân viên', '2021-11-05', 1),
    (N'khách hàng', '2021-11-05', 1)
GO

--Danh mục
INSERT INTO DanhMuc
    (TenDanhMuc, Hinh, NgayTao, IsDeleted, IsLockEdit)
VALUES(N'Gaming đồ họa', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352691284513627_Group%2073@3x.png', N'2021-11-05', 0, 1),--1
    (N'Doanh nhân', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352693282588825_Group%2073@3x.png', N'2021-11-05', 0, 1),--2
    (N'Mỏng nhẹ', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352694501010620_Group%2073@3x.png', N'2021-11-05', 0, 1),--3
    (N'Sinh viên', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/10/637353308751863585_Img@3x.png', N'2021-11-05', 0, 1),--4
    (N'Văn phòng', N'https://images.fpt.shop/unsafe/fit-in/125x125/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/9/9/637352695747985641_Group%2073@3x.png', N'2021-11-05', 0, 1)--5
GO

--Thương hiệu
INSERT INTO ThuongHieu
    (TenThuongHieu, Hinh, NgayTao, IsDeleted, IsLockEdit)
VALUES(N'Macbook', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494668267616_Macbook-Apple@2x.jpg', N'2021-11-05', 0, 1),--1
    (N'Asus', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666861275_Asus.jpg', N'2021-11-05', 0, 1),--2
    (N'Acer nhẹ', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666704995_Acer@2x.jpg', N'2021-11-05', 0, 1),--3
    (N'Dell', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494666861275_Dell@2x.jpg', N'2021-11-05', 0, 1),--4
    (N'HP', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/7/15/637619564183327279_HP@2x.png', N'2021-11-05', 0, 1),--5
    (N'Lenovo', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340494668267616_Lenovo@2x.jpg', N'2021-11-05', 0, 1),--6
    (N'Microsoft', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2021/1/13/637461259692529909_Microsoft@2x.png', N'2021-11-05', 0, 1),--7
    (N'MSI', N'https://images.fpt.shop/unsafe/fit-in/108x40/filters:quality(90):fill(white)/fptshop.com.vn/Uploads/Originals/2020/8/26/637340493755614653_MSI@2x.jpg', N'2021-11-05', 0, 1)--8
GO

--CPU
INSERT INTO CPU
    (BrandModifier, TenCPU, HangCPU, NgayTao)
VALUES(N'i9', N'11900K', N'Intel', N'2021-11-05'),--1
    (N'i9', N'11900KF', N'Intel', N'2021-11-05'),--2
    (N'i7', N'11700K', N'Intel', N'2021-11-05'),--3
    (N'i7', N'11700KF', N'Intel', N'2021-11-05'),--4
    (N'i5', N'11600K', N'Intel', N'2021-11-05'),--5
    (N'i5', N'11600KF', N'Intel', N'2021-11-05'),--6
    (N'i3', N'10300T', N'Intel', N'2021-11-05'),--7
    (N'i3', N'10100', N'Intel', N'2021-11-05'),--8
    (null, N'Ryzen 3 5300U', N'AMD', N'2021-11-05'),--9
    (null, N'Ryzen 3 5400U', N'AMD', N'2021-11-05'),--10
    (null, N'Ryzen 5 5500U', N'AMD', N'2021-11-05'),--11
    (null, N'Ryzen 5 5600X', N'AMD', N'2021-11-05'),--12
    (null, N'Ryzen 7 5700G', N'AMD', N'2021-11-05'),--13
    (null, N'Ryzen 7 5800H', N'AMD', N'2021-11-05'),--14
    (null, N'Ryzen 9 5900', N'AMD', N'2021-11-05'),--15
    (null, N'Ryzen 9 5900HS', N'AMD', N'2021-11-05'),--16
    (null, N'M1', N'Apple', N'2021-11-05'),--17
    (null, N'M1 Pro', N'Apple', N'2021-11-05'),--18
    (null, N'M1 Pro Max', N'Apple', N'2021-11-05')--19
GO

--GPU
INSERT INTO GPU
    (TenGPU, LoaiGPU,--NVIDIA GeForce, AMD Radeon, Card Onboard
    NgayTao)
VALUES(null, N'Card Onboard', N'2021-11-05'),--1
    (N'RX 540X', N'AMD Radeon', N'2021-11-05'),--2
    (N'RX 550X', N'AMD Radeon', N'2021-11-05'),--3
    (N'RX 560X', N'AMD Radeon', N'2021-11-05'),--4
    (N'RX 570X', N'AMD Radeon', N'2021-11-05'),--5
    (N'RX 580X', N'AMD Radeon', N'2021-11-05'),--6
    (N'RTX3070 8GB', N'NVIDIA GeForce', N'2021-11-05'),--7
    (N'RTX3050 4GB', N'NVIDIA GeForce', N'2021-11-05'),--8
    (N'RTX3060 6GB', N'NVIDIA GeForce', N'2021-11-05'),--9
    (N'RTX 3060 Max-Q 6GB', N'NVIDIA GeForce', N'2021-11-05'),--10
    (N'RTX 2070 8GB', N'NVIDIA GeForce', N'2021-11-05'),--11
    (N'GTX 1650 4G-GDDR6', N'NVIDIA GeForce', N'2021-11-05'),--12
    (N'MX450 2GB', N'NVIDIA GeForce', N'2021-11-05')--13
GO

--OCung
INSERT INTO OCung (LoaiOCung, DungLuong, NgayTao)
VALUES(1, 128, N'2021-11-05'),--1
(1, 256, N'2021-11-05'),--2
(1, 512, N'2021-11-05'),--3
(1, 1024, N'2021-11-05')--4
GO



--Sản phẩm
INSERT INTO SanPham
    (TenSanPham, MaDanhMuc, MaThuongHieu, SoLuong, GiaNhap, GiaBan, BaoHanh, MaCPU, RAM, MaGPU, ManHinh,Pin, MaOCung, Hinh, NgayTao)
VALUES(N'Laptop Gaming Acer Aspire 7 A715 42G R1SB', 1, 2, 5, 18000000, 19990000, 12, 11, 8, 12, N'15.6" FHD (1920 x 1080) IPS, Anti-Glare, 144Hz', N'4 Cell 48Whr', 2, N'https://product.hstatic.net/1000026716/product/laptop_gaming_acer_aspire_7_a715_42g_r1sb_9520016e22274791bb4e1697764c57c4.jpg', N'2021-11-05'),
    (N'Laptop gaming Lenovo Legion 5 Pro 16ACH6H 82JQ005YVN', 1, 6, 5, 45000000, 49990000, 24, 14, 16, 7, N'16.0 inch WQXGA (2560x1600) IPS 500nits Anti-glare, 165Hz, 100% sRGB, Dolby Vision, HDR 400, Free-Sync, G-Sync, DC dimmer', N'80Whrs', 4, N'https://phucanhcdn.com/media/product/43536_lap_len_leg5p82jq005yvn_a.jpg', N'2021-11-05'),
    (N'Laptop Apple MacBook Pro M1 2020 8GB/256GB (MYD82SA/A)', 3, 1, 5, 30000000, 33490000, 12, 17, 8, 1, N'13.3 inch', N'Built‑in 58.2‑watt‑hour lithium‑polymer battery, 61W USB‑C Power Adapter', 2, N'https://lh3.googleusercontent.com/6iW6tc0lHp4paCYznq-gC5mEXEGMSBmrSq2I4MaXdPne5XWQI4l8m-bGRVCRFH94d4PEqtUIdH3FERr-VNDWaT2k9qcZ5Ey_PQ=w1000-rw', N'2021-11-05')
	

INSERT INTO NguoiDung(Ten, Username, Password, MaQuyen, NgayTao)
VALUES(N'admin',N'admin',N'123',1,N'2021-11-05'),
(N'quanly',N'quanly',N'123',2,N'2021-11-05'),
(N'nhanvien',N'nhanvien',N'123',3,N'2021-11-05'),
(N'khach1',N'khach1',N'123',4,N'2021-11-05'),
(N'khach2',N'khach2',N'123',4,N'2021-11-05'),
(N'khach3',N'khach3',N'123',4,N'2021-11-05')
GO
select *from NguoiDung