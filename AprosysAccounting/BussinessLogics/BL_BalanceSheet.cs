using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_BalanceSheet
    {
        public string DownloadBalanceSheet(DateTime dtStart, DateTime dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    BO_BalanceSheet RetainedEarning = GetRetainedEarning(dtStart, dtEnd, db);
                    List<BO_BalanceSheet> exp = GetBalanceSheetByDate(dtStart, dtEnd);
                    //var exp = db.GetBalanceSheetByDate(dtStart, dtEnd).ToList();
                    if (RetainedEarning != null)
                        exp.Add(RetainedEarning);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/BalanceSheet.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    string rptName = db.ReportConfigs.First(x => x.active == true && x.repoConfigID == 12).title;
                    ReportParameter[] parameters = new ReportParameter[6];
                    parameters[0] = new ReportParameter("TitleParam", rptName != "" ? rptName : "MOMAND ENTERPRISES");
                    parameters[1] = new ReportParameter("StartDate", dtStart.ToString("yyyy-MM-dd"));
                    parameters[2] = new ReportParameter("EndDate", dtEnd.ToString("yyyy-MM-dd"));
                    parameters[3] = new ReportParameter("TotalAssetsAmount", exp.Where(x => x.HeadAccount == 1).Sum(x => (x.DEBIT - x.Credit)).ToString());
                    parameters[4] = new ReportParameter("TotalLiabilities", exp.Where(x => x.HeadAccount == 2).Sum(x => (x.Credit - x.DEBIT)).ToString());
                    parameters[5] = new ReportParameter("TotalEquity", exp.Where(x => x.HeadAccount == 3).Sum(x => (x.Credit - x.DEBIT)).ToString());
                    rd.Value = exp;
                    rd.Name = "DataSet1";
                    rv.LocalReport.SetParameters(parameters);
                    rv.LocalReport.DataSources.Add(rd);
                    byte[] bt = rv.LocalReport.Render("PDF");
                    return WriteBytesToTempFile(bt, ".pdf");
                }
                catch (Exception ex)
                {
                    //Logger.Write("DownloadAuthReportByDate", ex.Message, ex.StackTrace, Logger.LogType.ErrorLog);
                    throw;
                }
            }
        }

        private List<BO_BalanceSheet> GetBalanceSheetByDate(DateTime dtStart, DateTime dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                List<int?> headAccounts = new List<int?>() { 1, 2, 3 };
                List<BO_BalanceSheet> obj = new List<BO_BalanceSheet>();
                BO_BalanceSheet bs;
                var exp = (from GL in db.Acc_GL
                           join CA in db.Acc_COA on GL.CoaId equals CA.CoaId
                           where headAccounts.Contains(CA.HeadAccount) &&
                           GL.IsActive == true && GL.ActivityTimestamp >= dtStart && GL.ActivityTimestamp <= dtEnd
                           select new
                           {
                               HeadAccount = CA.HeadAccount,
                               Debit = GL.Debit,
                               Credit = GL.Credit,
                               TreeName = CA.TreeName
                           }).GroupBy(x => x.HeadAccount).ToList();
                for (int i = 0; i < exp.Count; i++)
                {
                    var exp1 = exp[i].ToList();
                    var accountName = exp1.Select(x => new { TreeName = x.TreeName }).ToList();
                    decimal? debit = 0m;
                    decimal? credit = 0m;
                    string treeName;
                    int headAccount = 0;
                    foreach (var item in accountName.Distinct())
                    {
                        var dum = exp[i].Where(x => x.TreeName == item.TreeName);
                        debit = dum.Select(x => x.Debit).Sum();
                        credit = dum.Select(x => x.Credit).Sum();
                        headAccount = dum.FirstOrDefault(x => x.TreeName == item.TreeName).HeadAccount.Value;
                        treeName = item.TreeName;
                        bs = new BO_BalanceSheet() { HeadAccount = headAccount, TreeName = treeName, DEBIT = debit.Value, Credit = credit.Value };
                        obj.Add(bs);
                    }
                }
                return obj;
            }

        }

        private static BO_BalanceSheet GetRetainedEarning(DateTime dtStart, DateTime dtEnd, AprosysAccountingEntities db)
        {
            var TotalExpense = 0m;
            var TotalIncome = 0m;
            BO_BalanceSheet RetainedEarning = null;
            var iSList = db.Report_IncomeStatement(dtStart, dtEnd).ToList();
            foreach (var item in iSList)
            {
                TotalExpense += item.EXPAMOUNT.Value;
                TotalIncome += item.REVAMOUNT.Value;
            }
            if (TotalIncome > 0m)
            {
                RetainedEarning = new BO_BalanceSheet()
                {
                    HeadAccount = 3,
                    MainAccount = "Equity",
                    TreeName = "Retained Earning",
                    Credit = TotalIncome - TotalExpense,
                    DEBIT = 0
                };
            }

            return RetainedEarning;
        }

        string WriteBytesToTempFile(byte[] bt, string format)
        {
            string file = GetTempFileName(format);
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.OpenOrCreate))
            {
                fs.Write(bt, 0, bt.Length);
            }
            return file;
        }
        public static string GetTempFileName(string ext)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/temp/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ext);
        }
    }
}