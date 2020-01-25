using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_Service
    {
        public static string SaveRevenueSales(string name,decimal cost,string code)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var obj = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.TreeName.ToLower() == name.ToLower() && x.IsActive == true).FirstOrDefault();
                if (obj != null)
                {
                    return "Exists";
                }
                var objS = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5  && x.ServiceCode == code && x.IsActive == true).FirstOrDefault();
                if (objS != null)
                {
                    return "Code Already Exists";
                }

                var obj_COA = new ApprosysAccDB.Acc_COA();
                obj_COA.PId = 101;
                obj_COA.HeadAccount = 5;
                obj_COA.TreeName = name;
                obj_COA.CoaLevel = 2;
                obj_COA.OpeningBalance = 0;
                obj_COA.IsActive = true;
                obj_COA.Cost = cost;
                obj_COA.ServiceCode = code;
                db.Acc_COA.Add(obj_COA);
                db.SaveChanges();
                return "success";
            }

        }

        public static string EditRevenueSales(int id,string name, decimal cost,string code)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var objexist = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.CoaId == id).FirstOrDefault();
                var objS = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.ServiceCode == code && x.IsActive == true && x.CoaId != id).FirstOrDefault();
                if (objS != null)
                {
                    if (objS.ServiceCode.ToLower() == code.ToLower()) { return "Code Already Exists"; }
                    if (objS.TreeName.ToLower() == name.ToLower()) { return "Service Already Exists"; }


                }
                if (objexist.TreeName.ToLower() == name.ToLower() && objexist.ServiceCode.ToLower()==code.ToLower()) { objexist.Cost = cost;objexist.ServiceCode = code; db.SaveChanges(); return "Cost Updated"; }
                if (objexist.TreeName != name)
                {
                    var nameexists = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.TreeName.ToLower() == name.ToLower()).FirstOrDefault();
                    if (nameexists != null)
                    {
                        return "Exists";
                    }
                    
                    objexist.Cost = cost;
                    objexist.TreeName = name;
                    objexist.ServiceCode = code;
                    db.SaveChanges();
                    return "Service Updated";

                }
                return "success";
            }
        }
        public static MYJSONTblCustom LoadServiceTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var _customerlist = LoadService();//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Service> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _customerlist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.name.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredCategories = _customerlist;
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
                             name = c.name,
                             cost = c.cost,
                             code=c.code
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _customerlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Service> LoadService()
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_Service> List = (from _coa in db_aa.Acc_COA
                                         where _coa.HeadAccount == 5 && _coa.PId == 101 && _coa.IsActive == true
                                         select new BO_Service
                                         {
                                             id = _coa.CoaId,
                                             name = _coa.TreeName,
                                             cost = _coa.Cost ?? 0,
                                             code=_coa.ServiceCode
                                            

                                         }).OrderByDescending(x => x.id).ToList();
                return List;
            }
        }
        public static string DeleteRevenueService(int id)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var serviceTransaction = db.Acc_GL.Where(x => x.CoaId == id && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (serviceTransaction == null || serviceTransaction.Count > 0) { return "Transaction is Performed against Service, it can not be deleted "; }

                var obj = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.CoaId == id).FirstOrDefault();
                if (obj != null && obj.CoaId > 0)
                {
                    obj.IsActive = false;
                }
                db.SaveChanges();
                return "success";

            }
        }

        public static  BO_Service GetServiceByID(int id)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Acc_COA.Where(x => x.PId == 101 && x.HeadAccount == 5 && x.CoaId == id).FirstOrDefault();
                BO_Service obj_Service = new BussinessObject.BO_Service();
                obj_Service.id = id;
                obj_Service.name = obj.TreeName;
                obj_Service.code = obj.ServiceCode;
                obj_Service.cost = obj.Cost??0;
                return obj_Service;
            }
        }

    }
}