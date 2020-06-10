using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ToMoToStudy.Helper;
using ToMoToStudy.Models;

namespace ToMoToStudy.Controllers
{
    public class AjaxController : Controller
    {
        public JsonResult GetTk(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var ndDB = db.NguoiDungs.Where(x => x.IdNguoiDung == id).FirstOrDefault();
                if (ndDB is null) return null;
                return new JsonResult()
                {
                    Data = new { taikhoan = ndDB.TaiKhoan, matkhau = ndDB.MatKhau, email = ndDB.Email, idvaitro = ndDB.IdVaiTro },

                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        // GET: Ajax
        [HttpPost]
        public JsonResult PostTK(NguoiDung data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result);
            }

            if (!Regex.IsMatch(data.TaiKhoan, @"^[a-zA-Z0-9]+$"))
            {
                result.Message = "Tài khoản không dấu và chỉ chứa các kí tự từ a-z và 0-9";
                return Json(result);
            }
            if (!Regex.IsMatch(data.MatKhau, @"^[a-zA-Z0-9]+$"))
            {
                result.Message = "Mật khẩu không dấu chỉ chứa các kí tự từ a-z và 0-9";
                return Json(result);
            }
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(data.Email);
            }
            catch (FormatException)
            {
                result.Message = "Email không hợp lệ";
                return Json(result);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                NguoiDung tk = new NguoiDung();
                if (db.NguoiDungs.Any(x => x.TaiKhoan == data.TaiKhoan))
                {
                    result.Message = "Tài khoản đã tồn tại";
                    return Json(result);
                }
                if (db.NguoiDungs.Any(x => x.Email == data.Email))
                {
                    result.Message = "Email đã tồn tại";
                    return Json(result);
                }
                // chỉnh sửa tk
                if (data.IdNguoiDung > 0) tk = db.NguoiDungs.Where(x => x.IdNguoiDung == data.IdNguoiDung).FirstOrDefault();
                if (tk is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tk.TaiKhoan = data.TaiKhoan;
                tk.MatKhau = data.MatKhau;
                tk.Email = data.Email;
                tk.IdVaiTro = data.IdVaiTro;
                if (data.IdNguoiDung == 0) db.NguoiDungs.Add(tk);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GuiMailMatKhau(NguoiDung data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result);
            }
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(data.Email);
            }
            catch (FormatException)
            {
                result.Message = "Email không hợp lệ";
                return Json(result);
            }
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                NguoiDung tk = new NguoiDung();
                try
                {
                    if (db.NguoiDungs.Any(x => x.TaiKhoan == data.TaiKhoan))
                    {
                        tk = db.NguoiDungs.Where(x => x.TaiKhoan == data.TaiKhoan && x.Email == data.Email).FirstOrDefault();
                        if (tk != null)
                        {
                            string random = "";
                            random = RandomMatKhau.Get();
                            tk.MatKhau = random;
                            try
                            {
                                db.SaveChanges();
                                result.Success = true;
                            }
                            catch (Exception ex)
                            {
                                result.Message = ex.Message;
                            }
                        }
                        result.Message = "Không đúng email đăng ký";
                        return Json(result);
                    }
                    else
                    {
                        result.Message = "Không tìm thấy tài khoản";
                        return Json(result);
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }


        public JsonResult GetVaiTro(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var vaitro = db.VaiTroes.Where(x => x.IdVaiTro == id).FirstOrDefault();
                if (vaitro is null) return null;
                return new JsonResult()
                {
                    Data = new { tenvaitro = vaitro.TenVaiTro },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public JsonResult PostVaiTro(VaiTro data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                VaiTro vaitro = new VaiTro();

                // chỉnh sửa tk
                if (data.IdVaiTro > 0) vaitro = db.VaiTroes.Where(x => x.IdVaiTro == data.IdVaiTro).FirstOrDefault();
                if (vaitro is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                vaitro.TenVaiTro = data.TenVaiTro;
                vaitro.IdVaiTro = data.IdVaiTro;
                if (data.IdVaiTro == 0) db.VaiTroes.Add(vaitro);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult GetKhoa(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var khoa = db.Khoas.Where(x => x.IdKhoa == id).FirstOrDefault();
                if (khoa is null) return null;
                return new JsonResult()
                {
                    Data = new { NAME = khoa.TenKhoa },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public string PostKhoa(Khoa khoa)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                Khoa k = new Khoa();
                if (khoa.IdKhoa > 0) k = db.Khoas.Where(x => x.IdKhoa == khoa.IdKhoa).FirstOrDefault();
                if (k is null) return "Dữ liệu bất thường vui lòng thử lại sau";
                k.IdKhoa = khoa.IdKhoa;
                k.TenKhoa = khoa.TenKhoa;
                if (khoa.IdKhoa == 0) db.Khoas.Add(k);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            return "ok";
        }
        public JsonResult GetMonHoc(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == id).FirstOrDefault();
                if (monhoc is null) return null;
                return new JsonResult()
                {
                    Data = new { mamonhoc = monhoc.MaMon, tenmonhoc = monhoc.TenMonHoc, mota = monhoc.MoTa },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public JsonResult PostMonHoc(MonHoc data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                MonHoc tk = new MonHoc();

                // chỉnh sửa tk
                if (data.IdMonHoc > 0) tk = db.MonHocs.Where(x => x.IdMonHoc == data.IdMonHoc).FirstOrDefault();
                if (tk is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tk.TenMonHoc = data.TenMonHoc;
                tk.MoTa = data.MoTa;
                tk.MaMon = data.MaMon;
                tk.IdMonHoc = data.IdMonHoc;
                if (data.IdMonHoc == 0) db.MonHocs.Add(tk);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult PostMonKhoa(int IdKhoa, int IdMonHoc)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                // chỉnh sửa tk
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == IdMonHoc).FirstOrDefault();
                var khoa = db.Khoas.Where(x => x.IdKhoa == IdKhoa).FirstOrDefault();
                if (monhoc is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                monhoc.Khoas.Add(khoa);

                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult XoaMonKhoa(int IdMonHoc, int IdKhoa)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var monhoc = db.MonHocs.Where(x => x.IdMonHoc == IdMonHoc).FirstOrDefault();
                var khoa = monhoc.Khoas.Where(x => x.IdKhoa == IdKhoa).FirstOrDefault();
                if (monhoc is null || khoa is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    monhoc.Khoas.Remove(khoa);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult GetSinhVien(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var sinhvien = db.SinhViens.Where(x => x.IdSinhVien == id).FirstOrDefault();
                if (sinhvien is null) return null;
                return new JsonResult()
                {
                    Data = new { mssv = sinhvien.Mssv, malopsv = sinhvien.MaLopSv, nguoidung = sinhvien.IdNguoiDung },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public JsonResult PostSinhVien(SinhVien data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                SinhVien sv = new SinhVien();

                // chỉnh sửa tk
                if (data.IdSinhVien > 0) sv = db.SinhViens.Where(x => x.IdSinhVien == data.IdSinhVien).FirstOrDefault();
                if (sv is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                sv.Mssv = data.Mssv;
                sv.MaLopSv = data.MaLopSv;
                sv.IdNguoiDung = data.IdNguoiDung;
                sv.IdSinhVien = data.IdSinhVien;
                if (data.IdSinhVien == 0) db.SinhViens.Add(sv);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult XoaSinhVien(int IdSinhVien)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var sinhvien = db.SinhViens.Where(x => x.IdSinhVien == IdSinhVien).FirstOrDefault();
                if (sinhvien is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.SinhViens.Remove(sinhvien);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult GetLopHoc(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var lophoc = db.LopHocs.Where(x => x.IdLopHoc == id).FirstOrDefault();
                if (lophoc is null) return null;
                return new JsonResult()
                {
                    Data = new { malop = lophoc.MaLop, tenlop = lophoc.TenLop, mota = lophoc.MoTa, tinhtrang = lophoc.TinhTrang, cthoc = lophoc.IdCTHoc, nguoidung = lophoc.IdNguoiDung },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public JsonResult PostLopHoc(LopHoc data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                LopHoc lh = new LopHoc();
                // chỉnh sửa tk
                if (data.IdLopHoc > 0) lh = db.LopHocs.Where(x => x.IdLopHoc == data.IdLopHoc).FirstOrDefault();
                if (lh is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
               
                lh.TenLop = data.TenLop;
                lh.MoTa = data.MoTa;
                lh.TinhTrang = data.TinhTrang;
                lh.IdCTHoc = data.IdCTHoc;
                lh.IdNguoiDung = data.IdNguoiDung;
                if (data.IdLopHoc == 0)
                {
                    lh.TinhTrang = 1;
                    string random = "";
                    random = RandomChuoi.Get();
                    lh.MaLop = random;
                    db.LopHocs.Add(lh);
                    
                }
                    try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult PostSVLop(SVLop data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                SVLop svlop = new SVLop();

                // chỉnh sửa tk
                //if (data.IdLopHoc > 0) svlop = db.SVLops.Where(x => x.IdLopHoc == data.IdLopHoc).FirstOrDefault();
                if (svlop is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                svlop.IdLopHoc = data.IdLopHoc;
                svlop.IdSinhVien = data.IdSinhVien;
                //svlop.NgayThamGia = data.NgayThamGia;
                svlop.NgayThamGia = DateTime.Now;
                db.SVLops.Add(svlop);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult XoaSVLop(int IdLopHoc, int IdSinhVien)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var svlop = db.SVLops.Where(x => x.IdLopHoc == IdLopHoc && x.IdSinhVien == IdSinhVien).FirstOrDefault();
                if (svlop is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.SVLops.Remove(svlop);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult GetCTHoc(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var cthoc = db.CT_HOC.Where(x => x.IdCTHoc == id).FirstOrDefault();
                if (cthoc is null) return null;
                return new JsonResult()
                {
                    Data = new { tenct = cthoc.TenCT, ngaytao = cthoc.NgayTao, idnguoidung = cthoc.IdNguoiDung, idmonhoc = cthoc.IdMonHoc },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [HttpPost]
        public JsonResult PostCTHoc(CT_HOC data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                CT_HOC cthoc = new CT_HOC();
                // chỉnh sửa tk
                if (data.IdCTHoc > 0) cthoc = db.CT_HOC.Where(x => x.IdCTHoc == data.IdCTHoc).FirstOrDefault();
                if (cthoc is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                cthoc.TenCT = data.TenCT;
                cthoc.IdMonHoc = data.IdMonHoc;
                cthoc.IdNguoiDung = data.IdNguoiDung;
                if (data.IdCTHoc == 0)
                {
                    cthoc.NgayTao = DateTime.Now;
                    db.CT_HOC.Add(cthoc);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ThemCTBaiCu(CHITIET_CT_HOC data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                CHITIET_CT_HOC cthoc = new CHITIET_CT_HOC();
                // chỉnh sửa tk
                if (data.IdChiTiet > 0) cthoc = db.CHITIET_CT_HOC.Where(x => x.IdChiTiet == data.IdChiTiet).FirstOrDefault();
                if (cthoc is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                cthoc.IdCTHoc = data.IdCTHoc;
                cthoc.IdBaiHoc = data.IdBaiHoc;
                cthoc.IdTracNghiem = data.IdTracNghiem;
                cthoc.IdTuLuan = data.IdTuLuan;
                if (data.IdChiTiet == 0)
                {
                    db.CHITIET_CT_HOC.Add(cthoc);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }


        //----------------TỰ LUẬN-----------------------
        [HttpPost]
        public JsonResult PostTuLuan(TuLuan data, int? idCT)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TuLuan tuluan = new TuLuan();
                // chỉnh sửa tk
                if (data.IdTuLuan > 0) tuluan = db.TuLuans.Where(x => x.IdTuLuan == data.IdTuLuan).FirstOrDefault();
                if (tuluan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tuluan.TenTuLuan = data.TenTuLuan;
                tuluan.ThoiGian = data.ThoiGian;
                tuluan.HanChot = data.HanChot;
                tuluan.MoTa = data.MoTa;
                tuluan.IdNguoiDung = data.IdNguoiDung;
                if (data.IdTuLuan == 0)
                {
                    db.TuLuans.Add(tuluan);
                }
                try
                {
                    db.SaveChanges();
                    if (idCT != null && idCT > 0)
                    {
                        CHITIET_CT_HOC CT = new CHITIET_CT_HOC();
                        CT.IdTuLuan = tuluan.IdTuLuan;
                        CT.IdCTHoc = idCT;
                        db.CHITIET_CT_HOC.Add(CT);
                        db.SaveChanges();
                    }
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetTuLuanCauHoi(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tuluancauhoi = db.TuLuan_CauHoi.Where(x => x.IdCauHoiTuLuan == id).FirstOrDefault();
                if (tuluancauhoi is null) return null;
                return new JsonResult()
                {
                    //Data = new { idtuluan = tuluancauhoi.IdTuLuan, cauhoituluan = tuluancauhoi.CauHoiTuLuan },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PostTuLuanCauHoi(TuLuan_CauHoi data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TuLuan_CauHoi tuluancauhoi = new TuLuan_CauHoi();
                // chỉnh sửa tk
                if (data.IdCauHoiTuLuan > 0) tuluancauhoi = db.TuLuan_CauHoi.Where(x => x.IdCauHoiTuLuan == data.IdCauHoiTuLuan).FirstOrDefault();
                if (tuluancauhoi is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tuluancauhoi.CauHoiTuLuan = data.CauHoiTuLuan;
                //tuluancauhoi.IdTuLuan = data.IdTuLuan;
                if (data.IdCauHoiTuLuan == 0)
                {
                    db.TuLuan_CauHoi.Add(tuluancauhoi);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult XoaTuLuanCauHoi(int id)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tuluancauhoi = db.TuLuan_CauHoi.Where(x => x.IdCauHoiTuLuan == id).FirstOrDefault();
                if (tuluancauhoi is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.TuLuan_CauHoi.Remove(tuluancauhoi);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        public JsonResult GetTuLuanDapAn(int IdCauHoiTuLuan, int IdSinhVien)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tuluandapan = db.SinhVien_TL_DapAn.Where(x => x.IdSinhVien == IdSinhVien && x.IdCauHoiTuLuan == IdCauHoiTuLuan).FirstOrDefault();
                if (tuluandapan is null) return null;
                return new JsonResult()
                {
                    Data = new { dapan = tuluandapan.DapAn, diemtuluan = tuluandapan.DiemTuLuan },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PostTuLuanDapAn(SinhVien_TL_DapAn data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                SinhVien_TL_DapAn tuluandapan = new SinhVien_TL_DapAn();
                // chỉnh sửa tk
                if (data.IdSinhVien > 0 && data.IdCauHoiTuLuan > 0) tuluandapan = db.SinhVien_TL_DapAn.Where(x => x.IdSinhVien == data.IdSinhVien && x.IdCauHoiTuLuan == data.IdCauHoiTuLuan).FirstOrDefault();
                if (tuluandapan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                //tuluandapan.DapAn = data.DapAn;
                tuluandapan.DiemTuLuan = data.DiemTuLuan;
                if (data.IdCauHoiTuLuan == 0 && data.IdCauHoiTuLuan == 0)
                {
                    db.SinhVien_TL_DapAn.Add(tuluandapan);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        //END -----------------------------------------------------------
        //---------------------------BÀI HỌC ----------------------------
        [HttpPost]
        public JsonResult PostBaiHoc(BaiHoc data, int? idCT)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                BaiHoc baihoc = new BaiHoc();
                // chỉnh sửa tk
                if (data.IdBaiHoc > 0) baihoc = db.BaiHocs.Where(x => x.IdBaiHoc == data.IdBaiHoc).FirstOrDefault();
                if (baihoc is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                baihoc.TenBaiHoc = data.TenBaiHoc;
                baihoc.NoiDung = data.NoiDung;
                baihoc.Video = data.Video;
                baihoc.ChoPhepNopBai = true;
                baihoc.IdNguoiDung = data.IdNguoiDung;
                if (data.IdBaiHoc == 0)
                {
                    baihoc.ChoPhepNopBai = true;
                    db.BaiHocs.Add(baihoc);
                }
                try
                {
                    db.SaveChanges();
                    if (idCT != null && idCT > 0)
                    {
                        CHITIET_CT_HOC CT = new CHITIET_CT_HOC();
                        CT.IdBaiHoc = baihoc.IdBaiHoc;
                        CT.IdCTHoc = idCT;
                        db.CHITIET_CT_HOC.Add(CT);
                        db.SaveChanges();
                    }
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult PostFileHocVien(FileHocVien data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                FileHocVien filehocvien = new FileHocVien();
                // chỉnh sửa tk
                if (data.IdFileHocVien > 0) filehocvien = db.FileHocViens.Where(x => x.IdFileHocVien == data.IdFileHocVien).FirstOrDefault();
                if (filehocvien is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                filehocvien.Link = data.Link;
                filehocvien.NoiDung = data.NoiDung;
                filehocvien.IdBaiHoc = data.IdBaiHoc;
                filehocvien.IdSinhVien = data.IdSinhVien;
                if (data.IdFileHocVien == 0)
                {
                    filehocvien.NgayTao = DateTime.Now;
                    db.FileHocViens.Add(filehocvien);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult XoaFileHocVien(int IdFileHocVien)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var filehocvien = db.FileHocViens.Where(x => x.IdFileHocVien == IdFileHocVien).FirstOrDefault();
                if (filehocvien is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.FileHocViens.Remove(filehocvien);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult PostTracNghiem(TracNghiem data, int? idCT)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TracNghiem tracnghiem = new TracNghiem();
                // chỉnh sửa tk
                if (data.IdTracNghiem > 0) tracnghiem = db.TracNghiems.Where(x => x.IdTracNghiem == data.IdTracNghiem).FirstOrDefault();
                if (tracnghiem is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tracnghiem.MoTa = data.MoTa;
                tracnghiem.ThoiGian = data.ThoiGian;
                tracnghiem.HanChot = data.HanChot;
                tracnghiem.IdNguoiDung = data.IdNguoiDung;
                tracnghiem.TenTracNghiem = data.TenTracNghiem;
                if (data.IdTracNghiem == 0)
                {
                    db.TracNghiems.Add(tracnghiem);
                }
                try
                {
                    db.SaveChanges();
                    if (idCT != null && idCT > 0)
                    {
                        CHITIET_CT_HOC CT = new CHITIET_CT_HOC();
                        CT.IdTracNghiem = tracnghiem.IdTracNghiem;
                        CT.IdCTHoc = idCT;
                        db.CHITIET_CT_HOC.Add(CT);
                        db.SaveChanges();
                    }
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        public JsonResult GetTracNghiemCauHoi(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tracnghiemcauhoi = db.TracNghiem_CauHoi.Where(x => x.IdCauHoi == id).FirstOrDefault();
                if (tracnghiemcauhoi is null) return null;
                return new JsonResult()
                {
                    Data = new { noidungcauhoi = tracnghiemcauhoi.NoiDungCauHoi },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PostTracNghiemCauHoi(TracNghiem_CauHoi data,int idtracnghiem)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TracNghiem_CauHoi tracnghiemcauhoi = new TracNghiem_CauHoi();
                // chỉnh sửa tk
                if (data.IdCauHoi > 0) tracnghiemcauhoi = db.TracNghiem_CauHoi.Where(x => x.IdCauHoi == data.IdCauHoi).FirstOrDefault();
                if (tracnghiemcauhoi is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tracnghiemcauhoi.NoiDungCauHoi = data.NoiDungCauHoi;
                if (data.IdCauHoi == 0)
                {
                    tracnghiemcauhoi.IdNguoiDung = data.IdNguoiDung;
                    db.TracNghiem_CauHoi.Add(tracnghiemcauhoi);
                }
                try
                {
                    db.SaveChanges();
                    
                    if (idtracnghiem > 0)
                    {
                        var tncauhoi = db.TracNghiem_CauHoi.Where(x => x.IdCauHoi == tracnghiemcauhoi.IdCauHoi).FirstOrDefault();
                        var tn = db.TracNghiems.Where(x => x.IdTracNghiem == idtracnghiem).FirstOrDefault();
                        
                        tn.TracNghiem_CauHoi.Add(tncauhoi);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            result.Message = ex.Message;
                        }
                    }
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult XoaTracNghiemCauHoi(int id)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tracnghiemcauhoi = db.TracNghiem_CauHoi.Where(x => x.IdCauHoi == id).FirstOrDefault();
                if (tracnghiemcauhoi is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.TracNghiem_CauHoi.Remove(tracnghiemcauhoi);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }
        public JsonResult GetTracNghiemDapAn(int id)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tracnghiemdapan = db.TracNghiem_DapAn.Where(x => x.IdDapAn == id).FirstOrDefault();
                if (tracnghiemdapan is null) return null;
                return new JsonResult()
                {
                    Data = new { dapan = tracnghiemdapan.DapAn, dapandung = tracnghiemdapan.DapAnDung, idcauhoi = tracnghiemdapan.IdCauHoi },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpPost]
        public JsonResult PostTracNghiemDapAn(TracNghiem_DapAn data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TracNghiem_DapAn tracnghiemdapan = new TracNghiem_DapAn();
                // chỉnh sửa tk
                if (data.IdDapAn > 0) tracnghiemdapan = db.TracNghiem_DapAn.Where(x => x.IdDapAn == data.IdDapAn).FirstOrDefault();
                if (tracnghiemdapan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tracnghiemdapan.DapAn = data.DapAn;
                if (data.IdDapAn == 0)
                {
                    tracnghiemdapan.IdCauHoi = data.IdCauHoi;
                    tracnghiemdapan.DapAnDung = false;
                    db.TracNghiem_DapAn.Add(tracnghiemdapan);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult NopTracNghiem(SinhVien_BaiLam_TracNghiem data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                SinhVien_BaiLam_TracNghiem sinhviendapan = new SinhVien_BaiLam_TracNghiem();
                // chỉnh sửa tk
                if (data.IdBaiLam > 0) sinhviendapan = db.SinhVien_BaiLam_TracNghiem.Where(x => x.IdBaiLam == data.IdBaiLam).FirstOrDefault();
                if (sinhviendapan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                sinhviendapan.IdCauHoi = data.IdCauHoi;
                sinhviendapan.IdDapAn = data.IdDapAn;
                sinhviendapan.IdSinhVien = data.IdSinhVien;
                sinhviendapan.NgayNop = DateTime.Now;
                if (data.IdBaiLam == 0)
                {
                    
                    db.SinhVien_BaiLam_TracNghiem.Add(sinhviendapan);
                }
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }



        [HttpPost]
        public void PostCapNhatThuTu(SuaThuTuModel data)
        {
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var chitietCTHoc = db.CHITIET_CT_HOC.Where(x => x.IdChiTiet == data.idchitiet).FirstOrDefault();
                if (chitietCTHoc is null) return;

                chitietCTHoc.ThuTu = data.thutu;
                db.SaveChanges();
                return;
            }
        }

        [HttpPost]
        public JsonResult PostTracNghiemDapAnDung(TracNghiem_DapAn data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                TracNghiem_DapAn tracnghiemdapan = new TracNghiem_DapAn();
                // chỉnh sửa tk
                if (data.IdDapAn > 0) tracnghiemdapan = db.TracNghiem_DapAn.Where(x => x.IdDapAn == data.IdDapAn).FirstOrDefault();
                if (tracnghiemdapan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                if (data.DapAnDung == true )
                {
                    var ck = db.TracNghiem_DapAn.Where(x => x.IdCauHoi==data.IdCauHoi).ToList();
                    if (ck.Any(x => x.IdCauHoi == data.IdCauHoi && x.IdDapAn != data.IdDapAn && x.DapAnDung == true))
                    {
                        result.Message = "Đã có đáp án đúng";
                        return Json(result);
                    }    
                }
                tracnghiemdapan.DapAnDung = data.DapAnDung;
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult XoaTracNghiemDapAn(int id)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var tracnghiemdapan = db.TracNghiem_DapAn.Where(x => x.IdDapAn == id).FirstOrDefault();
                if (tracnghiemdapan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                try
                {
                    db.TracNghiem_DapAn.Remove(tracnghiemdapan);
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DangNhap(string tk, string mk)
        {
            ApiResult result = new ApiResult();
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var user = db.NguoiDungs.Where(x => x.TaiKhoan.Equals(tk) && x.MatKhau.Equals(mk)).FirstOrDefault();
                if (user is null)
                {
                    result.Message = "Sai tài khoản hoặc mật khẩu";
                    return Json(result);
                }
                try
                {
                    Session["loai"] = user.IdVaiTro;
                    Session["user"] = user;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DangXuat()
        {
            ApiResult result = new ApiResult();
                try
                {
                Session.Clear();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DangKy(NguoiDung data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result);
            }

            if (!Regex.IsMatch(data.TaiKhoan, @"^[a-zA-Z0-9]+$"))
            {
                result.Message = "Tài khoản không dấu và chỉ chứa các kí tự từ a-z và 0-9";
                return Json(result);
            }
            if (!Regex.IsMatch(data.MatKhau, @"^[a-zA-Z0-9]+$"))
            {
                result.Message = "Mật khẩu không dấu chỉ chứa các kí tự từ a-z và 0-9";
                return Json(result);
            }
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(data.Email);
            }
            catch (FormatException)
            {
                result.Message = "Email không hợp lệ";
                return Json(result);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                NguoiDung tk = new NguoiDung();
                if (db.NguoiDungs.Any(x => x.TaiKhoan == data.TaiKhoan))
                {
                    result.Message = "Tài khoản đã tồn tại";
                    return Json(result);
                }
                if (db.NguoiDungs.Any(x => x.Email == data.Email))
                {
                    result.Message = "Email đã tồn tại";
                    return Json(result);
                }
                // chỉnh sửa tk
                if (data.IdNguoiDung > 0) tk = db.NguoiDungs.Where(x => x.IdNguoiDung == data.IdNguoiDung).FirstOrDefault();
                if (tk is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                tk.TaiKhoan = data.TaiKhoan;
                tk.MatKhau = data.MatKhau;
                tk.Email = data.Email;
                tk.IdVaiTro = data.IdVaiTro;
                if (data.IdNguoiDung == 0) db.NguoiDungs.Add(tk);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CapNhatNguoiDung(NguoiDung data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Json(result);
            }
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(data.Email);
            }
            catch (FormatException)
            {
                result.Message = "Email không hợp lệ";
                return Json(result);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {

                NguoiDung nd = db.NguoiDungs.Where(x => x.IdNguoiDung == data.IdNguoiDung).FirstOrDefault();
                if (nd is null)
                {
                    result.Message = "Người dùng không tồn tại";
                    return Json(result);
                }
                if (db.NguoiDungs.Any(x => x.Email == data.Email && x.IdNguoiDung != data.IdNguoiDung))
                {
                    result.Message = "Email mới đã tồn tại";
                    return Json(result);
                }
                nd.Email = data.Email;
                nd.HoTen = data.HoTen;
                nd.IdVaiTro = 2;
                if (data.IdNguoiDung == 0) db.NguoiDungs.Add(nd);
                try
                {
                    db.SaveChanges();
                    var idSV = 0;
                    SinhVien SV = new SinhVien();

                    if (nd.SinhViens != null && nd.SinhViens.Count > 0)
                    {
                        SV = nd.SinhViens.FirstOrDefault();
                        idSV = nd.SinhViens.FirstOrDefault().IdSinhVien;
                    }
                    SV.Mssv = data.SinhViens.FirstOrDefault().Mssv;
                    SV.MaLopSv = data.SinhViens.FirstOrDefault().MaLopSv;
                    if (idSV == 0)
                    {
                        SV.IdNguoiDung = data.IdNguoiDung;
                        db.SinhViens.Add(SV);
                        db.SaveChanges();
                    }
                    else
                    {                        
                        SV.IdNguoiDung = data.IdNguoiDung;
                        db.SaveChanges();
                    }
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ThamGiaLop(string malop,int idnguoidung)
        {
            ApiResult result = new ApiResult();
            if (malop is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin";
                return Json(result.Data);
            }
            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                var lop=db.LopHocs.Where(x=>x.MaLop == malop).FirstOrDefault();
                var sv = db.SinhViens.Where(x => x.IdNguoiDung == idnguoidung).FirstOrDefault();


                SVLop svlop = new SVLop();
                //if (svlop is null)
                //{
                //    result.Message = "Vui lòng thử lại";
                //    return Json(result);
                //}
                //tuluandapan.DapAn = data.DapAn;
                svlop.IdSinhVien = sv.IdSinhVien;
                svlop.NgayThamGia = DateTime.Now;
                svlop.IdLopHoc = lop.IdLopHoc;
                db.SVLops.Add(svlop);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult PostThaoLuan(ThongBao data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin ";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                ThongBao thongbao = new ThongBao();
                if (thongbao is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                thongbao.CauThaoLuan= data.CauThaoLuan;
                thongbao.NgayTao = DateTime.Now;
                thongbao.IdLopHoc = data.IdLopHoc;
                thongbao.IdNguoiDung = data.IdNguoiDung;
               
                db.ThongBaos.Add(thongbao);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult PostBinhLuan(BinhLuan data)
        {
            ApiResult result = new ApiResult();
            if (data is null)
            {
                result.Message = "Vui lòng điền đầy đủ thông tin ";
                return Json(result.Data);
            }

            using (ToMoToDBEntities db = new ToMoToDBEntities())
            {
                BinhLuan binhluan = new BinhLuan();
                if (binhluan is null)
                {
                    result.Message = "Vui lòng thử lại";
                    return Json(result);
                }
                binhluan.NoiDung = data.NoiDung;
                binhluan.NgayBinhLuan = DateTime.Now;
                binhluan.IdNguoiDung = data.IdNguoiDung;
                if (data.IdBaiHoc !=0)
                {
                    binhluan.IdBaiHoc = data.IdBaiHoc;
                }
                if (data.IdThaoLuan != 0)
                {
                    binhluan.IdThaoLuan = data.IdThaoLuan;
                }
                db.BinhLuans.Add(binhluan);
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result);
        }


    }

} 