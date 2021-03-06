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
    public partial class withdrawUserControl : UserControl
    {
        private readonly IAccountRepository acctrepo;
        private static string accType = "";
        private static withdrawUserControl _instance;
        public static withdrawUserControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new withdrawUserControl(GlobalConfig.IAccountinstance);
                return _instance;
            }
        }

        public withdrawUserControl(IAccountRepository Acctrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
        }

        private void withdrawUserControl_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                accType = "Savings";
                string id = "";
                string foundcustomer;

                foreach (var item in UserSession.CurrentUserID)
                {
                    id = item.Split(",")[0];
                }
                using (SqlConnection con = new SqlConnection(Db._conString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand check = new SqlCommand("SELECT * FROM Accounts WHERE userID = @id AND AccType = @accType", con);
                        check.Parameters.AddWithValue("@id", id);
                        check.Parameters.AddWithValue("@accType", accType);
                        SqlDataReader sReader = check.ExecuteReader();
                        if (sReader.Read())
                        {
                            foundcustomer = sReader["AccNumber"].ToString();
                            UserSession.UserAccount.Add(sReader["AccNumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["userID"].ToString());

                            if (foundcustomer != null)
                            {
                                accNum.Text = foundcustomer;
                                accBal.Text = acctrepo.GetDbBalance(foundcustomer).ToString();
                                withdPanel.Visible = true;
                                withdAmnt.Visible = true;
                            }
                            else
                            {
                                withdAmnt.Visible = false;
                                withdPanel.Visible = false;
                            }
                        }

                    }
                }
              
            }
            else if (!radioButton1.Checked)
            {
                withdAmnt.Visible = false;
                withdPanel.Visible = false;
            }

            //if (radioButton1.Checked)
            //{
            //    accType = "Savings";
            //    string id = AuthenticationRepository.CurrentUser.Id;
            //    AccountData.LoadModelsFromFile();
            //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accType);
            //    if (foundcustomer != null)
            //    {
            //        accNum.Text = foundcustomer.AccountNumber;
            //        accBal.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
            //        withdPanel.Visible = true;
            //        withdAmnt.Visible = true;
            //    }
            //    else
            //    {
            //      withdAmnt.Visible = false;
            //       withdPanel.Visible = false;
            //    }

            //}
            //else if (!radioButton1.Checked)
            //{
            //   withdAmnt.Visible = false;
            //   withdPanel.Visible = false;

            //}
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
            {
                accType = "Current";
                string id = "";
                string foundcustomer;

                foreach (var item in UserSession.CurrentUserID)
                {
                    id = item.Split(",")[0];
                }
                using (SqlConnection con = new SqlConnection(Db._conString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand check = new SqlCommand("SELECT * FROM Accounts WHERE userID = @id AND AccType=@accType", con);
                        check.Parameters.AddWithValue("@id", id);
                        check.Parameters.AddWithValue("@accType", accType);
                        SqlDataReader sReader = check.ExecuteReader();
                        if (sReader.Read())
                        {
                            foundcustomer = sReader["AccNumber"].ToString();
                            UserSession.UserAccount.Add(sReader["AccNumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["userID"].ToString());

                            if (foundcustomer != null)
                            {
                                accNum.Text = foundcustomer;
                                accBal.Text = acctrepo.GetDbBalance(foundcustomer).ToString();
                                withdPanel.Visible = true;
                                withdAmnt.Visible = true;
                            }
                            else
                            {
                                withdAmnt.Visible = false;
                                withdPanel.Visible = false;
                            }
                        }

                    }
                }
            }
            else if (!radioButton2.Checked)
            {
                withdAmnt.Visible = false;
                withdPanel.Visible = false;
            }

            //if (radioButton2.Checked)
            //{
            //    accType = "Current";
            //    string id = AuthenticationRepository.CurrentUser.Id;
            //    AccountData.LoadModelsFromFile();
            //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accType);
            //    if (foundcustomer != null)
            //    {
            //        accNum.Text = foundcustomer.AccountNumber;
            //        accBal.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
            //       withdPanel.Visible = true;
            //        withdAmnt.Visible = true;
            //    }
            //    else
            //    {
            //        withdAmnt.Visible = false;
            //        withdPanel.Visible = false;
            //    }
            //}
            //else if (!radioButton2.Checked)
            //{
            //    withdAmnt.Visible = false;
            //   withdPanel.Visible = false;

            //}
        }

        private void withdAmnt_TextChanged(object sender, EventArgs e)
        {
            verifyBtn.Show();
        }

        private void enterPass_TextChanged(object sender, EventArgs e)
        {
            confirmWithd.Show();
        }

        private void confirmWithd_Click(object sender, EventArgs e)
        {

            decimal withdAmount = Convert.ToDecimal(withdAmnt.Text);

            if (withdAmount < 0 || withdAmount == 0)
            {
                MessageBox.Show("Amount cannot be less than or equall to zero");
            }
            else
            {
                try
                {
                    string id = "";
                    string note = "Withdrawal";
                    int accno = 0;
                    foreach (var item in UserSession.CurrentUserID)
                    {
                        id = item.Split(",")[0];
                    }
                    foreach (var item in UserSession.UserAccount)
                    {
                        accno = Convert.ToInt32(item.Split(",")[0]);
                    }
                    using (SqlConnection con = new SqlConnection(Db._conString))
                    {
                        con.Open();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            SqlCommand store = new SqlCommand("insert into transactions (userID,AccNumber,Amount,note,AccType,CreationDate) values ('" + id + "','" + accno + "','" + -withdAmount + "','" + note + "','" + accType + "','" + DateTime.Now + "') ", con);
                            store.ExecuteNonQuery();
                            MessageBox.Show("Withdrawal was Successfully Made");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
           
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            withdPanel.Visible = false;
            verifyBtn.Visible = false;
            enterPass.Visible = false;
            confirmWithd.Visible = false;

            //decimal withdAmount = Convert.ToDecimal(withdAmnt.Text);
            //try
            //{

            //    var accnum = AccountData.Accounts.Find(a => a.OwnerId == AuthenticationRepository.CurrentUser.Id && a.Type == accType);
            //    if (accnum != null)
            //    {

            //        acctrepo.Withdraw(accnum.AccountNumber, withdAmount, accType);
            //        Thread.Sleep(3000);
            //        MessageBox.Show("Withdrawer was Successfully done");

            //    }
            //    else
            //    {
            //        MessageBox.Show("An Error occcur");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //radioButton1.Checked = false;
            //radioButton2.Checked = false;
            //withdPanel.Visible = false;
            //verifyBtn.Visible = false;
            //enterPass.Visible = false;
            //confirmWithd.Visible = false;
        }

        private void verifyBtn_Click(object sender, EventArgs e)
        {
            enterPass.Show();
        }
    }
}
