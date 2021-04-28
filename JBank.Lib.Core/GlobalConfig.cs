using JBank.Lib.Core.Interface;
using JBank.Lib.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace JBank.Lib.Core
{
    public class GlobalConfig
    {
        public static IAccountRepository IAccountinstance { get; private set; }
        
        public static ICustomerRepository ICustomerinstance { get; private set; }
        public static IAuthenticationRepository IAuthenticationinstance { get; private set; }
       

        public static void AddIinstance()
        {
            AccountRepository op = new AccountRepository();
            CustomerRepository oc = new CustomerRepository();
            AuthenticationRepository ar = new AuthenticationRepository();
          
            IAccountinstance = op;
            ICustomerinstance = oc;
            IAuthenticationinstance = ar;
        }

        public static void RemoveIinstance()
        {
            IAccountinstance = null;
            ICustomerinstance = null;
            IAuthenticationinstance = null;
        }
    }
}
