using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApprosysAccDB;
using AprosysAccounting.BussinessObject;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Equity
    {
        public static string InsertEquityInfo(BO_Equity obj)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transType = (int)Constants.TransactionTypes.Equity;
                        obj.invoiceNo = Util.GetNextVoucher(transType);
                        //obj.transDate = DateTime.Now;
                        //Parent Entry
                        var GLParent = new Acc_GL() { CoaId = 0, UserId = obj.userId, Debit = Math.Abs(obj.amount), Credit = Math.Abs(obj.amount), ActivityTimestamp = obj.activityTime, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.userId, CreatedDate = DateTime.Now, ModifiedBy = obj.userId, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLParent);
                        db.SaveChanges();
                        //Bank Entry
                        if (obj.accountId > 0)
                        {
                            var GLBank = new Acc_GL() { TranId = GLParent.GlId, CoaId = obj.accountId, UserId = obj.userId, Debit = (obj.isdeposit == true ? obj.amount : 0), Credit = (obj.isdeposit == false ? obj.amount : 0), ActivityTimestamp = obj.activityTime, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.userId, CreatedDate = DateTime.Now, ModifiedBy = obj.userId, ModifiedDate = DateTime.Now };
                            GLBank.Comments = obj.comments;
                            db.Acc_GL.Add(GLBank);
                        }
                        else
                        {
                            ///Cash Entry
                            var GLBank = new Acc_GL() { TranId = GLParent.GlId, CoaId = 11, UserId = obj.userId, Debit = (obj.isdeposit == true ? obj.amount : 0), Credit = (obj.isdeposit == false ? obj.amount : 0), ActivityTimestamp = obj.activityTime, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.userId, CreatedDate = DateTime.Now, ModifiedBy = obj.userId, ModifiedDate = DateTime.Now };
                            GLBank.Comments = obj.comments;
                            db.Acc_GL.Add(GLBank);
                        }

                        //Investor Entry
                        var GLRecvable = new Acc_GL() { TranId = GLParent.GlId, CoaId = obj.investorId, UserId = obj.userId, Debit = (obj.isdeposit == false ? obj.amount : 0), Credit = (obj.isdeposit == true ? obj.amount : 0), ActivityTimestamp = obj.activityTime, TranTypeId = transType, InvoiceNo = obj.invoiceNo, IsActive = true, CreatedBy = obj.userId, CreatedDate = DateTime.Now, ModifiedBy = obj.userId, ModifiedDate = DateTime.Now };
                        db.Acc_GL.Add(GLRecvable);


                        db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        /// <summary>
        /// Get Share Holders Equity 
        /// COAID of  Equity=3
        /// COAID of Shareholder's Equity=24
        /// </summary>
        /// <returns></returns>
        public static List<BO_EquityShareHolders> GetShareHoldersName()
        {
            List<BO_EquityShareHolders> lst_equity = null;
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                lst_equity = (from equity in db.Acc_COA
                              where equity.PId == 3 && equity.HeadAccount == 24 && equity.IsActive == true
                              select new BO_EquityShareHolders
                              {
                                  coaId = equity.CoaId,
                                  treeName = equity.TreeName ?? "",
                              }).ToList();
                return lst_equity.OrderBy(x => x.coaId).ToList();
            }
        }

        public static List<BO_CapitalManagement> GetInvestorList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    List<BO_CapitalManagement> obj = db.Acc_COA.Where(x => x.PId == 3 && x.HeadAccount == 24 && x.CoaLevel == 2 && x.IsActive == true).Select(x => new BO_CapitalManagement
                    {
                        investorId = x.CoaId,
                        investorName = x.TreeName,
                        amount = db.Acc_GL.Where(y => y.TranTypeId == 11 & y.IsActive == true && y.CoaId == x.CoaId).Select(y => ((y.Credit ?? 0) - (y.Debit ?? 0))).Sum(),
                    }).OrderByDescending(x => x.investorId).ToList();
                    return obj;
                }
                catch { throw; }
            }
        }


        public static string SaveInvestor(int? CoaId, string investorName, decimal? openningBalance)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var chk = CoaId == null ? new Acc_COA() : db.Acc_COA.FirstOrDefault(x => x.CoaId == CoaId);
                if (chk.CoaId != 0)
                {
                    if (!chk.TreeName.Equals(investorName))
                    {
                        if (db.Acc_COA.FirstOrDefault(x => x.TreeName.Equals(investorName)) != null)
                            return "Investor Name Already Exists.";
                    }
                    chk.TreeName = investorName;
                    //chk.OpeningBalance = openningBalance;
                }
                else
                {
                    if (db.Acc_COA.FirstOrDefault(x => x.TreeName.Equals(investorName)) != null)
                        return "Investor Name Already Exists.";
                    //insert into Acc_COA (PId,HeadAccount,TreeName,CoaLevel,IsActive) values (3,24,'Investor B',1,1)
                    chk.TreeName = investorName;
                    //chk.OpeningBalance = openningBalance;
                    chk.PId = 3;
                    chk.HeadAccount = 24;
                    chk.CoaLevel = 2;
                    chk.IsActive = true;
                    db.Acc_COA.Add(chk);
                }
                db.SaveChanges();
            }


            return "success";
        }
        public static string DeleteInvestor(int CoaId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var chk = db.Acc_COA.FirstOrDefault(x => x.CoaId == CoaId);
                if (chk != null)
                    chk.IsActive = false;
                else
                    return "Record not found";
                db.SaveChanges();
            }


            return "success";
        }

        public static MYJSONTblCustom LoadEquityInfoTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.End_Date = Param.End_Date.AddDays(1);
            var _pvoucherlist = LoadEquityInfo(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Equity> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _pvoucherlist
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.amount != 0 && c.amount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.investorName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.accountNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.activity.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _pvoucherlist;
            }

            Func<BO_Equity, dynamic> orderingFunction = null;
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

                             Equity = c.investorName,
                             Amount = c.amount,

                             BankAccount = c.accountNo,
                             ActivityType = c.activity,
                             ActivityDate = c.activityTime,
                             InvoiceNo = c.invoiceNo,
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _pvoucherlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Equity> LoadEquityInfo(JQueryDataTableParamModel Param)
        {
            List<BO_Equity> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                obj = (from gl in db_aa.Acc_GL
                       join coa in db_aa.Acc_COA on gl.CoaId equals coa.CoaId
                       where gl.TranTypeId == 11 && gl.IsActive == true && coa.PId == 3

                       select new BO_Equity
                       {
                           invoiceNo = gl.InvoiceNo,
                           investorName = coa.TreeName,
                           amount = ((gl.Credit != null && gl.Credit > 0) ? (gl.Credit ?? 0) : (gl.Debit ?? 0)),
                           activity = (gl.Credit != null && gl.Credit > 0) ? "Deposit" : "WithDraw",
                           activityTime = gl.ActivityTimestamp.Value,
                       }).ToList();
                if (obj != null && obj.Count > 0)
                {
                    for (int i = 0; i < obj.Count; i++)
                    {
                        string _invoiceno = obj[i].invoiceNo;
                        int _coaId = obj[i].investorId;
                        string _accountNo = (from gl in db_aa.Acc_GL
                                             join coa in db_aa.Acc_COA on gl.CoaId equals coa.CoaId
                                             where gl.TranTypeId == 11 && gl.IsActive == true && gl.InvoiceNo == _invoiceno && gl.CoaId != _coaId
                                             select coa.TreeName).FirstOrDefault();
                        obj[i].accountNo = _accountNo;
                    }

                }
            }
            return obj.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 3)))).ToList();
          
        }

        public static string DeleteEquityInvoice(string invoiceNo, int empID)
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

    }
}