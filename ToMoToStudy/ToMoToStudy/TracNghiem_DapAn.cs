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
    
    public partial class TracNghiem_DapAn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TracNghiem_DapAn()
        {
            this.SinhVien_BaiLam_TracNghiem = new HashSet<SinhVien_BaiLam_TracNghiem>();
        }
    
        public int IdDapAn { get; set; }
        public string DapAn { get; set; }
        public Nullable<bool> DapAnDung { get; set; }
        public int IdCauHoi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien_BaiLam_TracNghiem> SinhVien_BaiLam_TracNghiem { get; set; }
        public virtual TracNghiem_CauHoi TracNghiem_CauHoi { get; set; }
    }
}