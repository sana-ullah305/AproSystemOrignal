//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApprosysAccDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Acc_PartialCredit
    {
        public int ID { get; set; }
        public string InvoiceNum { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool Deleted { get; set; }
        public int AddedBy { get; set; }
    }
}
