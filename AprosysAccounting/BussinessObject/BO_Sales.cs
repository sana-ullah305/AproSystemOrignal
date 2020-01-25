using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_SaleInvoice
    {
        public string invoiceNo { get; set; }
        public DateTime saleDate { get; set; }
        public int customerID { get; set; }
        public List<BO_LineItems> items { get; set; }
        public string comments { get; set; }
        public decimal netAmount { get; set; }
        public decimal paid { get; set; }
        public int empID { get; set; }
        public DateTime? createdDate { get; set; }
        public bool IsSalesCredit { get; set; }
        public int salesPersonId { get; set; }
    }

    public class BO_Sales
    {
        public string invoiceNo { get; set; }
        public DateTime sellDate { get; set; }
        public int customerID { get; set; }
        public string customerName { get; set; }
        public decimal netAmount { get; set; } = 0;
        public decimal paid { get; set; } = 0;
        public decimal balance { get { return netAmount - paid; } }
        public bool isCustomerDeleted { get; set; }
        public bool isSalesCredit { get; set; }

        public DateTime? creditPaidDate { get; set; }
    }
}