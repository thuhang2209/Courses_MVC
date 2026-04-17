using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace QuanLyHocPhanMVC.Controllers
{
    public class DashboardController : Controller
    {
        string connectionString = @"Server=DESKTOP-AHVKPFM\SQLEXPRESS01;Database=QuanLyHocPhan;Trusted_Connection=True;";

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userRole = HttpContext.Session.GetString("Role");
            string username = HttpContext.Session.GetString("Username");

            ViewBag.Username = username;
            ViewBag.Role = userRole;

            if (userRole == "Admin")
            {
                LoadAdminDashboard();
            }
            else if (userRole == "GiaoVien")
            {
                LoadTeacherDashboard();
            }

            return View();
        }

        private void LoadAdminDashboard()
        {
            int tongHocPhan = 0;
            int tongTinChi = 0;
            int tongNguoiDung = 0;
            int soAdmin = 0;
            int soGiaoVien = 0;

            List<int> tinChi = new List<int>();
            List<int> soLuong = new List<int>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Tổng học phần
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM HocPhan", conn);
                tongHocPhan = (int)cmd1.ExecuteScalar();

                // Tổng tín chỉ
                SqlCommand cmd2 = new SqlCommand("SELECT SUM(SoTinChi) FROM HocPhan", conn);
                var result = cmd2.ExecuteScalar();
                tongTinChi = result != null ? (int)result : 0;

                // Tổng người dùng
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM NguoiDung", conn);
                tongNguoiDung = (int)cmd3.ExecuteScalar();

                // Số Admin
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM NguoiDung WHERE Role='Admin'", conn);
                soAdmin = (int)cmd4.ExecuteScalar();

                // Số Giáo viên
                SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM NguoiDung WHERE Role='GiaoVien'", conn);
                soGiaoVien = (int)cmd5.ExecuteScalar();

                // Thống kê số môn theo tín chỉ
                SqlCommand cmd6 = new SqlCommand(@"
                SELECT SoTinChi, COUNT(*) as SoLuong
                FROM HocPhan
                GROUP BY SoTinChi
                ORDER BY SoTinChi
                ", conn);

                SqlDataReader reader = cmd6.ExecuteReader();

                while (reader.Read())
                {
                    tinChi.Add(Convert.ToInt32(reader["SoTinChi"]));
                    soLuong.Add(Convert.ToInt32(reader["SoLuong"]));
                }
            }

            ViewBag.TongHocPhan = tongHocPhan;
            ViewBag.TongTinChi = tongTinChi;
            ViewBag.TongNguoiDung = tongNguoiDung;
            ViewBag.SoAdmin = soAdmin;
            ViewBag.SoGiaoVien = soGiaoVien;

            ViewBag.TinChi = tinChi;
            ViewBag.SoLuong = soLuong;

            ViewBag.IsAdmin = true;
        }

        private void LoadTeacherDashboard()
        {
            int tongHocPhan = 0;
            int tongTinChi = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Tổng học phần
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM HocPhan", conn);
                tongHocPhan = (int)cmd1.ExecuteScalar();

                // Tổng tín chỉ
                SqlCommand cmd2 = new SqlCommand("SELECT SUM(SoTinChi) FROM HocPhan", conn);
                var result = cmd2.ExecuteScalar();
                tongTinChi = result != null ? (int)result : 0;
            }

            ViewBag.TongHocPhan = tongHocPhan;
            ViewBag.TongTinChi = tongTinChi;
            ViewBag.IsAdmin = false;
        }
    }
}