//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OzetAPP.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_kategoriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_kategoriler()
        {
            this.tbl_islemler = new HashSet<tbl_islemler>();
        }
    
        public int kategori_id { get; set; }
        [Required]
        public string kategori_ad { get; set; }
        public string kategori_tuketimtur { get; set; }
        public Nullable<int> kategori_kullanici { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_islemler> tbl_islemler { get; set; }
        public virtual tbl_kullanicilar tbl_kullanicilar { get; set; }
    }
}
