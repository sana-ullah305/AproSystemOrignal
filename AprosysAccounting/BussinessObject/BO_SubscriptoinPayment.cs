using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_SubscriptoinPayment
    {
        public string invoiceNo { get; set; }
        public DateTime? activityDate { get; set; }
        public string customerName { get; set; }
        public decimal? paidAmount { get; set; }

        public decimal? totalDues { get; set; }
        public int sort { get; set; }

    }
}