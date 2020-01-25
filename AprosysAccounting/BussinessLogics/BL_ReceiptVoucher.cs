using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_ReceiptVoucher
    {
        //public static string SaveSuspendedDuesofCustomer(int CustID,int EmpId)
        //{
        //  decimal pendingdues=  BL_Customer.GetCustomerBalance(CustID)??0;
        //    if (pendingdues > 0)
        //    {
        //        BO_ReceiptVoucher _rvoucher = new BO_ReceiptVoucher();
        //        _rvoucher.rCustomerId = CustID;
        //        _rvoucher.rRecived = pendingdues;
        //        _rvoucher.empID = EmpId;
        //        _rvoucher.rComments = "Clear Pending Dues of Subscription Customer";
        //        _rvoucher.rActivityDate = BL_Common.GetDatetime();
        //        _rvoucher.TypeId = 2;
        //        _rvoucher.rbalance = pendingdues;//its not enter in db , sets it for Acc REC entry

        //        SaveReceiptVoucher(_rvoucher);
        //    }
        //    return "success";
        //}
        public static string SaveReceiptVoucher(BO_ReceiptVoucher _rvoucher)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        int transType = (int)Constants.TransactionTypes.ReceiptVoucher;
                        _rvoucher.invoiceNo = Util.GetNextVoucher(transType);
                        bool isPostPaid = _rvoucher.TypeId == 2 ? true : false;
                        //Parent Entry

                        var GLParent = new Acc_GL() { CoaId = 0, UserId = _rvoucher.empID, Comments = _rvoucher.rComments, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = transType, Debit = _rvoucher.rRecived, Credit = _rvoucher.rRecived, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                        string InvoiceID = GLParent.InvoiceNo;
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        //Acc Receiveable for Type=1 , Subscrtiption Receiveable=2
                        if (_rvoucher.TypeId == 1 || (_rvoucher.TypeId == 2 && _rvoucher.rbalance != 0))
                        {
                            //Acc Receivable
                            var GLpaid = new Acc_GL() { TranId = GLParent.GlId, UserId = _rvoucher.empID, CoaId = 10, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, Credit = _rvoucher.rRecived, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLpaid);
                        }
                        else
                        {
                            //Subscription Sales
                            var GLSalesSub = new Acc_GL() { TranId = GLParent.GlId, UserId = _rvoucher.empID, CoaId = 100, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, Credit = _rvoucher.rRecived, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLSalesSub);
                        }
                        //Cash Entry
                        var GLCash = new Acc_GL() { TranId = GLParent.GlId, UserId = _rvoucher.empID, CoaId = 11, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = transType, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, Debit = _rvoucher.rRecived, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLCash);
                        db.SaveChanges();
                        transaction.Commit();
                        return InvoiceID;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static string EditReceiptVoucher(BO_ReceiptVoucher _rvoucher)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool isPostPaid = _rvoucher.TypeId == 2 ? true : false;
                        var rctL = db.Acc_GL.Where(x => x.InvoiceNo == _rvoucher.invoiceNo && x.IsActive == true).ToList();
                        rctL.ForEach(x => x.IsActive = false);
                        var parent = rctL.Find(x => x.CoaId == 0);
                        parent.Comments = _rvoucher.rComments;
                        parent.CustId = _rvoucher.rCustomerId;
                        parent.ActivityTimestamp = _rvoucher.rActivityDate;
                        parent.Debit = _rvoucher.rRecived;
                        parent.Credit = _rvoucher.rRecived;
                        parent.InvoiceNo = _rvoucher.invoiceNo;
                        parent.UserId = parent.ModifiedBy = _rvoucher.empID;
                        parent.ModifiedDate = DateTime.Now;
                        parent.IsActive = true;
                        db.SaveChanges();
                        if (_rvoucher.TypeId == 1 || (_rvoucher.TypeId == 2 && _rvoucher.rbalance != 0))
                        {
                            var paid = rctL.Find(x => x.CoaId == 10);
                            if (paid != null)
                            {
                                paid.CustId = _rvoucher.rCustomerId;
                                paid.ActivityTimestamp = _rvoucher.rActivityDate;
                                paid.Credit = _rvoucher.rRecived;
                                paid.UserId = paid.ModifiedBy = _rvoucher.empID;
                                paid.ModifiedDate = DateTime.Now;
                                paid.IsActive = true;
                            }
                            else
                            {
                                var GLpaid = new Acc_GL() { TranId = parent.GlId, UserId = _rvoucher.empID, CoaId = 10, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = parent.TranTypeId, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, Credit = _rvoucher.rRecived, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                                db.Acc_GL.Add(GLpaid);
                            }
                        }
                        else
                        {
                            var paid = rctL.Find(x => x.CoaId == 100);
                            if (paid != null)
                            {
                                paid.CustId = _rvoucher.rCustomerId;
                                paid.ActivityTimestamp = _rvoucher.rActivityDate;
                                paid.Credit = _rvoucher.rRecived;
                                paid.UserId = paid.ModifiedBy = _rvoucher.empID;
                                paid.IsActive = true;
                                paid.ModifiedDate = DateTime.Now;
                            }
                            else
                            {
                                var GLSalesSub = new Acc_GL() { TranId = parent.GlId, UserId = _rvoucher.empID, CoaId = 100, CustId = _rvoucher.rCustomerId, ActivityTimestamp = _rvoucher.rActivityDate, TranTypeId = parent.TranTypeId, InvoiceNo = _rvoucher.invoiceNo, IsActive = true, Credit = _rvoucher.rRecived, IsPostpaid = isPostPaid, CreatedBy = _rvoucher.empID, ModifiedBy = _rvoucher.empID, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                                db.Acc_GL.Add(GLSalesSub);
                            }
                        }
                        var cash = rctL.Find(x => x.CoaId == 11);
                        cash.CustId = _rvoucher.rCustomerId;
                        cash.ActivityTimestamp = _rvoucher.rActivityDate;
                        cash.IsActive = true;
                        cash.Debit = _rvoucher.rRecived;
                        cash.ModifiedDate = DateTime.Now;
                        cash.UserId = cash.ModifiedBy = _rvoucher.empID;
                        db.SaveChanges();
                        transaction.Commit();
                        return _rvoucher.invoiceNo;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static MYJSONTblCustom LoadReceiptVoucherTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _rvoucherlist = LoadReceiptVoucher(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_ReceiptVoucherTable> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _rvoucherlist
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.amount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.customerName.ToString().ToLower().Contains(Param.sSearch.ToLower())


                    );
            }
            else
            {
                filteredCategories = _rvoucherlist;
            }
            Func<BO_ReceiptVoucherTable, dynamic> orderingFunction = null;
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
                             Amount = c.amount,



                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _rvoucherlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_ReceiptVoucherTable> LoadReceiptVoucher(JQueryDataTableParamModel Param)
        {
            List<BO_ReceiptVoucherTable> lst_receiptVoucherTable = new List<BO_ReceiptVoucherTable>();
            //Param.Start_Date = Param.Start_Date.AddHours(-12);
            Param.End_Date = Param.End_Date.AddDays(1);
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                BO_ReceiptVoucherTable obj;
                if (Param.Searchcheckbox)
                {
                    var lst = db_aa.GetSubscriptionVoucherList("", Param.Start_Date, Param.End_Date);
                    foreach (var item in lst.ToList())
                    {
                        obj = new BO_ReceiptVoucherTable();
                        obj.invoiceNo = item.InvoiceNo;
                        obj.activityDate = item.ActivityTimestamp;
                        obj.customerName = item.Customer;
                        obj.amount = item.Amount;

                        lst_receiptVoucherTable.Add(obj);
                    }
                }
                else
                {

                    var lst = db_aa.GetReceiptVoucherList("", 1, Param.Start_Date, Param.End_Date);
                    foreach (var item in lst.ToList())
                    {
                        obj = new BO_ReceiptVoucherTable();
                        obj.invoiceNo = item.InvoiceNo;
                        obj.activityDate = item.ActivityTimestamp;
                        obj.customerName = item.Customer;
                        obj.amount = item.Amount;

                        lst_receiptVoucherTable.Add(obj);
                    }
                }




            }
            if (Param.SearchType != 0)
            {

            }

            // List<BO_ReceiptVoucherTable> ListtoReturn = new List<BO_ReceiptVoucherTable>();
            // ListtoReturn = lst_receiptVoucherTable;
            return lst_receiptVoucherTable.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
        }

        public static string DeleteReceiptVoucher(string invoiceNo, int empID)
        {
            using (ApprosysAccDB.AprosysAccountingEntities db = new AprosysAccountingEntities())
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

        public static BO_ReceiptVoucher GetReceiptVoucherByInvoiceId(int custType, string _rvoucherInvoiceId)
        {
            BO_ReceiptVoucher _rvoucher = new BussinessObject.BO_ReceiptVoucher();
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                if (custType == 1)
                {
                    var obj = db.GetReceiptVoucherListByVoucherNO(_rvoucherInvoiceId, 0).ToList();
                    if (obj != null)
                    {
                        #region voucher object
                        _rvoucher.invoiceNo = obj[0].InvoiceNo;
                        _rvoucher.rActivityDate = obj[0].ActivityTimestamp ?? BL_Common.GetDatetime();
                        _rvoucher.TypeId = 1;
                        _rvoucher.rCustomerId = obj[0].CustId;
                        _rvoucher.rRecived = obj[0].Amount ?? 0;
                        _rvoucher.rComments = obj[0].Comments ?? "";
                        _rvoucher.rGrossAmount = 1;
                        _rvoucher.rbalance = BL_Customer.GetCustomerBalance(obj[0].CustId) ?? 0;

                        #endregion
                    }
                }
                if (custType == 2)
                {
                    var obj = db.GetSubscriptionByVoucherNo(_rvoucherInvoiceId).ToList();
                    {
                        if (obj != null)
                        {
                            #region voucher object
                            _rvoucher.invoiceNo = obj[0].InvoiceNo;
                            _rvoucher.rActivityDate = obj[0].ActivityTimestamp ?? BL_Common.GetDatetime();
                            _rvoucher.TypeId = 2;
                            _rvoucher.rCustomerId = obj[0].CustId;
                            _rvoucher.rRecived = obj[0].Amount ?? 0;
                            _rvoucher.rComments = obj[0].Comments ?? "";
                            _rvoucher.rGrossAmount = 1;
                            _rvoucher.rbalance = BL_Customer.GetCustomerBalance(obj[0].CustId) ?? 0;
                            _rvoucher.DueDate = BL_Customer.GetSubCustomerList(2).Where(x => x.Id == obj[0].CustId).FirstOrDefault().DueDate;
                            _rvoucher.rSubscriptionAmount = obj[0].SubscriptionAmount;
                            #endregion
                        }
                    }
                }
                return _rvoucher;

            }

        }
    }
}