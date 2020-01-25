using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class StockOutController : Appcode.BaseAprosysAccountingController
    {
        // GET: StockOut
        public ActionResult Index()
        {
            return View();
        }

        public string SaveStockOut(string stockOut)
        {
            var si = BussinessLogics.BL_Common.Deserialize<BO_StockIn>(stockOut);
            si.empID = UserAprosysAccounting.id;
            return BussinessLogics.BL_Common.Serialize(BL_StockOut.SaveStockOut(si));
        }
    }
}