using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_PaymentHistory
    {
        public static MYJSONTblCustom GetPaymentHistoryList(JQueryDataTableParamModel Param, HttpRequestBase Request, int userId)
        {

            List<CustomerPaymentHistory> customerPayementHistoryList = GetPaymentHistoryData(Param, userId);
            //var salesList = GetSales(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<CustomerPaymentHistory> filteredCategories = customerPayementHistoryList;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = customerPayementHistoryList
                   .Where(
                      c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.customerName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    
                    || c.amount > 0 && c.amount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.recievedBy.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   );
            }
            else
            {
                filteredCategories = customerPayementHistoryList;
            }
            Func<CustomerPaymentHistory, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<CustomerPaymentHistory> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.customerName : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.invoiceNo : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.customerId : 0); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.userID : 0); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.recievedBy : ""); }
                    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.amount : 0); }
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

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             customerId = c.customerId,
                             customerName = c.customerName,
                             invoiceNo = c.invoiceNo,
                             transactionDate = c.transactionDate,
                             activityTimestamp = c.activityTimestamp,
                             amount = c.amount,
                             userID = c.userID,
                             recievedBy = c.recievedBy,
                             glId = c.glId,
                             tranId = c.tranId,
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = customerPayementHistoryList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;
        }

        private static List<CustomerPaymentHistory> GetPaymentHistoryData(JQueryDataTableParamModel Param, int userId)
        {
            DateTime startDateParameter = Param.Start_Date == DateTime.MinValue ? DateTime.Now.AddDays(-5) : Param.Start_Date.AddDays(-1);
            DateTime endDateParameter = Param.End_Date == DateTime.MinValue ? DateTime.Now.AddDays(5) : Param.End_Date.AddDays(1);
            List<CustomerPaymentHistory> customerPaymentHistoryList = new List<CustomerPaymentHistory>();
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                string userName = db_aa.Users.FirstOrDefault(x => x.Id == userId).UserName;
                var List = (from cus in db_aa.Customers
                                //join u in db_aa.Users on u.SalesPersonId equals sp.Id
                                //join itm in db_aa.Items on cus.TypeId equals itm.ItemTypeId
                            join gl in db_aa.Acc_GL on cus.Id equals gl.CustId
                            where gl.Credit > 0 && gl.IsActive == true && gl.CoaId == 10 && gl.ActivityTimestamp >= startDateParameter && gl.ActivityTimestamp <= endDateParameter
                            select new CustomerPaymentHistory
                            {
                                glId = gl.GlId,
                                tranId = gl.TranId,
                                customerId = cus.Id,
                                customerName = cus.FirstName + " " + cus.LastName,
                                invoiceNo = gl.InvoiceNo,
                                activityTimestamp = gl.ActivityTimestamp,
                                amount = gl.Credit,
                                recievedBy = userName,
                                userID = userId
                            }).ToList();
                customerPaymentHistoryList = List;
            }
            return customerPaymentHistoryList.OrderByDescending(x => (x.activityTimestamp)).ToList();
        }

        public  string DeletePaymentHistory(int tranId, int empID)
        {

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {

                var obj = db_aa.Acc_GL.Where(x => x.TranId == tranId && x.TranTypeId == 10 && x.IsActive == true || (x.GlId==tranId));
                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        item.IsActive = false;
                        item.ModifiedBy = empID;
                        item.ModifiedDate = DateTime.Now;
                    }
                }
              
                db_aa.SaveChanges();
                return "Success";
            }
        }

        public static void DeletePartialPayments(int tranId, int Invoice, int User)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var refTrans = db.Acc_GL.Where(x => (x.TranId == tranId) && x.IsActive == true).ToList();
                foreach (var item in refTrans)
                {
                    item.IsActive = false;
                    item.ModifiedBy = User;
                    item.ModifiedDate = DateTime.Now;
                }
                db.SaveChanges();
            }
        }
    }
}