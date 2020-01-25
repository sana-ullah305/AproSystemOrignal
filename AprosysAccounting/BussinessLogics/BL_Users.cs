using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;


namespace AprosysAccounting.BussinessLogics
{
    public class BL_Users
    {
        public static string SaveUser(BO_Users _user,int UserID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    //var objcheck = db.Users.Where(x => x.UserName.ToLower() == _user.userId.ToLower()).FirstOrDefault();
                    //if (objcheck != null)
                    //{
                    //    return "User Already Exists";
                    //}

                    var obj = _user.id == 0 ? new ApprosysAccDB.User() : db.Users.Where(x => x.Id == _user.id).FirstOrDefault();
                    if (_user.id > 0)
                    {
                        var checkCust = db.Users.Where(x => x.UserName.ToLower() == _user.userId.ToLower() && x.Id != _user.id && x.IsActive == true).FirstOrDefault();
                        if (checkCust != null)
                        {
                            return "UserID Already Exists";
                        }
                    }
                    if (obj != null && obj.Id > 0)
                    {
                        obj.ModifiedBy = UserID;
                        obj.ModifiedOn = BL_Common.GetDatetime();

                    }
                    obj.Id = _user.id;

                    obj.LastName = _user.lastName ?? "";
                    obj.FirstName = _user.firstName ?? "";
                    obj.UserName = _user.userId;
                    obj.Password = _user.password;
                    obj.Phone = _user.phone;
                    obj.Email = _user.email;
                    obj.IsActive = true;
                    obj.Address = _user.address;
                    
                    
                    obj.IsActive = true;
                    obj.AdminRights = _user.adminRights;
                    if (obj.Id ==0)
                    {
                        obj.CreatedBy = UserID;
                        obj.CreatedOn = BL_Common.GetDatetime();
                        var objcheck = db.Users.Where(x => x.UserName.ToLower() == _user.userId.ToLower() && x.Id != _user.id && x.IsActive == true).FirstOrDefault();
                        if (objcheck != null)
                        {
                            return "UserID Already Exists";
                        }
                        db.Users.Add(obj);
                    }
                      
                    db.SaveChanges();
                    return "success";

                    // return "Insertion Failed";
                }
                catch { throw; }
            }
        }

        public static MYJSONTblCustom LoadUserTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _userlist = LoadUsers(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Users> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _userlist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.lastName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.userId.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.email.ToString().ToLower().Contains(Param.sSearch.ToLower())
                     );
            }
            else
            {
                filteredCategories = _userlist;
            }
            Func<BO_Users, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_Users> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.lastName : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.firstName : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.userId : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.email : ""); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.phone : ""); }

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
                             lastName = c.lastName,
                             firstName = c.firstName,
                             userId = c.userId,
                             password = c.password,
                             email = c.email,
                             phone = c.phone
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _userlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }


        private static IList<BO_Users> LoadUsers(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_Users> List = (from _user in db_aa.Users
                                       where _user.IsActive == true
                                       orderby _user.Id descending
                                       select new BO_Users

                                       {
                                           id = _user.Id,
                                           lastName = _user.LastName ?? "",
                                           firstName = _user.FirstName ?? "",
                                           userId = _user.UserName ?? "",
                                           password = _user.Password ?? "",
                                           phone = _user.Phone ?? "",
                                           email = _user.Email ?? "",
                                           address = _user.Address ?? ""



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

                List<BO_Users> ListtoReturn = new List<BO_Users>();
                ListtoReturn = List;
                return ListtoReturn;
            }
        }
        public static string DeleteUser(int _id,int userID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                var vendTransaction = db.Acc_GL.Where(x => x.UserId == _id && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (vendTransaction == null || vendTransaction.Count > 0) { return "Transaction is Performed from User, it can not be deleted "; }

                var obj = db.Users.Where(x => x.Id == _id).FirstOrDefault();

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

        public static BO_Users GetUserByID(int userId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                BO_Users _user = new BussinessObject.BO_Users();
                if (obj != null && !string.IsNullOrEmpty(obj.UserName))
                {
                    _user.id = obj.Id;
                    _user.lastName = obj.LastName ?? "";
                    _user.firstName = obj.FirstName ?? "";
                    _user.phone = obj.Phone ?? "";
                    _user.email = obj.Email ?? "";
                    _user.userId = obj.UserName ;
                    _user.password = obj.Password ;
                    _user.address = obj.Address ?? "";
                    _user.adminRights = obj.AdminRights ?? false;

                }
                return _user;

            }
        }


        public static BO_Users Login(HttpSessionStateBase session, string UserName, string Password, out Constants.LoginResult result)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                BO_Users user = new BO_Users();

                var obj = db.Users.Where(x => x.UserName == UserName && x.Password == Password && x.IsActive == true).FirstOrDefault();
                if (obj != null)
                {
                    result = Constants.LoginResult.Success;
                    user.firstName = obj.FirstName;
                    user.lastName = obj.LastName;
                    user.id = obj.Id;
                    user.phone = obj.Phone;
                    user.userId = obj.UserName;
                   // user.adminRights = obj.AdminRights??false;
                }
                else
                {
                    result = Constants.LoginResult.WrongPwd;
                }

                return user;

            }
        }
    }
}