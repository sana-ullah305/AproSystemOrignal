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
    
    public partial class Acc_GLTxLinks
    {
        public int GLTxLinkID { get; set; }
        public Nullable<int> GLID { get; set; }
        public Nullable<int> RelGLID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public Nullable<int> TranTypeID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}