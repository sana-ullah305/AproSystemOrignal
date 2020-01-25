using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_OilGradeManagement
    {
        public int Id { get; set; }
        public string OilGade { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}