using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_OilGradeManagement
    {
        public static MYJSONTblCustom LoadOilGradeTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var _oilGradeList = GetOilGradeManagementList(Param);
            IEnumerable<BO_OilGradeManagement> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _oilGradeList
                   .Where(
                    c => c.Id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.OilGade.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredCategories = _oilGradeList;
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
                             Id = c.Id,
                             OilGade = c.OilGade ?? "",
                             IsActive = c.IsActive ?? false,
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _oilGradeList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;
        }
        public static IList<BO_OilGradeManagement> GetOilGradeManagementList(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_OilGradeManagement> List = (from o in db_aa.OilGrades
                                                    where o.IsActive == true
                                                    orderby o.Id descending
                                                    select new BO_OilGradeManagement
                                                    {
                                                        Id = o.Id,
                                                        OilGade = o.OilGade ?? "",
                                                        IsActive = o.IsActive ?? false
                                                    }).OrderByDescending(x => x.Id).ToList();
                List<BO_OilGradeManagement> ListtoReturn = new List<BO_OilGradeManagement>();
                ListtoReturn = List;
                return ListtoReturn;
            }
        }

        public static string DeleteGradeRecord(int Id)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var chk = db_aa.Items.FirstOrDefault(x => x.OilGradeId == Id && x.IsActive == true);
                if (chk != null)
                    return "You Can't Delete Oil Grade Because Its being Transacted";
                var delRcd = db_aa.OilGrades.FirstOrDefault(x => x.Id == Id);
                if (delRcd != null)
                {
                    delRcd.IsActive = false;
                    db_aa.SaveChanges();
                    return "success";
                }
                else
                    return "Record Not Found";
            }


        }

        public static string SaveGrade(int? Id, string Grade)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var chk = db_aa.OilGrades.FirstOrDefault(x => x.Id == Id) != null ? db_aa.OilGrades.FirstOrDefault(x => x.Id == Id) : db_aa.OilGrades.Add(new OilGrade() { OilGade = Grade, IsActive = true });
                if (chk != null || chk.Id != 0)
                {
                    chk.OilGade = Grade;
                    chk.IsActive = true;
                    db_aa.SaveChanges();
                    return "success";
                }
                else
                    return "Record Not Found";
            }


        }
    }
}