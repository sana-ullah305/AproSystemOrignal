using AprosysAccounting.BussinessLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class BanksConfigurationController : Appcode.BaseAprosysAccountingController
    {
        // GET: BanksConfiguration
        public ActionResult Index()
        {
            return View();
        }

        public string RenameBank(int bankID, string name)
        {
            return BL_Common.Serialize(BL_BanksConfiguration.RenameBank(bankID,name));
        }

        public string AddNewBankAccoount(string bankName, string bankAccount, decimal? depositAmmount)
        {
            return BL_Common.Serialize(BL_BanksConfiguration.AddNewBankAccoount(bankName, bankAccount, depositAmmount));
        }


        public string DeleteBankAccount(int CoaId)
        {
            return BL_Common.Serialize(BL_BanksConfiguration.DeleteBankAccount(CoaId));
        }
    }
}