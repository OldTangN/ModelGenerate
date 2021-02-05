using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlHelp
    {
        public string userId = "";
        private string userPwd = "";
        private string dataBase = "";
        private string server = "";

        private MySqlConnection connection;
        private MySqlDataReader sdr;
        private MySqlCommand cmd;

        public MySqlHelp(string server, string uId, string uPwd, string uDB)
        {
            userId = uId;
            userPwd = uPwd;
            dataBase = uDB;
            this.server = server;
        }

        public MySqlConnection GetConnection()
        {
            //server=10.136.74.230;Port=3306;uid=root;pwd=Haier@74230;database=tbzdgx;Charset=utf8;Connection Timeout=5;
            string connectionString = "server=" + server + ";port=3306;uid=" + userId + ";pwd=" + userPwd + ";database=" + dataBase + ";";
            if (dataBase == "")
            {
                return null;
            }

            //try
            //{
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
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
            using (cmd = new MySqlCommand(cmdText, GetConnection()))
            {
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        public int ExecuteNonQuery(string cmdText, MySqlParameter[] paras, CommandType ct)
        {
            int res;
            using (cmd = new MySqlCommand(cmdText, GetConnection()))
            {
                cmd.Parameters.Add(paras);
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        public int ExecuteNonQuery(string cmdText, MySqlParameter paras, CommandType ct)
        {
            int res;
            using (cmd = new MySqlCommand(cmdText, GetConnection()))
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
            cmd = new MySqlCommand(cmdText, GetConnection());
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        public DataTable ExecuteQuery(string cmdText, MySqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new MySqlCommand(cmdText, GetConnection());
            cmd.Parameters.Add(paras);
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        public DataTable ExecuteQuery(string cmdText, MySqlParameter paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new MySqlCommand(cmdText, GetConnection());
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

