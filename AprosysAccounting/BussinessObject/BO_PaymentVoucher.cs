using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_PaymentVoucher
    {
        public string invoiceNo { get; set; }
        public DateTime? activityDate { get; set; }
        public int expenseTypeCategory { get; set; }
        public int expensetype { get; set; }
        public decimal? paid { get; set; }
        public decimal? balance { get; set; }
        public decimal? grossAmount { get; set; }
        public string comments { get; set; }
        public int empID { get; set; }
        public int? bankId { get; set; }
        public string checkNo { get; set; }
        public int? bankTransferAccountId { get; set; }
        public int paymentMode { get; set; }
    }
    public class BO_PaymentVoucherTable
    {
        public string invoiceNo { get; set; }
        public DateTime? activityDate { get; set; }
        public string hEADTYPE { get; set; }
        public decimal? amount { get; set; }
        public string name{get;set; }
    }
}