using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class StockInController : Appcode.BaseAprosysAccountingController
    {
        // GET: StockIn
        public ActionResult Index()
        {
            return View();
        }

        public string SaveStockIn(string stockIn)
        {
            var si = BussinessLogics.BL_Common.Deserialize<BO_StockIn>(stockIn);
            si.empID = UserAprosysAccounting.id;
            return BussinessLogics.BL_Common.Serialize(BL_StockIn.SaveStockIn(si));
        }
    }
}