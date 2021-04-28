using System;
using System.Collections.Generic;
using System.Text;

namespace JBank.Lib.Model
{
    public class Transact
    {
        public Transact(string id, string accno, decimal amt, string note, string type, DateTime date)
        {
            AccountNumber = accno;
            Amount = amt;
            Note = note;
            AccountType = type;
            TransactionDate = date;
            Id = id;
        }

        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string AccountType { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
