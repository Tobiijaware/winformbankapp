using JBank.Lib.Core.Interface;
using JBank.Lib.Data;
using JBank.Lib.Model;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JBank.Lib.Core.Repository
{
   public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository()
        {

        }
        public void CreateCustomer(Customer cust)
        {
            
            CustomerData.AddModel(cust);
            CustomerData.AddModelToFile(cust);
        }

        public void GetStatements()
        {
            throw new NotImplementedException();
        }
    }
}
