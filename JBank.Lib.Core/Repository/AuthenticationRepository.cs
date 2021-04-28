using JBank.Lib.Common;
using JBank.Lib.Core.Interface;
using JBank.Lib.Data;
using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace JBank.Lib.Core.Repository
{
   public class AuthenticationRepository : IAuthenticationRepository
    {
        public static Customer CurrentUser;
        private static CustomerRepository custrepository;
        public AuthenticationRepository()
        {
            custrepository = new CustomerRepository();
        }

        public string[] Register(Customer cust)
        {
            string[] res = new string[2];
            if (EmailExist(cust.Email))
            {
                res[0] = "failed";
                res[1] = "Email Already Exists";
            }
            else
            {
                custrepository.CreateCustomer(cust);
                res[0] = "success";
            }
            return res;

        }

        public bool Login(string email, string password)
        {
            if (!IsValidDBCred(email, password))
            {
                return false;
            }
            return true;

        }

        public void Logout()
        {
            throw new NotImplementedException();
        }


        private bool IsValidCred(string email, string password)
        {
            bool passwordMatch = false;
            foreach (var cust in CustomerData.Customers)
            {

                if (Utilities.CompareHash(cust.PasswordSalt, cust.PasswordHash, password))
                {
                    passwordMatch = true;

                }


            }

            return passwordMatch;
        }

        private bool IsValidDBCred(string email, string password)
        {
            try
            {
                bool passwordMatch = false;
                using (SqlConnection con = new SqlConnection(Db.connectionString))
                {
                    SqlCommand check = new SqlCommand("SELECT * FROM customers WHERE email = @email", con);
                    check.Parameters.Add(new SqlParameter("@email", email));
                    con.Open();

                    SqlDataReader reader = check.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var hash = Convert.FromBase64String((string)reader[4]);
                            byte[] salt = Convert.FromBase64String((string)reader[5]);

                            if (Utilities.CompareHash(salt, hash, password))
                            {
                                passwordMatch = true;

                            }
                          

                        }
                    }

                  
                    reader.Close();
                }
                return passwordMatch;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }


        internal bool EmailExist(string email)
        {
            foreach (Customer c in CustomerData.Customers)
            {
                if (c.Email == email)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
