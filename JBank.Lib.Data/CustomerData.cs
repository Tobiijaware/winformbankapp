using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JBank.Lib.Data
{
    public class CustomerData
    {
        public static string file = @"C:\Users\hp\source\repos\JBank\JBank.Lib.Data\DataSource\Customer.txt";

        public static List<Customer> Customers = new List<Customer>();

        public static void AddModel(Customer cust)
        {
            Customers.Add(cust);
        }

        public static void AddModelToFile(Customer c)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                // creates an array of the user's values
                string[] values = { c.Id, c.FirstName, c.LastName, c.Email, c.DateCreated.ToString(), Convert.ToBase64String(c.PasswordHash), Convert.ToBase64String(c.PasswordSalt) };
                // creates a new line
                string line = string.Join(";", values);
                // writes the line
                sw.WriteLine(line);


                // flushes the buffer
                sw.Flush();
            }
        }

        public static void RemoveModelFromFile()
        {
            throw new NotImplementedException();
        }

        public static void LoadModelsFromFile()
        {
            Customers.Clear();
            // Opens the file for reading
            using (StreamReader sr = new StreamReader(file))
            {
                string s;
                // Reads it line by line
                while ((s = sr.ReadLine()) != null)
                {
                    // splits the string line using semicolons
                    string[] values = s.Split(';');

                    byte[] byte_array1 = Convert.FromBase64String(values[5]);
                    byte[] byte_array2 = Convert.FromBase64String(values[6]);
                    Customer c = new Customer(values[0], values[1], values[2], values[3], DateTime.Parse(values[4]), byte_array1, byte_array2);

                    AddModel(c);

                }
            }

        }
    }
}
