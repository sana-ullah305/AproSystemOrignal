using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Expense
    {
        public static string SaveAdministrativeExpense(string name)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Acc_COA.Where(x => x.PId == 19 && x.HeadAccount == 4 && x.TreeName.ToLower() == name.ToLower() && x.IsActive == true).FirstOrDefault();
                if (obj != null)
                {
                    return "Expense Already Exists";
                }
                var obj_COA = new ApprosysAccDB.Acc_COA();
                obj_COA.PId = 19;
                obj_COA.HeadAccount = 4;
                obj_COA.TreeName = name;
                obj_COA.CoaLevel = 2;
                obj_COA.OpeningBalance = 0;
                obj_COA.IsActive = true;
                db.Acc_COA.Add(obj_COA);
                db.SaveChanges(); return "success";
                //return "success";
            }
        }
        public static MYJSONTblCustom LoadExpenseTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var _expenseList = LoadExpense();//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Service> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _expenseList
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.name.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredCategories = _expenseList;
            }
            /*Func<BO_Customers, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_Customers> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.lastName : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.firstName : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.phone : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.email : ""); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.openingBalance : 0); }
                    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.balance : 0); }
                    sSortDir[i] = Request["sSortDir_" + i + ""]; // asc or desc
                    //  var sortDirection = Request["sSortDir_0"];
                    if (sSortDir[i] == "asc")
                    {
                        query = (i == 0) ? filteredCategories.OrderBy(orderingFunction) : query.ThenBy(orderingFunction);
                    }
                    else
                    {
                        query = (i == 0) ? filteredCategories.OrderByDescending(orderingFunction) : query.ThenByDescending(orderingFunction);
                    }
                    filteredCategories = query;

                }

            }
            */
            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             id = c.id,
                             name = c.name
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _expenseList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Service> LoadExpense()
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_Service> List = (from _coa in db_aa.Acc_COA
                                         where _coa.HeadAccount == 4 && _coa.PId == 19 && _coa.IsActive == true
                                         select new BO_Service
                                         {
                                             id = _coa.CoaId,
                                             name = _coa.TreeName

                                         }).OrderByDescending(x => x.id).ToList();
                return List;
            }
        }

        public static string EditAdministrativeExpense(int id, string name)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var obj = db.Acc_COA.Where(x => x.PId == 19 && x.HeadAccount == 4 && x.CoaId == id && x.IsActive == true).FirstOrDefault();
                if (obj != null && obj.TreeName == name) { return "Expense Already Exists"; }
                var obj2 = db.Acc_COA.Where(x => x.PId == 19 && x.HeadAccount == 4 && x.TreeName == name && x.IsActive == true).FirstOrDefault();
                if (obj2 != null) { return "Expense Already Exists"; }
                obj.TreeName = name;
                db.SaveChanges(); return "success";//return obj_COA.CoaId;

            }
        }

        public static string DeleteAdministrativeExpense(int id)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var vendTransaction = db.Acc_GL.Where(x => x.CoaId == id && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (vendTransaction == null || vendTransaction.Count > 0) { return "Transaction is Performed from Expense, it can not be deleted "; }


                var obj = db.Acc_COA.Where(x => x.PId == 19 && x.HeadAccount == 4 && x.CoaId == id).FirstOrDefault();
                if (obj != null && obj.CoaId > 0)
                {
                    obj.IsActive = false;
                }
                db.SaveChanges();
                return "success";

            }
        }

        public static string GetExpenseByID(int id)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Acc_COA.Where(x => x.PId == 19 && x.HeadAccount == 4 && x.CoaId == id).FirstOrDefault();
                return obj.TreeName;

            }
        }

    }
}