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
    
    public partial class Reminder
    {
        public int SubscriptionID { get; set; }
        public Nullable<byte> CurrentCycleRemindercount { get; set; }
        public string ReminderLog { get; set; }
        public Nullable<System.DateTime> LastReminderSentDate { get; set; }
    
        public virtual Subscription Subscription { get; set; }
    }
}
