using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_CreditSales
    {

        public static MYJSONTblCustom GetUnPaidCreditSalesList(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var creditSalesList = GetUnPaidCreditSales(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_CreditSales> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = creditSalesList
                   .Where(
                      c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.customerName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   || c.netAmount > 0 && c.netAmount.ToString().ToLower().Contains(Param.sSearch.ToLower())
                   );
            }
            else
            {
                filteredCategories = creditSalesList;
            }
            //Func<BO_Sales, dynamic> orderingFunction = null;
            //int iSortColums = Convert.ToInt32(Param.iSortingCols);

            //if (iSortColums > 0)
            //{
            //    var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
            //    var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
            //    var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
            //    var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
            //    var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
            //    var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
            //    IOrderedEnumerable<BO_Sales> query = null;
            //    int[] iSortCol = new int[iSortColums];
            //    string[] sSortDir = new string[iSortColums];
            //    for (int _i = 0; _i < iSortCol.Length; _i++)
            //    {
            //        int i = _i;
            //        iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
            //        if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.invoiceNo : ""); }
            //        else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.sellDate : DateTime.MinValue); }
            //        else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.customerName : ""); }
            //        else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.netAmount : 0); }
            //        else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.paid : 0); }
            //        else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.balance : 0); }
            //        sSortDir[i] = Request["sSortDir_" + i + ""]; // asc or desc
            //        //  var sortDirection = Request["sSortDir_0"];
            //        if (sSortDir[i] == "asc")
            //        {
            //            query = (i == 0) ? filteredCategories.OrderBy(orderingFunction) : query.ThenBy(orderingFunction);
            //        }
            //        else
            //        {
            //            query = (i == 0) ? filteredCategories.OrderByDescending(orderingFunction) : query.ThenByDescending(orderingFunction);
            //        }
            //        filteredCategories = query;

            //    }

            //}

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             invoiceNo = c.invoiceNo,
                             sellDate = c.sellDate,
                             customerName = c.customerName,
                             netAmount = c.netAmount,
                             customerID = c.customerID,

                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = creditSalesList.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }

        public static List<BO_CreditSales> GetUnPaidCreditSales(JQueryDataTableParamModel Param)
        {

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<BO_CreditSales> lst_CreditSales = new List<BO_CreditSales>();
                //Param.Start_Date = BL_Common.GetDatetime().AddDays(-7);
                Param.End_Date = Param.End_Date.AddDays(1);
                var lst = db_aa.Credit_GetUnpaidCreditSales(Param.Start_Date, Param.End_Date).ToList();
                var partiallyPaid = GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                BO_CreditSales obj;
                if (lst != null && lst.Count > 0)
                {
                    foreach (var _sales in lst.ToList())
                    {
                        obj = new BO_CreditSales();
                        obj.invoiceNo = _sales.InvoiceNo;
                        obj.sellDate = _sales.SalesDate.Value;
                        obj.customerName = _sales.CustomerName;
                        obj.netAmount = _sales.Amount ?? 0;
                        obj.customerID = _sales.CustId;
                        if (partiallyPaid.ContainsKey(obj.invoiceNo))
                        {
                            obj.netAmount = obj.netAmount - partiallyPaid[obj.invoiceNo].Sum(x => x.Amount);
                        }
                        if (obj.netAmount > 0)
                        {
                            lst_CreditSales.Add(obj);
                        }
                    }

                }
                return lst_CreditSales.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
            }
            //   return lst_CreditSales;
        }

        public static List<BO_UnpaidCreditCustomer> GetUnPaidCreditCustomerList()
        {

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var lst = db_aa.Credit_GetUnPaidCustomers();
                List<BO_UnpaidCreditCustomer> lst_cust = new List<BO_UnpaidCreditCustomer>();
                BO_UnpaidCreditCustomer obj;
                foreach (var _creditCustomer in lst.ToList())
                {
                    if (GetUnPaidCreditCustomerInvoices(_creditCustomer.Id).Count > 0)
                    {
                        obj = new BO_UnpaidCreditCustomer();
                        obj.Id = _creditCustomer.Id;
                        obj.Name = _creditCustomer.CustomerName;
                        lst_cust.Add(obj);
                    }
                }


                return lst_cust.OrderBy(x => x.Name).ToList();
            }
        }

        public static List<BO_UnpaidCreditCustomerInvoice> GetUnPaidCreditCustomerInvoices(int custID)
        {

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                var lst = db_aa.Credit_GetUnPaidCustomersInvoice(custID);
                List<BO_UnpaidCreditCustomerInvoice> lst_cust = new List<BO_UnpaidCreditCustomerInvoice>();
                BO_UnpaidCreditCustomerInvoice obj;
                var salePersons = GetSalePersonNameById(custID);
                var partialPayments = GetAllPartialPayments().GroupBy(x => x.InvoiceNum).ToDictionary(x => x.Key);
                foreach (var _creditCustomer in lst.ToList())
                {
                    obj = new BO_UnpaidCreditCustomerInvoice();
                    obj.invoiceNo = _creditCustomer.InvoiceNo;
                    obj.salePerson = salePersons[custID];
                    obj.sellDate = _creditCustomer.SalesDate.Value;
                    obj.netAmount = _creditCustomer.Amount ?? 0;
                    obj.paidAmount = 0;
                    obj.invoiceAmount = obj.netAmount;
                    if (partialPayments.ContainsKey(_creditCustomer.InvoiceNo))
                    {
                        obj.paidAmount = partialPayments[_creditCustomer.InvoiceNo].Sum(x => x.Amount);
                        obj.netAmount = obj.invoiceAmount - obj.paidAmount;
                    }
                    if (obj.invoiceAmount > obj.paidAmount)
                    {
                        lst_cust.Add(obj);
                    }
                }


                return lst_cust.OrderByDescending(x => (Convert.ToInt32(x.invoiceNo.Remove(0, 4)))).ToList();
            }
        }

        public static string UpdateUnPaidCreditSales(BO_CreditSalesUpdate obj, int userId)
        {

            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {

                //var objcheck = db.Customers.Where(x => x.LastName.ToLower() == _customer.lastName.ToLower() && x.FirstName.ToLower() == _customer.firstName.ToLower()).FirstOrDefault();
                //if (objcheck != null)
                //{
                //    return "Customer Already Exists";
                //}

                //Check there is no Previous Payment
                var parialPayments = GetAllPartialPayments().Where(x => x.InvoiceNum == obj.invoiceNo).ToList();
                decimal? AlreadyPaid = null;
                if (parialPayments.Count > 0)
                {
                    AlreadyPaid = parialPayments.Sum(x => x.Amount);
                }
                if (AlreadyPaid.HasValue && !obj.Amount.HasValue)
                {
                    throw new Exception("Full Payment not possible once partial is made");
                }
                if (obj.PaymentMode == Constants.PaymentMode.Cash)
                {
                    obj.BankName = null;
                    obj.DocumentID = null;
                }
                else if (string.IsNullOrEmpty(obj.DocumentID) || string.IsNullOrEmpty(obj.BankName))
                {
                    throw new Exception("Bank Or Document ID can  not be empty in Cheque Payment");

                }
                else
                {
                    obj.DocumentID = obj.BankName + " - " + obj.DocumentID;
                }
                var dbobj = db.Acc_GL.Where(x => x.InvoiceNo == obj.invoiceNo && x.CustId == obj.customerID).ToList();
                if (obj.Amount.HasValue)
                {
                    var unPaid = GetUnPaidCreditCustomerInvoices(obj.customerID).Where(x => x.invoiceNo == obj.invoiceNo).First();
                    if (obj.Amount > unPaid.netAmount)
                    {
                        throw new Exception("Paid Amount Can not be more than Owed");
                    }
                    else
                    {
                        InsertPartialPayments(obj.Amount.Value, userId, obj.invoiceNo, obj.DocumentID, obj.Comment, obj.creditPaidDate);
                        return obj.invoiceNo;
                    }
                }

                return obj.invoiceNo;
            }
        }
        public static void DeletePartialPayments(int TranID, int Invoice, int User)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var refTrans = db.Acc_GL.Where(x => (x.GlId == TranID || x.TranId == TranID) && x.IsActive == true).ToList();
                foreach (var item in refTrans)
                {
                    item.IsActive = false;
                    item.ModifiedBy = User;
                    item.ModifiedDate = DateTime.Now;
                }
                db.SaveChanges();
            }




        }
        public static void DeletePartialPayments(AprosysAccountingEntities db, string Invoice, int User)
        {

            var refTrans = db.Acc_GL.Where(x => x.InvoiceNo == Invoice && x.IsActive == true && x.TranTypeId == (int)Constants.TransactionTypes.PaymentAgainstCreditSales).ToList();
            foreach (var item in refTrans)
            {
                item.IsActive = false;
                item.ModifiedBy = User;
                item.ModifiedDate = DateTime.Now;
            }
            db.SaveChanges();


        }

        public static void InsertPartialPayments(decimal Amount, int User, string Invoice, string DocID, string Comment, DateTime ActivityTime)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var trans = db.Database.BeginTransaction();
                try
                {
                    //Get Customer From Original Invoice
                    var refTrans = db.Acc_GL.Where(x => x.InvoiceNo == Invoice && x.IsActive == true && x.TranTypeId == 2 && x.CustId.HasValue).FirstOrDefault();
                    var sellerID = db.Customers.Where(x => x.Id == refTrans.CustId).Select(x => x.SalesPersonId).FirstOrDefault();
                    //We make a Dummy Entry for reference
                    var GLParent = new Acc_GL() { CoaId = 0, UserId = User, CustId = refTrans.CustId, ActivityTimestamp = ActivityTime, Debit = Amount, Credit = Amount, TranTypeId = (int)Constants.TransactionTypes.PaymentAgainstCreditSales, InvoiceNo = Invoice, IsActive = true, CreatedBy = User, CreatedDate = DateTime.Now, ModifiedBy = User, ModifiedDate = DateTime.Now, SalesPersonId = sellerID };
                    GLParent.DocumentId = DocID;
                    GLParent.Comments = Comment;
                    db.Acc_GL.Add(GLParent);
                    db.SaveChanges();
                    var GLCash = new Acc_GL()
                    {
                        CoaId = (int)Constants.COAID.CASH,
                        DocumentId = DocID,
                        Comments = Comment,
                        TranId = GLParent.GlId,
                        UserId = User,
                        CustId = refTrans.CustId,
                        ActivityTimestamp = ActivityTime,
                        Debit = Amount,
                        Credit = 0,
                        TranTypeId = (int)Constants.TransactionTypes.PaymentAgainstCreditSales,
                        InvoiceNo = Invoice,
                        IsActive = true,
                        CreatedBy = User,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = User,
                        ModifiedDate = DateTime.Now,
                        SalesPersonId = sellerID
                    };
                    var GL_Recv = new Acc_GL()
                    {
                        CoaId = (int)Constants.COAID.AccountRecievable,
                        DocumentId = DocID,
                        Comments = Comment,
                        TranId = GLParent.GlId,
                        UserId = User,
                        CustId = refTrans.CustId,
                        ActivityTimestamp = ActivityTime,
                        Debit = 0,
                        Credit = Amount,
                        TranTypeId = (int)Constants.TransactionTypes.PaymentAgainstCreditSales,
                        InvoiceNo = Invoice,
                        IsActive = true,
                        CreatedBy = User,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = User,
                        ModifiedDate = DateTime.Now,
                        SalesPersonId = sellerID
                    };
                    db.Acc_GL.Add(GLCash);
                    db.Acc_GL.Add(GL_Recv);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    throw;
                }


            }
        }
        public static List<BO_PartialPayment> GetAllPartialPayments()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.Acc_GL.Where(x => x.TranTypeId == (int)Constants.TransactionTypes.PaymentAgainstCreditSales

                &&
                x.CoaId.Value == 11
                &&
                x.IsActive == true
                ).Select(x => new BO_PartialPayment
                {
                    InvoiceNum = x.InvoiceNo,
                    Amount = x.Debit.Value,
                    CreatedDate = x.ActivityTimestamp.Value
                }).ToList(); ;
            }

        }

        public static Dictionary<int, string> GetSalePersonNameById(int customerId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var dta = (from c in db.Customers
                           join s in db.SalesPersons on c.SalesPersonId equals s.Id
                           where c.Id == customerId
                           select new
                           {
                               customerId = c.Id,
                               SalePersonName = s.FirstName + " " + s.LastName
                           }).ToDictionary(k => k.customerId, v => v.SalePersonName);
                return dta;
            }
        }
    }
}