using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_Banks
    {

        public static string InsertBankTransfer(BO_Banks obj)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transType = (int)Constants.TransactionTypes.BanksTransfer;
                        obj.invoiceNo = Util.GetNextVoucher(transType);
                        //obj.transDate = DateTime.Now;
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = obj.empID, Debit = Math.Abs(obj.amount), Credit = Math.Abs(obj.amount), ActivityTimestamp = obj.transDate, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.empID, CreatedDate = DateTime.Now, ModifiedBy = obj.empID, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        //Bank Entry
                        var GLBank = new Acc_GL() { TranId = GLParent.GlId, CoaId = obj.bankID, UserId = obj.empID, Debit = obj.amount > 0 ? obj.amount : (decimal?)null, Credit = obj.amount < 0 ? Math.Abs(obj.amount) : (decimal?)null, ActivityTimestamp = obj.transDate, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.empID, CreatedDate = DateTime.Now, ModifiedBy = obj.empID, ModifiedDate = DateTime.Now };
                        GLBank.Comments = obj.comment;
                        db.Acc_GL.Add(GLBank);

                        if (!string.IsNullOrEmpty(obj.documentId))
                        {
                            // Receivable Entry
                            var GLRecvable = new Acc_GL() { TranId = GLParent.GlId, CoaId = 11, UserId = obj.empID, Debit = obj.amount < 0 ? Math.Abs(obj.amount) : (decimal?)null, Credit = obj.amount > 0 ? obj.amount : (decimal?)null, ActivityTimestamp = obj.transDate, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.empID, CreatedDate = DateTime.Now, ModifiedBy = obj.empID, ModifiedDate = DateTime.Now ,DocumentId=obj.documentId};
                            db.Acc_GL.Add(GLRecvable);
                        }
                        else
                        {
                            //Cash Entry
                            var GLCash = new Acc_GL() { TranId = GLParent.GlId, CoaId = 11, UserId = obj.empID, Debit = obj.amount < 0 ? Math.Abs(obj.amount) : (decimal?)null, Credit = obj.amount > 0 ? obj.amount : (decimal?)null, ActivityTimestamp = obj.transDate, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.empID, CreatedDate = DateTime.Now, ModifiedBy = obj.empID, ModifiedDate = DateTime.Now };
                            db.Acc_GL.Add(GLCash);
                        }
                            db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public static List<BanksList> GetBanksList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    return db.GetBanksWithBalances().Select(x => new BanksList { bankID = x.BankID, bankName = x.BANKNAME, amount = x.AMOUNT }).OrderBy(x => x.bankName).ToList();
                }
                catch { throw; }
            }
        }

      
      
    }
}