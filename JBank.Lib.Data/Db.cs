using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JBank.Lib.Data
{
    public static class Db
    {

       /* protected SqlConnection _conn;
        protected SqlCommand _cmd;*/
        public static string connectionString = "Data Source=LAPTOP-R0IBRK5V;Initial Catalog=bankapp;Integrated Security=True";

       /* public SqlConnection GetConnection()
        {
            _conn = new SqlConnection(_conStr);
            return _conn;
        }

        public async Task<bool> QueryExecute(string stmt)
        {
            GetConnection();
            using (_cmd = new SqlCommand(stmt, _conn))
            {                
                if (_conn == null)
                    throw new Exception("Failed to connect");

                _conn.Open();
                await _cmd.ExecuteNonQueryAsync();
            }
            _conn.Close();
            return true;
        }

        public async Task<bool> QueryExecuteTran(string stmt, string stmt2)
        {
            var conn = GetConnection(); 
            conn.Open();
            var tran = conn.BeginTransaction();

            using (_cmd = new SqlCommand(stmt, _conn))
            {
                try
                {
                    if (conn == null)
                        throw new Exception("Failed to connect");

                    _cmd.Transaction = tran;

                    //_cmd.CommandType = System.Data.CommandType.Text;

                    //_conn.Open();
                    await _cmd.ExecuteNonQueryAsync();

                    // second statment
                    _cmd.CommandText = stmt2;
                    await _cmd.ExecuteNonQueryAsync();

                    tran.Commit();
                }catch(Exception)
                {
                    tran.Rollback();
                }
            }
            
            tran.Dispose();
            _conn.Close();
            return true;
            
        }*/
        
    }
}
