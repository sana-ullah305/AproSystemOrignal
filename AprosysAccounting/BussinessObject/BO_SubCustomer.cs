using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AprosysAccounting.BussinessObject
{
    public class BO_SubCustomer : BO_Customers
    {
      //  public BO_SubCustomer() { }
        public int subid { get; set; }
        //public string lastName { get; set; }
        //public string firstName { get; set; }
        //public string phone { get; set; }
        //public string email { get; set; }
        //public decimal openingBalance { get; set; }
        //public DateTime? startDate { get; set; }
        public int dueDate { get; set; }

        //public DateTime? paymentDate { get; set; }
        //public string comments { get; set; }
        public decimal subscriptionAmount { get; set; }
        public int subcreatedBy { get; set; }
        public int submodifiedBy { get; set; }
        public DateTime subcreatedOn { get; set; }
        public DateTime submodifiedOn { get; set; }

        public DateTime? subLastReminderSent { get; set; }
        public bool subisActive { get; set; }
        public bool subStatus { get; set; }
        public string  subscriptionStatus { get; set; }
        //public DateTime? subDisableStartDate { get; set; }
        //public DateTime? subDisableEndDate { get; set; }
        //public int subEnableBy { get; set; }
        //public int subDisableBy { get; set; }
        //public DateTime subEnableActivityDate { get; set; }
        //public DateTime subDisableActivityDate { get; set; }
        public List<BO_SubscriptionExcludingDates> sed { get; set; }

        /// <summary>
        /// Customer.CustId
        /// </summary>
        public int CustId { get; set; }
    }
    public class BO_SubscriptionExcludingDates
    {
        public int subid { get; set; }
        public DateTime? subDisableStartDate { get; set; }
        public DateTime? subDisableEndDate { get; set; }
        public bool sedisActive { get; set; }
        //public int subEnableBy { get; set; }
        //public int subDisableBy { get; set; }
        //public DateTime subEnableActivityDate { get; set; }
        //public DateTime subDisableActivityDate { get; set; }
    }
}