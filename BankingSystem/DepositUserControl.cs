using JBank.Lib.Core.Repository;
using JBank.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using JBank.Lib.Core.Interface;
using JBank.Lib.Core;
using JBank.Lib.Model;
using System.Data.SqlClient;

namespace JBankUI
{
    public partial class DepositUserControl : UserControl
    {
        private readonly IAccountRepository acctrepo;
        private static string accType = "";
        private static string id = "";
        private static string foundcustomer = "";
        private static DepositUserControl _instance;
        public static DepositUserControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DepositUserControl(GlobalConfig.IAccountinstance);
                return _instance;
            }
        }
        public DepositUserControl(IAccountRepository Acctrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DepositUserControl_Load(object sender, EventArgs e)
        {
           
          
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void depositAmnt_TextChanged(object sender, EventArgs e)
        {
          
            verifyBtn.Show();
        }

        private void verifyBtn_Click(object sender, EventArgs e)
        {
            enterPass.Show();
        }

        private void enterPass_TextChanged(object sender, EventArgs e)
        {
            confirmBtn.Show();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {

            decimal depositAmount = Convert.ToDecimal(depositAmnt.Text);

            if (depositAmount < 0 || depositAmount == 0)
            {
                MessageBox.Show("Amount cannot be less than or equall to zero");
            }
            else
            {
                try
                {
                    string id = "";
                    string note = "Deposit";
                    int accno = 0;
                    foreach (var item in UserSession.CurrentUserID)
                    {
                        id = item.Split(",")[0];
                    }
                    foreach (var item in UserSession.UserAccount)
                    {
                        accno = Convert.ToInt32(item);

                    }
                    using (SqlConnection con = new SqlConnection(Db._conString))
                    {
                        con.Open();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            SqlCommand store = new SqlCommand("insert into transactions (userID,AccNumber,Amount,note,AccType,CreationDate) " +
                                "values ('" + id + "','" + accno + "','" + depositAmount + "','" + note + "','" + accType + "','" + DateTime.Now + "') ", con);
                            store.ExecuteNonQuery();
                            MessageBox.Show("Deposit was Successfully done");
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
            depositAccPanel.Visible = false;
            verifyBtn.Visible = false;
            enterPass.Visible = false;
            confirmBtn.Visible = false;
            //decimal depositAmount = Convert.ToDecimal(depositAmnt.Text);
            //try
            //{

            //    var accnum = AccountData.Accounts.Find(a => a.OwnerId == AuthenticationRepository.CurrentUser.Id && a.Type == accType);
            //    if (accnum != null)
            //    {

            //        acctrepo.Deposit(accnum.AccountNumber, depositAmount, accType);
            //        Thread.Sleep(1000);
            //        MessageBox.Show("Deposit was Successfully done");

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
            //depositAccPanel.Visible = false;
            //verifyBtn.Visible = false;
            //enterPass.Visible = false;
            //confirmBtn.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                if (radioButton1.Checked)
                {
                    accType = "Savings";
                   

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
                                    depositAccPanel.Visible = true;
                                    depositAmnt.Visible = true;
                                }
                                else
                                {
                                    depositAmnt.Visible = false;
                                    depositAccPanel.Visible = false;
                                }
                            }

                        }
                    }

                }
                else if (!radioButton1.Checked)
                {
                    depositAmnt.Visible = false;
                    depositAccPanel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //if (radioButton1.Checked)
            //{
            //     accType = "Savings";
            //    string id = AuthenticationRepository.CurrentUser.Id;
            //    AccountData.LoadModelsFromFile();
            //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accType);
            //    if(foundcustomer != null)
            //    {
            //        accNum.Text = foundcustomer.AccountNumber;
            //        accBal.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
            //        depositAccPanel.Visible = true;
            //        depositAmnt.Visible = true;
            //    }
            //    else
            //    {
            //        depositAmnt.Visible = false;
            //        depositAccPanel.Visible = false;
            //    }

            //}
            //else if(!radioButton1.Checked)
            //{
            //    depositAmnt.Visible = false;
            //    depositAccPanel.Visible = false;

            //}
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
            {
                 accType = "Current";
               

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
                                depositAccPanel.Visible = true;
                                depositAmnt.Visible = true;
                            }
                            else
                            {
                                depositAmnt.Visible = false;
                                depositAccPanel.Visible = false;
                            }
                        }

                    }
                }
            }
            else if (!radioButton2.Checked)
            {
                depositAmnt.Visible = false;
                depositAccPanel.Visible = false;
            }
    //if (radioButton2.Checked)
    //{
    //     accType = "Current";
    //    string id = AuthenticationRepository.CurrentUser.Id;
    //    AccountData.LoadModelsFromFile();
    //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accType);
    //    if(foundcustomer != null)
    //    {
    //        accNum.Text = foundcustomer.AccountNumber;
    //        accBal.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
    //        depositAccPanel.Visible = true;
    //        depositAmnt.Visible = true;
    //    }
    //    else{
    //        depositAmnt.Visible = false;
    //        depositAccPanel.Visible = false;
    //    }
    //}
    //else if (!radioButton2.Checked)
    //{
    //    depositAmnt.Visible = false;
    //    depositAccPanel.Visible = false;

    //}
}
    }
}
