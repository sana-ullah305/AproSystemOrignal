namespace AprosysAccounting.BussinessLogics
{
    public static class Constants
    {

        public const string SESSION_USERKEY = "CurrentUser";
        public enum ExportType
        {
            PDF=1,
            EXCEL=2
        }
        public enum LoginResult
        {
            Success,
            UserUnknown,
            WrongPwd,
            UserDisabled,
            PwdExpired
        }

        public enum COAID
        {
            CASH=11,
            AccountRecievable=10,
            TransactionParent = 0
        }
        public enum TransactionTypes
        {
            Purchase = 1,
            Sales = 2,
            ReceiptVoucher=3,
            PaymentVoucher=4,
            SubscriptionDue = 5,
            BanksTransfer = 6,
            StockIn = 7,
            StockOut = 8,
            PaymentAgainstCreditSales=10,
            Equity=11,
            Bonus = 12
        }

        public enum PaymentMode
        {
            Cash=1,
            Cheque=2
        }
    }
}