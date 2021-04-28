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
        private static string checkSF = "";
        private static string checkCF = "";
        
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

                using (SqlConnection con = new SqlConnection(Db.connectionString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand check = new SqlCommand("SELECT * FROM account WHERE id = @id AND acctype=@accType", con);
                        check.Parameters.AddWithValue("@id", id);
                        check.Parameters.AddWithValue("@accType", accTypeFrm);
                        SqlDataReader sReader = check.ExecuteReader();

                        if (sReader.Read())
                        {
                            foundcustomer = sReader["accnumber"].ToString();
                            UserSession.UserAccount.Add(sReader["accnumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["id"].ToString());


                            if (foundcustomer != null)
                            {
                                accNumFrom.Text = foundcustomer;
                                accBalFrm.Text = acctrepo.GetDBBalance(foundcustomer).ToString();
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



                /* accTypeFrm = "Savings";
                 checkSF = accTypeFrm;
                 string id = AuthenticationRepository.CurrentUser.Id;
                 AccountData.LoadModelsFromFile();
                 var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accTypeFrm);
                 if (foundcustomer != null)
                 {
                     accNumFrom.Text = foundcustomer.AccountNumber;
                     accBalFrm.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
                     accDetailsFrm.Visible = true;
                     toPanel.Visible = true;
                 }
                 else
                 {
                     accDetailsFrm.Visible = false;
                     toPanel.Visible = false;
                 }*/

            }
            else if (!radioButton1.Checked)
            {
                accDetailsFrm.Visible = false;
                toPanel.Visible = false;
                radioButton1.Checked = false;

            }
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

                using (SqlConnection con = new SqlConnection(Db.connectionString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand check = new SqlCommand("SELECT * FROM account WHERE id = @id AND acctype=@accType", con);
                        check.Parameters.AddWithValue("@id", id);
                        check.Parameters.AddWithValue("@accType", accTypeFrm);
                        SqlDataReader sReader = check.ExecuteReader();

                        if (sReader.Read())
                        {
                            foundcustomer = sReader["accnumber"].ToString();
                            UserSession.UserAccount.Add(sReader["accnumber"].ToString());
                            UserSession.CurrentUserID.Add(sReader["id"].ToString());


                            if (foundcustomer != null)
                            {
                                accNumFrom.Text = foundcustomer;
                                accBalFrm.Text = acctrepo.GetDBBalance(foundcustomer).ToString();
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
                /* accTypeFrm = "Current";
                 checkCF = accTypeFrm;
                 string id = AuthenticationRepository.CurrentUser.Id;
                 AccountData.LoadModelsFromFile();
                 var foundcustomer = AccountData.Accounts.Find(c => c.OwnerId == id && c.Type == accTypeFrm);
                 if (foundcustomer != null)
                 {
                     accNumFrom.Text = foundcustomer.AccountNumber;
                     accBalFrm.Text = acctrepo.GetBalance(foundcustomer.AccountNumber).ToString();
                     accDetailsFrm.Visible = true;
                     toPanel.Visible = true;
                 }
                 else
                 {
                     accDetailsFrm.Visible = false;
                     toPanel.Visible = false;
                 }*/

            }
            else if (!radioButton2.Checked)
            {
                accDetailsFrm.Visible = false;
                toPanel.Visible = false;
                radioButton2.Checked = false;

            }
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

            try
            {
                string id = "";
                string note = "Transfer";
                int accno = 0;
                var recipient = recieverNum.Text;
                foreach (var item in UserSession.CurrentUserID)
                {
                    id = item.Split(",")[0];
                }
                foreach (var item in UserSession.UserAccount)
                {
                    accno = Convert.ToInt32(item.Split(",")[0]);
                }

                using (SqlConnection con = new SqlConnection(Db.connectionString))
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand store = new SqlCommand("insert into transactions (accnumber,amount,note,acctype,creationdate,id) values ('" + accno + "','" + -tranAmnt + "','" + note + "','" + accTypeFrm + "','" + DateTime.Now + "','" + id + "') ", con);
                        store.ExecuteNonQuery();
                       
                    }
                }

                using (SqlConnection con = new SqlConnection(Db.connectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        SqlCommand store = new SqlCommand("insert into transactions (accnumber,amount,note,acctype,creationdate,id) values ('" + recipient + "','" + tranAmnt + "','" + note + "','" + accTypeFrm + "','" + DateTime.Now + "','" + id + "') ", con);
                        store.ExecuteNonQuery();

                    }
                }

                MessageBox.Show("Transfer Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            toPanel.Visible = false;
            verifyBtn.Visible = false;
            passCfrm.Visible = false;
            confirm.Visible = false;
        }

        private void recieverNum_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string reAcc = recieverNum.Text;
                string recipient = null;
                // var accnumTo = AccountData.Accounts.Find(a => a.AccountNumber == reAcc);
                if (reAcc != null)
                {

                    using (SqlConnection con = new SqlConnection(Db.connectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            SqlCommand check = new SqlCommand("SELECT * FROM account WHERE accnumber=@reAcc", con);
                            check.Parameters.AddWithValue("@reAcc", reAcc);
                            SqlDataReader sReader = check.ExecuteReader();
                            while (sReader.Read())
                            {
                                recipient = sReader["id"].ToString();

                            }
                             sReader.Close();


                            SqlCommand checkid = new SqlCommand("SELECT * FROM customers WHERE id=@recipient", con);
                            checkid.Parameters.AddWithValue("@recipient", recipient);
                            sReader = checkid.ExecuteReader();
                            while (sReader.Read())
                            {
                                accNameTo.Text = sReader["firstname"].ToString().Trim() + " " + sReader["lastname"].ToString().Trim();
                             
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
            
        }

        private void recieverNum_Validated(object sender, EventArgs e)
        {
           
        }
    }
}
