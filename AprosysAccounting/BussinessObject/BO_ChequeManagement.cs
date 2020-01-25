using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_ChequeManagement
    {
        public string invoiceNo { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string bankName { get; set; }
        public string documentNo { get; set; }
        public DateTime? chequeReceivedDate { get; set; }
        public decimal chequeReceivedAmount { get; set; }

    }
    public class UnpaidCheckList
    {
        public int glId { get; set; }
        public string documentId { get; set; }
    }
}