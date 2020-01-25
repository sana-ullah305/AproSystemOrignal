using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Inventory
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public decimal itemquantity { get; set; }
    }
}