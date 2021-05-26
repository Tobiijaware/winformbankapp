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
    public partial class DashboardUsersControl : UserControl
    {
        private readonly IAccountRepository acctrepo;
        private static DashboardUsersControl _instance;
        public static DashboardUsersControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DashboardUsersControl(GlobalConfig.IAccountinstance);
                return _instance;
            }
        }
        public DashboardUsersControl(IAccountRepository Acctrepo)
        {
            InitializeComponent();
            acctrepo = Acctrepo;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void DashboardUsersControl_Load(object sender, EventArgs e)
        {

            string id = "";
            string foundcustomer = "";
            foreach (var item in UserSession.CurrentUserID)
            {
                id = item.Split(",")[0];
            }

            using (SqlConnection con = new SqlConnection(Db._conString))
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand check = new SqlCommand("SELECT * FROM Accounts WHERE userID = @id", con);
                    check.Parameters.AddWithValue("@id", id);

                    SqlDataReader sReader = check.ExecuteReader();
                    while (sReader.Read())
                    {
                        foundcustomer = sReader["AccType"].ToString().Trim();
                        var accno = sReader["AccNumber"].ToString().Trim();

                        if (foundcustomer == "Savings")
                        {
                            savAmnt.Text = acctrepo.GetDbBalance(accno).ToString();
                            UCSavingPanel.Visible = true;

                        }
                        else if (foundcustomer == "Current")
                        {
                            currAmnt.Text = acctrepo.GetDbBalance(accno).ToString();
                            UCcurrentPanel.Visible = true;
                        }
                    }

                }
            }

            //string id = AuthenticationRepository.CurrentUser.Id;
            //AccountData.LoadModelsFromFile();
            //var foundcustomer = AccountData.Accounts.FindAll(c => c.OwnerId == id);
            //foreach (var item in foundcustomer)
            //{
            //    if (item.Type == "Savings")
            //    {
            //            savAmnt.Text = acctrepo.GetBalance(item.AccountNumber).ToString();
            //        UCSavingPanel.Visible = true;
            //    }
            //    else if (item.Type == "Current")
            //    {
            //        currAmnt.Text = acctrepo.GetBalance(item.AccountNumber).ToString();
            //        UCcurrentPanel.Visible = true;
            //    }
            //}

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UCcurrentPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void UCSavingPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
