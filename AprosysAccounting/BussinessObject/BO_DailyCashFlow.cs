using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_DailyCashFlow
    {
        public string TYPENAME { get; set; }
        public int ID { get; set; }
        public string NAME { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TAX { get; set; }
        public int TYPEID { get; set; }
        public int GROUPID { get; set; }
        public string WeekDay { get; set; }
    }

    public class BO_WeeklyCashFlow
    {
        public string TYPE { get; set; }
        public int TYPED { get; set; }
        public int Sort { get; set; }
        public decimal? MON { get; set; }
        public decimal? TUE { get; set; }
        public decimal? WED { get; set; }
        public decimal? THU { get; set; }
        public decimal? FRI { get; set; }
        public decimal? SAT { get; set; }
        public decimal? SUN { get; set; }
        public decimal? TOTAL { get; set; }
        public DateTime? MonDate { get; set; }
        public DateTime? TueDate { get; set; }
        public DateTime? WedDate { get; set; }
        public DateTime? ThuDate { get; set; }
        public DateTime? FriDate { get; set; }
        public DateTime? SatDate { get; set; }
        public DateTime? SunDate { get; set; }

    }
}