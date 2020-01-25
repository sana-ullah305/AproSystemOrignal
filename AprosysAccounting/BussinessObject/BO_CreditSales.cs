using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_CreditSales
    {
        public string invoiceNo { get; set; }
       
        public int customerID { get; set; }
        public string customerName { get; set; }
        public DateTime sellDate { get; set; }

        public decimal netAmount { get; set; } = 0;
        public decimal paidAmount { get; set; } = 0;

    }

    public class BO_UnpaidCreditCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class BO_UnpaidCreditCustomerInvoice
    {
        public string invoiceNo { get; set; }
        public string salePerson { get; set; }
        public DateTime sellDate { get; set; }
        public decimal invoiceAmount { get; set; }
        public decimal paidAmount { get; set; }
        public decimal owedAmount { get { return invoiceAmount - paidAmount; } }
        public decimal netAmount { get; set; } 
    }

    public class BO_CreditSalesUpdate
    {
        public string invoiceNo { get; set; }

        public decimal? Amount { get; set; }
        public int customerID { get; set; }
        public DateTime creditPaidDate { get; set; }
        public int modifiedBy { get; set; }

        public string DocumentID { get; set; }

        public string BankName { get; set; }
       public BussinessLogics.Constants.PaymentMode PaymentMode { get; set; }
        public string Comment { get; set; }
    }
}