using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Vendor
    {
        public static List<BO_Vendorlist> GetVendorList()
        {
            List<BO_Vendorlist> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                obj = (from v in db_aa.Vendors
                       where v.IsActive == true
                       select new BO_Vendorlist
                       {
                           Id = v.ID,
                           Name = (v.FirstName ?? "") + " " + (v.LastName ?? "")
                       }).ToList();

                return obj;
            }
        }
        public static bool IsVendorModified(BO_Vendor Uiobject, ApprosysAccDB.Vendor dbobject)
        {
            bool res = false;
            if (Uiobject.firstName != dbobject.FirstName) res = true; return res;
            if (Uiobject.lastName != dbobject.LastName) res = true; return res;
            if (Uiobject.phone != dbobject.Phone) res = true; return res;
            if (Uiobject.email != dbobject.Email) res = true; return res;
            return res;
        }
        public static string SaveVendor(BO_Vendor _vendor,int userId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {

                    //var objcheck = db.Vendors.Where(x => x.LastName.ToLower() == _vendor.lastName.ToLower() && x.FirstName.ToLower() == _vendor.firstName.ToLower()).FirstOrDefault();
                    //if (objcheck != null)
                    //{
                    //    return "Vendor Already Exists";

                    //}

                    var obj = _vendor.id == 0 ? new ApprosysAccDB.Vendor() : db.Vendors.Where(x => x.ID == _vendor.id && x.IsActive==true).FirstOrDefault();

                    //bool isVendorModified = false;
                    //  if (obj != null && obj.ID > 0) { isVendorModified = IsVendorModified(_vendor, obj); }
                    if (_vendor.id > 0)
                    {
                        var checkCust = db.Vendors.Where(x => x.LastName.ToLower() == _vendor.lastName.ToLower() && x.FirstName.ToLower() == _vendor.firstName.ToLower() && x.ID != _vendor.id && x.IsActive == true).FirstOrDefault();
                        if (checkCust != null)
                        {
                            return "Vendor Already Exists";
                        }
                    }
                    if (obj != null && obj.ID > 0)
                    {
                        obj.ModifiedBy = userId;
                        obj.ModifiedOn = BL_Common.GetDatetime();
                    }
                    obj.ID = _vendor.id;
                    obj.LastName = _vendor.lastName;
                    obj.FirstName = _vendor.firstName;
                    obj.Phone = _vendor.phone ?? "";
                    obj.Email = _vendor.email;
                    obj.Terms = _vendor.terms;
                    obj.CreditLimit = _vendor.creditLimit;
                    obj.Balance = _vendor.balance;
                    obj.Misc = _vendor.misc;
                    obj.IsActive = true;
                    obj.CNIC = _vendor.cnic;
                    obj.NTN = _vendor.ntn;
                    if (obj.ID == 0)
                    {
                        obj.CreatedBy = userId;
                        obj.CreatedOn = BL_Common.GetDatetime();
                        var objcheck = db.Vendors.Where(x => x.LastName.ToLower() == _vendor.lastName.ToLower() && x.FirstName.ToLower() == _vendor.firstName.ToLower() && x.IsActive == true).FirstOrDefault();
                        if (objcheck != null)
                        {
                            return "Vendor Already Exists";
                        }
                        db.Vendors.Add(obj);
                    }


                    db.SaveChanges();
                    return "success";

                    // return "Insertion Failed";
                }
                catch { throw; }
            }
        }
        public static MYJSONTblCustom LoadVendorTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _vendorlist = LoadVendors(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Vendor> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _vendorlist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.lastName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.email.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   // || c.creditLimit > 0 && c.creditLimit.ToString().Contains(Param.sSearch.ToLower())
                    //|| c.terms != null && c.terms.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = _vendorlist;
            }
            Func<BO_Vendor, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                var Sortable6 = Convert.ToBoolean(Request["bSortable_6"]);
                IOrderedEnumerable<BO_Vendor> query = null;
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
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.terms : ""); }
                    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.creditLimit : 0); }
                    else if (iSortCol[i] == 6) { orderingFunction = (c => iSortCol[i] == 6 && Sortable6 ? c.balance : 0); }
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
                             id = c.id,
                             lastname = c.lastName,
                             firstName = c.firstName,
                             phone = c.phone,
                             email = c.email,
                             cnic= c.cnic ,
                             terms = c.terms,
                             creditLimit = c.creditLimit,
                             balance = c.balance

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _vendorlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Vendor> LoadVendors(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_Vendor> List = (from _Vendor in db_aa.Vendors
                                        where _Vendor.IsActive == true
                                        orderby _Vendor.ID descending
                                        select new BO_Vendor

                                        {
                                            id = _Vendor.ID,
                                            lastName = _Vendor.LastName ?? "",
                                            firstName = _Vendor.FirstName ?? "",
                                            phone = _Vendor.Phone ?? "",
                                            email = _Vendor.Email ?? "",
                                            cnic =_Vendor.CNIC ?? "",
                                            terms = _Vendor.Terms ?? "",
                                            creditLimit = _Vendor.CreditLimit ?? 0,
                                            balance = _Vendor.Balance ?? 0,
                                            misc = _Vendor.Misc ?? ""


                                        }).OrderByDescending(x => x.id).ToList();
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

                List<BO_Vendor> ListtoReturn = new List<BO_Vendor>();
                ListtoReturn = List;
                return ListtoReturn;
            }
        }

        public static string DeleteVendor(int vendorID,int userID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var vendTransaction = db.Acc_GL.Where(x => x.VendorId == vendorID && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (vendTransaction == null || vendTransaction .Count>0) { return "Transaction is Performed from Vendor, it can not be deleted "; }

                var obj = db.Vendors.Where(x => x.ID == vendorID).FirstOrDefault();

                if (obj != null && obj.ID > 0)
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
        public static string GetVendorBalance(int vendorID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var alpha = db.Report_AccountsPayable(vendorID).FirstOrDefault();
                if (alpha != null ) return alpha.AMOUNT.ToString();
                else return "0";

            }
        }

        public static BO_Vendor GetVendorByID(int vendorID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Vendors.Where(x => x.ID == vendorID).FirstOrDefault();
                BO_Vendor _vendor = new BussinessObject.BO_Vendor();
                if (obj != null && obj.ID > 0)
                {
                    _vendor.id = obj.ID;
                    _vendor.lastName = obj.LastName ?? "";
                    _vendor.firstName = obj.FirstName ?? "";
                    _vendor.phone = obj.Phone ?? "";
                    _vendor.email = obj.Email ?? "";
                    _vendor.creditLimit = obj.CreditLimit ?? 0;
                    _vendor.terms = obj.Terms ?? "";
                    _vendor.misc = obj.Misc ?? "";
                    _vendor.balance = obj.Balance ?? 0;
                    _vendor.cnic = obj.CNIC ?? "";
                    _vendor.ntn = obj.NTN ?? "";
                }
                return _vendor;

            }
        }
    }
}