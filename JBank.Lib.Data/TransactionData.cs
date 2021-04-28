using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JBank.Lib.Model;

namespace JBank.Lib.Data
{
   public class TransactionData
    {
        private static string file = @"C:\Users\hp\source\repos\JBank\JBank.Lib.Data\DataSource\Transaction.txt";


        public static List<Transact> Transactions = new List<Transact>();
        public static void AddModel(Transact t)
        {
            Transactions.Add(t);

        }
        public static void AddModelToFile(Transact t)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                // iterates over the users

                // creates an array of the user's values
                string[] values = { t.Id, t.AccountNumber, t.Amount.ToString(), t.Note, t.AccountType, t.TransactionDate.ToString() };
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
            Transactions.Clear();
            // Opens the file for reading
            using (StreamReader sr = new StreamReader(file))
            {
                string s;
                // Reads it line by line
                while ((s = sr.ReadLine()) != null)
                {
                    // splits the string line using semicolons
                    string[] values = s.Split(';');


                    Transact t = new Transact(values[0], values[1], Decimal.Parse(values[2]), values[3], values[4], DateTime.Parse(values[5]));


                    // adds a new model with those values
                    AddModel(t);

                }
            }
        }
    }
}
