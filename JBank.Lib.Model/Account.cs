using System;

namespace JBank.Lib.Model
{
    public class Account
    {
        public Account(string acctno, string id, decimal initialdeposit, string type, DateTime date)
        {
            AccountNumber = acctno;
            Balance = initialdeposit;
            OwnerId = id;
            Type = type;
            MinBalance = AccountType.GetMinBalance(type);
            DateCreated = date;


        }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string OwnerId { get; set; }
        public virtual string Type { get; set; }
        public virtual decimal MinBalance { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
