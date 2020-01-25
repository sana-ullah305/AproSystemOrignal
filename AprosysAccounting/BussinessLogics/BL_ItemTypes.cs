using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApprosysAccDB;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_ItemTypes
    {
        public static List<BO_ItemTypes> GetItemTypes()
        {
           List<BO_ItemTypes> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {


                obj = (from itemtypes in db_aa.ItemTypes
                      where itemtypes.IsActive==true
                       select new BO_ItemTypes
                       {
                           id = itemtypes.Id,
                           name = itemtypes.Name

                       }).ToList();

                return obj;
            }

        }

    }
}