using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Report_SalesPerson
    {
        public string CustomerName { get; set; }
        public string SalesPersonName { get; set; }
        public string OrignalSalesPersonName { get; set; }

        public int CustomerID { get; set; }
        public int SalesPersonID { get; set; }
        public int OrinalSalePersonId { get; set; }

        public string InvoiceID { get; set; }

        public DateTime TransactionDate { get; set; }


        public decimal Sales{ get; set; }
        public decimal Recieved { get; set; }
        public decimal OpeningBalance { get; set; }
        public string DocID { get; set; }

        public string Comment { get; set; }
    }
}