using ApprosysAccDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_BanksConfiguration
    {
        public static string RenameBank(int bankID, string name)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var acc=  db.Acc_COA.Where(x => x.CoaId == bankID).FirstOrDefault();
                    acc.TreeName = name;
                    db.SaveChanges();
                    return "success";
                }
                catch(Exception ex) { throw ex; }
            }
        }

        //Return Names of ALL Banks 
        public static List<string> GetAllBankNames()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var acc = db.tblBanks.ToList();
                    List<string> lst_bankName = new List<string>();
                    lst_bankName = acc.Select(x=>x.Name.ToString()).ToList();
                    
                    return lst_bankName;
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public static string AddNewBankAccoount(string bankName, string bankAccount, decimal? depositAmmount)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var chk = db.Acc_COA.FirstOrDefault(x => x.TreeName == bankAccount);
                    if (chk != null)
                        return String.Format("Account No: {0} is already exist.",bankAccount);
                    var acc = new Acc_COA();
                    acc.PId = 104;
                    acc.CoaNo = "";
                    acc.HeadAccount = 1;
                    acc.TreeName = bankAccount;
                    acc.CoaLevel = 2;
                    acc.OpeningBalance = depositAmmount;
                    acc.IsActive = true;
                    db.Acc_COA.Add(acc);
                    db.SaveChanges();
                    return "success";
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public static string DeleteBankAccount(int CoaId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var res = db.Acc_GL.Where(x => x.CoaId == CoaId && x.IsActive == true).ToList() ;
                    if (res == null || res.Count > 0) { return "Transaction is Performed against Account, it can not be deleted "; }
                    
                    var chk = db.Acc_COA.FirstOrDefault(x => x.CoaId == CoaId);
                    if (chk != null)
                        chk.IsActive = false;
                    db.SaveChanges();
                    return "success";
                }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}