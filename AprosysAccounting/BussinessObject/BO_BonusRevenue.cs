using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_BonusRevenue
    {
        public int? TransactionId { get; set; }
        public DateTime? activityDate { get; set; }
        public string RevenueAccount { get; set; }
        public string BankAccount { get; set; }
        public decimal? BonusAmount { get; set; }
        public string Misc { get; set; }
    }
}