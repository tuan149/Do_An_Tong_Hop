using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToMoToStudy.Helper;

namespace ToMoToStudy.Controllers
{
    public class HomeController : Controller
    {
        ToMoToDBEntities db = new ToMoToDBEntities();
        public ActionResult Index()
        {
            return View();
        }
            
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ThongTinTaiKhoan()
        {
            if (Session["user"] is null) return View("Index");
            return View();
        }

        public ActionResult LayLaiMatKhau()
        {
            return View();
        }

        public ActionResult ChiTietCTHoc(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                var user = (NguoiDung)Session["user"];
                var cth = db.CT_HOC.Where(x => x.IdCTHoc == id && x.IdNguoiDung==user.IdNguoiDung).FirstOrDefault();
                if (cth is null) return HttpNotFound();
                return View(cth);
            }
           
        }
        
        #region ---------------------------- đây là chỗ để để ba cái xàm " Partial "--------------------------
        [HttpPost]
        public ActionResult ModalLogin()
        {
            return PartialView("_PartialLogin");
        }
        [HttpPost]
        public ActionResult TabThongTinTaiKhoan()
        {
            return PartialView("_PartialThongTinTaiKhoan");
        }
        #endregion----------------------------------------------------------------------------------------------


        public ActionResult ChiTietBaiHoc(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                var user = (NguoiDung)Session["user"];
                var baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == id && x.IdNguoiDung == user.IdNguoiDung).FirstOrDefault();
                if (baihoc is null) return HttpNotFound();
                baihoc.NguoiDung = baihoc.NguoiDung;
                return View(baihoc);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChiTietBaiHoc(int id, BaiHoc data)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                var baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == id).FirstOrDefault();
                if (baihoc is null) return HttpNotFound();
                baihoc.TenBaiHoc = data.TenBaiHoc;
                baihoc.NoiDung = data.NoiDung;
                baihoc.Video = data.Video;
                //baihoc.ChoPhepNopBai = data.ChoPhepNopBai;
                // tuluan.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
                return View(baihoc);
            }

        }

        public ActionResult ChiTietTracNghiem(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                var user = (NguoiDung)Session["user"];
                var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id && x.IdNguoiDung == user.IdNguoiDung).FirstOrDefault();
                if (tracnghiem is null) return HttpNotFound();
                tracnghiem.NguoiDung = tracnghiem.NguoiDung;
               // tracnghiem.TracNghiem_CauHoi = tracnghiem.TracNghiem_CauHoi;
                return View(tracnghiem);
            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChiTietTracNghiem(int id, TracNghiem data)
        {
            using (db)
            {
                var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id).FirstOrDefault();
                if (tracnghiem is null) return HttpNotFound();
                tracnghiem.TenTracNghiem = data.TenTracNghiem;
                tracnghiem.MoTa = data.MoTa;
                tracnghiem.ThoiGian = data.ThoiGian;
                tracnghiem.HanChot = data.HanChot;
                db.SaveChanges();
                return View(tracnghiem);
            }

        }

        [ValidateInput(false)]
        public ActionResult ChiTietTuLuan(int id)
        {
            if (Session["user"] is null) return View("Index");
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
                return View(tuluan);
            }
            
        }

        public ActionResult DanhSachLopHoc()
        {
            if (Session["user"] is null) return View("Index");
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
                //lophoc.MaLop = data.TenLop;
                lophoc.TenLop = data.TenLop;
                lophoc.MoTa = data.MoTa;
                lophoc.TinhTrang = data.TinhTrang;
                lophoc.IdCTHoc = data.IdCTHoc;
                //lophoc.IdNguoiDung = data.IdNguoiDung;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachLopHoc");
        }

        [ValidateInput(false)]
        public ActionResult ThamGiaTuLuan(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                //var user = (NguoiDung)Session["user"];
                var tuluan = db.TuLuans.Where(x => x.IdTuLuan == id).FirstOrDefault();
                if (tuluan is null) return HttpNotFound();
                tuluan.NguoiDung = tuluan.NguoiDung;
                tuluan.TuLuan_CauHoi = tuluan.TuLuan_CauHoi;
                return View(tuluan);
            }
        }

        [ValidateInput(false)]
        public ActionResult ThamGiaTracNghiem(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                var user = (NguoiDung)Session["user"];
                var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id).FirstOrDefault();
                if (tracnghiem is null) return HttpNotFound();
                //tracnghiem.NguoiDung = tracnghiem.NguoiDung;

                //var dsBaiLam = db.SinhVien_BaiLam_TracNghiem.Where(x => x.IdNguoiDung == user.IdNguoiDung).ToList();
                //if (dsBaiLam.Count == 0)
                //{
                //    var tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == id).FirstOrDefault();
                //    var listCauHoi = tracnghiem.TracNghiem_CauHoi.ToList();
                //    listCauHoi.Shuffle();
                //    foreach (var cauhoi in listCauHoi)
                //    {

                //    }
                //}

                //tracnghiem.TracNghiem_CauHoi = tracnghiem.TracNghiem_CauHoi;
                return View(tracnghiem);
            }
        }
        [ValidateInput(false)]
        public ActionResult ThamGiaBaiHoc(int id)
        {
            if (Session["user"] is null) return View("Index");
            using (db)
            {
                //var user = (NguoiDung)Session["user"];
                var baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == id).FirstOrDefault();
                if (baihoc is null) return HttpNotFound();
                return View(baihoc);
            }
        }


        public ActionResult ChuongTrinhHoc()
        {
            if (Session["user"] is null) return RedirectToAction("Index");
            return View();
        }

        public ActionResult LuuTru()
        {
            if (Session["user"] is null) return RedirectToAction("Index");
            return View();
        }

        public ActionResult BaiLamTuLuan(int? IdLopHoc,int? IdNoiDung)
            {
            if (Session["user"] is null) return RedirectToAction("Index");
            ViewBag.IdLopHoc = IdLopHoc;
            ViewBag.IdTuLuan = IdNoiDung;
            return View();
        }
        public ActionResult BaiLamTracNghiem(int? IdLopHoc, int? IdNoiDung)
        {
            if (Session["user"] is null) return RedirectToAction("Index");
            ViewBag.IdLopHoc = IdLopHoc;
            ViewBag.IdTracNghiem = IdNoiDung;
            return View();
        }
        public ActionResult BaiLamBaiHoc(int? IdLopHoc, int? IdNoiDung)
        {
            if (Session["user"] is null) return RedirectToAction("Index");
            ViewBag.IdLopHoc = IdLopHoc;
            ViewBag.IdBaiHoc = IdNoiDung;
            return View();
        }
    }
}