using System;
using System.Collections.Generic;
using System.Text;

namespace JBank.Lib.Model
{

  

    public class Customer
    {
        public Customer(string id, string fname, string lname, string email, DateTime date, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = id;
            DateCreated = date;
            LastName = lname;
            FirstName = fname;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
        public string Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
