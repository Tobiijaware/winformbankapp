 using JBank.Lib.Common;
using JBank.Lib.Core;
using JBank.Lib.Core.Interface;
using JBank.Lib.Core.Repository;
using JBank.Lib.Data;
using JBank.Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JBankUI
{
    public partial class CreateAccount : UserControl
    {
        private readonly IAccountRepository acctrepo;
        private readonly IAuthenticationRepository authrepo;
        private readonly ICustomerRepository cusRepo;
        public CreateAccount(IAccountRepository Acctrepo, ICustomerRepository Custrepo, IAuthenticationRepository Authrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
            authrepo = Authrepo;
            cusRepo = Custrepo;
        }

        public CreateAccount()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {

            var accType = "";
            var initialDeposit = 0;
            var narrate = "";

            if (radioButton1.Checked)
            {
                accType = radioButton1.Text;
                initialDeposit = Convert.ToInt32(textBox1.Text);
                narrate = note.Text;
                
                if (initialDeposit < 1000)
                {
                    MessageBox.Show("Initial deposit must be 1000 and above");

                }
                else
                {
                    CreateTheAccount(accType, initialDeposit, narrate);
                }
            }

            else
            {
                accType = radioButton2.Text;
                initialDeposit = Convert.ToInt32(textBox3.Text);
                narrate = note.Text;
                if (initialDeposit < 0)
                {
                    MessageBox.Show("Sorry you can't deposit less than 0");
                }
                else
                {
                    CreateTheAccount(accType, initialDeposit, narrate);

                }
            }

        }

        private void CreateTheAccount(string accType, int initialDeposit, string narrate)
        {
            try
            {
                var hashes = Utilities.GenerateHash(CusAdd.customerDetails[4]);
                byte[] passwordhash = hashes[0];
                byte[] passwordsalt = hashes[1];
                var date = DateTime.Now;
                string acctno = Utilities.GenerateNumber();
                var newacct = new Account(acctno, CusAdd.customerDetails[0], initialDeposit, accType, date);
                var newcust = new Customer(CusAdd.customerDetails[0], CusAdd.customerDetails[1], CusAdd.customerDetails[2], CusAdd.customerDetails[3], date, passwordhash, passwordsalt);
                AuthenticationRepository.CurrentUser = newcust;
                var res = authrepo.Register(newcust);

                var id = CusAdd.customerDetails[0];
                var firstname = CusAdd.customerDetails[1];
                var lastname = CusAdd.customerDetails[2];
                var email = CusAdd.customerDetails[3];
                var hash = Convert.ToBase64String(passwordhash);
                var salt = Convert.ToBase64String(passwordsalt);


                if (res[0] == "failed")
                {
                    MessageBox.Show(res[1]);
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(Db._conString))
                    {
                        connection.Open();

                        SqlCommand command = connection.CreateCommand();
                        SqlTransaction transaction;

                        //start a local transaction
                        transaction = connection.BeginTransaction("SampleTransaction");

                        //Must assign both transaction object and connection
                        //to command object for a pending local transaction
                        command.Connection = connection;
                        command.Transaction = transaction;

                        try
                        {
                            command.CommandText =
                                "INSERT INTO Customers (Id, FirstName, LastName, email, Regdate, PasswordHarsh, PasswordSalt) " +
                                "VALUES (@id, @firstname, @lastname, @email, @date, @hash, @salt)";

                            command.Parameters.Add(new SqlParameter("@id", id));
                            command.Parameters.Add(new SqlParameter("@firstname", firstname));
                            command.Parameters.Add(new SqlParameter("@lastname", lastname));
                            command.Parameters.Add(new SqlParameter("@email", email));
                            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                            command.Parameters.Add(new SqlParameter("@hash", hash));
                            command.Parameters.Add(new SqlParameter("@salt", salt));

                            command.ExecuteNonQuery();

                            command.CommandText =
                                "INSERT INTO Accounts (userID, AccNumber, InitialDeposit, AccType, CreationDate) " +
                                "VALUES (@id, @acctno, @initialdeposit, @acctype, @date)";

                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@id", id));
                            command.Parameters.Add(new SqlParameter("@acctno", acctno));
                            command.Parameters.Add(new SqlParameter("@initialdeposit", initialDeposit));
                            command.Parameters.Add(new SqlParameter("@acctype", accType));
                            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));

                            command.ExecuteNonQuery();

                            command.CommandText =
                                "INSERT INTO Transactions (userID, AccNumber, Amount, Note, AccType, CreationDate)" +
                                "VALUES (@id, @accnum, @amnt, @note, @acctype, @date)";

                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@id", id));
                            command.Parameters.Add(new SqlParameter("@accnum", acctno));
                            command.Parameters.Add(new SqlParameter("@amnt", initialDeposit));
                            command.Parameters.Add(new SqlParameter("@note", narrate));
                            command.Parameters.Add(new SqlParameter("@acctype", accType));
                            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));

                            command.ExecuteNonQuery();

                            //commit the transaction
                            transaction.Commit();

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                            //roll back the transaction if it was not succesfull
                            try
                            {
                                transaction.Rollback();
                            }

                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }
                    }

                    acctrepo.CreateAccount(newacct, initialDeposit);
                    MessageBox.Show("Your Account have been Created Successfully: Login to your Dashboard");
                    Thread.Sleep(1000);
                    this.Hide();
                    Login lf = new Login(GlobalConfig.IAuthenticationinstance);
                    lf.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                savingsPanel.Visible = true;
                panel2.Visible = false;
                CurrentPanel.Visible = false;
                
            }
        }

        private void CurrentPanel_Paint(object sender, PaintEventArgs e)
        {
            //this.Visible = false;
        }

        private void savingsPanel_Paint(object sender, PaintEventArgs e)
        {
            //this.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //this.Visible = true;
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged_2(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                savingsPanel.Visible = false;
                panel2.Visible = false;
                CurrentPanel.Visible = true;
            }
        }

        private void note_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
