using AprosysAccounting.Appcode;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
namespace AprosysAccounting.Controllers
{
    public class CustomerController : BaseAprosysAccountingController
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCustomerList(int typeID)
        {
           
            var List = BL_Customer.GetCustomerList(typeID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerListBySalesPersonID(int salesPersonID)
        {

            var List = BL_Customer.GetCustomerListBySalesPersonID(salesPersonID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerListByType(int typeID)
        {

            var List = BL_Customer.GetCustomerListByType(typeID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubCustomerList(int typeID)
        {

            var List = BL_Customer.GetSubCustomerList(typeID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public string GetCustomerBalance(int custId)
        {
            var pl = BL_Customer.GetCustomerBalance(custId);
            return BL_Common.Serialize(pl);
        }

        public ActionResult LoadCustomerTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Customer.LoadCustomerTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string SaveCustomer(string paramcustomer)
        {
            
            var _customer = BL_Common.Deserialize<BO_Customers>(paramcustomer);
            return BL_Common.Serialize((BL_Customer.SaveCustomer(_customer, UserAprosysAccounting.id)));
         
        }
        public string DeleteCustomer(int customerID)
        {
            var pl = BL_Customer.DeleteCustomer(customerID, UserAprosysAccounting.id); 
            return BL_Common.Serialize(pl);
        }

        public ActionResult GetCustomerByID(int customerID)
        {
            BO_Customers obj = new BussinessObject.BO_Customers();
            obj = BL_Customer.GetCustomerByID(customerID);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}