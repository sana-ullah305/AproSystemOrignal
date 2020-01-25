using ApprosysAccDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_SubscriptionManagement
    {
        static readonly object objLck = new object();
        public static string ValidateDueDates(DateTime actingDate)
        {
            lock (objLck)
            {


                using (AprosysAccountingEntities db = new AprosysAccountingEntities())
                {
                    Appcode.Logger.Write("Subscription Routine", "ValidateDueDates", "DateTime: " + actingDate.ToString() + " ," + actingDate.ToString(), Appcode.Logger.LogType.InformationLog);
                    // var custl = db.Customers.Where(x => x.TypeId == 2 && x.IsActive == true).Select(x => x.Id).ToList();
                    var custl = (from t1 in db.Customers.Where(x => x.IsActive == true)
                                 join t2 in db.Subscriptions.Where(s => s.IsActive == true) on t1.Id equals t2.CustId
                                 select t1.Id).ToList();




                    var subsl = db.Subscriptions.Where(x => x.IsActive == true).ToList();
                    var subexcdates = (from s in db.Subscriptions join e in db.SubscriptionExcludingDates on s.CustId equals e.CustId select new { e.CustId, e.SubDisableStartDate, e.SubDisableEndDate }).ToList();
                    int transType = (int)Constants.TransactionTypes.SubscriptionDue;
                    var glList = db.Acc_GL.Where(x => x.TranTypeId == transType && x.IsActive == true && x.CoaId == 10).Select(x => new { x.GlId, x.CustId, x.SubscriptionDueDate }).ToList();//.ToList().Where(x => x.ActivityTimestamp.Value.Date.Month == BL_Common.GetDatetime().Month && x.ActivityTimestamp.Value.Date.Year == BL_Common.GetDatetime().Year).Select(x => new { x.GlId, x.CustId, x.ActivityTimestamp }).ToList();
                    glList = glList.Where(x => custl.Contains(x.CustId.Value)).ToList();
                    //if (glList.Count == 0) { return "Ledger is empty"; }
                    foreach (var cust in custl)
                    {
                        var sub = subsl.SingleOrDefault(x => x.CustId == cust);
                        var subStartDate = sub.StartDate;
                        while (subStartDate <= new DateTime(actingDate.Year, actingDate.Month, DateTime.DaysInMonth(actingDate.Year, actingDate.Month)))
                        {
                            // do something with target.Month and target.Year
                            var monthDue = subStartDate;
                            var dueDate = new DateTime(monthDue.Year, monthDue.Month, sub.DueDate);
                            if ((/*(monthDue.Month < DateTime.Now.Month && monthDue < DateTime.Now) ||*/
                                (dueDate <= actingDate.Date && dueDate >= sub.StartDate)) &&
                                !glList.Exists(x => x.CustId == cust && x.SubscriptionDueDate.Value.Month == dueDate.Month && x.SubscriptionDueDate.Value.Year == dueDate.Year))
                            {
                                // Need to add a check which stops Excluding Entries in Acc_GL , however theese entries is removed in Exclusion Region
                                
                                if (sub.SubStatus == true)
                                {
                                    var subExclusionsDates = db.SubscriptionExcludingDates.Where(x => x.CustId == sub.Id && x.IsActive == true).ToList();
                                    if (subExclusionsDates != null && subExclusionsDates.Count > 0)
                                    {
                                     
                                        DateTime _startdisabledate = subExclusionsDates[0].SubDisableStartDate ?? BL_Common.GetDatetime();
                                        DateTime _enddisabledate = subExclusionsDates[0].SubDisableEndDate ?? BL_Common.GetDatetime();
                                        if (dueDate < _startdisabledate)
                                        {
                                            #region Insertion into AccGL
                                            using (var transaction = db.Database.BeginTransaction())
                                            {
                                                try
                                                {
                                                    var vNo = GetNextVoucher();
                                                    //if (!(sub.SubStatus == true && dueDate > sub.SubDisableStartDate && dueDate < sub.SubDisableEndDate))
                                                    // if (!(sub.SubStatus == true && dueDate > subexcdates.Where( x=>x.SubDisableStartDate > dueDate).FirstOrDefault() && dueDate < sub.SubDisableEndDate))
                                                    {
                                                        //Parent Entry
                                                        var GLParent = new Acc_GL() { CoaId = 0, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Debit = sub.SubscriptionAmount, Credit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                        db.Acc_GL.Add(GLParent);
                                                        db.SaveChanges();
                                                        //Account Receivable
                                                        var GLRec = new Acc_GL() { TranId = GLParent.GlId, CoaId = 10, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Debit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, IsPostpaid = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                        db.Acc_GL.Add(GLRec);
                                                        //Subscription Sales
                                                        var GLSubSales = new Acc_GL() { TranId = GLParent.GlId, CoaId = 100, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Credit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, IsPostpaid = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                        db.Acc_GL.Add(GLSubSales);
                                                        db.SaveChanges();
                                                    }

                                                    transaction.Commit();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Appcode.Logger.Write("Subscription Routine", "Validate Due Dates", ex.ToString(), Appcode.Logger.LogType.ErrorLog);
                                                    transaction.Rollback();
                                                    throw;
                                                }

                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                
                                {
                                    #region Insertion into AccGL
                                    using (var transaction = db.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var vNo = GetNextVoucher();
                                            //if (!(sub.SubStatus == true && dueDate > sub.SubDisableStartDate && dueDate < sub.SubDisableEndDate))
                                            // if (!(sub.SubStatus == true && dueDate > subexcdates.Where( x=>x.SubDisableStartDate > dueDate).FirstOrDefault() && dueDate < sub.SubDisableEndDate))
                                            {
                                                //Parent Entry
                                                var GLParent = new Acc_GL() { CoaId = 0, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Debit = sub.SubscriptionAmount, Credit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                db.Acc_GL.Add(GLParent);
                                                db.SaveChanges();
                                                //Account Receivable
                                                var GLRec = new Acc_GL() { TranId = GLParent.GlId, CoaId = 10, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Debit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, IsPostpaid = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                db.Acc_GL.Add(GLRec);
                                                //Subscription Sales
                                                var GLSubSales = new Acc_GL() { TranId = GLParent.GlId, CoaId = 100, CustId = cust, ActivityTimestamp = actingDate, SubscriptionDueDate = dueDate, TranTypeId = transType, Credit = sub.SubscriptionAmount, InvoiceNo = vNo, IsActive = true, IsPostpaid = true, CreatedDate = actingDate, CreatedBy = 1, ModifiedBy = 1, ModifiedDate = actingDate };
                                                db.Acc_GL.Add(GLSubSales);
                                                db.SaveChanges();
                                            }

                                            transaction.Commit();
                                        }
                                        catch (Exception ex)
                                        {
                                            Appcode.Logger.Write("Subscription Routine", "Validate Due Dates", ex.ToString(), Appcode.Logger.LogType.ErrorLog);
                                            transaction.Rollback();
                                            throw;
                                        }

                                    }
                                    #endregion
                                }

                            }
                            subStartDate = subStartDate.AddMonths(1);

                        }
                     
                           // if (sub.SubStatus == true)
                            {
                                #region Exclusion work 
                                var subExclusionsDates = db.SubscriptionExcludingDates.Where(x => x.CustId == sub.Id && x.IsActive == true).ToList();
                                if (subExclusionsDates != null && subExclusionsDates.Count > 0)
                                {
                                    for (int i = 0; i < subExclusionsDates.Count; i++)
                                    {
                                        DateTime _startdisabledate = subExclusionsDates[i].SubDisableStartDate ?? BL_Common.GetDatetime();
                                        DateTime _enddisabledate = subExclusionsDates[i].SubDisableEndDate ?? BL_Common.GetDatetime();
                                        var subDeactive = db.Acc_GL.Where(x => (x.SubscriptionDueDate >= _startdisabledate) && (x.SubscriptionDueDate < _enddisabledate) && (x.IsActive == true) && (x.CustId == cust)).ToList();
                                        if (subDeactive != null && subDeactive.Count > 0)
                                        {
                                            for (int j = 0; j < subDeactive.Count; j++)
                                            {
                                                subDeactive[j].IsActive = false;
                                                subDeactive[j].Comments = "Disabled From Routine by Disable Feature";
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                     
                     
                    }
                    return "success";
                }
            }
        }

        private static string GetNextVoucher()
        {
            string voucher = BL_Common.GetLastVoucher(5);
            if (String.IsNullOrEmpty(voucher)) { return ("SUBS-1"); }
            var increment = Convert.ToInt32(voucher.Split('-')[1]) + 1;
            return "SUBS-" + increment;
        }
    }
}