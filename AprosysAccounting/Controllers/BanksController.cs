using System;
using System.Linq;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class BanksController : Appcode.BaseAprosysAccountingController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string InsertBankTransfer(string param)
        {
            var obj = BL_Common.Deserialize<BO_Banks>(param);
            obj.empID = UserAprosysAccounting.id;
            if (!Config.AllowEditDate)
            {
                obj.transDate = DateTime.Now;
            }
            return BL_Common.Serialize(BL_Banks.InsertBankTransfer(obj));
        }

        public string GetBanksList()
        {
         
            return BL_Common.Serialize(BL_Banks.GetBanksList());
        }

        public string GetCurrentCash()
        {
            var banks = BL_Banks.GetBanksList().Find(x => x.bankID == 11);
            
            return BL_Common.Serialize(banks);
        }

       
    }
}
