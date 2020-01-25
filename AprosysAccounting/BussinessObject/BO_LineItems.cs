using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_LineItems
    {
        public int glid { get; set; }
        public int itemID { get; set; }
        public int coaID { get; set; }
        public bool isServiceItem { get; set; }
        public decimal qty { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? tax { get; set; }
        public decimal? amount { get; set; }

        
    }
}