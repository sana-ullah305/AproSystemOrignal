using ApprosysAccDB;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_CustomerReport
    {
        public static List<BussinessObject.BO_CustomerReport> GetReportData(DateTime startDate, DateTime endDate, List<int> CustomerIDs)
        {
            List<BussinessObject.BO_CustomerReport> toret = new List<BussinessObject.BO_CustomerReport>();
            using (ApprosysAccDB.AprosysAccountingEntities db = new ApprosysAccDB.AprosysAccountingEntities())
            {
                bool FilterGL = false;
                List<int> custIDs = new List<int>();
                if (CustomerIDs != null && CustomerIDs.Count > 0)
                {
                    custIDs = CustomerIDs.ToList();
                    FilterGL = true;
                }
                else
                {
                    FilterGL = false;
                }
                var filteredList1 = db.Acc_GL.Where(x =>


                x.IsActive == true &&
                (
                  (x.TranTypeId == (int)Constants.TransactionTypes.Sales && x.CoaId == (int)Constants.COAID.TransactionParent)
                    ||
                    (x.TranTypeId == (int)Constants.TransactionTypes.PaymentAgainstCreditSales

                    && x.CoaId == (int)Constants.COAID.CASH)

                    ||
                    (x.TranTypeId == (int)Constants.TransactionTypes.Sales && (x.CoaId == (int)Constants.COAID.CASH)





                    )
                    )
                   );
                if (FilterGL)
                {
                    filteredList1 = filteredList1.Where(x => custIDs.Contains(x.CustId.Value));
                }
                //First Get Opening balance
                var lst = filteredList1.Where(x => x.ActivityTimestamp < startDate)
                .GroupBy(x => new { x.CoaId, x.GlId, x.CustId, x.SalesPersonId }).Select(x => new BussinessObject.BO_CustomerReport
                {
                    OpeningBalance = (x.Key.CoaId == 0) ? (x.Sum(y => y.Debit.Value)) : (0 - x.Sum(y => y.Debit.Value)),
                    CustomerID = x.Key.CustId.Value,
                    TransactionDate = startDate,
                    SalesPersonID = x.Key.SalesPersonId.Value

                }).ToList();

                toret.AddRange(lst);
                lst = filteredList1.Where(x => x.ActivityTimestamp > startDate && x.ActivityTimestamp < endDate).Select(x => new BussinessObject.BO_CustomerReport
                {
                    Sales = x.CoaId == 0 ? x.Debit.Value : 0,
                    Recieved = x.CoaId != 0 ? x.Debit.Value : 0,
                    Comment = x.Comments,
                    DocID = x.DocumentId,
                    InvoiceID = x.InvoiceNo,
                    CustomerID = x.CustId.Value,
                    TransactionDate = x.ActivityTimestamp.Value,
                    SalesPersonID = x.SalesPersonId.Value
                }).ToList();
                //List<int> salesPersonIds = filteredList1.Select(x => x.SalesPersonId.Value).Distinct().ToList();
                toret.AddRange(lst);
                var cust = db.Customers.ToDictionary(x => x.Id);
                var salesPersons = db.SalesPersons.ToDictionary(x => x.Id);
                foreach (var item in toret)
                {
                    var currentCust = cust[item.CustomerID];

                    item.CustomerName = currentCust.FirstName + " " + currentCust.LastName;
                    //item.SalesPersonID = currentCust.SalesPersonId.GetValueOrDefault();
                    if (salesPersons.ContainsKey(item.SalesPersonID))
                    {
                        item.SalesPersonName = salesPersons[item.SalesPersonID].FirstName + " " + salesPersons[item.SalesPersonID].LastName;
                    }
                }
                return toret;
            }
        }
    }
}