using ApprosysAccDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public static class TaxesHelper
    {
        public static List<KeyValuePair<int, decimal>> GetTaxes()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    List<KeyValuePair<int, decimal>> taxesList = new List<KeyValuePair<int, decimal>>();
                    //Sales Tax
                    taxesList.Add(new KeyValuePair<int, decimal>(1, db.Taxes.Where(x => x.taxesID == 1).FirstOrDefault().taxPercent.Value));
                    //Services Tax
                    taxesList.Add(new KeyValuePair<int, decimal>(2, db.Taxes.Where(x => x.taxesID == 2).FirstOrDefault().taxPercent.Value));
                    return taxesList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}