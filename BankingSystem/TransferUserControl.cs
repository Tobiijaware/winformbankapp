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
using System.Windows.Forms;

namespace JBankUI
{
    public partial class TransferUserControl : UserControl
    {
        private readonly IAccountRepository acctrepo;
        private static string accTypeFrm = "";
        private static string accTypeTo = "";
        private static string reAcc = "";
        private static string recipient = "";
        
        private static TransferUserControl _instance;
        public static TransferUserControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TransferUserControl(GlobalConfig.IAccountinstance);
                return _instance;
            }
        }

        public TransferUserControl(IAccountRepository Acctrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TransferUserControl_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                accTypeFrm = "Savings";
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
                        check.Parameters.AddWithValue("@accType", accTypeFrm);
                        SqlDataReader sReader = check.ExecuteReader();
                        if (sReader.Read())
                        {
                            foundcustomer = sReader["AccNumber"].ToString();
                            UserSession.UserAccount.Add(sReader["AccNumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["userID"].ToString());

                            if (foundcustomer != null)
                            {
                                accNumFrom.Text = foundcustomer;
                                accBalFrm.Text = acctrepo.GetDbBalance(foundcustomer).ToString();
                                accDetailsFrm.Visible = true;
                                toPanel.Visible = true;
                            }
                            else
                            {
                                accDetailsFrm.Visible = false;
                                toPanel.Visible = false;
                            }
                        }

                    }
                }
            }
            else if (!radioButton1.Checked)
            {
                accDetailsFrm.Visible = false;
                toPanel.Visible = false;
                radioButton1.Checked = false;
            }
            //if (radioButton1.Checked)
            //{
            //    accTypeFrm = "Savings";
            //    checkSF = accTypeFrm;
            //    string id = AuthenticationRepository.CurrentUser.Id;
            //    AccountData.LoadModelsFromFile();
            //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accTypeFrm);
            //    if (foundcustomer != null)
            //    {
            //        accNumFrom.Text = foundcustomer.AccountNumber;
            //        accBalFrm.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
            //        accDetailsFrm.Visible = true;
            //        toPanel.Visible = true;
            //    }
            //    else
            //    {
            //        accDetailsFrm.Visible = false;
            //        toPanel.Visible = false;
            //    }

            //}
            //else if (!radioButton1.Checked)
            //{
            //    accDetailsFrm.Visible = false;
            //    toPanel.Visible = false;
            //    radioButton1.Checked = false;

            //}
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
            {
                accTypeFrm = "Current";
                //checkCF = accTypeFrm;
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
                        check.Parameters.AddWithValue("@accType", accTypeFrm);
                        SqlDataReader sReader = check.ExecuteReader();
                        if (sReader.Read())
                        {
                            foundcustomer = sReader["AccNumber"].ToString();
                            UserSession.UserAccount.Add(sReader["AccNumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["userID"].ToString());

                            if (foundcustomer != null)
                            {
                                accNumFrom.Text = foundcustomer;
                                accBalFrm.Text = acctrepo.GetDbBalance(foundcustomer).ToString();
                                accDetailsFrm.Visible = true;
                                toPanel.Visible = true;
                            }
                            else
                            {
                                accDetailsFrm.Visible = false;
                                toPanel.Visible = false;
                            }
                        }

                    }
                }
            }
            else if (!radioButton2.Checked)
            {
                accDetailsFrm.Visible = false;
                toPanel.Visible = false;
                radioButton2.Checked = false;
            }

            //if (radioButton2.Checked)
            //{
            //    accTypeFrm = "Current";
            //    checkCF = accTypeFrm;
            //    string id = AuthenticationRepository.CurrentUser.Id;
            //    AccountData.LoadModelsFromFile();
            //    var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accTypeFrm);
            //    if (foundcustomer != null)
            //    {
            //        accNumFrom.Text = foundcustomer.AccountNumber;
            //        accBalFrm.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
            //        accDetailsFrm.Visible = true;
            //        toPanel.Visible = true;
            //    }
            //    else
            //    {
            //        accDetailsFrm.Visible = false;
            //        toPanel.Visible = false;
            //    }

            //}
            //else if (!radioButton2.Checked)
            //{
            //    accDetailsFrm.Visible = false;
            //    toPanel.Visible = false;
            //    radioButton2.Checked = false;

            //}
        }


        private void transAmnt_TextChanged(object sender, EventArgs e)
        {
           
            verifyBtn.Show();
        }

        private void verifyBtn_Click(object sender, EventArgs e)
        {
            passCfrm.Show();
        }

        private void passCfrm_TextChanged(object sender, EventArgs e)
        {
            confirm.Show();
        }

        private void confirm_Click(object sender, EventArgs e)
        {


            decimal tranAmnt = Convert.ToDecimal(transAmnt.Text);

            if (tranAmnt < 0 || tranAmnt ==0)
            {
                MessageBox.Show("Amount cannot be less than or equall to zero");
            }
            else
            {
                try
                {
                    string id = "";
                    string note = "Transfer";
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
                            SqlCommand store = new SqlCommand("insert into Transactions (userID,AccNumber,Amount,Note,AccType,CreationDate) values ('" + id + "', '" + accno + "','" + -tranAmnt + "','" + note + "','" + accTypeFrm + "','" + DateTime.Now + "') ", con);
                            store.ExecuteNonQuery();
                            
                        }
                    }
                    using (SqlConnection con = new SqlConnection(Db._conString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            SqlCommand store = new SqlCommand("insert into Transactions (userID,AccNumber,Amount,Note,AccType,CreationDate) values ('" + recipient + "','" + reAcc + "','" + tranAmnt + "','" + note + "','" + accTypeTo + "','" + DateTime.Now + "') ", con);
                            store.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Transfer Successful");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
          
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            toPanel.Visible = false;
            verifyBtn.Visible = false;
            passCfrm.Visible = false;
            confirm.Visible = false;

            //decimal tranAmnt = Convert.ToDecimal(transAmnt.Text);

            //try
            //{

            //    var accnumFrm = AccountData.Accounts.Find(a => a.OwnerId == AuthenticationRepository.CurrentUser.Id && a.Type == accTypeFrm);
            //    var accnumTo = AccountData.Accounts.Find(a => a.AccountNumber == recieverNum.Text);
            //    Customer recipient = null;
            //    recipient = CustomerData.Customers.Find(c => c.Id == accnumTo.OwnerId);
            //    DialogResult res = MessageBox.Show($"Confirm Transfer of {tranAmnt} to {recipient.FullName} ", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //    if (res == DialogResult.OK)
            //    {
            //        var transferresponse = acctrepo.Transfer(accnumFrm.AccountNumber,accnumTo.AccountNumber, tranAmnt, accTypeFrm);
            //        if (transferresponse[0] == "failed")
            //        {
            //            MessageBox.Show(transferresponse[1]);
            //        }
            //        else
            //        {
            //            MessageBox.Show("TRANSFER WAS SUCCESSFUL DONE");
            //        }
            //    }
            //    if (res == DialogResult.Cancel)
            //    {
            //        MessageBox.Show("TRANSFER WAS CANCELLED BY YOU");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //radioButton1.Checked = false;
            //radioButton2.Checked = false;
            //toPanel.Visible = false;
            //verifyBtn.Visible = false;
            //passCfrm.Visible = false;
            //confirm.Visible = false;
        }
        private void recieverNum_TextChanged(object sender, EventArgs e)
        {

            try
            {
                 reAcc = recieverNum.Text;
               
              
                // var accnumTo = AccountData.Accounts.Find(a => a.AccountNumber == reAcc);
                if (reAcc != null)
                {
                    using (SqlConnection con = new SqlConnection(Db._conString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            SqlCommand check = new SqlCommand("SELECT * FROM Accounts WHERE AccNumber = @reAcc", con);
                            check.Parameters.AddWithValue("@reAcc", reAcc);
                            SqlDataReader sReader = check.ExecuteReader();
                            while (sReader.Read())
                            {
                                recipient = sReader["userID"].ToString();
                                accTypeTo = sReader["AccType"].ToString();

                               
                            }
                            sReader.Close();

                            SqlCommand checkid = new SqlCommand("SELECT * FROM Customers WHERE id=@recipient", con);
                            checkid.Parameters.AddWithValue("@recipient", recipient);
                            sReader = checkid.ExecuteReader();
                            while (sReader.Read())
                            {
                                accNameTo.Text = sReader["FirstName"].ToString()+" " + sReader["LastName"].ToString();

                            }

                        }
                    }
                    accDetailsTo.Show();
                    transAmnt.Show();
                    //Customer recipient = null;
                    //recipient = CustomerData.Customers.Find(c => c.Id == accnumTo.OwnerId);
                }
                else
                {
                    MessageBox.Show("Account not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //if (recieverNum.Text.ToString().Length  == 8  )  
            //{
            //    string reAcc = recieverNum.Text;
            //    var accnumTo = AccountData.Accounts.Find(a => a.AccountNumber == reAcc);
            //    if (accnumTo != null)
            //    {
            //        Customer recipient = null;
            //        recipient = CustomerData.Customers.Find(c => c.Id == accnumTo.OwnerId);
            //        accNameTo.Text = recipient.FullName;
            //        accDetailsTo.Show();
            //        transAmnt.Show();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Account not found");
            //    }
            //}

        }

        private void recieverNum_Validated(object sender, EventArgs e)
        {
           
        }
    }
}

