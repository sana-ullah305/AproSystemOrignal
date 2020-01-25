using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Customers
    {
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }

        public string phone { get; set; }
        public string email { get; set; }
        public int? salesPerson { get; set; }
        public string salesPersonName { get; set; }
        public decimal openingBalance { get; set; }
        public DateTime? startDate { get; set; }
      
        public string misc { get; set; }
        public int createdBy { get; set; }
        public int modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isActive { get; set; }
        public decimal balance { get; set; }
        public string cnic { get; set; }
        public string ntn { get; set; }
    }

    public class BO_Customerlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DueDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }
        public string PhoneNo { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public int? SalesPersonId { get; set; }
    }

    public class BO_CustomerReport
    {
        public string CustomerName { get; set; }
        public string SalesPersonName { get; set; }

        public int CustomerID { get; set; }
        public int SalesPersonID { get; set; }

        public string InvoiceID { get; set; }

        public DateTime TransactionDate { get; set; }


        public decimal Sales { get; set; }
        public decimal Recieved { get; set; }
        public decimal OpeningBalance { get; set; }
        public string DocID { get; set; }

        public string Comment { get; set; }
    }
}