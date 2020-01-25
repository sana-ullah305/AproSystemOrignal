using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_DailyCashFlow
    {

        public static List<BO_DailyCashFlow> GetDailyCashFlow(DateTime date)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var day = date.DayOfWeek.ToString();
                var toret = db.GetDailyCashFlow(date).ToList().Select(x => new BO_DailyCashFlow { AMOUNT = x.AMOUNT, GROUPID = x.GROUPID, ID = x.ID, NAME = x.NAME, Quantity = x.Quantity, TAX = x.TAX, TYPEID = x.TYPEID, TYPENAME = x.TYPENAME, UnitPrice = x.UnitPrice, WeekDay = day }).ToList();
                #region PartialPaymentFix
                var partials = BL_CreditSales.GetAllPartialPayments();
                var opening = toret.Where(x => x.TYPEID == 0 && x.GROUPID == 0).First();
                
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
                var lst_partialCreditInvoiceNo = toret.Where(x => x.TYPENAME == "Partial Credit Collected").ToList();
                toret = toret.Where(x => x.TYPENAME != "Partial Credit Collected").ToList();
                if (lst_partialCreditInvoiceNo != null && lst_partialCreditInvoiceNo.Count > 0)
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
                var toretShop = toret.Where(x => x.TYPENAME == "Shop").ToList(); if (toretShop == null || toretShop.Count == 0) { toret.Add(new BO_DailyCashFlow { AMOUNT = 0, TYPEID = 3, GROUPID = 1, TYPENAME = "Shop", NAME = "" }); }
                var toretMonthly = toret.Where(x => x.TYPENAME == "Monthly").ToList(); if (toretMonthly == null || toretMonthly.Count == 0) { toret.Add(new BO_DailyCashFlow { AMOUNT = 0, TYPEID = 2, GROUPID = 1, TYPENAME = "Monthly", NAME = "" }); }
             
                var salescount = toret.Where(x => x.TYPENAME == "Sales").ToList();
                if (salescount.Count >= 2)
                {
                    /*Delete Sales , when TypeNAme of Sales count > 2 , its an empty entry*/
                    toret.RemoveAll(x => x.TYPENAME == "Sales" && x.NAME == "");
                   
                }
                #endregion
                return toret;
            }

        }
    }
}