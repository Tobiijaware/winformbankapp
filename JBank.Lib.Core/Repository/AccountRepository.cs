using JBank.Lib.Core.Interface;
using JBank.Lib.Data;
using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace JBank.Lib.Core.Repository
{
   public class AccountRepository : IAccountRepository
    {
        public AccountRepository()
        {

        }
        public void CreateAccount(Account acct, decimal initialdeposit)
        {
            AccountData.AddModel(acct);
            var newtransact = new Transact(acct.OwnerId, acct.AccountNumber, initialdeposit, "Deposit to Account", acct.Type, acct.DateCreated);
            TransactionData.AddModel(newtransact);
            TransactionData.AddModelToFile(newtransact);
            AccountData.AddModelToFile(acct);
        }

        public void Deposit(string accno, decimal amt, string type)
        {
            var TransactionDate = DateTime.Now;
            var id = Guid.NewGuid().ToString();
            var newtransact = new Transact(id, accno, amt, "Deposit to Account", type, TransactionDate);
            TransactionData.AddModel(newtransact);
            TransactionData.AddModelToFile(newtransact);

        }

        public string[] Transfer(string senderNumber, string receiverNumber, decimal amt, string type)
        {
            var res = new string[2];
            var senderaccount = AccountData.Accounts.Find(a => AuthenticationRepository.CurrentUser.Id == a.OwnerId && a.Type == type);
            var bal = GetBalance(senderNumber);
            if ((bal - amt) < senderaccount.MinBalance)
            {
                res[0] = "failed";
                res[1] = "INSUFFICIENT FUNDS: MINIMUM BALANCE EXCCEDED!";
                return res;
            }
            else
            {
                Withdraw(senderNumber, amt, type);
                Deposit(receiverNumber, amt, type);
                res[0] = "success";
            }

            return res;


        }

        public string[] Withdraw(string accno, decimal amt, string type)
        {
            var account = AccountData.Accounts.Find(a => AuthenticationRepository.CurrentUser.Id == a.OwnerId && a.Type == type);
            var bal = GetBalance(accno);
            var res = new string[2];
            // Ensure the minimum balance is not exceeded
            if ((bal - amt) < account.MinBalance)
            {
                res[0] = "failed";
                res[1] = "INSUFFICIENT FUNDS : MINIMUM BALANCE EXCEEDED!";
                return res;
            }
            else
            {
                var TransactionDate = DateTime.Now;
                var id = Guid.NewGuid().ToString();
                var withdraw = new Transact(id, accno, -amt, "Withdraw", type, TransactionDate);
                TransactionData.AddModel(withdraw);
                TransactionData.AddModelToFile(withdraw);
                res[0] = "success";
            }
            return res;

        }

        public decimal GetBalance(string accno)
        {
            decimal balance = 0;
            TransactionData.LoadModelsFromFile();
            var tran = TransactionData.Transactions.FindAll(n => n.AccountNumber == accno);
            foreach (Transact t in tran)
            {

                    balance += t.Amount;
                
            }
            return balance;
        }

        //getdb balance
        public decimal GetDBBalance(string accno)
        {
            decimal balance = 0;
            using (SqlConnection con = new SqlConnection(Db.connectionString))
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand check = new SqlCommand("SELECT * FROM transactions WHERE accnumber = @accno", con);
                    check.Parameters.AddWithValue("@accno", accno);
                   
                    SqlDataReader sReader = check.ExecuteReader();

                    while (sReader.Read())
                    {
                         balance += Convert.ToDecimal(sReader["amount"]); UserSession.CurrentUserID.Add(sReader["id"].ToString());


                     
                    }


                }
            }
            return balance;
        }



        public string CreateDBAccount(string id,Account acct, decimal initialdeposit)
        {
            string isCreated = "";
            using (SqlConnection connection = new SqlConnection(Db.connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                   
                    command.CommandText =
                       "INSERT INTO account (id,accnumber,initialdeposit,acctype,date) " +
                       "VALUES (@id,@acctno,@initialdeposit,@accType,@date)";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.Parameters.Add(new SqlParameter("@acctno", acct.AccountNumber));
                    command.Parameters.Add(new SqlParameter("@initialDeposit", initialdeposit));
                    command.Parameters.Add(new SqlParameter("@accType", acct.Type));
                    command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                    command.ExecuteNonQuery();

                    command.CommandText =
                      "INSERT INTO transactions (accnumber,amount,note,acctype,creationdate,id) " +
                      "VALUES (@acctno,@initialdeposit,@FirstDeposit,@accType,@date,@id)";
                    command.Parameters.Clear();

                    command.Parameters.Add(new SqlParameter("@acctno", acct.AccountNumber));
                    command.Parameters.Add(new SqlParameter("@initialDeposit", initialdeposit));
                    command.Parameters.Add(new SqlParameter("@FirstDeposit", "deposit"));
                    command.Parameters.Add(new SqlParameter("@accType", acct.Type));
                    command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.ExecuteNonQuery();





                    // Attempt to commit the transaction.
                    transaction.Commit();

                    isCreated = "success";


                    AccountData.AddModel(acct);
                    var newtransact = new Transact(acct.OwnerId, acct.AccountNumber, initialdeposit, "Deposit to Account", acct.Type, acct.DateCreated);
                    TransactionData.AddModel(newtransact);
                    TransactionData.AddModelToFile(newtransact);
                    AccountData.AddModelToFile(acct);

                }
                catch (Exception ex)
                {
                    isCreated = ex.Message;

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        isCreated = ex2.Message;
                    }
                }
                return isCreated;
            }


           
        }
    }
}
