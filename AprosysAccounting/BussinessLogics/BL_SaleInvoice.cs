using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_SaleInvoice
    {
        List<Acc_GLTxLinks> GLTxLinksinsertedList = new List<Acc_GLTxLinks>();
        List<Acc_GL> updatedList = new List<Acc_GL>();

        public static string SaveSales(BO_SaleInvoice pi, System.Data.Entity.DbContextTransaction trans = null, AprosysAccountingEntities context = null)
        {
            
            
            //if (pi.items.GroupBy(x => x.itemID).Where(x => x.Count() > 1).Count() == 1) { return "bad request"; }
            //if transaction data is delted 


            AprosysAccountingEntities db = context == null ? new AprosysAccountingEntities() : context;

            #region if Customer , Items And Services Deleted
            var _cust = db.Customers.Where(x => x.Id == pi.customerID && x.IsActive == false).FirstOrDefault();
            if (_cust != null)
            {
                throw new Exception("Customer is not Exist ");
            }
          
            List<int> itemids = pi.items.Select(x => x.itemID).ToList();
            var _item = db.Items.Where(x => itemids.Contains(x.Id) && x.IsActive == false).FirstOrDefault();
            if (_item != null)
            {
                throw new Exception("Items not Exist ");
            }
            List<int> serviceids = pi.items.Select(x => x.coaID).ToList();
            var _service = db.Acc_COA.Where(x => serviceids.Contains(x.CoaId) && x.IsActive == false).FirstOrDefault();
            if (_service != null)
            {
                throw new Exception("Services not Exist ");
            }
            #endregion


            var transaction = trans == null ? db.Database.BeginTransaction() : trans;
            try
            {
                string InvoiceNumber;
                var editVal = trans != null ? db.Acc_GL.Where(x => x.IsActive == false && x.CoaId == 0).Select(x => new { createdBy = x.CreatedBy.Value, createdDate = x.CreatedDate.Value }).FirstOrDefault() : new { createdBy = pi.empID, createdDate = DateTime.Now };
                //Getting Purchased Stock for FIFO Base deduction
                var GLPurchases = db.Acc_GL.Where(x => (x.IsActive ?? false) && x.CoaId == 6 && x.QuantityBalance > 0 &&
                (x.TranTypeId == 1 || x.TranTypeId == 7)).OrderBy(x => x.ActivityTimestamp).AsEnumerable();
                int transType = (int)Constants.TransactionTypes.Sales;
                pi.invoiceNo = trans == null ? Util.GetNextVoucher(transType) : pi.invoiceNo;
                bool _isSalesCredit = pi.IsSalesCredit;
                //Parent Entry
                var GLParent = new Acc_GL() { CoaId = 0, UserId = pi.empID, Comments = pi.comments, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, Debit = pi.netAmount, Credit = pi.netAmount, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit , SalesPersonId =pi.salesPersonId};
                InvoiceNumber = GLParent.InvoiceNo;
                db.Acc_GL.Add(GLParent);
                db.SaveChanges();
                foreach (var item in pi.items)
                {
                    //In Case of Service Item
                    if (item.isServiceItem)
                    {
                        var GLServSales = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, TaxPercent = item.tax, Quantity = item.qty, CoaId = item.coaID, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, UnitPrice = item.unitPrice, Credit = item.amount - item.tax, IsPostpaid = true, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit , SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLServSales);
                        db.SaveChanges();
                        continue;
                    }
                    var qty = item.qty;
                    var purchaseAmount = new List<decimal>();
                    #region --- FIFO base deduction ---
                    List<Acc_GLTxLinks> txLinks = new List<Acc_GLTxLinks>();
                    var itemPurchases = GLPurchases.Where(x => x.ItemId == item.itemID).ToList();
                    foreach (var objpurchase in itemPurchases)
                    {
                        var qtytodeduct = 0m;
                        var bal = qty - objpurchase.QuantityBalance;
                        if (bal == 0 || bal < 0)
                        {
                            qtytodeduct = qty;
                        }
                        else
                        {
                            qtytodeduct = objpurchase.QuantityBalance ?? 0;
                        }
                        objpurchase.QuantityBalance -= qtytodeduct;
                        Acc_GLTxLinks objTxLinks = new Acc_GLTxLinks() { UnitPrice = item.unitPrice, Credit = item.amount, TranTypeID = transType, Quantity = qtytodeduct, IsActive = true, ItemID = item.itemID, RelGLID = objpurchase.GlId };
                        txLinks.Add(objTxLinks);
                        db.Acc_GLTxLinks.Add(objTxLinks);
                        qty = qty - qtytodeduct;
                        purchaseAmount.Add(objpurchase.UnitPrice ?? 0);
                        if (qty == 0) { break; }
                    }
                    decimal avgPurchaseUnitPrice = purchaseAmount.Average();
                    //Stock Entry
                    var GLStock = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 6, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = -item.qty, QuantityBalance = item.qty, UnitPrice = avgPurchaseUnitPrice, Credit = avgPurchaseUnitPrice * item.qty, TaxPercent = item.tax, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit ,SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLStock);
                    //COGS Entry
                    var GLCOGS = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 13, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, UnitPrice = avgPurchaseUnitPrice, Debit = avgPurchaseUnitPrice * item.qty, TaxPercent = item.tax, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLCOGS);
                    //Sales Entry
                    var GLSales = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 14, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, UnitPrice = item.unitPrice, Credit = item.amount - item.tax, TaxPercent = item.tax, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLSales);

                    db.SaveChanges();
                    txLinks.ForEach(x => x.GLID = GLStock.GlId);
                    #endregion
                }
                if (pi.IsSalesCredit)
                {
                    pi.paid = 0;
                }
                if (pi.paid > 0)
                {
                    //Paid Entry
                    var GLpaid = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 11, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Debit = pi.paid, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit , SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLpaid);
                }
                decimal balance = pi.netAmount - pi.paid;
                if (balance != 0)
                {
                    //Acc Receivable Entry
                    var GLpayable = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 10, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Debit = balance, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit , SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLpayable);
                }
                decimal tax = pi.items.Sum(x => x.tax ?? 0);
                if (tax > 0)
                {
                    //Tax Entry
                    var GLTax = new Acc_GL() { TranId = GLParent.GlId, UserId = pi.empID, CoaId = 99, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Credit = tax, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit , SalesPersonId = pi.salesPersonId };
                    db.Acc_GL.Add(GLTax);
                }
                db.SaveChanges();
                if (trans == null)
                {
                    transaction.Commit();
                    db.Dispose();
                    transaction.Dispose();
                }
                return InvoiceNumber;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public static string DeleteSaleInvoice(string voucherNo, int empID, System.Data.Entity.DbContextTransaction trans = null, AprosysAccountingEntities context = null)
        {
            AprosysAccountingEntities db = context == null ? new AprosysAccountingEntities() : context;
            var transaction = trans == null ? db.Database.BeginTransaction() : trans;
            try
            {
                var snv = db.Acc_GL.Where(x => x.InvoiceNo == voucherNo && x.IsActive == true
                &&
                x.TranTypeId==2
                ).ToList();
                foreach (var item in snv.Where(x => x.CoaId == 6))
                {
                    var txLinks = db.Acc_GLTxLinks.Where(x => x.GLID == item.GlId && x.IsActive == true).ToList();
                    //Revert Stock to purchases
                    foreach (var txlinks in txLinks)
                    {
                        var pnv = db.Acc_GL.SingleOrDefault(x => x.GlId == txlinks.RelGLID);
                        pnv.QuantityBalance += txlinks.Quantity;
                        txlinks.IsActive = false;
                    }
                }
                foreach (var item in snv)
                {
                    item.IsActive = false;
                    if (trans == null) { item.ModifiedBy = empID; item.ModifiedDate = DateTime.Now; }
                }
                BL_CreditSales.DeletePartialPayments(db, voucherNo,empID);

                db.SaveChanges();
                
                if (trans == null)
                {
                    transaction.Commit();
                    db.Dispose();
                    transaction.Dispose();
                }
                
                return "Success";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public static MYJSONTblCustom GetSalesList(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var salesList = GetSales(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Sales> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = salesList
                   .Where(
                      c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.customerName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.paid > 0 && c.paid.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.netAmount > 0 && c.netAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   );
            }
            else
            {
                filteredCategories = salesList;
            }
            Func<BO_Sales, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_Sales> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.invoiceNo : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.sellDate : DateTime.MinValue); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.customerName : ""); }
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
                             sellDate = c.sellDate,
                             customerName = c.customerName,
                             netAmount = c.netAmount,
                             paid = ( c.paid),
                             balance = c.balance,
                             isCustomerActive = c.isCustomerDeleted,
                             isSalesCredit = (c.netAmount-c.paid)!=0,
                             cashPaidDate = ( (c.isSalesCredit && c.paid==0) ? (DateTime?)null : (c.creditPaidDate ?? c.sellDate))
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = salesList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }

        public static List<BO_Sales> GetSales(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                //Param.Start_Date = BL_Common.GetDatetime().AddDays(-7);
                Param.End_Date = Param.End_Date.AddDays(1);
                var lst = db_aa.GetSaleInvoiceList(Param.Start_Date, Param.End_Date);
                List<BO_Sales> lst_Sales = new List<BO_Sales>();
                BO_Sales obj;
                var partial=BL_CreditSales.GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                foreach (var _sales in lst.ToList())
                {
                    obj = new BO_Sales();
                    obj.invoiceNo = _sales.InvoiceNo;
                    obj.sellDate = _sales.ActivityTimestamp.Value;
                    obj.customerName = _sales.cstName;
                    obj.netAmount = _sales.NETAMOUNT ?? 0;
                    obj.paid = _sales.PAID;
                    if (_sales.IsSalesCredit.GetValueOrDefault())
                    {
                        obj.paid = 0;
                        if (partial.ContainsKey(obj.invoiceNo))
                        {
                            obj.paid = partial[obj.invoiceNo].Sum(x => x.Amount);
                            obj.creditPaidDate = partial[obj.invoiceNo].Max(x => x.CreatedDate);
                        }
                    }
                    
                    obj.isCustomerDeleted = _sales.isCustomerActive;
                    obj.isSalesCredit = _sales.IsSalesCredit ?? false;
                    obj.creditPaidDate = _sales.CreditPaidDate;
                    lst_Sales.Add(obj);
                }
                return lst_Sales.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
            }
        }

        public static string UpdateSaleInvoice(BO_SaleInvoice pi)
        {
            
            //if (pi.items.GroupBy(x => x.itemID).Where(x => x.Count() > 1).Count() == 1) { return "bad request"; }
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (DeleteSaleInvoice(pi.invoiceNo, pi.empID, transaction, db) == "Success")
                        {
                            SaveSales(pi, transaction, db);
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        return pi.invoiceNo;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static List<Report_GetSaleInvoice_Result> GetSaleByInvoiceId(string _saleInvoiceId)
        {
            List<Report_GetSaleInvoice_Result> _si = new List<Report_GetSaleInvoice_Result>();
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                _si = db.Report_GetSaleInvoice(_saleInvoiceId).ToList();
            }
            //Partial Credit
            var PartialPayments = BL_CreditSales.GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
            foreach (var item in _si)
            {
                if (item.IsSalesCredit.GetValueOrDefault() > 0)
                {
                    item.PAID = 0;
                    if (PartialPayments.ContainsKey(item.InvoiceNo))
                    {
                        item.PAID = PartialPayments[item.InvoiceNo].Sum(x => x.Amount);
                    }
                }
            }
            return _si;
        }

        public static List<KeyValuePair<int, string>> GetSalesPersonList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.SalesPersons.Where(x => x.IsActive == true).Select(z => new { z.Id, z.FirstName, z.LastName }).ToDictionary(z => Convert.ToInt32(z.Id), q => String.Format("{0} {1}",q.FirstName, q.LastName)).OrderBy(x => x.Value).ToList();
            }
        }
    }
}