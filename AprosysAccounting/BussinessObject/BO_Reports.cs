using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Reports
    {
    }
    public class BO_CashFlow
    {
        public string invoiceNo { get; set; }
        public string transType { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public DateTime activityTimeStamp { get; set; }
    }

    public class BO_TrialBalance
    {
        public string account { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
    }
}