using ApprosysAccDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class Util
    {
        public static string GetNextVoucher(int transTypeID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                string voucher = BL_Common.GetLastVoucher(transTypeID);
                string preFix = String.Empty;
                switch (transTypeID)
                {
                    case 1:
                        preFix = "PNV-";
                        break;
                    case 2:
                        preFix = "SNV-";
                        break;
                    case 3:
                        preFix = "RCT-";
                        break;
                    case 4:
                        preFix = "PMT-";
                        break;
                    case 5:
                        preFix = "SUBS-";
                        break;
                    case 6:
                        preFix = "BT-";
                        break;
                    case 7:
                        preFix = "STI-";
                        break;
                    case 8:
                        preFix = "STO-";
                        break;
                    case 11:
                        preFix = "EQ-";
                        break;
                    default:
                        break;
                }
                if (String.IsNullOrEmpty(voucher)) { return (preFix + "1"); }
                var increment = Convert.ToInt32(voucher.Split('-')[1]) + 1;
                return (preFix + increment);
            }
        }
    }
}