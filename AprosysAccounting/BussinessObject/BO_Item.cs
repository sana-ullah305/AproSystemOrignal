using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{

    public class BO_Item
    {
        public int id { get; set; }
        public int itemTypeId { get; set; }
        public string name { get; set; }
        public string itemCode { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public decimal minQuantity { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal sellPrice { get; set; }
        public decimal taxPercent { get; set; }
        public int createdBy { get; set; }
        public int modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isActive { get; set; }
        public bool isTaxable { get; set; }
        public decimal? stock { get; set; }
        public int oilGradeId { get; set; }
        public string oilGrade { get; set; }
        public decimal? packingInLitre { get; set; }
        public int? quantityInCarton { get; set; }
        public decimal? stockInLtrs { get; set; }

        public decimal? stockInAmount { get; set; }
        
    }

    public class BO_ServicesProvide
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal cost { get; set; }

        public string serviceCode { get; set; }
    }
}