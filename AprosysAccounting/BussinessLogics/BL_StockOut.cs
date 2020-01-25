using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_StockOut
    {

        public static string SaveStockOut(BO_StockIn so)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transType = (int)Constants.TransactionTypes.StockOut;
                        string invNo = Util.GetNextVoucher(transType);
                        so.date = DateTime.Now;
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = so.empID, Comments = so.comments, ActivityTimestamp = so.date, TranTypeId = transType, Debit = so.netAmount, Credit = so.netAmount, InvoiceNo = invNo, IsActive = true, CreatedBy = so.empID, CreatedDate = DateTime.Now, ModifiedBy = so.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        foreach (var item in so.items)
                        {
                            //Stock Entry
                            var GLStock = new Acc_GL() { TranId = GLParent.GlId, UserId = so.empID, CoaId = 6, ActivityTimestamp = so.date, TranTypeId = transType, InvoiceNo = invNo, IsActive = true, ItemId = item.itemID, Quantity = -item.qty, QuantityBalance = item.qty, UnitPrice = item.unitPrice, Credit = item.amount, CreatedBy = so.empID, CreatedDate = DateTime.Now, ModifiedBy = so.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLStock);
                        }
                        //Counter Entry
                        var GLpayable = new Acc_GL() { TranId = GLParent.GlId, UserId = so.empID, CoaId = so.coaID, ActivityTimestamp = so.date, TranTypeId = transType, InvoiceNo = invNo, IsActive = true, Debit = so.netAmount, CreatedBy = so.empID, CreatedDate = DateTime.Now, ModifiedBy = so.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLpayable);
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
    }
}