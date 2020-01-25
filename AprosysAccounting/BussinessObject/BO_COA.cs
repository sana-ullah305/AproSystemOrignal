using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_COA
    {
    }
    public class BO_ExpenseType
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class BO_Expense
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}