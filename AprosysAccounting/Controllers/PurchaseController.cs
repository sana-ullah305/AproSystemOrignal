using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class PurchaseController : Appcode.BaseAprosysAccountingController
    {
        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        public string SavePurchaseInvoice(string purchaseInvoice)
        {
            var pl = BL_Common.Deserialize<BO_PurchaseInvoice>(purchaseInvoice);
            pl.empID = UserAprosysAccounting.id;
            if (!Config.AllowEditDate)
            {
                pl.purchaseDate = DateTime.Now;
            }
            return BL_Common.Serialize( BL_PurchaseInvoice.SavePurchase(pl));
        }

        public ActionResult GetPurchasesList(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_PurchaseInvoice.GetPurchasesList(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }


        public string DeletePurchaseInvoice(string invoiceId)
        {
            return BL_Common.Serialize(BL_PurchaseInvoice.DeletePurchaseInvoice(invoiceId, UserAprosysAccounting.id));
        }

        public string EditPurchaseInvoice(string purchaseInvoice)
        {
            var pl = BL_Common.Deserialize<BO_PurchaseInvoice>(purchaseInvoice);
            pl.empID = UserAprosysAccounting.id;
            return BL_Common.Serialize(BL_PurchaseInvoice.EditPurchases(pl));
        }
        public ActionResult GetPurchaseByInvoiceId(string _purchaseInvoiceId)
        {
            return Json(BL_PurchaseInvoice.GetPurchaseByInvoiceId(_purchaseInvoiceId), JsonRequestBehavior.AllowGet);
        }
    }
}