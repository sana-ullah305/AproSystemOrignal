using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_Inventory
    {
        public static MYJSONTblCustom LoadInventoryTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _inventoryList = LoadInventory(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Inventory> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _inventoryList
                   .Where(
                    c =>
                    !string.IsNullOrEmpty(c.itemCode) && c.itemCode.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || !string.IsNullOrEmpty(c.itemName) && c.itemName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.itemquantity>0 && c.itemquantity.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredCategories = _inventoryList;
            }
            //Func<BO_Inventory, dynamic> orderingFunction = null;
            //int iSortColums = Convert.ToInt32(Param.iSortingCols);

            //if (iSortColums > 0)
            //{
            //    var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
            //    var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
            //    var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
            //    var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
            //    var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
            //    var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
            //    IOrderedEnumerable<BO_Inventory> query = null;
            //    int[] iSortCol = new int[iSortColums];
            //    string[] sSortDir = new string[iSortColums];
            //    for (int _i = 0; _i < iSortCol.Length; _i++)
            //    {
            //        int i = _i;
            //        iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
            //        if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.name : ""); }
            //        else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.itemCode : ""); }
            //        else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.unit : ""); }
            //        else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.taxPercent : 0); }
            //        else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.purchasePrice : 0); }
            //        else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.sellPrice : 0); }
            //        sSortDir[i] = Request["sSortDir_" + i + ""]; // asc or desc
            //        //  var sortDirection = Request["sSortDir_0"];
            //        if (sSortDir[i] == "asc")
            //        {
            //            query = (i == 0) ? filteredCategories.OrderBy(orderingFunction) : query.ThenBy(orderingFunction);
            //        }
            //        else
            //        {
            //            query = (i == 0) ? filteredCategories.OrderByDescending(orderingFunction) : query.ThenByDescending(orderingFunction);
            //        }
            //        filteredCategories = query;

            //    }

            //}

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             id = c.itemId,
                             itemCode = c.itemCode,
                             itemName = c.itemName,
                             itemquantity = c.itemquantity
                            
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _inventoryList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Inventory> LoadInventory(JQueryDataTableParamModel Param)
        {


            List<BO_Inventory> lst_inventory = new List<BO_Inventory>();
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                BO_Inventory obj;
                var lst = db_aa.GetStockList(0).ToList();
                foreach (var item in lst.ToList())
                {
                    obj = new BO_Inventory();
                    obj.itemId = item.ItemId ?? 0;
                    obj.itemCode = item.ItemCode;
                    obj.itemName = item.Name;
                    obj.itemquantity = item.QTY ?? 0;
                    lst_inventory.Add(obj);
                }

                //if (Param.SearchType != 0)
                //{
                //    if (Param.SearchType == 1 && Param.SearchValue != null && Param.SearchValue != "")// && Param.SearchValue != null && Param.SearchValue != " ")
                //    {
                //        List = List.Where(x => x.name.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();
                //    }
                //    else if (Param.SearchType == 2)// && Param.SearchValue != null && Param.SearchValue != " ")
                //    {
                //        List = List.Where(x => x.itemCode.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();

                //    }

                //}

                //List<BO_Inventory> ListtoReturn = new List<BO_Inventory>();
                //ListtoReturn = lst;
                return lst_inventory;
            }
        }
    }
}