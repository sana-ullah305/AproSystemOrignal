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
    
    public partial class usp_GetSubDuesofCustomer_Result
    {
        public Nullable<int> TranId { get; set; }
        public Nullable<System.DateTime> SubscriptionDueDate { get; set; }
        public Nullable<int> CustId { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> AllDueBefore { get; set; }
    }
}