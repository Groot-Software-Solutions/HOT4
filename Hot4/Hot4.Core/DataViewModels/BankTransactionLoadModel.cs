namespace Hot4.Core.DataViewModels
{
    public class BankTransactionLoadModel
    {
        private DateTime _TransactionDate;
        public DateTime TransactionDate
        {
            get { return _TransactionDate; }
            set { _TransactionDate = value; }
        }

        private string _TransactionType;
        public string TransactionType
        {
            get { return _TransactionType; }
            set { _TransactionType = value; }
        }

        private string _TransactionDescription;
        public string TransactionDescription
        {
            get { return _TransactionDescription; }
            set { _TransactionDescription = value; }
        }

        private string _Branch;
        public string Branch
        {
            get { return _Branch; }
            set { _Branch = value; }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        private decimal _Balance;
        public decimal Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }

        private string _Identifier;
        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }

        private string _BankRef;
        public string BankReference
        {
            get { return _BankRef; }
            set { _BankRef = value; }
        }
    }
}
