using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Subscription
    {
        public static string SaveSubscriptionCustomer(BO_SubCustomer _Subcustomer, int userid)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    //var objcheck = db.Customers.Where(x => x.LastName.ToLower() == _Subcustomer.lastName.ToLower() && x.FirstName.ToLower() == _Subcustomer.firstName.ToLower()).FirstOrDefault();
                    //if (objcheck != null)
                    //{
                    //    return "Customer Already Exists";
                    //}

                    var objcheck = db.Subscriptions.Where(x => x.CustId == _Subcustomer.id && x.IsActive == true).FirstOrDefault();
                    if (objcheck != null)
                    {
                        return "Customer Already Exists";
                    }


                    var obj = _Subcustomer.id == 0 ? new ApprosysAccDB.Customer() : db.Customers.Where(x => x.Id == _Subcustomer.id).FirstOrDefault();

                    //if (obj != null && obj.Id > 0)
                    //{
                    //    obj.ModifiedBy = _Subcustomer.modifiedBy;
                    //    obj.ModifiedOn = BL_Common.GetDatetime();

                    //}
                    //obj.Id = _Subcustomer.id;
                    //obj.LastName = _Subcustomer.lastName;
                    //obj.FirstName = _Subcustomer.firstName;
                    //obj.Phone = _Subcustomer.phone ?? "";
                    //obj.Email = _Subcustomer.email;
                    //obj.StartDate = _Subcustomer.startDate;

                    //obj.OpeningBalance = _Subcustomer.openingBalance;
                    //obj.Misc = _Subcustomer.misc;
                    //obj.TypeId = 2;

                    //obj.CreatedBy = userid;
                    //obj.CreatedOn = BL_Common.GetDatetime();

                    //obj.IsActive = true;
                    //db.Customers.Add(obj);
                    //db.SaveChanges();


                    var objsub = _Subcustomer.id != 0 ? new ApprosysAccDB.Subscription() : db.Subscriptions.Where(x => x.Id == _Subcustomer.subid).FirstOrDefault();
                    // objsub.Id = _Subcustomer.subid;
                    //objsub.CustId = obj.Id;
                    objsub.CustId = _Subcustomer.id;
                    objsub.SubscriptionAmount = _Subcustomer.subscriptionAmount;
                    objsub.DueDate = _Subcustomer.dueDate;
                    objsub.StartDate = _Subcustomer.startDate ?? BL_Common.GetDatetime();
                    obj.CreatedOn = BL_Common.GetDatetime();
                    obj.CreatedBy = userid;
                    objsub.IsActive = true;
                    obj.Misc = _Subcustomer.misc;
                    objsub.SubStatus = _Subcustomer.subStatus;

                    db.Subscriptions.Add(objsub);
                    db.SaveChanges();
                    if (_Subcustomer.subStatus)
                    {
                        var obj1 = new ApprosysAccDB.SubscriptionExcludingDate();
                        obj1.CustId = objsub.Id;
                        obj1.SubDisableStartDate = BL_Common.GetDatetime();
                        obj1.SubDisableEndDate = new DateTime(2050, 12, 31);
                        obj1.IsActive = true;
                        db.SubscriptionExcludingDates.Add(obj1);
                        db.SaveChanges();

                        /*if (_Subcustomer.sed != null && _Subcustomer.sed.Count > 0)
                        {
                            for (int i = 0; i < _Subcustomer.sed.Count; i++)
                            {
                                try
                                {
                                    var obj1 = new ApprosysAccDB.SubscriptionExcludingDate();
                                    obj1.CustId = objsub.Id;
                                    obj1.SubDisableStartDate = _Subcustomer.sed[i].subDisableStartDate;
                                    obj1.SubDisableEndDate = _Subcustomer.sed[i].subDisableEndDate;
                                    obj1.IsActive = true;
                                    db.SubscriptionExcludingDates.Add(obj1);
                                    db.SaveChanges();
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        */
                    }



                    //db.SaveChanges();
                    Func<DateTime, string> act = new Func<DateTime, string>(BL_SubscriptionManagement.ValidateDueDates);
                    act.BeginInvoke(DateTime.Now, null, null);
                    return "success";


                    // return "Insertion Failed";
                }
                catch (Exception ex) { throw; }
            }
        }


        public static string EditSubscriptionCustomer(BO_SubCustomer _Subcustomer, int userid)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    //var objcheck = db.Customers.Where(x => x.LastName.ToLower() == _Subcustomer.lastName.ToLower() && x.FirstName.ToLower() == _Subcustomer.firstName.ToLower()).FirstOrDefault();
                    //if (objcheck != null)
                    //{
                    //    return "Customer Already Exists";
                    //}

                    var objsub = db.Subscriptions.Where(x => x.Id == _Subcustomer.id).FirstOrDefault();
                    objsub.CustId = objsub.CustId;
                    objsub.SubscriptionAmount = _Subcustomer.subscriptionAmount;
                    objsub.DueDate = _Subcustomer.dueDate;
                    objsub.StartDate = _Subcustomer.startDate ?? BL_Common.GetDatetime();
                    objsub.ModifiedOn = BL_Common.GetDatetime();
                    objsub.ModifiedBy = userid;
                    objsub.IsActive = true;

                    objsub.SubStatus = _Subcustomer.subStatus;

                    if (!_Subcustomer.subStatus)
                    {
                        var obj1 = db.SubscriptionExcludingDates.Where(x => x.CustId == _Subcustomer.id && x.SubDisableEndDate == new DateTime(2050, 12, 31)).FirstOrDefault();

                        if (obj1 != null && obj1.SubDisableEndDate.Value.Date == new DateTime(2050, 12, 31).Date)
                        {
                            obj1.SubDisableEndDate = BL_Common.GetDatetime();
                            db.SaveChanges();
                        }
                    }
                    if (_Subcustomer.subStatus)
                    {
                        var obj1 = db.SubscriptionExcludingDates.Where(x => x.CustId == _Subcustomer.id).FirstOrDefault();
                        if (obj1 != null && obj1.SubDisableEndDate.Value.Date == new DateTime(2050, 12, 31).Date)
                        {
                            obj1.SubDisableStartDate = BL_Common.GetDatetime();
                            db.SaveChanges();
                        }
                        else
                        {
                            var obj2 = new ApprosysAccDB.SubscriptionExcludingDate();
                            obj2.CustId = objsub.Id;
                            obj2.SubDisableStartDate = BL_Common.GetDatetime();
                            obj2.SubDisableEndDate = new DateTime(2050, 12, 31);
                            obj2.IsActive = true;
                            db.SubscriptionExcludingDates.Add(obj2);
                            db.SaveChanges();
                        }

                    }

                    var obj = db.Customers.Where(x => x.Id == objsub.CustId).FirstOrDefault();
                    obj.ModifiedBy = userid;
                    obj.ModifiedOn = BL_Common.GetDatetime();
                    obj.Misc = _Subcustomer.misc;
                    db.SaveChanges();

                    Func<DateTime, string> act = new Func<DateTime, string>(BL_SubscriptionManagement.ValidateDueDates);
                    act.BeginInvoke(DateTime.Now, null, null);

                    return "success";

                    // return "Insertion Failed";
                }
                catch (Exception ex) { throw; }
            }
        }
        public static MYJSONTblCustom LoadSubscriptionTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _sublist = LoadSubscription(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_SubCustomer> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _sublist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.lastName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.subscriptionAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.dueDate > 0 && c.openingBalance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //|| c.subStatus 
                    );
            }
            else
            {
                filteredCategories = _sublist;
            }
            /*Func<BO_SubCustomer, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_SubCustomer> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.lastName : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.firstName : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.phone : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.subscriptionAmount : 0); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.dueDate : 0); }
                    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.startDate.ToString() : ""); }
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
            */
            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             subId = c.subid,
                             lastname = c.lastName,
                             firstName = c.firstName,
                             phone = c.phone,
                             subscriptionAmount = c.subscriptionAmount,
                             dueDate = c.dueDate,
                             startDate = c.startDate,
                             lastReminderSentDate = c.subLastReminderSent,
                             subscriptionStatus = c.subscriptionStatus,

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _sublist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result.OrderByDescending(x => x.subId);
            return _MYJSONTbl;

        }
        public static IList<BO_SubCustomer> LoadSubscription(JQueryDataTableParamModel Param)
        {
            List<BO_SubCustomer> lst_subTable = new List<BO_SubCustomer>();
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                BO_SubCustomer obj;

                var lst = db_aa.GetSubscriptionList();
                var reminders = db_aa.Reminders.Where(x => x.LastReminderSentDate.HasValue).Select(x => new { x.SubscriptionID, x.LastReminderSentDate }).GroupBy(x => x.SubscriptionID).ToDictionary(x => x.Key);
                foreach (var item in lst.ToList())
                {
                    obj = new BO_SubCustomer();
                    obj.subid = item.subid;
                    obj.lastName = item.LastName;
                    obj.firstName = item.FirstName;
                    obj.phone = item.Phone;
                    obj.subscriptionAmount = item.SubscriptionAmount;
                    obj.dueDate = item.DueDate;
                    obj.startDate = item.StartDate;
                    obj.subStatus = item.SubStatus ?? false;
                    obj.subscriptionStatus = (item.SubStatus == true ? "Suspended" : "Active");
                    if (obj.subStatus)
                    {
                        //obj.subDisableStartDate = item.SubDisableStartDate;
                        //obj.subDisableEndDate = item.SubDisableEndDate;
                    }
                    if (reminders.ContainsKey(item.subid))
                    {
                        obj.subLastReminderSent = reminders[item.subid].OrderByDescending(x => x.LastReminderSentDate).First().LastReminderSentDate.Value;
                    }
                    lst_subTable.Add(obj);
                }
            }
            if (Param.SearchType != 0)
            {
                //if (Param.SearchType == 1)// && Param.SearchValue != null && Param.SearchValue != " ")
                //{
                //    List = List.Where(x => x.name.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();
                //}
                //else if (Param.SearchType == 2)// && Param.SearchValue != null && Param.SearchValue != " ")
                //{
                //    List = List.Where(x => x.itemCode.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();

                //}

            }

            List<BO_SubCustomer> ListtoReturn = new List<BO_SubCustomer>();
            ListtoReturn = lst_subTable;
            return ListtoReturn;
        }

        public static string DeleteSubscriptionCustomer(int subcustomerID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var custTransaction = (from cust in db.Customers
                                       join sub in db.Subscriptions on cust.Id equals sub.CustId
                                       join Gl in db.Acc_GL on cust.Id equals Gl.CustId
                                       where sub.Id == subcustomerID && Gl.IsActive == true
                                       select new
                                       {
                                           Id = sub.Id


                                       }).ToList();


                // var custTransaction = db.Acc_GL.Where(x => x.CustId == subcustomerID && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (custTransaction == null || custTransaction.Count > 0) { return "Transaction is Performed against this Customer, it can not be deleted "; }


                var obj = db.Subscriptions.Where(x => x.Id == subcustomerID).FirstOrDefault();

                obj.ModifiedBy = 1;
                obj.ModifiedOn = BL_Common.GetDatetime();
                obj.IsActive = false;

                db.SaveChanges();
                //var cus = db.Customers.Where(x => x.Id == obj.CustId).FirstOrDefault();
                //cus.ModifiedBy = 1;
                //cus.ModifiedOn = BL_Common.GetDatetime();
                //cus.IsActive = false;
                //db.SaveChanges();
                return "success";
            }
        }

        public static BO_SubCustomer GetSubCustByID(int subCustId)
        {
            BO_SubCustomer obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                obj = (from sub in db_aa.Subscriptions
                       join cus in db_aa.Customers on sub.CustId equals cus.Id
                       where sub.Id == subCustId

                       select new BO_SubCustomer
                       {


                           subid = sub.Id,
                           subscriptionAmount = sub.SubscriptionAmount,
                           dueDate = sub.DueDate,
                           startDate = sub.StartDate,
                           lastName = cus.LastName,
                           firstName = cus.FirstName,
                           phone = cus.Phone,
                           email = cus.Email,
                           openingBalance = cus.OpeningBalance ?? 0,
                           misc = cus.Misc,
                           subStatus = sub.SubStatus ?? false,
                           //sed = new List<BO_SubscriptionExcludingDates>(),
                           //subDisableStartDate=sub.SubDisableStartDate,
                           //subDisableEndDate=sub.SubDisableEndDate

                       }).FirstOrDefault();

                var lst_sed = db_aa.SubscriptionExcludingDates.Where(x => x.CustId == subCustId).ToList();
                if (lst_sed != null && lst_sed.Count > 0)
                {
                    List<BO_SubscriptionExcludingDates> ll = new List<BO_SubscriptionExcludingDates>();
                    for (int i = 0; i < lst_sed.Count; i++)
                    {


                        BO_SubscriptionExcludingDates _Sed = new BO_SubscriptionExcludingDates();
                        _Sed.subDisableStartDate = lst_sed[i].SubDisableStartDate;
                        _Sed.subDisableEndDate = lst_sed[i].SubDisableEndDate;
                        ll.Add(_Sed);


                    }
                    obj.sed = ll;

                }

                //obj.sed.Add( lst_sed);
            }
            return obj;
        }

        public static List<BO_SubscriptionReminder> GetReminderLogs(int subCustId)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                return db_aa.Reminders.Where(x => x.SubscriptionID == subCustId).Select(x => new BO_SubscriptionReminder
                {
                    Log = x.ReminderLog,
                    AlertSentDate = x.LastReminderSentDate
                }).ToList(); ;
            }
        }
        public string ClearSuspendedDuesofCustomer(int SubCustId)
        {


            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                DateTime? subExclusionDate = BL_Common.GetDatetime();
                var totalpaidcustomer = db.usp_GetTotalPaidofCustomer(SubCustId).FirstOrDefault();//.Select (x => new { x.CustId, x.TotPaid, x.LastPaid });
                decimal _totpaid =  (totalpaidcustomer == null ? 0 : (totalpaidcustomer.TotPaid ?? 0));
                var totalduesfromstart = db.usp_GetSubDuesofCustomer(SubCustId).ToList().Select(x => new { x.CustId, x.SubscriptionDueDate, x.Debit, x.AllDueBefore }).ToList();
                for (int i = 0; i < totalduesfromstart.Count(); i++)
                {
                    if (_totpaid <= totalduesfromstart[i].AllDueBefore)
                    {

                        subExclusionDate = totalduesfromstart[i].SubscriptionDueDate;
                        //string t= "Update  SubscriptionExcludingDates SET SubDisableStartDate = '" + subExclusionDate + "' where CustId = " + SubCustId+ " and SubDisableEndDate = '"+new DateTime(2015,12,31).Date.Date+"'";
                        db.Database.ExecuteSqlCommand("Update  SubscriptionExcludingDates SET SubDisableStartDate = '" + subExclusionDate + "' where CustId=" + SubCustId + " and SubDisableEndDate= '2050 - 12 - 31 00:00:00.000'");

                        break;

                    }

                }
                Func<DateTime, string> act = new Func<DateTime, string>(BL_SubscriptionManagement.ValidateDueDates);
                act.BeginInvoke(DateTime.Now, null, null);

                //var lst_totalpaid = (from GL in db.Acc_GL
                //                     join C in db.Customers on GL.CustId equals C.Id
                //                     join S in db.Subscriptions on C.Id equals S.CustId
                //                     where GL.IsActive == true && C.IsActive == true && S.IsActive == true
                //                     select new
                //                     {
                //                         tranid = GL.TranId,
                //                         credit = GL.Credit,
                //                         subDueDate = GL.SubscriptionDueDate,
                //                         debit = GL.Debit,
                //                     }
                //                ).ToList();
                //decimal totalpaid = lst_totalpaid.Sum(x => x.credit).Value;
                //db.Database.ExecuteSqlCommand("Update  SubscriptionExcludingDates SET SubDisableStartDate = '" + BL_Common.GetDatetime() + "' where CustId=" + SubCustId);
                //  db.Database.ExecuteSqlCommand("Update  Subscription SET StartDate = '" + BL_Common.GetDatetime() + "' where Id=" + SubCustId);
            }

            //var lst_sed = db_aa.SubscriptionExcludingDates.Where(x => x.CustId == SubCustId).ToList();
            //if (lst_sed != null && lst_sed.Count > 0)
            //{
            //    List<BO_SubscriptionExcludingDates> ll = new List<BO_SubscriptionExcludingDates>();
            //    for (int i = 0; i < lst_sed.Count; i++)
            //    {


            //        BO_SubscriptionExcludingDates _Sed = new BO_SubscriptionExcludingDates();
            //        //_Sed.subDisableStartDate = BL_Common.GetDatetime();
            //        //_Sed.sedisActive = true;

            //        db_aa.SubscriptionExcludingDates.Add(_Sed);
            //        db_aa.SaveChanges();
            //    }
            //    db_aa.SaveChanges();

            //}

            return "success";
        }


    }
}