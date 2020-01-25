using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using AprosysAccounting.Appcode;

namespace AprosysAccounting.Controllers
{
    public class ExpenseController : Appcode.BaseAprosysAccountingController
    {
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }
        public string SaveAdministrativeExpense(string name)
        {

            //  var Pvoucher = BL_Common.Deserialize<BO_COA>(pvoucher);
            return   BL_Common.Serialize(BL_Expense.SaveAdministrativeExpense(name));

        }
        public ActionResult LoadExpenseTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Expense.LoadExpenseTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string DeleteAdministrativeExpense(int id)
        {
            return BL_Common.Serialize((BL_Expense.DeleteAdministrativeExpense(id)));

        }
        public string GetExpenseByID(int id)
        {
            return BL_Common.Serialize((BL_Expense.GetExpenseByID(id)));

        }
  
        public string EDITAdministrativeExpense(int id,string name)
        {
            return BL_Common.Serialize(BL_Expense.EditAdministrativeExpense(id,name));
        }
    }
}