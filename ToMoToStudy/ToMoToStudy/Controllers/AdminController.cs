using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToMoToStudy.Controllers
{
    public class AdminController : Controller
    {
        ToMoToDBEntities db = new ToMoToDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            return View();
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(NguoiDung nguoidung)
        {
            if (db is null) return HttpNotFound();
            if (String.IsNullOrEmpty(nguoidung.TaiKhoan) || String.IsNullOrEmpty(nguoidung.MatKhau))
            {
                ViewBag.Error = "Không được để trống";
                return View("AdminLogin");
            }
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var adlogin = db.NguoiDungs.Where(x => x.TaiKhoan.Equals(nguoidung.TaiKhoan.ToLower().Trim()) && x.MatKhau.Equals(nguoidung.MatKhau)).FirstOrDefault();
                if (adlogin is null)
                {
                    ViewBag.Error = "Sai thông tin đăng nhập";
                    return View("AdminLogin");
                }
                Session["AdminLogin"] = adlogin;
                int idRole = ((NguoiDung)Session["AdminLogin"]).IdVaiTro;
                if (idRole != 1)
                {
                    ViewBag.Error = "Không đủ quyền truy cập !!!";
                    return View("AdminLogin");
                }
                return RedirectToAction("Index", "Admin");
            }
        }
        public ActionResult AdminLogout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        
        public ActionResult DanhSachVaiTro()
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            return View();
        }
        public ActionResult DanhSachNguoiDung()
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");

            return View();
        }
        public ActionResult ChiTietNguoiDung(int id)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var taikhoan = db.NguoiDungs.Where(x => x.IdNguoiDung == id).FirstOrDefault();
                if (taikhoan is null) return HttpNotFound();
                return View(taikhoan);

            }
        }
        [HttpPost]
        public ActionResult ChiTietNguoiDung(int id, NguoiDung data)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(data.Email);
            }
            catch (FormatException)
            {
                ViewBag.Error = "Email không hợp lệ";
                return View("ChiTietNguoiDung", data);
            }
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var taikhoan = db.NguoiDungs.Where(x => x.IdNguoiDung == id).FirstOrDefault();
                if (taikhoan is null) return HttpNotFound();
                taikhoan.IdVaiTro = data.IdVaiTro;
                taikhoan.TaiKhoan = data.TaiKhoan;
                taikhoan.MatKhau = data.MatKhau;
                taikhoan.Email = data.Email;
                taikhoan.HoTen = data.HoTen;
                taikhoan.Avatar = data.Avatar;
                db.SaveChanges();

            }
            return RedirectToAction("DanhSachNguoiDung");
        }
        public ActionResult XoaNguoiDung(int id)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var taikhoan = db.NguoiDungs.Where(x => x.IdNguoiDung == id).FirstOrDefault();
                if (taikhoan is null) return HttpNotFound();
                db.NguoiDungs.Remove(taikhoan);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachNguoiDung");
        }
        
        public ActionResult Khoa()
        {
            return View();
        }
       
        public ActionResult MonHoc()
        {
            return View();
        }
      
        public ActionResult ChiTietMonHoc(int id)
        {
            using (db)
            {
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == id).FirstOrDefault();
                if (monhoc is null) return HttpNotFound();
                return View(monhoc);
            }
        }
        [HttpPost]
        public ActionResult ChiTietMonHoc(int id, MonHoc data)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == id).FirstOrDefault();
                if (monhoc is null) return HttpNotFound();
                monhoc.MaMon = data.MaMon;
                monhoc.TenMonHoc = data.TenMonHoc;
                monhoc.MoTa = data.MoTa;
                db.SaveChanges();
            }
            return RedirectToAction("MonHoc");
        }
        public ActionResult XoaMonHoc(int id)
        {
            using (db)
            {
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == id).FirstOrDefault();
                if (monhoc is null) return HttpNotFound();
                db.MonHocs.Remove(monhoc);
                db.SaveChanges();
            }
            return RedirectToAction("MonHoc");
        }
     
        public ActionResult CTHoc()
        {
            return View();
        }
        public ActionResult XoaMonHocKhoa(int id)
        {
            using (db)
            {
                var khoa = db.Khoas.Where(x => x.IdKhoa == id).FirstOrDefault();
                if (khoa is null) return HttpNotFound();
                db.Khoas.Remove(khoa);
                db.SaveChanges();
            }
            return RedirectToAction("Khoa");
        }
      
        public ActionResult DanhSachSinhVien()
        {
            return View();
        }
        
        public ActionResult DanhSachLopHoc()
        {
            return View();
        }
        public ActionResult ChiTietLopHoc(int id)
        {
            using (db)
            {
                var lophoc = db.LopHocs.Where(x => x.IdLopHoc == id).FirstOrDefault();
                if (lophoc is null) return HttpNotFound();
                return View(lophoc);
            }
        }
        [HttpPost]
        public ActionResult ChiTietLopHoc(int id, LopHoc data)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var lophoc = db.LopHocs.Where(x => x.IdLopHoc == id).FirstOrDefault();
                if (lophoc is null) return HttpNotFound();
                lophoc.MaLop = data.TenLop;
                lophoc.TenLop = data.TenLop;
                lophoc.MoTa = data.MoTa;
                lophoc.TinhTrang = data.TinhTrang;
                lophoc.IdCTHoc = data.IdCTHoc;
                //lophoc.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachLopHoc");
        }
        
        public ActionResult ChiTietCTHoc(int id)
        {
            using (db)
            {
                var cthoc = db.CT_HOC.Where(x => x.IdCTHoc == id).FirstOrDefault();
                if (cthoc is null) return HttpNotFound();
                return View(cthoc);
            }
        }
        [HttpPost]
        public ActionResult ChiTietCTHoc(int id, CT_HOC data)
        {
            //if (Session["AdminLogin"] is null) return View("  ");
            using (db)
            {
                var cthoc = db.CT_HOC.Where(x => x.IdCTHoc == id).FirstOrDefault();
                if (cthoc is null) return HttpNotFound();
                cthoc.TenCT = data.TenCT;
                cthoc.NgayTao = data.NgayTao;
                cthoc.IdNguoiDung = data.IdNguoiDung;
                cthoc.IdMonHoc = data.IdMonHoc;
                db.SaveChanges();

            }
            return RedirectToAction("CTHoc");
        }
        
        public ActionResult DanhSachTuLuan()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult ChiTietTuLuan(int id)
        {
            using (db)
            {
                var tuluan = db.TuLuans.Where(x => x.IdTuLuan == id).FirstOrDefault();
                if (tuluan is null) return HttpNotFound();
                tuluan.NguoiDung = tuluan.NguoiDung;
                tuluan.TuLuan_CauHoi = tuluan.TuLuan_CauHoi;
                return View(tuluan);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChiTietTuLuan(int id, TuLuan data)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var tuluan = db.TuLuans.Where(x => x.IdTuLuan == id).FirstOrDefault();
                if (tuluan is null) return HttpNotFound();
                tuluan.TenTuLuan = data.TenTuLuan;
                tuluan.ThoiGian = data.ThoiGian;
                tuluan.HanChot = data.HanChot;
                tuluan.MoTa = data.MoTa;
                // tuluan.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachTuLuan");
        }



        #region ---------------------------- đây là chỗ để để ba cái xàm " Partial "--------------------------
        [HttpPost]
        public ActionResult GetDapAnTracNghiemCauHoi(int id)
        {
            // id bai tu luan
            ViewBag.Id = id;
            return PartialView("_PartialListTN");
        }


        [HttpPost]
        public ActionResult GetListQuestion(int id)
        {
            // id bai tu luan
            ViewBag.Id = id;
            return PartialView("_PartialListQuestion");
        }

        [HttpPost]
        public ActionResult GetSinhVienDapAnTN(int idSV, int idTN)
        {
            ViewBag.IdTN = idTN;
            ViewBag.IdSV = idSV;
            return PartialView("_PartialSinhVienDapAnTN");
        }

        [HttpPost]
        public ActionResult ModalThemBaiHoc()
        {
            return PartialView("_PartialThemBaiHoc");
        }
        [HttpPost]
        public ActionResult ModalThemTuLuan()
        {
            return PartialView("_PartialThemTuLuan");
        }
        [HttpPost]
        public ActionResult ModalThemTracNghiem()
        {
            return PartialView("_PartialThemTracNghiem");
        }
        #endregion----------------------------------------------------------------------------------------------


        public ActionResult DanhSachBaiHoc()
        {
            return View();
        }
        public ActionResult ChiTietBaiHoc(int id)
        {
            using (db)
            {
                var baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == id).FirstOrDefault();
                if (baihoc is null) return HttpNotFound();
                baihoc.NguoiDung = baihoc.NguoiDung;
                return View(baihoc);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChiTietBaiHoc(int id, BaiHoc data)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == id).FirstOrDefault();
                if (baihoc is null) return HttpNotFound();
                baihoc.TenBaiHoc = data.TenBaiHoc;
                baihoc.NoiDung = data.NoiDung;
                baihoc.Video = data.Video;
                baihoc.ChoPhepNopBai = data.ChoPhepNopBai;
                // tuluan.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachBaiHoc");
        }
        public ActionResult DanhSachTracNghiem()
        {
            return View();
        }
        public ActionResult ChiTietTracNghiem(int id)
        {
            using (db)
            {
                var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id).FirstOrDefault();
                if (tracnghiem is null) return HttpNotFound();
                tracnghiem.NguoiDung = tracnghiem.NguoiDung;
                //tracnghiem.TracNghiem_CauHoi = tracnghiem.TracNghiem_CauHoi;
                return View(tracnghiem);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChiTietTracNghiem(int id, TracNghiem data)
        {
            //if (Session["AdminLogin"] is null) return View("AdminLogin");
            using (db)
            {
                var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id).FirstOrDefault();
                if (tracnghiem is null) return HttpNotFound();
                tracnghiem.TenTracNghiem = data.TenTracNghiem;
                tracnghiem.MoTa = data.MoTa;
                tracnghiem.ThoiGian = data.ThoiGian;
                tracnghiem.HanChot = data.HanChot;
                //tracnghiem.TracNghiem_CauHoi = tracnghiem.TracNghiem_CauHoi;
                //tracnghiem.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachTracNghiem");
        }
    }
}