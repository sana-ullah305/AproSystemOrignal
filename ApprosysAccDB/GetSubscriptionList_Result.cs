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
    
    public partial class GetSubscriptionList_Result
    {
        public int subid { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public int DueDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<bool> SubStatus { get; set; }
        public Nullable<System.DateTime> SubDisableStartDate { get; set; }
        public Nullable<System.DateTime> SubDisableEndDate { get; set; }
    }
}
