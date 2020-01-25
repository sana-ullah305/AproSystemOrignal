using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;



namespace AprosysAccounting.BussinessLogics
{
    public class BL_ChequeManagement
    {
        public static MYJSONTblCustom LoadOutStandingChequeTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            Param.iSortingCols = 0;
            var outStandingChequelist = LoadOutStandingCheque(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_ChequeManagement> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = outStandingChequelist
                   .Where(
                    c => c.invoiceNo.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.bankName != null && c.bankName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //   || c.firstName.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //   || c.phone.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //   || c.email.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //   || c.CNIC.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //   || c.openingBalance > 0 && c.openingBalance.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    //|| c.balance > 0 && c.balance.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredCategories = outStandingChequelist;
            }

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             customerId = c.customerId,
                             customerName = c.customerName,
                             invoiceNo = c.invoiceNo,
                             bankName = c.bankName,
                             documentNo = c.documentNo,
                             chequeReceivedDate = c.chequeReceivedDate,
                             chequeReceivedAmount = c.chequeReceivedAmount


                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = outStandingChequelist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }

        /// <summary>
        /// Get List of Not Deposited Cheques
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public static List<BO_ChequeManagement> LoadOutStandingCheque(JQueryDataTableParamModel Param)
        {
            List<BO_ChequeManagement> lst = null;
            DateTime startdate=DateTime.MinValue;
            DateTime enddate = DateTime.MaxValue;
            if (Param != null)
            {
                 startdate = Param.Start_Date;
                 enddate = Param.End_Date.AddDays(1);
            }
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                lst = (from gl in db.Acc_GL
                       join cust in db.Customers on gl.CustId equals cust.Id
                       where gl.IsActive == true && cust.IsActive == true && gl.TranTypeId == 10 && 
                       gl.ActivityTimestamp > startdate  && gl.ActivityTimestamp <= enddate &&
                       gl.CoaId == 10 && gl.DocumentId != null
                       orderby gl.ActivityTimestamp descending
                       select new BO_ChequeManagement

                       {
                           customerId = cust.Id,
                           customerName = (cust.FirstName ?? "") + " " + (cust.LastName ?? ""),
                           invoiceNo = gl.InvoiceNo,
                          
                           documentNo = gl.DocumentId ?? "",
                           chequeReceivedDate = gl.ActivityTimestamp,
                           chequeReceivedAmount = gl.Credit ?? 0


                       }).OrderByDescending(x => x.chequeReceivedDate).ToList();

                //Get DocID of Deposited Cheques
                var depositedlst= db.Acc_GL.Where(x => x.TranTypeId == (int)Constants.TransactionTypes.BanksTransfer
                &&
                x.DocumentId != null && x.IsActive==true
                ).Select(x => x.DocumentId).ToList();
                lst.RemoveAll(x => depositedlst.Contains(x.documentNo));

                if (Param!=null && Param.SearchType != 0)
                {

                }


                return lst;
            }
        }

    }


}