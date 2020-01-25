using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_StockIn
    {
        public static string SaveStockIn(BO_StockIn si)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transType = (int)Constants.TransactionTypes.StockIn;
                        string invNo = Util.GetNextVoucher(transType);
                        si.date = DateTime.Now;
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = si.empID, Comments = si.comments, ActivityTimestamp = si.date, TranTypeId = transType, Debit = si.netAmount, Credit = si.netAmount, InvoiceNo = invNo, IsActive = true, CreatedBy = si.empID, CreatedDate = DateTime.Now, ModifiedBy = si.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        foreach (var item in si.items)
                        {
                            //Stock Entry
                            var GLStock = new Acc_GL() { TranId = GLParent.GlId, UserId = si.empID, CoaId = 6, ActivityTimestamp = si.date, TranTypeId = transType, InvoiceNo = invNo, IsActive = true, ItemId = item.itemID, Quantity = item.qty, QuantityBalance = item.qty, UnitPrice = item.unitPrice, Debit = item.amount, CreatedBy = si.empID, CreatedDate = DateTime.Now, ModifiedBy = si.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLStock);
                        }
                        //Counter Entry
                        var GLpayable = new Acc_GL() { TranId = GLParent.GlId, UserId = si.empID, CoaId = si.coaID, ActivityTimestamp = si.date, TranTypeId = transType, InvoiceNo = invNo, IsActive = true, Credit = si.netAmount, CreatedBy = si.empID, CreatedDate = DateTime.Now, ModifiedBy = si.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLpayable);
                        db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}