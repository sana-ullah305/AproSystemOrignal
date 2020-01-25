using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_BalanceSheet
    {
        public string MainAccount { get; set; }
        public int? HeadAccount { get; set; }
        public string TreeName { get; set; }
        public decimal? DEBIT { get; set; }
        public decimal? Credit { get; set; }
    }
}