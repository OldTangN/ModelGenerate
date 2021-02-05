using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlHelper
{
    public class SQLHelp
    {
        public string userId = "";
        private string userPwd = "";
        private string dataBase = "";

        private SqlConnection connection;
        private SqlDataReader sdr;
        private SqlCommand cmd;

        public SQLHelp(string uId, string uPwd, string uDB)
        {
            userId = uId;
            userPwd = uPwd;
            dataBase = uDB;
        }

        public SqlConnection GetConnection()
        {
            string connectionString = "Server=.;UID='" + userId + "';PWD='" + userPwd + "';DATABASE='" + dataBase + "'";
            if (dataBase == "")
            {
                return null;
            }

            //try
            //{
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            else if (connection.State == ConnectionState.Broken)
            {
                connection.Close();
                connection.Open();
            }
            return connection;
        }

        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            using (cmd = new SqlCommand(cmdText, GetConnection()))
            {
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        public int ExecuteNonQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            int res;
            using (cmd = new SqlCommand(cmdText, GetConnection()))
            {
                cmd.Parameters.Add(paras);
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        public int ExecuteNonQuery(string cmdText, SqlParameter paras, CommandType ct)
        {
            int res;
            using (cmd = new SqlCommand(cmdText, GetConnection()))
            {
                cmd.Parameters.Add(paras);
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        public DataTable ExecuteQuery(string cmdText, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConnection());
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        public DataTable ExecuteQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConnection());
            cmd.Parameters.Add(paras);
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        public DataTable ExecuteQuery(string cmdText, SqlParameter paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConnection());
            cmd.Parameters.Add(paras);
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
    }
}

