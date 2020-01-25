using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_SalesPersonManagement
    {
        public static List<KeyValuePair<int, string>> GetCustomersList()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.Customers.Where(x => x.IsActive == true && x.SalesPersonId == null).Select(z => new { z.Id, z.FirstName, z.LastName }).ToDictionary(z => Convert.ToInt32(z.Id), q => String.Format("{0} {1}", q.FirstName, q.LastName)).OrderBy(x => x.Value).ToList();
            }
        }
        public static List<KeyValuePair<int, string>> GetCustomersListBySalePersonID(int salePersonID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.Customers.Where(x => x.IsActive == true && (x.SalesPersonId == null || x.SalesPersonId == salePersonID)).Select(z => new { z.Id, z.FirstName, z.LastName }).ToDictionary(z => Convert.ToInt32(z.Id), q => String.Format("{0} {1}", q.FirstName, q.LastName)).OrderBy(x => x.Value).ToList();
            }
        }
        public static string SaveSalesPerson(BO_SalesPersonManagement _salesPerson, int UserID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {

                    var obj = _salesPerson.Id == 0 ? new ApprosysAccDB.SalesPerson() : db.SalesPersons.Where(x => x.Id == _salesPerson.Id && x.IsActive == true).FirstOrDefault();
                    if (_salesPerson.Id > 0)
                    {
                        var checkCust = db.SalesPersons.Where(x => x.LastName.ToLower() == _salesPerson.lastName.ToLower() && x.FirstName.ToLower() == _salesPerson.firstName.ToLower() && x.Id != _salesPerson.Id && x.IsActive == true).FirstOrDefault();
                        if (checkCust != null)
                        {
                            return "Sales Person Already Exists";
                        }
                    }

                    if (obj != null && obj.Id > 0)
                    {
                        obj.ModifiedBy = UserID;
                        obj.ModifiedOn = BL_Common.GetDatetime();

                        var customerList = db.Customers.Where(c => c.SalesPersonId == obj.Id && c.IsActive == true).ToList();
                        if (customerList != null)
                            foreach (var item in customerList)
                            {
                                item.SalesPersonId = null;
                            }

                    }
                    obj.Id = _salesPerson.Id;
                    obj.LastName = _salesPerson.lastName;
                    obj.FirstName = _salesPerson.firstName;
                    obj.Phone = _salesPerson.phone ?? "";
                    obj.Email = _salesPerson.email;
                    obj.CNIC = _salesPerson.cnic;
                    obj.NTN = _salesPerson.ntn;
                    if (_salesPerson.startDate == null)
                    {
                        _salesPerson.startDate = DateTime.Now.Date;
                    }
                    obj.StartDate = _salesPerson.startDate;

                    obj.OpeningBalance = _salesPerson.openingBalance;
                    obj.Misc = _salesPerson.misc;
                    if (obj.Id == 0)
                    {
                        obj.CreatedBy = UserID;
                        obj.CreatedOn = BL_Common.GetDatetime();
                        var objcheck = db.SalesPersons.Where(x => x.LastName.ToLower() == _salesPerson.lastName.ToLower() && x.FirstName.ToLower() == _salesPerson.firstName.ToLower() && x.IsActive == true).FirstOrDefault();
                        if (objcheck != null)
                        {
                            return "Sales Person Already Exists";
                        }
                        db.SalesPersons.Add(obj);
                    }
                    obj.IsActive = true;
                    db.SaveChanges();
                    if (_salesPerson.customersIDs != null)
                    {
                        var customerList = db.Customers.Where(c => _salesPerson.customersIDs.Contains(c.Id) && c.IsActive == true).ToList();
                        foreach (var item in customerList)
                        {
                            item.SalesPersonId = obj.Id;
                        }
                        db.SaveChanges();
                    }

                    return obj.Id.ToString();

                    // return "Insertion Failed";
                }
                catch { throw; }
            }
        }

        public static MYJSONTblCustom LoadSalesPersonTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var _salesPersonlist = LoadISalesPerson(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_SalesPersonManagement> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _salesPersonlist
                   .Where(
                    c => c.Id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.lastName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.email.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.openingBalance > 0 && c.openingBalance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    // || c.dueDate != null && c.dueDate.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //|| c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _salesPersonlist;
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
                             Id = c.Id,
                             lastName = c.lastName,
                             firstName = c.firstName,
                             phone = c.phone,
                             email = c.email,
                             cnic = c.cnic ?? "",

                             openingBalance = c.openingBalance,
                             startDate = c.startDate,
                             createdOn = c.createdOn,
                             createdBy = c.createdBy,
                             isActive = c.isActive,
                             modifiedOn = c.modifiedOn,
                             modifiedBy = c.modifiedBy,
                             //dueDate = _salesPerson.DueDate,
                             misc = c.misc ?? ""

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _salesPersonlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_SalesPersonManagement> LoadISalesPerson(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_SalesPersonManagement> List = (from _salesPerson in db_aa.SalesPersons
                                                       where _salesPerson.IsActive == true //&& _salesPerson.TypeId==1
                                                       orderby _salesPerson.Id descending
                                                       select new BO_SalesPersonManagement

                                                       {
                                                           Id = _salesPerson.Id,
                                                           lastName = _salesPerson.LastName ?? "",
                                                           firstName = _salesPerson.FirstName ?? "",
                                                           phone = _salesPerson.Phone ?? "",
                                                           email = _salesPerson.Email ?? "",
                                                           cnic = _salesPerson.CNIC ?? "",

                                                           openingBalance = _salesPerson.OpeningBalance ?? 0,
                                                           startDate = _salesPerson.StartDate,
                                                           createdOn = _salesPerson.CreatedOn,
                                                           createdBy = _salesPerson.CreatedBy,
                                                           isActive = _salesPerson.IsActive,
                                                           modifiedOn = _salesPerson.ModifiedOn,
                                                           modifiedBy = _salesPerson.ModifiedBy,
                                                           //dueDate = _salesPerson.DueDate,
                                                           misc = _salesPerson.Misc ?? ""


                                                       }).OrderByDescending(x => x.Id).ToList();
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

                List<BO_SalesPersonManagement> ListtoReturn = new List<BO_SalesPersonManagement>();
                ListtoReturn = List;
                return ListtoReturn;
            }
        }
        public static List<string> GetSalesPersonNamesByID(List<int> salePersonID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.SalesPersons.Where(x => salePersonID.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName).ToList();
            }
        }
        public static BO_SalesPersonManagement GetSalesPersonByID(int salePersonID)
        {
            BO_SalesPersonManagement _salesPerson = new BussinessObject.BO_SalesPersonManagement();
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.SalesPersons.Where(x => x.Id == salePersonID).FirstOrDefault();
                if (obj != null && obj.Id > 0)
                {
                    _salesPerson.Id = obj.Id;
                    _salesPerson.lastName = obj.LastName ?? "";
                    _salesPerson.firstName = obj.FirstName ?? "";
                    _salesPerson.phone = obj.Phone ?? "";
                    _salesPerson.email = obj.Email ?? "";
                    _salesPerson.cnic = obj.CNIC;
                    _salesPerson.ntn = obj.NTN;
                    _salesPerson.openingBalance = obj.OpeningBalance ?? 0;
                    _salesPerson.startDate = obj.StartDate;
                    _salesPerson.misc = obj.Misc ?? "";
                    obj.ModifiedOn = BL_Common.GetDatetime();
                }
                if (_salesPerson != null)
                    _salesPerson.customersIDs = db.Customers.Where(x => x.SalesPersonId == salePersonID).Select(x => x.Id).ToList() ?? null;

            }
            return _salesPerson;
        }

        public static string DeleteSalesPerson(int salesPersonID, int userID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var salespersonTransaction = db.Acc_GL.Where(x => x.SalesPersonId == salesPersonID && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (salespersonTransaction == null || salespersonTransaction.Count > 0) { return "Transaction is Performed against Sales Person, it can not be deleted "; }

                var obj = db.SalesPersons.Where(x => x.Id == salesPersonID).FirstOrDefault();

                if (obj != null && obj.Id > 0)
                {
                    obj.ModifiedBy = userID;
                    obj.ModifiedOn = BL_Common.GetDatetime();

                }
                obj.IsActive = false;
                db.SaveChanges();
                return "success";
            }
        }
    }
}