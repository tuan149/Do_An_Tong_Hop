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
    
    public partial class TuLuan_CauHoi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TuLuan_CauHoi()
        {
            this.SinhVien_TL_DapAn = new HashSet<SinhVien_TL_DapAn>();
            this.TuLuans = new HashSet<TuLuan>();
        }
    
        public int IdCauHoiTuLuan { get; set; }
        public string CauHoiTuLuan { get; set; }
        public int IdNguoiDung { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien_TL_DapAn> SinhVien_TL_DapAn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TuLuan> TuLuans { get; set; }
    }
}
