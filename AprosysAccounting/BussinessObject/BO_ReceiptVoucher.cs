using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_ReceiptVoucher
    {
        public string invoiceNo { get; set; }
        public DateTime rActivityDate { get; set; }
        public int rCustomerId { get; set; }
        public decimal rRecived { get; set; }
        public decimal rGrossAmount { get; set; }
        public decimal rbalance { get; set; }
        public string rComments { get; set; }
        public int TypeId { get; set; }
        public int DueDate { get; set; }
        public int empID { get; set; }
        public decimal rSubscriptionAmount { get; set; }
    }
    public class BO_ReceiptVoucherTable
    {
        public string invoiceNo { get; set; }
        public DateTime? activityDate { get; set; }
        public string customerName { get; set; }
        public decimal? amount { get; set; }
        public int sort { get; set; }
    }
}