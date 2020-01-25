using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Equity
    {
        public string invoiceNo { get; set; }
        public int userId { get; set; }
        public int investorId { get; set; }
        public string investorName { get; set; }
      
        public decimal amount { get; set; }
        public int accountId { get; set; }
        public string accountNo { get; set; }
        //public string documentId { get; set; }
        public DateTime activityTime { get; set; }
        public string comments { get; set; }
        public bool isdeposit { get; set; }
        public string activity { get; set; }
    }
    public class BO_EquityShareHolders
    {
        public int coaId { get; set; }
        public string treeName { get; set; }
    }
    public class BO_CapitalManagement
    {
        public int? investorId { get; set; }
        public string investorName { get; set; }
        public decimal? amount { get; set; }
    }
}