using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_PartialPayment
    {
        public string InvoiceNum { get; set; }
        public DateTime CreatedDate { get; set; } 
        public decimal Amount { get; set; }
    }
}