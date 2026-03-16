using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace QuanLyHocPhanMVC.Controllers
{
    public class DashboardController : Controller
    {
        string connectionString = @"Server=DESKTOP-AHVKPFM\SQLEXPRESS01;Database=QuanLyHocPhan;Trusted_Connection=True;";

        public IActionResult Index()
        {
            int tongHocPhan = 0;
            int tongTinChi = 0;

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
                tongTinChi = (int)cmd2.ExecuteScalar();

                // Thống kê số môn theo tín chỉ
                SqlCommand cmd3 = new SqlCommand(@"
                SELECT SoTinChi, COUNT(*) as SoLuong
                FROM HocPhan
                GROUP BY SoTinChi
                ORDER BY SoTinChi
                ", conn);

                SqlDataReader reader = cmd3.ExecuteReader();

                while (reader.Read())
                {
                    tinChi.Add(Convert.ToInt32(reader["SoTinChi"]));
                    soLuong.Add(Convert.ToInt32(reader["SoLuong"]));
                }
            }

            ViewBag.TongHocPhan = tongHocPhan;
            ViewBag.TongTinChi = tongTinChi;

            ViewBag.TinChi = tinChi;
            ViewBag.SoLuong = soLuong;

            return View();
        }
    }
}