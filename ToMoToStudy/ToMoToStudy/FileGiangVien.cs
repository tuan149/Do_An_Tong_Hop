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
    
    public partial class FileGiangVien
    {
        public int IdFileThongBao { get; set; }
        public Nullable<int> IdBaiHoc { get; set; }
        public Nullable<int> IdThaoLuan { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string Link { get; set; }
    
        public virtual BaiHoc BaiHoc { get; set; }
        public virtual ThongBao ThongBao { get; set; }
    }
}