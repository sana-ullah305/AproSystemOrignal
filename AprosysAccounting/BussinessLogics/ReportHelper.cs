using ApprosysAccDB;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.BussinessLogics
{
    public class WeeklyClass
    {
        public decimal AmountaddedInSales = 0;
        public decimal AmountaddedInShop = 0;
        public decimal AmountSubFromCredit = 0;
    }
    public class ReportHelper
    {
        public string DownloadIncomeStatement(DateTime dtStart, DateTime dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_IncomeStatement(dtStart, dtEnd);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/IncomeStatement.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
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

        public string DownloadWeeklyCashFlow(DateTime dtStart)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    //                    var exp = db.Report_WeeklyCashFlow(dtStart).ToList();

                    var toret = db.Report_WeeklyCashFlow(dtStart).ToList().Select(x => new BO_WeeklyCashFlow { TYPE = x.TYPE, TYPED = x.TYPED, Sort = x.Sort, TOTAL = x.TOTAL, SUN = x.SUN, MON = x.MON, TUE = x.TUE, WED = x.WED, THU = x.THU, FRI = x.FRI, SAT = x.SAT, SunDate = x.SunDate, MonDate = x.MonDate, TueDate = x.TueDate, WedDate = x.WedDate, ThuDate = x.ThuDate, FriDate = x.FriDate, SatDate = x.SatDate }).ToList();
                    var partials = BL_CreditSales.GetAllPartialPayments();
                    List<WeeklyClass> lst = new List<WeeklyClass>();

                    var lst_WeeklySalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault();
                    var ldst_WeeklyShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault();
                    var ldst_WeeklyPartialCredit = toret.Where(x => x.TYPE == "Cash In Partial Credit").FirstOrDefault();



                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.MonDate).FirstOrDefault(), ldst_WeeklyPartialCredit.MON ?? 0, lst_WeeklySalesAmount.MON ?? 0, ldst_WeeklyShopAmount.MON ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.TueDate).FirstOrDefault(), ldst_WeeklyPartialCredit.TUE ?? 0, lst_WeeklySalesAmount.TUE ?? 0, ldst_WeeklyShopAmount.TUE ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.WedDate).FirstOrDefault(), ldst_WeeklyPartialCredit.WED ?? 0, lst_WeeklySalesAmount.WED ?? 0, ldst_WeeklyShopAmount.WED ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.ThuDate).FirstOrDefault(), ldst_WeeklyPartialCredit.THU ?? 0, lst_WeeklySalesAmount.THU ?? 0, ldst_WeeklyShopAmount.THU ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.FriDate).FirstOrDefault(), ldst_WeeklyPartialCredit.FRI ?? 0, lst_WeeklySalesAmount.FRI ?? 0, ldst_WeeklyShopAmount.FRI ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.SatDate).FirstOrDefault(), ldst_WeeklyPartialCredit.SAT ?? 0, lst_WeeklySalesAmount.SAT ?? 0, ldst_WeeklyShopAmount.SAT ?? 0));
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.SunDate).FirstOrDefault(), ldst_WeeklyPartialCredit.SUN ?? 0, lst_WeeklySalesAmount.SUN ?? 0, ldst_WeeklyShopAmount.SUN ?? 0));

                    #region No Need to Write too long Code
                    /*
                    decimal MonPartialCreditleft = toret.Select(x => new { amount = x.MON, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal monSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().MON ?? 0;
                    decimal monShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().MON ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.MonDate).FirstOrDefault(), MonPartialCreditleft, monSalesAmount, monShopAmount));

                    decimal TuePartialCreditleft = toret.Select(x => new { amount = x.TUE, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal tueSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().TUE ?? 0;
                    decimal tueShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().TUE ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.TueDate).FirstOrDefault(), TuePartialCreditleft, tueSalesAmount, tueShopAmount));

                    decimal WedPartialCreditleft = toret.Select(x => new { amount = x.WED, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal wedSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().WED ?? 0;
                    decimal wedShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().WED ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.WedDate).FirstOrDefault(), WedPartialCreditleft, wedSalesAmount, wedShopAmount));

                    decimal ThursPartialCreditleft = toret.Select(x => new { amount = x.THU, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal thursSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().THU ?? 0;
                    decimal thursShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().THU ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.ThuDate).FirstOrDefault(), ThursPartialCreditleft, thursSalesAmount, thursShopAmount));

                    decimal friPartialCreditleft = toret.Select(x => new { amount = x.FRI, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal friSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().FRI??0;
                    decimal friShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().FRI ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.FriDate).FirstOrDefault(), friPartialCreditleft, friSalesAmount, friShopAmount));

                    decimal SatPartialCreditleft = toret.Select(x => new { amount = x.SAT, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal satSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().SAT ?? 0;
                    decimal satShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().SAT ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.SatDate).FirstOrDefault(), SatPartialCreditleft, satSalesAmount, satShopAmount));

                    decimal SunPartialCreditleft = toret.Select(x => new { amount = x.SUN, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit").FirstOrDefault().amount ?? 0;
                    decimal sunSalesAmount = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().SUN ?? 0;
                    decimal sunShopAmount = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault().SUN ?? 0;
                    lst.Add(GetWeeklySalesAmount(toret, partials, toret.Select(x => x.SunDate).FirstOrDefault(), SunPartialCreditleft,sunSalesAmount, sunShopAmount));
                    */
                    #endregion


                    //var toretmon = toret.Where(x => new { x.FriDate ? DateTime.Now; }).ToList();
                    //toret.RemoveAt(2);
                    //toret.RemoveAt(4);
                    var cashInSalesEntry =toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault();
                    cashInSalesEntry.MON = lst[0].AmountaddedInSales;cashInSalesEntry.TUE = lst[1].AmountaddedInSales;cashInSalesEntry.WED = lst[3].AmountaddedInSales;cashInSalesEntry.THU = lst[3].AmountaddedInSales; cashInSalesEntry.FRI= lst[4].AmountaddedInSales; cashInSalesEntry.SAT = lst[5].AmountaddedInSales; cashInSalesEntry.SUN = lst[6].AmountaddedInSales;cashInSalesEntry.TOTAL = lst.Sum(x => x.AmountaddedInSales);

                    var cashInShopEntry = toret.Where(x => x.TYPE == "Cash In Shop").FirstOrDefault();
                    cashInShopEntry.MON = lst[0].AmountaddedInShop; cashInShopEntry.TUE = lst[1].AmountaddedInShop; cashInShopEntry.WED = lst[3].AmountaddedInShop; cashInShopEntry.THU = lst[3].AmountaddedInShop; cashInShopEntry.FRI = lst[4].AmountaddedInShop; cashInShopEntry.SAT = lst[5].AmountaddedInShop; cashInShopEntry.SUN = lst[6].AmountaddedInShop; cashInShopEntry.TOTAL = lst.Sum(x => x.AmountaddedInShop);


                    var RemainingCashInCreditEntry = toret.Where(x => x.TYPE == "Cash In Partial Credit").FirstOrDefault();
                    RemainingCashInCreditEntry.MON = lst[0].AmountSubFromCredit; RemainingCashInCreditEntry.TUE = lst[1].AmountSubFromCredit; RemainingCashInCreditEntry.WED = lst[2].AmountSubFromCredit; RemainingCashInCreditEntry.THU = lst[3].AmountSubFromCredit; RemainingCashInCreditEntry.FRI = lst[4].AmountSubFromCredit; RemainingCashInCreditEntry.SAT = lst[5].AmountSubFromCredit; RemainingCashInCreditEntry.SUN = lst[6].AmountSubFromCredit; RemainingCashInCreditEntry.TOTAL = lst.Sum(x => x.AmountSubFromCredit);
                    //toret.Insert (2,new BO_WeeklyCashFlow { TYPE = "Cash In Sales", MON = lst[0].AmountaddedInSales, TUE = lst[1].AmountaddedInSales, WED = lst[2].AmountaddedInSales, THU = lst[3].AmountaddedInSales, FRI = lst[4].AmountaddedInSales, SAT = lst[5].AmountaddedInSales, SUN = lst[6].AmountaddedInSales,TOTAL = lst.Sum(x=>x.AmountaddedInSales), TYPED = 1, Sort = 3 });
                    //toret.Add(new BO_WeeklyCashFlow { TYPE = "Cash In Partial Credit", MON = lst[0].AmountSubFromCredit, TUE = lst[1].AmountSubFromCredit, WED = lst[2].AmountSubFromCredit, THU = lst[3].AmountSubFromCredit, FRI = lst[4].AmountSubFromCredit, SAT = lst[5].AmountSubFromCredit, SUN = lst[6].AmountSubFromCredit, TOTAL = lst.Sum(x => x.AmountSubFromCredit), TYPED = 1, Sort = 5 });
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/WeeklyCashFlow.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = toret;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
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

        public WeeklyClass GetWeeklySalesAmount(List<BO_WeeklyCashFlow> toret, List<BO_PartialPayment> partials, DateTime? dt_fri, decimal DayPartialCredit ,decimal DaySalesAmount, decimal DayShopAmount)
        {
            //   var dt_fri = toret.Select(x => x.FriDate).FirstOrDefault();
            decimal AmountAddedINSales = 0;
            decimal AmountAddedINShop = 0;
            //  decimal fridayPartialCreditleft = 0;
            var lst_fri_partialCreditInvoiceNo = partials.Where(x => x.CreatedDate.Date >= dt_fri.Value.Date && x.CreatedDate.Date <= dt_fri.Value.Date).ToList();
            if (lst_fri_partialCreditInvoiceNo != null && lst_fri_partialCreditInvoiceNo.Count > 0)
            {
                //fridayPartialCreditleft = toret.Select(x => new { amount = x.FRI, type = x.TYPE }).Where(x => x.type == "Cash In Partial Credit" &&).FirstOrDefault().amount ?? 0;
               // AmountAddedINSales = fridayPartialCreditleft;
                var lst_FriInvoice = lst_fri_partialCreditInvoiceNo.GroupBy(x => x.InvoiceNum).Select(y => new { invoiceno = y.First().InvoiceNum, Amount = y.Sum(z => z.Amount) }).ToList();
                if (lst_FriInvoice != null && lst_FriInvoice.Count > 0)
                {
                    for (int i = 0; i < lst_FriInvoice.Count; i++)
                    {
                        #region 
                        string invoiceno = lst_FriInvoice[i].invoiceno;
                        decimal amount = lst_FriInvoice[i].Amount;
                        var lst_unitPrice = new List<Acc_GL>();
                        using (AprosysAccountingEntities db = new AprosysAccountingEntities())
                        {
                            //lst_unitPrice = db.Acc_GL.Where(x => x.InvoiceNo == invoiceno && x.CoaId == 14).OrderBy(x => x.UnitPrice).ToList();
                             lst_unitPrice = (from Acc in db.Acc_GL
                                                 join COA in db.Acc_COA on Acc.CoaId equals COA.CoaId
                                                 where Acc.IsActive == true && COA.IsActive == true && (COA.PId == 101 || Acc.CoaId == 14)
                                                 && Acc.InvoiceNo == invoiceno
                                                 select Acc).OrderBy(x => x.UnitPrice).ToList();
                        }
                        if (lst_unitPrice != null && lst_unitPrice.Count > 0)
                        {
                            for (int j = 0; j < lst_unitPrice.Count; j++)/*Unit Prices in Ascending order*/
                            {
                                if (amount >= lst_unitPrice[j].UnitPrice) /*Paid Amount is Greater then Unit price*/
                                {
                                    //toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = 1});

                                    for (int k = 1; k <= lst_unitPrice[j].Quantity; k++) /*Multiple Quantity of Item*/
                                    {
                                        if (amount >= lst_unitPrice[j].UnitPrice)
                                        {
                                            //toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = 1 });
                                            amount = amount - lst_unitPrice[j].UnitPrice ?? 0;
                                            if (lst_unitPrice[j].CoaId == 14) { DayPartialCredit = DayPartialCredit - lst_unitPrice[j].UnitPrice ?? 0; AmountAddedINSales = AmountAddedINSales + lst_unitPrice[j].UnitPrice ?? 0; }
                                            else { DayPartialCredit = DayPartialCredit - lst_unitPrice[j].UnitPrice ?? 0; AmountAddedINShop = AmountAddedINShop + lst_unitPrice[j].UnitPrice ?? 0; }
                                            // ReceivedCreditAmount = ReceivedCreditAmount - lst_unitPrice[j].UnitPrice ?? 0;
                                           

                                        }
                                        //if (k == lst_unitPrice[j].Quantity)
                                        //    toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice * k ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = k });
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
               /* AmountAddedINSales = AmountAddedINSales - fridayPartialCreditleft;*/
              //  var fridaySalesData = toret.Where(x => x.TYPE == "Cash In Sales").FirstOrDefault().MON;
                // decimal? SalesCashAmount = fridaySalesData.FRI;
                //decimal? SalesCashAmount = toret.Select(x => new { amount = x.FRI, type = x.TYPE }).Where(x => x.type == "Cash In Sales").FirstOrDefault().amount;
              

                //decimal? fridaytotal = fridaySalesData.SUN + fridaySalesData.MON + fridaySalesData.TUE + fridaySalesData.WED + fridaySalesData.THU + AmountAddedINSales + fridaySalesData.SAT;
                // toret.RemoveAt(2);
                //toret.Add(new BO_WeeklyCashFlow { TYPE = "Cash In Sales", MON = 0, TUE = 0, WED = 0, THU = 0, FRI = AmountAddedINSales, SAT = 0, SUN = 0, TOTAL = fridaytotal, TYPED = 1, Sort = 3 });

            }
            AmountAddedINSales = AmountAddedINSales + DaySalesAmount;
            AmountAddedINShop = AmountAddedINShop + DayShopAmount;
            WeeklyClass obj = new BussinessLogics.WeeklyClass();
            obj.AmountaddedInSales = AmountAddedINSales;
            obj.AmountSubFromCredit = DayPartialCredit;
            obj.AmountaddedInShop = AmountAddedINShop;
            return obj;
        }
        public string DownloadBankStatement(int CoaID, DateTime dtStart, DateTime dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    dtStart = dtStart.Date;
                    dtEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 23, 59, 59, 999);
                    var exp = db.Report_BankStatement3(CoaID, dtStart, dtEnd);

                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/BankStatement.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "DataSet1";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(new ReportParameter("beginDate", dtStart.ToString("yyyy-MM-dd")));
                    rv.LocalReport.SetParameters(new ReportParameter("endDate", dtEnd.ToString("yyyy-MM-dd")));
                    //e.DataSources.Add(rd);
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

        public string DownloadDailyCashFlow(DateTime date)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    /*var exp = db.GetDailyCashFlow(date).OrderBy(x => x.TYPEID).ToList();
                    #region PartialPaymentFix
                    var partials = BL_CreditSales.GetAllPartialPayments();
                    var opening = exp.Where(x => x.TYPEID == 0 && x.GROUPID == 0).First();
                    opening.AMOUNT += partials.Where(x => x.CreatedDate < date).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
                    var partial = partials.Where(x => x.CreatedDate.Date == date.Date).GroupBy(x => x.InvoiceNum);
                    partial.ToList().ForEach(x =>
                    exp.Insert(exp.Count - 2, new GetDailyCashFlow_Result
                    {
                        AMOUNT = x.Sum(y => y.Amount),
                        TYPEID = 10,
                        GROUPID = 1,
                        NAME = x.Key,
                        TYPENAME = "Partial Credit"
                    }
                    ));


                    #endregion
                    */
                    var toret = db.GetDailyCashFlow(date).ToList().Select(x => new BO_DailyCashFlow { AMOUNT = x.AMOUNT, GROUPID = x.GROUPID, ID = x.ID, NAME = x.NAME, Quantity = x.Quantity, TAX = x.TAX, TYPEID = x.TYPEID, TYPENAME = x.TYPENAME, UnitPrice = x.UnitPrice }).ToList();
                    #region PartialPaymentFix
                    var partials = BL_CreditSales.GetAllPartialPayments();
                    var opening = toret.Where(x => x.TYPEID == 0 && x.GROUPID == 0).First();
                    //opening.AMOUNT += partials.Where(x => x.CreatedDate < date).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
                    partials.Where(x => x.CreatedDate.Date == date.Date).GroupBy(x => x.InvoiceNum).ToList().ForEach(x =>
                    {
                        toret.Add(
                        new BO_DailyCashFlow
                        {
                            AMOUNT = x.Sum(y => y.Amount),
                            TYPEID = 10,
                            GROUPID = 1,
                            TYPENAME = "Partial Credit Collected",
                            NAME = x.Key,


                        });
                    });
                    List<BO_DailyCashFlow> lst_partialCreditInvoiceNo = toret.Where(x => x.TYPENAME == "Partial Credit Collected").ToList();
                    toret = toret.Where(x => x.TYPENAME != "Partial Credit Collected").ToList();
                    if (lst_partialCreditInvoiceNo != null && lst_partialCreditInvoiceNo.Count>0)
                    {
                        decimal ReceivedCreditAmount = lst_partialCreditInvoiceNo.Select(x => x.AMOUNT).Sum();
                        for (int i = 0; i < lst_partialCreditInvoiceNo.Count; i++)
                        {
                            string invoiceno = lst_partialCreditInvoiceNo[i].NAME;
                            decimal amount = lst_partialCreditInvoiceNo[i].AMOUNT;
                            //var lst_unitPrice = db.Acc_GL.Where(x => x.InvoiceNo == invoiceno && x.CoaId == 14).OrderBy(x => x.UnitPrice).ToList();
                            var lst_unitPrice = (from Acc in db.Acc_GL
                                                 join COA in db.Acc_COA on Acc.CoaId equals COA.CoaId
                                                 where Acc.IsActive == true && COA.IsActive == true && (COA.PId == 101 || Acc.CoaId == 14)
                                                 && Acc.InvoiceNo == invoiceno
                                                 select Acc).OrderBy(x => x.UnitPrice).ToList();


                            if (lst_unitPrice != null && lst_unitPrice.Count > 0)
                            {
                                for (int j = 0; j < lst_unitPrice.Count; j++)/*Unit Prices in Ascending order*/
                                {
                                    if (amount >= lst_unitPrice[j].UnitPrice) /*Paid Amount is Greater then Unit price*/
                                    {
                                        //toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = 1});

                                        for (int k = 1; k <= lst_unitPrice[j].Quantity; k++) /*Multiple Quantity of Item*/
                                        {
                                            if (amount >= lst_unitPrice[j].UnitPrice)
                                            {
                                                if (lst_unitPrice[j].CoaId == 14)
                                                {
                                                    toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = 1 });
                                                }
                                                else
                                                {
                                                    toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice ?? 0, TYPEID = 3, GROUPID = 1, TYPENAME = "Shop", NAME = BL_Service.GetServiceByID(lst_unitPrice[j].CoaId ?? 0).name, UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = 1 });
                                                }
                                                amount = amount - lst_unitPrice[j].UnitPrice ?? 0;
                                                ReceivedCreditAmount = ReceivedCreditAmount - lst_unitPrice[j].UnitPrice ?? 0;

                                            }
                                            //if (k == lst_unitPrice[j].Quantity)
                                            //    toret.Add(new BO_DailyCashFlow { AMOUNT = lst_unitPrice[j].UnitPrice * k ?? 0, TYPEID = 1, GROUPID = 1, TYPENAME = "Sales", NAME = BL_Item.GetItemNameByItemId(lst_unitPrice[j].ItemId ?? 0), UnitPrice = lst_unitPrice[j].UnitPrice ?? 0, TAX = (lst_unitPrice[j].TaxPercent ?? 0) / (lst_unitPrice[j].Quantity ?? 0), Quantity = k });
                                        }
                                    }
                                }
                            }
                            toret.Add(new BO_DailyCashFlow { AMOUNT = amount, TYPEID = 10, GROUPID = 1, TYPENAME = "Partial Credit Collected", NAME = invoiceno, });
                        }
                        decimal check = ReceivedCreditAmount;
                    }
                    //toret = toret.Where(x => x.TYPENAME != "Partial Credit Collected").ToList();

                    toret = toret.GroupBy(x => x.NAME).Select(y => new BO_DailyCashFlow { WeekDay = y.First().WeekDay, NAME = y.First().NAME, TYPENAME = y.First().TYPENAME, GROUPID = y.First().GROUPID, TYPEID = y.First().TYPEID, AMOUNT = y.Sum(z => z.AMOUNT), TAX = y.Sum(z => z.TAX), Quantity = y.Sum(z => z.Quantity), UnitPrice = y.First().UnitPrice }).ToList();
                    List<BO_DailyCashFlow> toretShop = toret.Where(x => x.TYPENAME == "Shop").ToList(); if (toretShop == null || toretShop.Count == 0) { toret.Add(new BO_DailyCashFlow { AMOUNT = 0, TYPEID = 3, GROUPID = 1, TYPENAME = "Shop", NAME = "" }); }
                    List<BO_DailyCashFlow> toretMonthly = toret.Where(x => x.TYPENAME == "Monthly").ToList(); if (toretMonthly == null || toretMonthly.Count == 0) { toret.Add(new BO_DailyCashFlow { AMOUNT = 0, TYPEID = 2, GROUPID = 1, TYPENAME = "Monthly", NAME = "" }); }

                    List<BO_DailyCashFlow> salescount = toret.Where(x => x.TYPENAME == "Sales").ToList();
                    if (salescount.Count >= 2)
                    {
                        /*Delete Sales , when TypeNAme of Sales count > 2 , its an empty entry*/
                        toret.RemoveAll(x => x.TYPENAME == "Sales" && x.NAME == "");

                    }
                    #endregion
                    
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/DailyCashFlow.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = toret;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
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

        public string DownloadMonthlySubscription(int year, int month, int type, Constants.ExportType exportType,int SubsType)
        {
           // return "";
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {

                    var exp =  db.Report_MonthlySubscription(year,SubsType).ToList();
                   
                    switch (type)
                    {
                        case 1:
                            exp = exp.Where(x => month == 1 ? x.JANDUE <= 0 : month == 2 ? x.FEBPAID >= x.FEBDUE : month == 3 ? x.MARPAID >= x.MARDUE : month == 4 ? x.APRPAID >= x.APRDUE : month == 5 ? x.MAYPAID >= x.MAYDUE : month == 6 ? x.JUNPAID >= x.JUNDUE : month == 7 ? x.JULPAID >= x.JULDUE : month == 8 ? x.AUGPAID >= x.AUGDUE : month == 9 ? x.SEPPAID >= x.SEPDUE : month == 10 ? x.OCTPAID >= x.OCTDUE : month == 11 ? x.NOVPAID >= x.NOVDUE : month == 12 ? x.DECPAID >= x.DECDUE : true).ToList();
                            //exp = exp.Where(x => x.TOTDUE <= 0 && x.TOTPAID>0).ToList();
                            break;
                        case 2:
                            exp = exp.Where(x => month == 1 ? x.JANDUE > 0 : month == 2 ? x.FEBPAID < x.FEBDUE : month == 3 ? x.MARPAID < x.MARDUE : month == 4 ? x.APRPAID < x.APRDUE : month == 5 ? x.MAYPAID < x.MAYDUE : month == 6 ? x.JUNPAID < x.JUNDUE : month == 7 ? x.JULPAID < x.JULDUE : month == 8 ? x.AUGPAID < x.AUGDUE : month == 9 ? x.SEPPAID < x.SEPDUE : month == 10 ? x.OCTPAID < x.OCTDUE : month == 11 ? x.NOVPAID < x.NOVDUE : month == 12 ? x.DECPAID < x.DECDUE : true).ToList();
                            //exp = exp.Where(x => x.TOTDUE > 0 ).ToList();
                            break;
                        default:
                            break;
                    }
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/MonthlySubscription.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(new ReportParameter("month", month.ToString()));
                    //e.DataSources.Add(rd);
                    string format;
                    string extension;
                    switch (exportType)
                    {
                        case Constants.ExportType.PDF:
                            format = "PDF";
                            extension = ".pdf";
                            break;
                        case Constants.ExportType.EXCEL:
                            format = "EXCELOPENXML";
                            extension = ".xlsx";
                            break;
                        default:
                            throw new NotImplementedException();
                            break;
                    }
                    byte[] bt = rv.LocalReport.Render(format);
                    return WriteBytesToTempFile(bt, extension);
                }
                catch (Exception ex)
                {
                    //Logger.Write("DownloadAuthReportByDate", ex.Message, ex.StackTrace, Logger.LogType.ErrorLog);
                    throw;
                }
            }
            
        }

        public string DownloadReceiptVoucher(string voucherNo, int typeID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.GetReceiptVoucherListByVoucherNO(voucherNo, typeID);
                    int? salesPerson = db.Acc_GL.Where(x => x.InvoiceNo == voucherNo
                      &&
                      x.TranTypeId == (int)Constants.TransactionTypes.Sales


                    ).Select(x => x.SalesPersonId).FirstOrDefault();
                    string salesPersonName = "";
                    if (salesPerson.HasValue)
                    {
                        var ssp = BL_SalesPersonManagement.GetSalesPersonByID(salesPerson.Value);
                        salesPersonName = ssp.firstName + " " + ssp.lastName;
                    }
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/ReceiptVoucher.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    var paremeters = AppendSaleInvoiceReportParams().ToList();
                    paremeters.Add(new ReportParameter("SalesPerson", salesPersonName));
                    rv.LocalReport.SetParameters(paremeters);
                    
                    
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
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

        public string DownloadSaleInvoice(string voucherNo)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_GetSaleInvoice(voucherNo).ToList();//.OrderByDescending(x => x.TYPED == "TOTALS");
                    exp.RemoveAll(x => x.TYPED == "TOTALS");
                    #region PartialCredit Adjustment
                    var partials = BL_CreditSales.GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                    if (exp.Count > 0)
                    {

                        decimal TotalPaid = exp.Sum(x => x.AMOUNT.GetValueOrDefault());
                        decimal InvoiceAmount = TotalPaid;
                        decimal TotalBalance = 0;
                        if (exp.First().IsSalesCredit.GetValueOrDefault() == 1)
                        {
                            TotalPaid = 0;
                            if (partials.ContainsKey(voucherNo))
                            {
                                TotalPaid = partials[voucherNo].Sum(x => x.Amount);

                            }
                            TotalBalance = InvoiceAmount - TotalPaid;
                        }
                        foreach (var item in exp)
                        {
                            item.PAID = TotalPaid;
                            item.BALANCE = TotalBalance;
                            item.QuantityInLitre = item.QuantityInLitre != null ? item.QuantityInLitre : 0;
                        }
                    }

                    #endregion
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/SaleInvoice.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(AppendSaleInvoiceReportParams());

                    int? salesPerson = db.Acc_GL.Where(x => x.InvoiceNo == voucherNo
                      &&
                      x.TranTypeId == (int)Constants.TransactionTypes.Sales


                    ).Select(x => x.SalesPersonId).FirstOrDefault();
                    string salesPersonName = "";
                    if (salesPerson.HasValue)
                    {
                        var ssp = BL_SalesPersonManagement.GetSalesPersonByID(salesPerson.Value);
                        //salesPersonName = ssp.firstName + " " + ssp.lastName;
                        salesPersonName =  ssp.firstName + " " + ssp.lastName;
                    }
                    rv.LocalReport.SetParameters(new ReportParameter("SalesPerson", salesPersonName));

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
        public string DownloadCreditSaleInvoice(string voucherNo)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_GetSaleInvoice(voucherNo).ToList();//.OrderByDescending(x => x.TYPED == "TOTALS");
                    exp.RemoveAll(x => x.TYPED == "TOTALS");
                    #region PartialCredit Adjustment
                    var partials = BL_CreditSales.GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                    if (exp.Count > 0)
                    {

                        decimal TotalPaid = exp.Sum(x => x.AMOUNT.GetValueOrDefault());
                        decimal InvoiceAmount = TotalPaid;
                        decimal TotalBalance = 0;
                        if (exp.First().IsSalesCredit.GetValueOrDefault() == 1)
                        {
                            TotalPaid = 0;
                            if (partials.ContainsKey(voucherNo))
                            {
                                TotalPaid = partials[voucherNo].Sum(x => x.Amount);

                            }
                            TotalBalance = InvoiceAmount - TotalPaid;
                        }
                        foreach (var item in exp)
                        {
                            item.PAID = TotalPaid;
                            item.BALANCE = TotalBalance;
                        }
                    }

                    #endregion
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/CreditSaleInvoice.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(AppendSaleInvoiceReportParams());
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
        public string DownloadAuthReportByDate(string voucherNo)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_GetSaleInvoice(voucherNo).ToList();//.OrderByDescending(x => x.TYPED == "TOTALS");
                    exp.RemoveAll(x => x.TYPED == "TOTALS");
                    #region PartialCredit Adjustment
                    var partials = BL_CreditSales.GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                    if (exp.Count > 0)
                    {

                        decimal TotalPaid = exp.Sum(x => x.AMOUNT.GetValueOrDefault());
                        decimal InvoiceAmount = TotalPaid;
                        decimal TotalBalance = 0;
                        if (exp.First().IsSalesCredit.GetValueOrDefault() == 1)
                        {
                            TotalPaid = 0;
                            if (partials.ContainsKey(voucherNo))
                            {
                                TotalPaid = partials[voucherNo].Sum(x => x.Amount);

                            }
                            TotalBalance = InvoiceAmount - TotalPaid;
                        }
                        foreach (var item in exp)
                        {
                            item.PAID = TotalPaid;
                            item.BALANCE = TotalBalance;
                        }
                    }

                    #endregion
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/SaleInvoice.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(AppendSaleInvoiceReportParams());
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

        public ReportParameter[] AppendSaleInvoiceReportParams()
        {
            string bullet = "\u2022";
            string lineBreak = "\n";
            ReportParameter[] parameters = new ReportParameter[4];
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var param = db.ReportConfigs.Where(x => x.active == true).FirstOrDefault();
                parameters[0] = new ReportParameter("AddressParam", param != null ? param.address : "2440 Amsterdam Avenue" + lineBreak + "New York, NY 10033" + lineBreak + "Phone: (917) 521 - 1100 Fax: (917) - 521 - 1114");
                parameters[1] = new ReportParameter("TitleParam", param != null ? param.title : "INFO-TECH DELTA COMPUTER, INC");
                parameters[2] = new ReportParameter("DetailTitleParam", param != null ? param.detailTitle : "Thank  you for business!");
                if (param == null)
                {
                    StringBuilder detailStr = new StringBuilder();
                    detailStr.Append(bullet + "All new computer parts are covered by manufacture warranty" + lineBreak);
                    detailStr.Append(bullet + "6 months warranty on used laptops" + lineBreak);
                    detailStr.Append(bullet + "2 weeks warranty on software repair" + lineBreak);
                    detailStr.Append(bullet + "3 month warranty on hardware repair" + lineBreak);
                    detailStr.Append(bullet + "Not responsible for data loss" + lineBreak);
                    detailStr.Append(bullet + "Any abuse or misuse will void warranty" + lineBreak);
                    detailStr.Append(bullet + "All returns and refunds are subject to 15% restocking fee" + lineBreak);
                    detailStr.Append(bullet + "No returns or refunds after 14 days of purchase" + lineBreak);
                    detailStr.Append(bullet + "Not responsible for any items left over a period greater than 60 days" + lineBreak);
                    detailStr.Append(bullet + "DCA License Numbers: 2033382, 2033280, 2033612" + lineBreak);
                    parameters[3] = new ReportParameter("DetailParam", detailStr.ToString());
                }
                else
                {
                    parameters[3] = new ReportParameter("DetailParam", param.detail);
                }
            }
            return parameters;
        }


        public string DownloadItemWiseSales(DateTime dtStart, DateTime dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_ItemWiseSale(dtStart, dtEnd);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/ItemWiseSales.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    string rptName = db.ReportConfigs.First(x => x.active == true && x.repoConfigID == 12).title;
                    ReportParameter parameters = new ReportParameter("TitleParam", rptName != "" ? rptName : "MOMAND ENTERPRISES");
                    rd.Value = exp;
                    rd.Name = "Ds";
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

        public string DownloadAccountsPayable(int vendorID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_AccountsPayable(vendorID);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/AccountsPayable.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
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

        public string DownloadAccountsReceivable(int custID, int typeID)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_AccountsReceivable(custID, typeID).ToList();

                    //Adjustment of Partial Credit
                   
                    exp.RemoveAll(x => x.AMOUNT.GetValueOrDefault() == 0);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/AccountsReceivable.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
                    rv.LocalReport.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(new ReportParameter("TypeID", typeID.ToString()));
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

        public string DownloadTrialBalance(DateTime startdate, DateTime enddate)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.Report_TrialBalance(startdate, enddate);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/TrialBalance.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "ds_trialBalance";
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

        public string DownloadPurchaseList(int vendorID, int itemID, DateTime? dtStart, DateTime? dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.GetPurchaseList(vendorID, itemID, dtStart, dtEnd);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/PurchaseList.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
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

        public string DownloadPaymentVoucherList(DateTime? dtStart, DateTime? dtEnd)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    var exp = db.GetPaymentVoucherList("", dtStart, dtEnd);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/PaymentVoucher.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "Ds";
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

        public string DownloadSalesPersonReport(DateTime dtStart, DateTime dtEnd,List<int> SalesPersons,List<int> Customers)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    
                    var exp = BL_SalesPersonReport.GetReportData(dtStart, dtEnd, SalesPersons, Customers);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/SalesPersonReport.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "DataSet1";
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(new ReportParameter (
                        "StartDate",
                        dtStart.ToString("yyyy-MM-dd")

                    ));

                    rv.LocalReport.SetParameters(new ReportParameter(
                        "EndDate",
                        dtEnd.ToString("yyyy-MM-dd")

                    ));
                    string SalesPersonNames = "ALL";
                    if(SalesPersons!=null && SalesPersons.Count > 0)
                    {
                        SalesPersonNames =string.Join(",", BL_SalesPersonManagement.GetSalesPersonNamesByID(SalesPersons));
                    }
                    rv.LocalReport.SetParameters(new ReportParameter(
                        "SalesPersons",
                       SalesPersonNames

                    ));
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

        public string DownloadCustomerReport(DateTime dtStart, DateTime dtEnd, List<int> Customers)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {

                    var exp = BL_CustomerReport.GetReportData(dtStart, dtEnd, Customers);
                    Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.LocalReport.ReportPath = "Reports/Definitions/CustomersReport.rdlc";
                    ReportDataSource rd = new ReportDataSource();
                    rd.Value = exp;
                    rd.Name = "DataSet1";
                    rv.LocalReport.DataSources.Add(rd);
                    //e.DataSources.Add(rd);
                    rv.LocalReport.SetParameters(new ReportParameter(
                        "StartDate",
                        dtStart.ToString("yyyy-MM-dd")

                    ));

                    rv.LocalReport.SetParameters(new ReportParameter(
                        "EndDate",
                        dtEnd.ToString("yyyy-MM-dd")

                    ));
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
        
    }
}