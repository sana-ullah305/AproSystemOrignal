using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Vendor
    {
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }

        public string phone { get; set; }
        public string email { get; set; }
        public string terms {get;set;}
        public decimal creditLimit { get; set; }
        public decimal balance { get; set; }
        public string misc { get; set; }
        public int createdBy { get; set; }
        public int modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isActive { get; set; }
        public string cnic { get; set; }
        public string ntn { get; set; }
        
    }

    public class BO_Vendorlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}