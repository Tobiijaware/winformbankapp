using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JBank.Lib.Core.Interface
{
    public interface IAccountRepository
    {
        void CreateAccount(Account acct, decimal initialdeposit);
        void Deposit(string accno, decimal amt, string type);
        string[] Withdraw(string accno, decimal amt, string type);
        string[] Transfer(string senderNumber, string receiverNumber, decimal amt, string type);
        decimal GetBalance(string accno);
        decimal GetDBBalance(string accno);
        string CreateDBAccount(string id , Account acct, decimal initialdeposit);
    }
}
