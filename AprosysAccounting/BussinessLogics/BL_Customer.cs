using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;



namespace AprosysAccounting.BussinessLogics
{
    public class BL_Customer
    {
        public static List<BO_Customerlist> GetCustomerList(int typeID)
        {
            List<BO_Customerlist> obj = null;

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                //if (typeID == 2)
                //{


                obj = (from cus in db_aa.Customers
                           // join subs in db_aa.Subscriptions on cus.Id equals subs.CustId
                       where cus.IsActive == true
                       // && (cus.TypeId == 2)
                       select new BO_Customerlist
                       {
                           Id = cus.Id,
                           Name = (cus.FirstName ?? "") + " " + (cus.LastName ?? ""),
                           //DueDate = subs.DueDate,
                           //SubscriptionAmount=subs.SubscriptionAmount,
                           FirstName = cus.FirstName ?? "",
                           LastName = cus.LastName ?? "",
                           Email = cus.Email ?? "",
                           StartDate = cus.StartDate != null ? cus.StartDate.ToString() : "",
                           PhoneNo = cus.Phone ?? "",
                       }).ToList();

                //}


                //if (typeID == 1)
                //{
                //    obj = (from cus in db_aa.Customers
                //           where cus.IsActive == true
                //           && (cus.TypeId == 1 || cus.TypeId == 3)
                //           select new BO_Customerlist
                //           {
                //               Id = cus.Id,
                //               Name = (cus.LastName ?? "") + " " + (cus.FirstName ?? ""),
                //               FirstName = cus.FirstName ?? "",
                //               LastName = cus.LastName ?? "",
                //               Email = cus.Email ?? "",
                //               StartDate = cus.StartDate != null ? cus.StartDate.ToString() : "",
                //               PhoneNo = cus.Phone ?? "",
                //           }).ToList();
                //}
                return obj.OrderBy(x => x.Name).ToList();
            }
        }


