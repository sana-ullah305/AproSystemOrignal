using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class ReportController : Appcode.BaseAprosysAccountingController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IncomeStatement()
        {
            return View();

        }
        public ActionResult WeeklyCashFlow()
        {
            return View();

        }

        public ActionResult SalesPerson()
        {
            ViewBag.SalesPersons = BL_SalesPersonManagement.LoadISalesPerson(new JQueryDataTableParamModel());
            return View();
        }
        public ActionResult ItemsSalesProfitList()
        {
            return View();

        }
        public ActionResult AccountsPayable()
        {
            return View();

        }
        public ActionResult AccountsReceivable()
        {
            return View();
        }
        public ActionResult TrialBalance()
        {
            return View();
        }

        public ActionResult PurchaseList()
        {
            return View();
        }

        public ActionResult PaymentVoucherList()
        {
            return View();
        }
        public ActionResult LoadCashFlowTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Reports.LoadCashFlowTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public String DownloadIncomeStatement(string dtStart, string dtEnd, bool? Preview)
        {
            try
            {
                DateTime dtimestart = Convert.ToDateTime(dtStart);
                DateTime dtimeend = Convert.ToDateTime(dtEnd).AddDays(1).AddSeconds(-1);
                //var isIncludeRepoligia = reporFilters.Riepilogo ?? true;
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadIncomeStatement(dtimestart, dtimeend);

                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_IncomeStatement.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }

        public String DownloadWeeklyCashFlow(string dtStart, bool? Preview)
        {
            try
            {
                DateTime dtimestart = Convert.ToDateTime(dtStart);
                // DateTime dtimeend = Convert.ToDateTime(dtEnd);
                //var isIncludeRepoligia = reporFilters.Riepilogo ?? true;
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadWeeklyCashFlow(dtimestart);

                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_WeeklyCashFlow.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }


        public String DownloadBankStatement(int bankID,string dtStart, string dtEnd, bool? Preview)
        {
            try
            {
                DateTime dtimestart = new DateTime(2000, 01, 01);
                if (!string.IsNullOrEmpty( dtStart))
                {
                  dtimestart=   Convert.ToDateTime(dtStart);
                }
                DateTime dtimeend = new DateTime(3000, 01, 01);
                if ( !string.IsNullOrEmpty( dtEnd))
                {
                  dtimeend=   Convert.ToDateTime(dtEnd);
                }
                // DateTime dtimeend = Convert.ToDateTime(dtEnd);
                //var isIncludeRepoligia = reporFilters.Riepilogo ?? true;
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadBankStatement(bankID, dtimestart,dtimeend);

                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "BankTransactions.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }
        public String DownloadMonthlySubscription(int year, int month, int type,bool? Preview, int SubsType)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                Constants.ExportType exportType = Constants.ExportType.EXCEL;
                string contentType = "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet";
                if (Preview == true)
                {
                    exportType = Constants.ExportType.PDF;
                    contentType= "application/pdf";
                }
                string file = RH.DownloadMonthlySubscription(year, month, type,exportType, SubsType);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_MonthlySubscription.xlsx"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType =contentType ;
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadReceiptVoucher(string voucher, int typeID, bool? Preview)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadReceiptVoucher(voucher, typeID);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_ReceiptVoucher.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }

        public String DownloadSaleInvoice(string invoiceId, bool? Preview)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadSaleInvoice(invoiceId);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_SaleInvoice.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {

                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }



        public String DownloadItemWiseSales(string dtStart, string dtEnd, bool? Preview)
        {
            try
            {
                DateTime dateStart = Convert.ToDateTime(dtStart);
                DateTime dateEnd = Convert.ToDateTime(dtEnd).AddDays(1).AddSeconds(-1);
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadItemWiseSales(dateStart, dateEnd);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_ItemSalesProfit.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }


        public String DownloadBalanceSheet(string dtStart, string dtEnd, bool? Preview)
        {
            try
            {
                dtStart = new DateTime(2019, 01, 01).ToString("yyyy-MM-dd");
                DateTime dateStart = Convert.ToDateTime(dtStart);
                DateTime dateEnd = Convert.ToDateTime(dtEnd).AddDays(1).AddSeconds(-1);
                BL_BalanceSheet BS = new BL_BalanceSheet();
                string file = BS.DownloadBalanceSheet(dateStart, dateEnd);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_BalanceSheet.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadAccountsPayable(int vendorID, bool? Preview)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadAccountsPayable(vendorID);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_AccountsPayable.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadAccountsReceivable(int? custID, int typeID, bool? Preview)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadAccountsReceivable(custID.GetValueOrDefault(), typeID);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_AccountsReceivable.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public ActionResult GETtrialBalance(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Reports.LoadTrialBalance(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public String DownloadTrialBalance(DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                dtEnd = dtEnd.AddDays(1).AddSeconds(-1);
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadTrialBalance(dtStart, dtEnd);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_TrialBalance.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadDailyCashFlow(string date)
        {
            try
            {
                DateTime datex = Convert.ToDateTime(date);
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadDailyCashFlow(datex);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", date + "_DailyCashFlow.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }

        public String DownloadPurchaseList(int vendorID, int itemID, bool? Preview, DateTime? dtStart, DateTime? dtEnd)
        {
            try
            {
                dtEnd = dtEnd.HasValue ? dtEnd.Value.AddDays(1).AddSeconds(-1) : dtEnd;
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadPurchaseList(vendorID, itemID, dtStart, dtEnd);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_PurchaseList.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadPaymentVoucherList(bool? Preview, DateTime? dtStart, DateTime? dtEnd)
        {
            try
            {
                dtEnd = dtEnd.HasValue ? dtEnd.Value.AddDays(1).AddSeconds(-1) : dtEnd;
                ReportHelper RH = new ReportHelper();
                string file = RH.DownloadPaymentVoucherList(dtStart, dtEnd);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "dPaymentVoucher.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadCreditSaleInvoice(string invoiceId, bool? Preview)
        {
            try
            {
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadSaleInvoice(invoiceId);
               // string file = RH.DownloadCreditSaleInvoice(invoiceId);
                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_SaleInvoice.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {

                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public String DownloadSalesPersonReport(string dtStart, string dtEnd,string salesPersonCSV, bool? Preview)
        {
            try
            {
                DateTime dtimestart = Convert.ToDateTime(dtStart);
                DateTime dtimeend = Convert.ToDateTime(dtEnd).AddDays(1).AddSeconds(-1);
                List<int> SalesPersonIDS = null;
                if(salesPersonCSV!=null && salesPersonCSV.Length > 0)
                {
                    SalesPersonIDS = salesPersonCSV.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                //var isIncludeRepoligia = reporFilters.Riepilogo ?? true;
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadSalesPersonReport(dtimestart, dtimeend,SalesPersonIDS,new System.Collections.Generic.List<int>());

                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_SalesPersonReport.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }



        //sanaullah
        public String DownloadCustomerReport(string customerIds,string dtStart, string dtEnd, bool? Preview)
        {
            try
            {
                DateTime dtimestart = Convert.ToDateTime(dtStart);
                DateTime dtimeend = Convert.ToDateTime(dtEnd).AddDays(1).AddSeconds(-1);
                List<int> customerIDS = null;
                if (customerIds != null && customerIds.Length > 0)
                {
                    customerIDS = customerIds.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                ReportHelper RH = new ReportHelper();
                //string file = RH.DownloadAuthReportByDate(new DateTime(2011,1,1), new DateTime(2020,1,1));
                string file = RH.DownloadCustomerReport(dtimestart, dtimeend, customerIDS);

                if (file == "1") { return "download"; }
                FileInfo newFile = new FileInfo(file);
                // offername+"_"+_item.display_name+"_"+offerDate+".pdf"
                string attachment = string.Format("attachment; filename=\"{0}\"", DateTime.Now.Ticks + "_CustomerReport.pdf"); //string.Format("attachment; filename={0}", "Report.pdf");
                Response.Clear();
                if (Preview != true)
                {
                    Response.AddHeader("content-disposition", attachment);
                }
                Response.ContentType = "application/pdf";
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
                Response.WriteFile(newFile.FullName);
                Response.Flush();
                newFile.Delete();
                Response.End();
                return "download";
            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);
            }

        }

        public ActionResult CustomerReports()
        {

            ViewBag.Customers = BL_Customer.LoadICustomers(new JQueryDataTableParamModel());
            return View();
        }
    }
}