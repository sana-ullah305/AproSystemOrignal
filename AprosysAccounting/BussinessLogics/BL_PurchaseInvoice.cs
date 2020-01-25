using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_PurchaseInvoice
    {
        public static string SavePurchase(BO_PurchaseInvoice pi)
        {
            if (pi.items.GroupBy(x => x.itemID).Where(x => x.Count() > 1).Count() == 1) { return "bad request"; }
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                       int transType = (int)Constants.TransactionTypes.Purchase;
                        pi.invoiceNo = Util.GetNextVoucher(transType);
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = pi.empID, Comments = pi.comments, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, Debit = pi.netAmount, Credit = pi.netAmount, InvoiceNo = pi.invoiceNo, IsActive = true, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        foreach (var item in pi.items)
                        {
                            //Stock Entry
                            var GLStock = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 6, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, QuantityBalance = item.qty, UnitPrice = item.unitPrice, Debit = item.amount, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLStock);
                        }
                        if (pi.paid > 0)
                        {
                            //Paid Entry
                            var GLpaid = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 11, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Credit = pi.paid, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLpaid);
                        }
                        decimal balance = pi.netAmount - pi.paid;
                        if (balance != 0)
                        {
                            //Acc Payable Entry
                            var GLpayable = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 12, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Credit = balance, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLpayable);
                        }
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

        public static MYJSONTblCustom GetPurchasesList(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var purchasesList = GetPurchases(Param.Start_Date, Param.End_Date);//it shoult take startDate, Enddate,VendorId
            IEnumerable<Purchases> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = purchasesList
                   .Where(
                      c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.vendorName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.paid > 0 && c.paid.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.netAmount > 0 && c.netAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   );
            }
            else
            {
                filteredCategories = purchasesList;
            }
            Func<Purchases, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<Purchases> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.invoiceNo : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.purchaseDate : DateTime.MinValue); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.vendorName : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.netAmount : 0); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.paid : 0); }
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

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             invoiceNo = c.invoiceNo,
                             purchaseDate = c.purchaseDate,
                             vendorID = c.vendorID,
                             vendorName = c.vendorName,
                             netAmount = c.netAmount,
                             paid = c.paid,
                             balance = c.balance,
                             isDeletable = c.isDeletable ?? false
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = purchasesList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }

        public static List<Purchases> GetPurchases(DateTime dtStart, DateTime dtEnd)
        {
            using (ApprosysAccDB.AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                dtEnd = dtEnd.AddDays(1);
                int transType = (int)Constants.TransactionTypes.Purchase;
                var accGlList = db.Acc_GL.Where(x => x.TranTypeId == transType && x.IsActive == true).ToList();
                accGlList = accGlList.Where(x => (x.ActivityTimestamp >= dtStart.Date && x.ActivityTimestamp <= dtEnd.Date)).ToList();
                var vendorsList = db.Vendors.Where(x => x.IsActive == true).ToList();
                return accGlList.Where(x => x.TranId.HasValue).GroupBy(x => x.TranId).
                    Select(x => new Purchases
                    {
                        invoiceNo = x.FirstOrDefault().InvoiceNo,
                        purchaseDate = x.FirstOrDefault().ActivityTimestamp.Value,
                        vendorID = x.FirstOrDefault().VendorId.Value,
                        vendorName = vendorsList.Where(zx => zx.ID == x.FirstOrDefault().VendorId).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault(),
                        netAmount = x.Where(qz => qz.CoaId == 6).Sum(qz => qz.Debit ?? 0),
                        paid = x.Where(qz => qz.CoaId == 11).Sum(qz => qz.Credit ?? 0),
                        isDeletable = x.Count(zx => zx.Quantity != zx.QuantityBalance) == 0
                    }).OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
            }
        }

        public static string EditPurchases(BO_PurchaseInvoice pi)
        {
            if (pi.items.GroupBy(x => x.itemID).Where(x => x.Count() > 1).Count() == 1) { return "bad request"; }
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transType = (int)Constants.TransactionTypes.Purchase;
                        var glPurchaseOld = db.Acc_GL.Where(x => x.InvoiceNo == pi.invoiceNo && x.IsActive == true).ToList();
                        glPurchaseOld.Where(x => x.CoaId != 0).ToList().ForEach(x => x.IsActive = false);
                        int transID = glPurchaseOld.Where(x => x.CoaId != 0).Select(x => x.TranId.Value).FirstOrDefault();
                        var glParent = glPurchaseOld.Find(x => x.CoaId == 0);
                        glParent.Credit = glParent.Debit = pi.netAmount;
                        glParent.VendorId = pi.vendorID;
                        glParent.ActivityTimestamp = pi.purchaseDate;
                        glParent.UserId = pi.empID;
                        glParent.Comments = pi.comments;
                        glParent.ModifiedDate = DateTime.Now;
                        glParent.ModifiedBy = pi.empID;

                        foreach (var item in pi.items)
                        {
                            var currItem = glPurchaseOld.Find(x => x.ItemId == item.itemID && x.CoaId == 6);
                            if (currItem == null)
                            {
                                //New Stock Entry
                                var GLStock = new Acc_GL() { TranId = transID, UserId = pi.empID, CoaId = 6, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, QuantityBalance = item.qty, UnitPrice = item.unitPrice, Debit = item.amount, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now };
                                db.Acc_GL.Add(GLStock);
                            }
                            else
                            {
                                if (currItem.Quantity == currItem.QuantityBalance) { currItem.UnitPrice = item.unitPrice; }
                                if (item.qty > currItem.Quantity)
                                {
                                    currItem.QuantityBalance = item.qty - (currItem.Quantity - currItem.QuantityBalance);
                                }
                                currItem.Debit = item.amount;
                                currItem.Quantity = item.qty;
                                currItem.UserId = pi.empID;
                                currItem.ActivityTimestamp = pi.purchaseDate;
                                currItem.IsActive = true;
                                currItem.VendorId = pi.vendorID;
                                currItem.ModifiedDate = DateTime.Now;
                                currItem.ModifiedBy = pi.empID;
                            }
                        }
                        db.SaveChanges();
                        if (pi.paid > 0)
                        {
                            //Paid Entry
                            var objPaid = glPurchaseOld.Find(x => x.CoaId == 11);
                            if (objPaid == null) { db.Acc_GL.Add(new Acc_GL() { TranId = transID, CoaId = 11, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, Credit = pi.paid, InvoiceNo = pi.invoiceNo, IsActive = true, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now }); }
                            else
                            {
                                objPaid.Credit = pi.paid;
                                objPaid.VendorId = pi.vendorID;
                                objPaid.ActivityTimestamp = pi.purchaseDate;
                                objPaid.UserId = pi.empID;
                                objPaid.IsActive = true;
                                objPaid.ModifiedDate = DateTime.Now;
                                objPaid.ModifiedBy = pi.empID;

                            }
                            db.SaveChanges();
                        }
                        decimal balance = pi.netAmount - pi.paid;
                        if (balance != 0)
                        {
                            //Acc Payable Entry
                            var objPayable = glPurchaseOld.Find(x => x.CoaId == 12);
                            if (objPayable == null) { db.Acc_GL.Add(new Acc_GL() { TranId = transID, CoaId = 12, VendorId = pi.vendorID, ActivityTimestamp = pi.purchaseDate, TranTypeId = transType, Credit = balance, InvoiceNo = pi.invoiceNo, IsActive = true, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now }); }
                            else
                            {
                                objPayable.Credit = balance;
                                objPayable.VendorId = pi.vendorID;
                                objPayable.ActivityTimestamp = pi.purchaseDate;
                                objPayable.UserId = pi.empID;
                                objPayable.IsActive = true;
                                objPayable.ModifiedDate = DateTime.Now;
                                objPayable.ModifiedBy = pi.empID;
                            }
                            db.SaveChanges();
                        }
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

        public static string DeletePurchaseInvoice(string invoiceNo, int empID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.DeletePurchase(invoiceNo, empID);
            }
            return "success";
        }

        public static List<Report_GetPurchaseInvoice_Result> GetPurchaseByInvoiceId(string _purchaseInvoiceId)
        {
            List<Report_GetPurchaseInvoice_Result> _si = new List<Report_GetPurchaseInvoice_Result>();
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                _si = db.Report_GetPurchaseInvoice(_purchaseInvoiceId).ToList();
            }
            return _si;
        }
    }
}