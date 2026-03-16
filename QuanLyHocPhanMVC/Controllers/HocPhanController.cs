using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using QuanLyHocPhanMVC.Models;

namespace QuanLyHocPhanMVC.Controllers
{
    public class HocPhanController : Controller
    {
        string connectionString = @"Server=DESKTOP-AHVKPFM\SQLEXPRESS01;Database=QuanLyHocPhan;Trusted_Connection=True;";

        public IActionResult Index(string search, int page = 1)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<HocPhan> list = new List<HocPhan>();

            int pageSize = 10;
            int start = (page - 1) * pageSize;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query;

                if (string.IsNullOrEmpty(search))
                {
                    query = @"SELECT * FROM HocPhan
                              ORDER BY IDHocPhan
                              OFFSET @start ROWS
                              FETCH NEXT @size ROWS ONLY";
                }
                else
                {
                    query = @"SELECT * FROM HocPhan
                              WHERE TenHocPhan LIKE @search
                              OR MaHocPhan LIKE @search
                              ORDER BY IDHocPhan
                              OFFSET @start ROWS
                              FETCH NEXT @size ROWS ONLY";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@size", pageSize);
                if (!string.IsNullOrEmpty(search))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HocPhan hp = new HocPhan();

                    hp.IDHocPhan = (int)reader["IDHocPhan"];
                    hp.MaHocPhan = reader["MaHocPhan"].ToString();
                    hp.TenHocPhan = reader["TenHocPhan"].ToString();
                    hp.SoTinChi = (int)reader["SoTinChi"];
                    hp.MoTa = reader["MoTa"].ToString();

                    list.Add(hp);
                }
            }

            ViewBag.Page = page;

            int totalRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HocPhan", conn);

                totalRows = (int)cmd.ExecuteScalar();
            }

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRows / pageSize);

            return View(list);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(HocPhan hp)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO HocPhan
                                (MaHocPhan,TenHocPhan,SoTinChi,MoTa)
                                VALUES
                                (@ma,@ten,@stc,@mota)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ma", hp.MaHocPhan);
                cmd.Parameters.AddWithValue("@ten", hp.TenHocPhan);
                cmd.Parameters.AddWithValue("@stc", hp.SoTinChi);
                cmd.Parameters.AddWithValue("@mota", hp.MoTa);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            HocPhan hp = new HocPhan();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM HocPhan WHERE IDHocPhan=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    hp.IDHocPhan = (int)reader["IDHocPhan"];
                    hp.MaHocPhan = reader["MaHocPhan"].ToString();
                    hp.TenHocPhan = reader["TenHocPhan"].ToString();
                    hp.SoTinChi = (int)reader["SoTinChi"];
                    hp.MoTa = reader["MoTa"].ToString();
                }
            }

            return View(hp);
        }

        [HttpPost]
        public IActionResult Edit(HocPhan hp)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"UPDATE HocPhan
                                SET MaHocPhan=@ma,
                                    TenHocPhan=@ten,
                                    SoTinChi=@stc,
                                    MoTa=@mota
                                WHERE IDHocPhan=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", hp.IDHocPhan);
                cmd.Parameters.AddWithValue("@ma", hp.MaHocPhan);
                cmd.Parameters.AddWithValue("@ten", hp.TenHocPhan);
                cmd.Parameters.AddWithValue("@stc", hp.SoTinChi);
                cmd.Parameters.AddWithValue("@mota", hp.MoTa);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM HocPhan WHERE IDHocPhan=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public IActionResult DanhSach(string search, int page = 1)
        {
            List<HocPhan> list = new List<HocPhan>();

            int pageSize = 10;
            int start = (page - 1) * pageSize;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query;

                if (string.IsNullOrEmpty(search))
                {
                    query = @"SELECT * FROM HocPhan
                      ORDER BY IDHocPhan
                      OFFSET @start ROWS
                      FETCH NEXT @size ROWS ONLY";
                }
                else
                {
                    query = @"SELECT * FROM HocPhan
                      WHERE TenHocPhan LIKE @search
                      OR MaHocPhan LIKE @search
                      ORDER BY IDHocPhan
                      OFFSET @start ROWS
                      FETCH NEXT @size ROWS ONLY";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@size", pageSize);

                if (!string.IsNullOrEmpty(search))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HocPhan hp = new HocPhan();

                    hp.IDHocPhan = (int)reader["IDHocPhan"];
                    hp.MaHocPhan = reader["MaHocPhan"].ToString();
                    hp.TenHocPhan = reader["TenHocPhan"].ToString();
                    hp.SoTinChi = (int)reader["SoTinChi"];
                    hp.MoTa = reader["MoTa"].ToString();

                    list.Add(hp);
                }
            }

            int totalRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string countQuery = "SELECT COUNT(*) FROM HocPhan";

                SqlCommand cmd = new SqlCommand(countQuery, conn);

                totalRows = (int)cmd.ExecuteScalar();
            }

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            ViewBag.Page = page;

            return View(list);
        }
        public IActionResult ChiTiet(int id)
        {
            HocPhan hp = new HocPhan();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM HocPhan WHERE IDHocPhan=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    hp.IDHocPhan = (int)reader["IDHocPhan"];
                    hp.MaHocPhan = reader["MaHocPhan"].ToString();
                    hp.TenHocPhan = reader["TenHocPhan"].ToString();
                    hp.SoTinChi = (int)reader["SoTinChi"];
                    hp.MoTa = reader["MoTa"].ToString();
                }
            }

            return View(hp);
        }
    }
}