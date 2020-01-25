using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_SubscriptionPayment
    {
        public static MYJSONTblCustom LoadSubscriptionPaymentVoucherTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _rspvoucherlist = LoadSubscriptionPaymentVoucher(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_SubscriptoinPayment> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _rspvoucherlist
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.paidAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.customerName.ToString().ToLower().Contains(Param.sSearch.ToLower())


                    );
            }
            else
            {
                filteredCategories = _rspvoucherlist;
            }
            Func<BO_SubscriptoinPayment, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                //var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                //var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                //var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                //var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                //var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                //var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                //IOrderedEnumerable<BO_ReceiptVoucher> query = null;
                //int[] iSortCol = new int[iSortColums];
                //string[] sSortDir = new string[iSortColums];
                //for (int _i = 0; _i < iSortCol.Length; _i++)
                //{
                //    int i = _i;
                //    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                //    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.lastName : ""); }
                //    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.firstName : ""); }
                //    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.phone : ""); }
                //    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.email : ""); }
                //    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.openingBalance : 0); }
                //    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.balance : 0); }
                //    sSortDir[i] = Request["sSortDir_" + i + ""]; // asc or desc

                //    if (sSortDir[i] == "asc")
                //    {
                //        query = (i == 0) ? filteredCategories.OrderBy(orderingFunction) : query.ThenBy(orderingFunction);
                //    }
                //    else
                //    {
                //        query = (i == 0) ? filteredCategories.OrderByDescending(orderingFunction) : query.ThenByDescending(orderingFunction);
                //    }
                //    filteredCategories = query;

                //}

            }

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             InvoiceNo = c.invoiceNo,
                             ActivityDate = c.activityDate,
                             CustomerName = c.customerName,
                             PaidAmount = c.paidAmount,
                             Totaldues = c.totalDues



                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _rspvoucherlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_SubscriptoinPayment> LoadSubscriptionPaymentVoucher(JQueryDataTableParamModel Param)
        {
            List<BO_SubscriptoinPayment> lst_SubscriptionPaymentVoucherTable = new List<BO_SubscriptoinPayment>();
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                BO_SubscriptoinPayment obj;

                var lst = db_aa.GetSubscriptionVoucherList("", Param.Start_Date, Param.End_Date);
                foreach (var item in lst.ToList())
                {
                    obj = new BO_SubscriptoinPayment();
                    obj.invoiceNo = item.InvoiceNo;
                    obj.activityDate = item.ActivityTimestamp;
                    obj.customerName = item.Customer;
                    obj.paidAmount = item.Amount;
                   // obj.sort = item.sort??0;
                    obj.totalDues = item.Amount;//Farooq
                    lst_SubscriptionPaymentVoucherTable.Add(obj);
                }






            }
            if (Param.SearchType != 0)
            {

            }

            // List<BO_ReceiptVoucherTable> ListtoReturn = new List<BO_ReceiptVoucherTable>();
            // ListtoReturn = lst_receiptVoucherTable;
            return lst_SubscriptionPaymentVoucherTable;
        }

    }
}