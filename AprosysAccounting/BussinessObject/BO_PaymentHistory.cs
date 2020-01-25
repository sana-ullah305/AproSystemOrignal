using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class CustomerPaymentHistory
    {
        public int glId { get; set; }
        public int? tranId { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string invoiceNo { get; set; }
        public DateTime? activityTimestamp { get; set; }
        public DateTime? transactionDate { get; set; }
        public decimal? amount { get; set; }
        public int userID { get; set; }
        public string recievedBy { get; set; }
    }
}