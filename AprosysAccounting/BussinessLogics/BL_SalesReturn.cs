using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_SalesReturn : BL_SaleInvoice
    {
        public static List<KeyValuePair<string, string>> GetSaleInvoicesList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                DateTime Start_Date = new DateTime(2000, 01, 01);
                DateTime End_Date = DateTime.Now.AddDays(1);
                return db.GetSaleInvoiceList(Start_Date, End_Date).Select(z => new { z.InvoiceNo, z.cstName }).ToDictionary(z => z.InvoiceNo, q => String.Format("{0}, {1}", q.InvoiceNo, q.cstName)).OrderBy(x => x.Value).ToList();
            }
        }
        public static List<Report_GetSaleInvoice_Result> GetOrderDetailsByInvoiceNo(string invoiceNo)
        {
            List<Report_GetSaleInvoice_Result> _si = new List<Report_GetSaleInvoice_Result>();
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                _si = db.Report_GetSaleInvoice(invoiceNo).ToList();
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

        public static string SaveSalesReturn(BO_SalesReturn salesReturn, int empId)
        {
            string InvoiceNo;
            BO_LineItems lineItem = null;
            BO_SaleInvoice obj = null;
            var orderDetails = GetOrderDetailsByInvoiceNo(salesReturn.InvoiceNo).FirstOrDefault(x => x.ItemCode == salesReturn.ItemCode);
            int? itemId = orderDetails.ItemId;
            var quantity = salesReturn.Quantity;
            var unitPrice = orderDetails.UnitPrice * (-1);
            var amount = orderDetails.UnitPrice * (-1 * quantity);
            var tex = orderDetails.TAX;
            obj = new BO_SaleInvoice();
            obj.items = new List<BO_LineItems>();
            obj.comments = salesReturn.Comments;
            obj.empID = empId;
            obj.invoiceNo = orderDetails.InvoiceNo;
            lineItem = new BO_LineItems()
            {
                amount = amount,
                unitPrice = unitPrice,
                qty = quantity,
                itemID = itemId ?? 0,
                isServiceItem = false,
                glid = 0,
                coaID = 0,
                tax = tex,

            };
            obj.items.Add(lineItem);
            obj.empID = empId;
            obj.saleDate = DateTime.Now;
            obj.netAmount = amount ?? 0m;
            obj.paid = amount ?? 0;
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                Acc_GL GLPurchases = db.Acc_GL.FirstOrDefault(x => (x.IsActive ?? false) && x.CoaId == 6 && x.ItemId == itemId.Value &&
                    (x.TranTypeId == 1 || x.TranTypeId == 7));
                GLPurchases.QuantityBalance = GLPurchases.QuantityBalance + quantity;

                var itmTransaction = db.Acc_GL.Where(x => (x.ItemId == itemId) && (x.InvoiceNo == salesReturn.InvoiceNo));
                foreach (var item in itmTransaction)
                {
                    //Comment for sales return goes here.
                    if (item.InvoiceNo == salesReturn.InvoiceNo && item.ItemId == itemId) { }
                    //item.Quantity = item.Quantity - quantity;

                }
                db.SaveChanges();
            }
            InvoiceNo = SaveSalesReturn(obj);
            return InvoiceNo;
        }

        public static string SaveSalesReturn(BO_SaleInvoice pi)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
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

                try
                {
                    //Getting Purchased Stock for FIFO Base deduction
                    var GLPurchases = db.Acc_GL.Where(x => (x.IsActive ?? false) && x.CoaId == 6 && x.QuantityBalance > 0 &&
                    (x.TranTypeId == 1 || x.TranTypeId == 7)).OrderBy(x => x.ActivityTimestamp).AsEnumerable();
                    int transType = (int)Constants.TransactionTypes.Sales;

                    bool _isSalesCredit = pi.IsSalesCredit;
                    var GLParentGlId = db.Acc_GL.FirstOrDefault(x => x.InvoiceNo.Equals(pi.invoiceNo) && (x.Debit.Value > 0 && x.Credit > 0)).GlId;
                    foreach (var item in pi.items)
                    {
                        //In Case of Service Item
                        if (item.isServiceItem)
                        {
                            var GLServSales = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, TaxPercent = item.tax, Quantity = item.qty, CoaId = item.coaID, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, UnitPrice = item.unitPrice, Credit = item.amount - item.tax, IsPostpaid = true, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
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
                        var GLStock = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 6, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = -item.qty, QuantityBalance = item.qty, UnitPrice = avgPurchaseUnitPrice, Credit = avgPurchaseUnitPrice * item.qty, TaxPercent = item.tax, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLStock);
                        //COGS Entry  -- COGS = Cost of goods sold
                        var GLCOGS = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 13, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, UnitPrice = avgPurchaseUnitPrice, Debit = avgPurchaseUnitPrice * item.qty, TaxPercent = item.tax, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLCOGS);
                        //Sales Entry
                        var GLSales = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 14, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, UnitPrice = item.unitPrice, Credit = item.amount - item.tax, TaxPercent = item.tax, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
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
                        var GLpaid = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 11, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Debit = pi.paid, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLpaid);
                    }
                    decimal balance = pi.netAmount - pi.paid;
                    if (balance != 0)
                    {
                        //Acc Receivable Entry
                        var GLpayable = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 10, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Debit = balance, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLpayable);
                    }
                    decimal tax = pi.items.Sum(x => x.tax ?? 0);
                    if (tax > 0)
                    {
                        //Tax Entry
                        var GLTax = new Acc_GL() { TranId = GLParentGlId, UserId = pi.empID, CoaId = 99, CustId = pi.customerID, ActivityTimestamp = pi.saleDate, TranTypeId = transType, InvoiceNo = pi.invoiceNo, IsActive = true, Credit = tax, CreatedBy = pi.empID, CreatedDate = DateTime.Now, ModifiedBy = pi.empID, ModifiedDate = DateTime.Now, IsSalesCredit = _isSalesCredit, SalesPersonId = pi.salesPersonId };
                        db.Acc_GL.Add(GLTax);
                    }
                    db.SaveChanges();
                    return pi.invoiceNo;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}