using System;
using System.Collections.Generic;

namespace AprosysAccounting.BussinessObject
{
    public class BO_PurchaseInvoice
    {
        public string invoiceNo { get; set; }
        public DateTime purchaseDate { get; set; }
        public int vendorID { get; set; }
        public List<BO_LineItems> items { get; set; }
        public string comments { get; set; }
        public decimal netAmount { get; set; }
        public decimal paid { get; set; }
        public int empID { get; set; }
    }

    public class Purchases
    {
        public string invoiceNo { get; set; }
        public DateTime purchaseDate { get; set; }
        public int vendorID { get; set; }
        public string vendorName { get; set; }
        public decimal netAmount { get; set; } = 0;
        public decimal paid { get; set; } = 0;
        public decimal balance { get { return netAmount - paid; } }
        public bool? isDeletable { get; set; }
    }
}