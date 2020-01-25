using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Banks
    {
        public DateTime transDate { get; set; }
        public string invoiceNo { get; set; }
        public int bankID { get; set; }
        public decimal amount { get; set; }
        public int empID { get; set; }
        public string comment { get; set; }
        public string documentId { get; set; }
    }

    public class BanksList {
        public int bankID { get; set; }
        public string bankName { get; set; }
        public decimal amount { get; set; }
        public DateTime transDate { get; set; }
    }

    
}