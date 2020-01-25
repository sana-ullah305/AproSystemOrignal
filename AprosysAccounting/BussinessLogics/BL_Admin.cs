using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_Admin
    {
        public static string ResetGL()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.ResetGL();

                //if (obj != null && obj.Id > 0)
                //{
                //    obj.ModifiedBy = 1;
                //    obj.ModifiedOn = BL_Common.GetDatetime();

                //}
                //obj.IsActive = false;

                ////        db.Items.Add(obj);
                //db.SaveChanges();
                return "success";
            }
        }
    }

}