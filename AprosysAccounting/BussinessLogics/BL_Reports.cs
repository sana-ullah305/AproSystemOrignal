using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Reports
    {
        public static MYJSONTblCustom LoadCashFlowTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {

            var _cashflow = LoadCashFlowTable(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_CashFlow> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _cashflow
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.debit != 0 && c.debit.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.credit != 0 && c.credit.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.transType.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _cashflow;
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
                             invoiceNo = c.invoiceNo,
                             activityTimeStamp = c.activityTimeStamp,
                             transType = c.transType,
                             debit = c.debit,
                             credit = c.credit

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _cashflow.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_CashFlow> LoadCashFlowTable(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_CashFlow> lst_cashFlow = new List<BussinessObject.BO_CashFlow>();

                var _Cashflow = db_aa.Report_CashFlow((DateTime.Now.AddMonths(-96)), BL_Common.GetDatetime()).ToList();
                foreach (var item in _Cashflow)
                {
                    BO_CashFlow obj = new BussinessObject.BO_CashFlow();
                    obj.invoiceNo = item.InvoiceNo;
                    obj.activityTimeStamp = item.ActivityTimestamp ?? BL_Common.GetDatetime();
                    obj.transType = item.TransType;
                    obj.debit = item.Debit;
                    obj.credit = item.Credit;
                    lst_cashFlow.Add(obj);
                }
                return lst_cashFlow;
            }

        }


        public static MYJSONTblCustom LoadTrialBalance(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {

            var _cashflow = LoadTrialBalance(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_TrialBalance> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _cashflow
                   .Where(
                    c => c.account.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.debit != 0 && c.debit.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.credit != 0 && c.credit.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _cashflow;
            }

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             account = c.account,
                             debit = c.debit,
                             credit = c.credit

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _cashflow.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_TrialBalance> LoadTrialBalance(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_TrialBalance> lst_cashFlow = new List<BussinessObject.BO_TrialBalance>();
                Param.End_Date = Param.End_Date.AddDays(1);
                var _Cashflow = db_aa.Report_TrialBalance(Param.Start_Date, Param.End_Date).ToList();
                foreach (var item in _Cashflow)
                {
                    BO_TrialBalance obj = new BussinessObject.BO_TrialBalance();
                    obj.account = item.ACCOUNT;

                    obj.debit = item.DEBIT;
                    obj.credit = item.CREDIT;
                    lst_cashFlow.Add(obj);
                }
                BO_TrialBalance obj1 = new BussinessObject.BO_TrialBalance();
                obj1.account = "SUM";
                obj1.credit = lst_cashFlow.Sum<BO_TrialBalance>(x => x.credit);
                obj1.debit = lst_cashFlow.Sum<BO_TrialBalance>(x => x.debit);
                lst_cashFlow.Insert(0, obj1);
                return lst_cashFlow;
            }

        }

    }
}