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

    public partial class tbl_islemler
    {
        public int islem_id { get; set; }
        public Nullable<int> islem_kullanici { get; set; }
        public Nullable<int> islem_kategori { get; set; }
        public Nullable<bool> islem_tip { get; set; }
        
        public Nullable<decimal> islem_deger1 { get; set; }
        
        public Nullable<decimal> islem_deger2 { get; set; }
        public Nullable<int> islem_hane { get; set; }
        public Nullable<System.DateTime> islem_tarih { get; set; }
        public Nullable<decimal> islem_bakiye { get; set; }

        public bool islem_islem { get; set; }
        [Required(ErrorMessage = "Bu Alan Bo� B�rak�lamaz")]
        public string islem_tutar { get; set; }

        public string islem_tuketim { get; set; }

        public virtual tbl_haneler tbl_haneler { get; set; }
        public virtual tbl_kategoriler tbl_kategoriler { get; set; }
        public virtual tbl_kullanicilar tbl_kullanicilar { get; set; }
    }
}
