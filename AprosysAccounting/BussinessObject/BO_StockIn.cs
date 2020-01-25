using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_StockIn
    {
        public int coaID { get; set; }
        public int empID { get; set; }
        public DateTime date { get; set; }
        public decimal netAmount { get; set; }
        public List<BO_LineItems> items { get; set; }
        public string comments { get; set; }
    }
}