        public static List<BO_Customerlist> GetCustomerListByType(int typeID)
        {
            List<BO_Customerlist> obj = null;

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                if (typeID == 2)
                {


                    obj = (from cus in db_aa.Customers
                           join subs in db_aa.Subscriptions on cus.Id equals subs.CustId
                           where cus.IsActive == true && subs.IsActive == true

                           select new BO_Customerlist
                           {
                               Id = cus.Id,
                               Name = (cus.FirstName ?? "") + " " + (cus.LastName ?? ""),
                               DueDate = subs.DueDate,
                               SubscriptionAmount = subs.SubscriptionAmount,
                               FirstName = cus.FirstName ?? "",
                               LastName = cus.LastName ?? "",
                               Email = cus.Email ?? "",
                               StartDate = cus.StartDate != null ? cus.StartDate.ToString() : "",
                               PhoneNo = cus.Phone ?? "",
                           }).ToList();

                }


                if (typeID == 1)
                {
                    obj = (from cus in db_aa.Customers
                           where cus.IsActive == true

                           select new BO_Customerlist
                           {
                               Id = cus.Id,
                               Name = (cus.FirstName ?? "") + " " + (cus.LastName ?? ""),
                               FirstName = cus.FirstName ?? "",
                               LastName = cus.LastName ?? "",
                               Email = cus.Email ?? "",
                               StartDate = cus.StartDate != null ? cus.StartDate.ToString() : "",
                               PhoneNo = cus.Phone ?? "",
                           }).ToList();
                }
                return obj.OrderBy(x => x.Name).ToList();
            }
        }


        public static List<BO_Customerlist> GetSubCustomerList(int typeID)
        {
            List<BO_Customerlist> obj_Sub = null;

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {

                obj_Sub = (from subs in db_aa.Subscriptions
                           join cus in db_aa.Customers on subs.CustId equals cus.Id
                           where cus.IsActive == true && subs.IsActive == true
                           select new BO_Customerlist
                           {
                               Id = cus.Id,
                               Name = (cus.FirstName ?? "") + " " + (cus.LastName ?? ""),
                               DueDate = subs.DueDate,
                               SubscriptionAmount = subs.SubscriptionAmount,
                               FirstName = cus.FirstName ?? "",
                               LastName = cus.LastName ?? "",
                               Email = cus.Email ?? "",
                               StartDate = cus.StartDate != null ? cus.StartDate.ToString() : "",
                               PhoneNo = cus.Phone ?? "",
                           }).ToList();



                return obj_Sub.OrderBy(x => x.Name).ToList();
            }
        }

        public static string SaveCustomer(BO_Customers _customer, int UserID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {

                    var obj = _customer.id == 0 ? new ApprosysAccDB.Customer() : db.Customers.Where(x => x.Id == _customer.id && x.IsActive == true).FirstOrDefault();
                    if (_customer.id > 0)
                    {
                        var checkCust = db.Customers.Where(x => x.LastName.ToLower() == _customer.lastName.ToLower() && x.FirstName.ToLower() == _customer.firstName.ToLower() && x.Id != _customer.id && x.IsActive == true).FirstOrDefault();
                        if (checkCust != null)
                        {
                            return "Customer Already Exists";
                        }
                    }

                    if (obj != null && obj.Id > 0)
                    {
                        obj.ModifiedBy = UserID;
                        obj.ModifiedOn = BL_Common.GetDatetime();

                    }
                    obj.Id = _customer.id;
                    obj.LastName = _customer.lastName;
                    obj.FirstName = _customer.firstName;
                    obj.SalesPersonId = _customer.salesPerson;
                    obj.Phone = _customer.phone ?? "";
                    obj.CNIC = _customer.cnic ?? "";
                    obj.NTN = _customer.ntn ?? "";

                    obj.Email = _customer.email;
                    if (_customer.startDate == null)
                    {
                        _customer.startDate = DateTime.Now.Date;
                    }
                    obj.StartDate = _customer.startDate;

                    obj.OpeningBalance = _customer.openingBalance;
                    obj.Misc = _customer.misc;
                    obj.TypeId = 1;
                    if (obj.Id == 0)
                    {
                        obj.CreatedBy = UserID;
                        obj.CreatedOn = BL_Common.GetDatetime();
                        var objcheck = db.Customers.Where(x => x.LastName.ToLower() == _customer.lastName.ToLower() && x.FirstName.ToLower() == _customer.firstName.ToLower() && x.IsActive == true).FirstOrDefault();
                        if (objcheck != null)
                        {
                            return "Customer Already Exists";
                        }
                        db.Customers.Add(obj);
                    }
                    obj.IsActive = true;
                    db.SaveChanges();
                    return obj.Id.ToString();

                    // return "Insertion Failed";
                }
                catch { throw; }
            }
        }
        public static MYJSONTblCustom LoadCustomerTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var _customerlist = LoadICustomers(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Customers> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _customerlist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.lastName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.email.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.cnic.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.openingBalance > 0 && c.openingBalance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    // || c.dueDate != null && c.dueDate.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _customerlist;
            }
            /*Func<BO_Customers, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_Customers> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.lastName : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.firstName : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.phone : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.email : ""); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.openingBalance : 0); }
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
            */
            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             id = c.id,
                             lastname = c.lastName,
                             firstName = c.firstName,
                             phone = c.phone,
                             cnic = c.cnic,
                             email = c.email,
                             openingBalance = c.openingBalance,
                             balance = c.balance,
                             salesPersonName = c.salesPersonName

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _customerlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Customers> LoadICustomers(JQueryDataTableParamModel Param)
        {
            List<BO_Customers> List;
            List<SalesPerson> salesPerson;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List = (from _customer in db_aa.Customers
                            //join _saleperson in db_aa.SalesPersons on _customer.SalesPersonId equals _saleperson.Id
                        where _customer.IsActive == true //&& _customer.TypeId==1
                        orderby _customer.Id descending
                        select new BO_Customers

                        {
                            id = _customer.Id,
                            lastName = _customer.LastName ?? "",
                            firstName = _customer.FirstName ?? "",
                            phone = _customer.Phone ?? "",
                            email = _customer.Email ?? "",
                            openingBalance = _customer.OpeningBalance ?? 0,
                            startDate = _customer.StartDate,
                            cnic = _customer.CNIC ?? "",
                            //dueDate = _customer.DueDate,
                            misc = _customer.Misc ?? "",
                            salesPerson = _customer.SalesPersonId
                            //salesPersonName = _saleperson.FirstName + " " + _saleperson.LastName
                        }).OrderByDescending(x => x.id).ToList();
                salesPerson = db_aa.SalesPersons.ToList();
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
            }
            foreach (var item in List)
            {
                if (item.salesPerson.GetValueOrDefault() != 0)
                {
                    var dumSalePerson = salesPerson.FirstOrDefault(x => x.Id == item.salesPerson.GetValueOrDefault());
                    item.salesPersonName = dumSalePerson.FirstName + " " + dumSalePerson.LastName;
                }
            }
            List<BO_Customers> ListtoReturn = new List<BO_Customers>();
            ListtoReturn = List;
            return ListtoReturn;
        }

        public static string DeleteCustomer(int customerID, int userID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var custTransaction = db.Acc_GL.Where(x => x.CustId == customerID && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (custTransaction == null || custTransaction.Count > 0) { return "Transaction is Performed from Customer, it can not be deleted "; }

                var obj = db.Customers.Where(x => x.Id == customerID).FirstOrDefault();

                if (obj != null && obj.Id > 0)
                {
                    obj.ModifiedBy = userID;
                    obj.ModifiedOn = BL_Common.GetDatetime();

                }
                obj.IsActive = false;

                //        db.Items.Add(obj);
                db.SaveChanges();
                return "success";
            }
        }
        public static decimal? GetCustomerBalance(int customerID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var alpha = db.GetCustomerBalance(customerID).FirstOrDefault();
                if (alpha != null) return alpha.Balance;
                else return 0;
            }
        }

        public static BO_Customers GetCustomerByID(int customerID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Customers.Where(x => x.Id == customerID).FirstOrDefault();
                BO_Customers _customer = new BussinessObject.BO_Customers();
                if (obj != null && obj.Id > 0)
                {
                    _customer.id = obj.Id;
                    _customer.lastName = obj.LastName ?? "";
                    _customer.firstName = obj.FirstName ?? "";
                    _customer.phone = obj.Phone ?? "";
                    _customer.email = obj.Email ?? "";
                    _customer.cnic = obj.CNIC ?? "";
                    _customer.openingBalance = obj.OpeningBalance ?? 0;
                    _customer.startDate = obj.StartDate;
                    _customer.misc = obj.Misc ?? "";
                    _customer.ntn = obj.NTN ?? "";
                    _customer.salesPerson = obj.SalesPersonId.GetValueOrDefault();
                    //obj.ModifiedOn = BL_Common.GetDatetime();


                }
                return _customer;

            }
        }


        public static List<BO_Customerlist> GetCustomerListBySalesPersonID(int salesPersonID)
        {
            List<BO_Customerlist> obj = null;

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {

                obj = (from cus in db_aa.Customers
                       where cus.IsActive == true && cus.SalesPersonId == salesPersonID
                       select new BO_Customerlist
                       {
                           Id = cus.Id,
                           Name = (cus.FirstName ?? "") + " " + (cus.LastName ?? ""),
                       }).ToList();
                return obj.OrderBy(x => x.Name).ToList();
            }
        }
    }
}