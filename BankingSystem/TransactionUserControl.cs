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
    public partial class TransactionUserControl : UserControl
    {

        private static TransactionUserControl _instance;
        public static TransactionUserControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TransactionUserControl();
                return _instance;
            }
        }

        public TransactionUserControl()
        {
            InitializeComponent();
        }

        private void TransactionUserControl_Load(object sender, EventArgs e)
        {
            string id = "";
            string display = "";
            foreach (var item in UserSession.CurrentUserID)
            {
                id = item.Split(",")[0];
            }

            using (SqlConnection con = new SqlConnection(Db.connectionString))
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand checktransac = new SqlCommand("SELECT * FROM transactions WHERE id = @id", con);
                    checktransac.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader sReader = checktransac.ExecuteReader();
                    while (sReader.Read())
                    {
                         display = sReader[0].ToString().Trim() + " " + sReader[1].ToString().Trim() + " " + sReader[2].ToString().Trim() + " " + sReader[3].ToString().Trim()+" "+ sReader[4].ToString().Trim();
                    }
                }
            }
          
        }

        private void transaction_panel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
