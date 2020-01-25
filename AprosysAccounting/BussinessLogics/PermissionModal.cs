using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System.Linq;

namespace AprosysAccounting.BussinessLogics
{
    public class PermissionModal
    {

        public static BO_Users GetUserDetail(int UserID)
        {
            using (AprosysAccountingEntities db =new AprosysAccountingEntities())
            {
                BO_Users user = new BO_Users();
                var obj = db.Users.Where(x => x.Id == UserID && x.IsActive == true).FirstOrDefault();
                if (obj != null)
                {
                    user.address = obj.Address;
                    user.email = obj.Email;
                    user.firstName = obj.FirstName;
                    user.lastName = obj.LastName;
                    user.id = obj.Id;
                    user.userId = obj.UserName;
                    user.adminRights = obj.AdminRights??false;
                }

                return user;
            }

        }

    }
}