//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToMoToStudy
{
    using System;
    using System.Collections.Generic;
    
    public partial class BinhLuan
    {
        public int IdBinhLuan { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> NgayBinhLuan { get; set; }
        public int IdNguoiDung { get; set; }
        public Nullable<int> IdBaiHoc { get; set; }
        public Nullable<int> IdThaoLuan { get; set; }
    
        public virtual BaiHoc BaiHoc { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual ThongBao ThongBao { get; set; }
    }
}