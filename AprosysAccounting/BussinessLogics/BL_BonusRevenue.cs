using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static AprosysAccounting.BussinessLogics.Constants;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_BonusRevenue
    {
        public static List<KeyValuePair<int, string>> GetRevenueAccountList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.Acc_COA.Where(x => x.PId == 25 && x.IsActive == true).Select(z => new { z.CoaId, z.TreeName }).ToDictionary(z => Convert.ToInt32(z.CoaId), q => String.Format("{0}", q.TreeName)).OrderBy(x => x.Value).ToList();
            }
        }
        public static List<KeyValuePair<int, string>> GetBankList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.Acc_COA.Where(x => x.PId == 104 && x.IsActive == true).Select(z => new { z.CoaId, z.TreeName }).ToDictionary(z => Convert.ToInt32(z.CoaId), q => String.Format("{0}", q.TreeName)).OrderBy(x => x.Value).ToList();
            }
        }


        public static MYJSONTblCustom GetBonusRevenueList(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var BonusRevenueList = GetBonusRevenue(Param);
            IEnumerable<BO_BonusRevenue> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
               filteredCategories = BonusRevenueList
                   .Where(
                      c => c.activityDate.Value.ToString("yyyy-MM-dd").ToLower().Contains(Param.sSearch.ToLower())
                    || c.RevenueAccount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.BankAccount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.BonusAmount != null && c.BonusAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   );
            }
            else
            {
                filteredCategories = BonusRevenueList;
            }
            Func<BO_BonusRevenue, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                IOrderedEnumerable<BO_BonusRevenue> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.activityDate : DateTime.MinValue); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.RevenueAccount : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.BankAccount : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.BonusAmount : 0m); }
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
                             TransactionId = c.TransactionId,
                             activityDate = c.activityDate ?? DateTime.MinValue,
                             RevenueAccount = c.RevenueAccount,
                             BankAccount = c.BankAccount,
                             BonusAmount = c.BonusAmount
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = BonusRevenueList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }

        public static List<BO_BonusRevenue> GetBonusRevenue(JQueryDataTableParamModel Param)
        {
            
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                Param.End_Date = Param.End_Date.AddDays(1);
                List<BO_BonusRevenue> lst_bonus = new List<BO_BonusRevenue>();
                //var temp = new BO_BonusRevenue();
                var lst_bonusList = (from glRevenue in db_aa.Acc_GL
                                     join glAccount in db_aa.Acc_GL on glRevenue.TranId equals glAccount.TranId
                                     join caRevenue in db_aa.Acc_COA on glRevenue.CoaId equals caRevenue.CoaId
                                     join caAccount in db_aa.Acc_COA on glAccount.CoaId equals caAccount.CoaId
                                     where caRevenue.PId == 25 && glRevenue.IsActive == true && glAccount.IsActive == true
                                     && glRevenue.ActivityTimestamp >= Param.Start_Date && glAccount.ActivityTimestamp <= Param.End_Date
                                     select new BO_BonusRevenue
                                     {
                                         TransactionId = glRevenue.TranId,
                                         BonusAmount = glRevenue.Credit,
                                         Misc = glRevenue.Comments,
                                         activityDate = glRevenue.CreatedDate,
                                         BankAccount = caAccount.TreeName,
                                         RevenueAccount = caRevenue.TreeName,
                                     }).ToList();
                //foreach (var item in lst_bonusList)
                //{
                //    if (temp.TransactionId != item.TransactionId)
                //    {
                //        lst_bonus.Add(item);
                //        temp = item;
                //    }
                //}
                lst_bonus = lst_bonusList.DistinctBy(x => x.TransactionId).ToList();
                return lst_bonus.OrderByDescending(x => x.activityDate).ToList();
            }
        }


        public static string SaveBonus(BO_BonusRevenue br, int empID, System.Data.Entity.DbContextTransaction trans = null, AprosysAccountingEntities context = null)
        {
            AprosysAccountingEntities db = context == null ? new AprosysAccountingEntities() : context;
            var transaction = trans == null ? db.Database.BeginTransaction() : trans;
            try
            {

                var editVal = trans != null ? db.Acc_GL.Where(x => x.IsActive == false && x.CoaId == 0).Select(x => new { createdBy = x.CreatedBy.Value, createdDate = x.CreatedDate.Value }).FirstOrDefault() : new { createdBy = empID, createdDate = DateTime.Now };
                if (br.TransactionId != null)
                {
                    var update = db.Acc_GL.Where(x =>

                    (x.TranId == br.TransactionId || x.GlId == br.TransactionId) && x.IsActive == true).ToList();
                    foreach (var rows in update)
                    {
                        rows.IsActive = false;
                    }
                    db.SaveChanges();
                }
                var GLParent = new Acc_GL() { CoaId = 5, UserId = empID, Comments = br.Misc, ActivityTimestamp = br.activityDate, Debit = br.BonusAmount, Credit = br.BonusAmount, TranTypeId = (int)TransactionTypes.Bonus, IsActive = true, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = empID, ModifiedDate = DateTime.Now };
                db.Acc_GL.Add(GLParent);
                db.SaveChanges();
                var GLDebitEntry = new Acc_GL() { TranId = GLParent.GlId, Comments = br.Misc, UserId = empID, CoaId = br.BankAccount != null ? Convert.ToInt32(br.BankAccount) : (int?)null, ActivityTimestamp = br.activityDate, TranTypeId = (int)TransactionTypes.Bonus, IsActive = true, Debit = br.BonusAmount, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = empID, ModifiedDate = DateTime.Now };
                db.Acc_GL.Add(GLDebitEntry);
                var GLCreditEntry = new Acc_GL() { TranId = GLParent.GlId, Comments = br.Misc, UserId = empID, CoaId = br.RevenueAccount != null ? Convert.ToInt32(br.RevenueAccount) : (int?)null, ActivityTimestamp = br.activityDate, TranTypeId = (int)TransactionTypes.Bonus, IsActive = true, Credit = br.BonusAmount, CreatedBy = editVal.createdBy, CreatedDate = editVal.createdDate, ModifiedBy = empID, ModifiedDate = DateTime.Now };
                db.Acc_GL.Add(GLCreditEntry);
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

        public static BO_BonusRevenue GetBonusRecordById(int transId)
        {
            BO_BonusRevenue obj_br = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var objBonus = (from glRevenue in db_aa.Acc_GL
                                join glAccount in db_aa.Acc_GL on glRevenue.TranId equals glAccount.TranId
                                join caRevenue in db_aa.Acc_COA on glRevenue.CoaId equals caRevenue.CoaId
                                join caAccount in db_aa.Acc_COA on glAccount.CoaId equals caAccount.CoaId
                                where
                                caRevenue.PId == 25 && glRevenue.IsActive == true && glAccount.IsActive == true
                                //&& glRevenue.TranId == transId

                                select new BO_BonusRevenue
                                {
                                    activityDate = glRevenue.CreatedDate,
                                    BankAccount = caAccount.CoaId.ToString(),
                                    RevenueAccount = caRevenue.CoaId.ToString(),
                                    BonusAmount = glRevenue.Credit,
                                    TransactionId = glRevenue.TranId,
                                    Misc = glRevenue.Comments ?? ""
                                }).ToList();
                obj_br = objBonus.FirstOrDefault(x => x.TransactionId == transId);
            }
            return obj_br;
        }


        public static string DeleteBonus(int transId)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var update = db_aa.Acc_GL.Where(x =>
                (x.TranId == transId || x.GlId == transId) && x.IsActive == true).ToList();
                foreach (var rows in update)
                {
                    rows.IsActive = false;
                }
                db_aa.SaveChanges();
                return "Success";
            }
        }
    }
}