using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_PaymentVoucher
    {
        public static List<BO_ExpenseType> GetPaymentVoucherExpenseTypeList()
        {
            List<BO_ExpenseType> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                //keyVal = coaL.Where(x => x.coaId == 10 || x.coaId == 12 || (x.pId == 4 && x.coaId != 13)).Select(x => new KeyValuePair<int, string>(x.coaId, x.treeName)).ToList();

                obj = (from _coa in db_aa.Acc_COA
                       where
                       // _coa.CoaId == 10 || _coa.CoaId == 12 || _coa.CoaId == 19 will add later
                         _coa.CoaId == 12 || _coa.CoaId == 19
                       select new BO_ExpenseType
                       {
                           id = _coa.CoaId,
                           name = _coa.TreeName
                       }).ToList();

                return obj;
            }
        }

        public static List<BO_Expense> GetExpense(int coaID)
        {
            List<BO_Expense> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                switch (coaID)
                {
                    case 10:
                        obj = (from _cust in db_aa.Customers where _cust.IsActive select new BO_Expense { id = _cust.Id, name = ((_cust.FirstName ?? "") + "" + (_cust.LastName ?? "")) }).ToList();
                        break;
                    case 12:
                        obj = (from _vend in db_aa.Vendors where _vend.IsActive ?? true select new BO_Expense { id = _vend.ID, name = ((_vend.FirstName ?? "") + "" + (_vend.LastName ?? "")) }).ToList();
                        break;

                    default:
                        obj = (from _coa in db_aa.Acc_COA where (_coa.IsActive == true && _coa.PId == coaID) select new BO_Expense { id = _coa.CoaId, name = (_coa.TreeName ?? "") }).ToList();
                        break;
                }


                return obj;
            }
        }


        public static string SavePaymentVoucher(BO_PaymentVoucher _rvoucher)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        int transType = (int)Constants.TransactionTypes.PaymentVoucher;
                        _rvoucher.invoiceNo = Util.GetNextVoucher(transType);
                        var actingID = _rvoucher.expenseTypeCategory == 10 || _rvoucher.expenseTypeCategory == 12 ? _rvoucher.expensetype : 0;
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = _rvoucher.empID, Comments = _rvoucher.comments, CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null, VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null, Debit = _rvoucher.paid, Credit = _rvoucher.paid, ActivityTimestamp = _rvoucher.activityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, CreatedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedBy = _rvoucher.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        int _coaId = 11;
                        if (_rvoucher.paymentMode == 2) { _coaId = _rvoucher.bankId??0; }
                        if (_rvoucher.paymentMode == 3) { _coaId = _rvoucher.bankTransferAccountId ?? 0; }
                        var GLPaid = new Acc_GL() { TranId = GLParent.GlId, UserId = _rvoucher.empID, CoaId = _coaId, CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null, VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null, Credit = _rvoucher.paid, ActivityTimestamp = _rvoucher.activityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, CreatedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedBy = _rvoucher.empID, ModifiedDate = DateTime.Now, DocumentId = _rvoucher.checkNo };
                        db.Acc_GL.Add(GLPaid);

                        var GLType = new Acc_GL() { TranId = GLParent.GlId, UserId = _rvoucher.empID, CoaId = _rvoucher.expenseTypeCategory == 10 || _rvoucher.expenseTypeCategory == 12 ? _rvoucher.expenseTypeCategory : _rvoucher.expensetype, CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null, VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null, Debit = _rvoucher.paid, ActivityTimestamp = _rvoucher.activityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, CreatedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedBy = _rvoucher.empID, ModifiedDate = DateTime.Now};
                        db.Acc_GL.Add(GLType);
                        db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static string EditPaymentVoucher(BO_PaymentVoucher _rvoucher)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var actingID = _rvoucher.expenseTypeCategory == 10 || _rvoucher.expenseTypeCategory == 12 ? _rvoucher.expensetype : 0;
                        var PreviousPaymentV = db.Acc_GL.Where(x => x.InvoiceNo == _rvoucher.invoiceNo).ToList();
                        //Parent Entry
                        var parent = PreviousPaymentV.Find(x => x.CoaId == 0 && x.IsActive == true);
                        parent.Comments = _rvoucher.comments;
                        parent.CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null;
                        parent.VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null;
                        parent.Debit = _rvoucher.paid;
                        parent.Credit = _rvoucher.paid;
                        parent.ActivityTimestamp = _rvoucher.activityDate;
                        parent.UserId = _rvoucher.empID;
                        parent.ModifiedBy = _rvoucher.empID;
                        parent.ModifiedDate = DateTime.Now;
                        db.SaveChanges();

                        var cash = PreviousPaymentV.Find(x => x.CoaId == 11 && x.IsActive == true);
                        cash.CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null;
                        cash.VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null;
                        cash.Credit = _rvoucher.paid;
                        cash.ActivityTimestamp = _rvoucher.activityDate;
                        cash.UserId = _rvoucher.empID;
                        cash.ModifiedBy = _rvoucher.empID;
                        cash.ModifiedDate = DateTime.Now;
                        db.SaveChanges();

                        var totals = PreviousPaymentV.Find(x => x.CoaId != 11 && x.CoaId != 0 && x.Debit > 0 && x.IsActive == true);
                        totals.CoaId = _rvoucher.expenseTypeCategory == 10 || _rvoucher.expenseTypeCategory == 12 ? _rvoucher.expenseTypeCategory : _rvoucher.expensetype;
                        totals.CustId = _rvoucher.expenseTypeCategory == 10 ? actingID : (int?)null;
                        totals.VendorId = _rvoucher.expenseTypeCategory == 12 ? actingID : (int?)null;
                        totals.Debit = _rvoucher.paid;
                        totals.ActivityTimestamp = _rvoucher.activityDate;
                        totals.UserId = _rvoucher.empID;
                        totals.ModifiedBy = _rvoucher.empID;
                        totals.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public static MYJSONTblCustom LoadPaymentVoucherTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.End_Date = Param.End_Date.AddDays(1);
            var _pvoucherlist = LoadPaymentVoucher(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_PaymentVoucherTable> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _pvoucherlist
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.amount != 0 && c.amount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.hEADTYPE.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.name.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _pvoucherlist;
            }
            Func<BO_PaymentVoucherTable, dynamic> orderingFunction = null;
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
                             HeadType = c.hEADTYPE,
                             Amount = c.amount,
                             Name = c.name


                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _pvoucherlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_PaymentVoucherTable> LoadPaymentVoucher(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var lst = db_aa.GetPaymentVoucherList("", Param.Start_Date, Param.End_Date);
                List<BO_PaymentVoucherTable> lst_paymentVoucherTable = new List<BO_PaymentVoucherTable>();
                BO_PaymentVoucherTable obj;
                foreach (var item in lst.ToList())
                {
                    obj = new BO_PaymentVoucherTable();
                    obj.invoiceNo = item.InvoiceNo;
                    obj.activityDate = item.ActivityTimestamp;
                    obj.hEADTYPE = item.HEADTYPE;
                    obj.amount = item.Amount ?? 0;
                    obj.name = item.Acting;
                    lst_paymentVoucherTable.Add(obj);
                }

                if (Param.SearchType != 0)
                {

                }
                return lst_paymentVoucherTable.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
            }
        }

        public static string DeletePaymentVoucher(string invoiceNo, int empID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Acc_GL.Where(x => x.InvoiceNo == invoiceNo).ToList();
                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        item.IsActive = false;
                        item.ModifiedBy = empID;
                        item.ModifiedDate = DateTime.Now;
                    }
                }
                db.SaveChanges();
            }
            return "success";
        }

        public static BO_PaymentVoucher GetPaymentVoucherByInvoiceId(string _pvoucherInvoiceId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.GetPaymentVoucherByVoucherNo(_pvoucherInvoiceId).ToList();
                BO_PaymentVoucher _pvoucher = new BussinessObject.BO_PaymentVoucher();

                if (obj != null)
                {
                    _pvoucher.invoiceNo = obj[0].InvoiceNo;
                    _pvoucher.activityDate = obj[0].ActivityTimestamp;
                    _pvoucher.expenseTypeCategory = obj[0].HEADTYPEID;
                    _pvoucher.expensetype = obj[0].ActingId;
                    _pvoucher.paid = obj[0].Amount;
                    _pvoucher.comments = obj[0].Comments;


                }
                return _pvoucher;

            }
        }
    }
}