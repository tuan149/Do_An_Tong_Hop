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
    
    public partial class CHITIET_CT_HOC
    {
        public int IdChiTiet { get; set; }
        public Nullable<int> IdCTHoc { get; set; }
        public Nullable<int> IdTuLuan { get; set; }
        public Nullable<int> IdTracNghiem { get; set; }
        public Nullable<int> IdBaiHoc { get; set; }
        public Nullable<int> ThuTu { get; set; }
        public Nullable<System.DateTime> NgayBatDau { get; set; }
    
        public virtual BaiHoc BaiHoc { get; set; }
        public virtual TracNghiem TracNghiem { get; set; }
        public virtual CT_HOC CT_HOC { get; set; }
        public virtual TuLuan TuLuan { get; set; }
    }
}
