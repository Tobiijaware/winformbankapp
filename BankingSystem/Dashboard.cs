using JBank.Lib.Common;
using JBank.Lib.Core.Interface;
using JBank.Lib.Core.Repository;
using JBank.Lib.Data;
using JBank.Lib.Model;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace JBankUI
{
    public partial class Dashboard : Form
    {
        private readonly IAccountRepository acctrepo;
        public Dashboard(IAccountRepository Acctrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
        }


        public static string userDetails(string email)
        {
            string user = "";
            using (SqlConnection con = new SqlConnection(Db._conString))
            {

                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand check = new SqlCommand("SELECT * FROM Customers WHERE email = @email", con);
                    check.Parameters.AddWithValue("@email", email);
                    SqlDataReader sReader = check.ExecuteReader();
                    while (sReader.Read())
                    {
                        user = sReader["firstname"].ToString() + " " + sReader["lastname"].ToString();
                        UserSession.CurrentUserID.Add(sReader["Id"].ToString());
                    }

                }
            }
            return user;
        }



        private void Dashboard_Load(object sender, EventArgs e)
        {
           
            try
            {
                string email = "";

                foreach (var item in UserSession.CurrentUser)
                {
                    email = item.Split(",")[0];

                }
                userName.Text = userDetails(email);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void userControlPanel_Paint(object sender, PaintEventArgs e)
        {

        }


        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            if (!userControlPanel.Controls.Contains(DashboardUsersControl.Instance))
            {
                userControlPanel.Controls.Add(DashboardUsersControl.Instance);
                DashboardUsersControl.Instance.Dock = DockStyle.Fill;
                DashboardUsersControl.Instance.BringToFront();
            }
            else
            {
                DashboardUsersControl.Instance.BringToFront();
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void depsoitBtn_Click(object sender, EventArgs e)
        {
            if (!userControlPanel.Controls.Contains(DepositUserControl.Instance))
            {
                userControlPanel.Controls.Add(DepositUserControl.Instance);
                DepositUserControl.Instance.Dock = DockStyle.Fill;
                DepositUserControl.Instance.BringToFront();

            }
            else
            {
                DepositUserControl.Instance.BringToFront();
            }
        }

        private void transferBtn_Click(object sender, EventArgs e)
        {
            if (!userControlPanel.Controls.Contains(TransferUserControl.Instance))
            {
                userControlPanel.Controls.Add(TransferUserControl.Instance);
                TransferUserControl.Instance.Dock = DockStyle.Fill;
                TransferUserControl.Instance.BringToFront();
            }
            else
            {
                TransferUserControl.Instance.BringToFront();
            }
        }

        private void withdrawBtn_Click(object sender, EventArgs e)
        {
            if (!userControlPanel.Controls.Contains(withdrawUserControl.Instance))
            {
                userControlPanel.Controls.Add(withdrawUserControl.Instance);
                withdrawUserControl.Instance.Dock = DockStyle.Fill;
                withdrawUserControl.Instance.BringToFront();
            }
            else
            {
                withdrawUserControl.Instance.BringToFront();
            }
        }

        private void transactionBtn_Click(object sender, EventArgs e)
        {

            if (!userControlPanel.Controls.Contains(TransactionUserControl.Instance))
            {
                userControlPanel.Controls.Add(TransactionUserControl.Instance);
                TransactionUserControl.Instance.Dock = DockStyle.Fill;
                TransactionUserControl.Instance.BringToFront();
            }
            else
            {
                TransactionUserControl.Instance.BringToFront();
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            AuthenticationRepository.CurrentUser = null;
            this.Hide();
            Login show = new Login();
            show.Show();
        }

        private void userName_Click(object sender, EventArgs e)
        {

        }

        private void anotherAccBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string id = "";
                foreach (var item in UserSession.CurrentUserID)
                {
                    id = item.Split(",")[0];
                }
                using (SqlConnection con = new SqlConnection(Db._conString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Accounts WHERE userID = @id", con);
                        check.Parameters.Add(new SqlParameter("@id", id));

                        int UserExist = (int)check.ExecuteScalar();
                        if (UserExist != 2)
                        {
                            SqlCommand checkacc = new SqlCommand("SELECT * FROM Accounts WHERE userID = @id", con);
                            checkacc.Parameters.Add(new SqlParameter("@id", id));
                            SqlDataReader sReader = checkacc.ExecuteReader();
                            while (sReader.Read())
                            {
                                if (sReader["AccType"].ToString() == "Savings")
                                {
                                    string accType = "Current";
                                    var date = DateTime.Now;
                                    string acctno = Utilities.GenerateNumber();
                                    var newacct = new Account(acctno, id, 0, accType, date);
                                    acctrepo.CreateDBAccount(id, newacct, 0);
                                    MessageBox.Show("Your Current Account has been successfully created for you");
                                }
                                else if (sReader["AccType"].ToString().Trim() == "Current")
                                {
                                    string accType = "Savings";
                                    var date = DateTime.Now;
                                    string acctno = Utilities.GenerateNumber();
                                    var newacct = new Account(acctno, id, 1000, accType, date);
                                    acctrepo.CreateDBAccount(id, newacct, 1000);
                                    MessageBox.Show("Your Savings Account has been successfully created for you");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("You Already Have Two Accounts");

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //        string id = AuthenticationRepository.CurrentUser.Id;
            //        //AccountData.LoadModelsFromFile();
            //        var foundcustomer = AccountData.Accounts.FindAll(c => c.OwnerId == id);
            //        var number = foundcustomer.Count;
            //        if(number > 1)
            //        {
            //            MessageBox.Show("You can not have more than two(2) Accounts");
            //        }
            //        else
            //        {
            //            foreach (var item in foundcustomer)
            //            {
            //                if (item.Type == "Savings")
            //                {

            //                    string accType = "Current"; 
            //                    var date = DateTime.Now;
            //                    string acctno = Utilities.GenerateNumber();
            //                    var newacct = new Account(acctno, id, 0, accType, date);
            //                    acctrepo.CreateAccount(newacct, 0);
            //                    MessageBox.Show("Your Current Account has been successfully created for you"); 

            //;
            //                }
            //                else if (item.Type == "Current")
            //                {

            //                    string accType = "Savings";
            //                    var date = DateTime.Now;
            //                    string acctno = Utilities.GenerateNumber();
            //                    var newacct = new Account(acctno, id, 1000, accType, date);
            //                    acctrepo.CreateAccount(newacct, 1000);
            //                    MessageBox.Show("Your Savings Account has been successfully created for you");

            //                }
            //            }
            //        }


        }
    }
    
}
