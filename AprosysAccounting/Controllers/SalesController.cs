using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class SalesController : Appcode.BaseAprosysAccountingController
    {
        // GET: Sales
        public ActionResult Index()
        {
            ViewBag.Taxes = BL_Common.Serialize(TaxesHelper.GetTaxes());
            return View();
        }

        public string SaveSaleInvoice(string paramSales)
        {
            var pl = BL_Common.Deserialize<BO_SaleInvoice>(paramSales);
            pl.empID = UserAprosysAccounting.id;
            if (!Config.AllowEditDate)
            {
                pl.saleDate = DateTime.Now;
            }
            return BL_Common.Serialize(BL_SaleInvoice.SaveSales(pl));
        }

        public string DeleteSaleInvoice(string invoiceId)
        {
            return BL_Common.Serialize(BL_SaleInvoice.DeleteSaleInvoice(invoiceId, UserAprosysAccounting.id));
        }

        public ActionResult GetSalesList(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_SaleInvoice.GetSalesList(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public string EditSaleInvoice(string paramSales)
        {
            var pl = BL_Common.Deserialize<BO_SaleInvoice>(paramSales);
            pl.empID = UserAprosysAccounting.id;
           
            return BL_Common.Serialize(BL_SaleInvoice.UpdateSaleInvoice(pl));
        }
        public ActionResult GetSaleByInvoiceId(string _saleInvoiceId)
        {
            return Json(BL_SaleInvoice.GetSaleByInvoiceId(_saleInvoiceId), JsonRequestBehavior.AllowGet);
        }

        public string GetSalesPersonList()
        {
            return BL_Common.Serialize(BL_SaleInvoice.GetSalesPersonList());
            
        }
    }
}