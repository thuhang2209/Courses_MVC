# QuanLyHocPhanMVC

Dự án ASP.NET Core MVC quản lý học phần cho môn Lập trình Web.

## Những gì đã nâng cấp
- Thêm phân quyền `Admin` và `GiaoVien` bằng session.
- Tách trang `Môn học của tôi` cho giáo viên để chỉ sửa mô tả các học phần được phân công.
- Thêm quản lý phân công giảng dạy nhiều-nhiều giữa giáo viên và học phần.
- Thêm màn hình quản lý người dùng cho Admin.
- Nâng cấp dashboard thống kê và hỗ trợ xuất CSV.
- Làm mới giao diện trang chủ, cho phép bấm trực tiếp vào card `Quản lý Học phần` và `Thống kê dữ liệu`.
- Thêm validation cho form tạo/sửa và tách riêng form đăng nhập để không ảnh hưởng mật khẩu cũ.

## Chức năng chính
- Đăng nhập, đăng xuất theo session.
- Quản lý học phần: xem, thêm, sửa, xóa, tìm kiếm, phân trang.
- Quản lý người dùng cho Admin.
- Quản lý phân công giảng dạy.
- Dashboard thống kê theo role và xuất dữ liệu.
- Giáo viên chỉ được sửa mô tả của học phần mình phụ trách.

## Dữ liệu chính
- `NguoiDung`: tài khoản người dùng, role `Admin` hoặc `GiaoVien`.
- `HocPhan`: thông tin học phần.
- `PhanCongHocPhan`: bảng trung gian phân công giáo viên phụ trách học phần.

## Script cần thiết
- `Script_PhanCongHocPhan.sql`: tạo bảng phân công nhiều-nhiều và dữ liệu mẫu.

## Cách chạy
1. Tạo database `QuanLyHocPhan` trong SQL Server / LocalDB.
2. Chạy các script tạo bảng và dữ liệu mẫu.
3. Kiểm tra connection string nếu cần.
4. Mở `QuanLyHocPhanMVC.sln` và chạy project.


## Ghi chú
- Project đang dùng ADO.NET với SQL Server.
- Phân quyền của giáo viên chỉ giới hạn ở việc cập nhật mô tả học phần được phân công.
