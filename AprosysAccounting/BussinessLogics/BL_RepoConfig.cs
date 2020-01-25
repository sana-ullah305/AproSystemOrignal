using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Linq;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_RepoConfig
    {
        public static string SaveItem(BO_RepoConfig obj)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                db.ReportConfigs.ToList().ForEach(x => x.active = false);
                db.ReportConfigs.Add(new ReportConfig() { title = obj.title, address = obj.address, detailTitle = obj.detailTitle, detail = obj.detail, active = true, createdBy = obj.empID.ToString(), createdDate = DateTime.Now, modifiedBy = obj.empID.ToString(), modifiedDate = DateTime.Now });
                db.SaveChanges();
                return "success";
            }
        }

        public static BO_RepoConfig GetReportConfig()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.ReportConfigs.Where(x => x.active == true).FirstOrDefault();
                BO_RepoConfig _repConfig = new BussinessObject.BO_RepoConfig();
                if (obj != null)
                {
                    //_repConfig.empID = obj.createdBy;
                    _repConfig.title = obj.title ?? "";
                    _repConfig.address = obj.address ?? "";
                    _repConfig.detailTitle = obj.detailTitle ?? "";
                    _repConfig.detail = obj.detail ?? "";

                }
                return _repConfig;

            }
        }

    }
}