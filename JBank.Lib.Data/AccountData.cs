using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace JBank.Lib.Data
{
    public class AccountData
    {
        private static string file = @"C:\Users\hp\source\repos\JBank\JBank.Lib.Data\DataSource\Account.txt";

        public static List<Account> Accounts = new List<Account>();
        public static void AddModel(Account acct)
        {
            Accounts.Add(acct);
            //AddModelToFile();
            //ADd
        }
        public static void AddModelToFile(Account a)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                // creates an array of the model values
                string[] values = { a.AccountNumber, a.OwnerId, a.Balance.ToString(), a.Type, a.DateCreated.ToString(), a.MinBalance.ToString() };
                // creates a new line
                string line = string.Join(";", values);
                // writes the line
                sw.WriteLine(line);
                // flushes the buffer
                sw.Flush();
            }
        }

        public static void LoadModelsFromFile()
        {
            Accounts.Clear();
            // Opens the file for reading only
            using (StreamReader sr = new StreamReader(file))
            {
                string s;
                // Reads it line by line
                while ((s = sr.ReadLine()) != null)
                {
                    // splits the string line using semicolons
                    string[] values = s.Split(';');
                    Account a = new Account(values[0], values[1], Decimal.Parse(values[2]), values[3], DateTime.Parse(values[4]));
                    a.MinBalance = Decimal.Parse(values[5]);
                    // adds a new model with those values
                    AddModel(a);


                }
            }
        }
    }
}
