using SqlHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelDesign
{
    public partial class frmModelBuilder : Form
    {
        public frmModelBuilder()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string UID = txtUser.Text.Trim();
            string UPWD = txtPwd.Text.Trim();
            string DB = txtDB.Text.Trim();
            string SERVEr = txtServer.Text.Trim();

            mysqlhelper = new MySqlHelp(SERVEr, UID, UPWD, DB);

            if (mysqlhelper.GetConnection() == null)
            {
                lblConnection.Text = "连接失败！！！";
            }
            else
            {
                lblConnection.Text = "连接成功~@@~";
                GetTables();
            }
        }

        private MySqlHelp mysqlhelper;// = new MySqlHelp("10.136.74.230", "root", "Haier@74230", "tbzdgx");

        public DataTable GetTables()
        {

            //string[] restriction = new string[4];
            //restriction[1] = "dbo";
            //var conn = sqlhelper.GetConnection();     
            //DataTable dt = conn.GetSchema("Tables", restriction);
            //conn.Close();

            var myconn = mysqlhelper.GetConnection();
            //DataTable dt = myconn.GetSchema("Tables", restriction);

            var dt = myconn.GetSchema("Tables");
            //myconn.GetSchema( "Restrictions" )
            foreach (System.Data.DataRow row in dt.Rows)
            {
                this.cmbTables.Items.Add(row["TABLE_NAME"]);//row[1]             

            }

            this.cmbTables.SelectedIndex = 0;
            return dt;
        }

        public DataTable GetColumns()
        {
            var conn = mysqlhelper.GetConnection();
            string[] restriction = new string[4];
            restriction[2] = cmbTables.Text;
            DataTable dt = conn.GetSchema("Columns", restriction);
            conn.Close();
            return dt;
        }

        private string DBTypeTransformToCSType(string dbType)
        {
            string csType = "UnknowType";
            switch (dbType)//row[7]
            {
                case "text":
                case "varchar":
                case "nvarchar":
                    csType = "string";
                    break;
                case "bit":
                    csType = "bool";
                    break;
                case "tinyint":
                    csType = "byte";
                    break;
                case "int":
                    csType = "int";
                    break;
                case "bigint":
                    csType = "long";
                    break;
                case "datetime":
                case "date":
                case "time":
                case "timestamp":
                    csType = "DateTime";
                    break;
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    csType = "decimal";
                    break;
                case "real":
                    csType = "float";
                    break;
                case "float":
                    csType = "float";
                    break;
                case "double":
                    csType = "double";
                    break;
                case "smallint":
                    csType = "short";
                    break;
                default:
                    break;
            }
            return csType;
        }

        public void WriteText(DataTable dt)
        {
            //创建类
            string uid = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string db = txtDB.Text.Trim();
            string tableName = cmbTables.Text;
            string tbName = tableName;

            StringBuilder builder = new StringBuilder();
            builder.Append($"public class {tbName}\r\n");
            builder.Append("{\r\n");

            //构造字段
            foreach (DataRow row in dt.Rows)
            {
                string csType = DBTypeTransformToCSType(row["DATA_TYPE"].ToString());//row[7]
                builder.Append($"{t}private {csType} {GetField(row["COLUMN_NAME"].ToString())};\r\n");//row[3]
            }
            builder.Append("\r\n");
            //构造空的类方法
            builder.Append($"{t}public {tbName}()" + " {  }\r\n");
            builder.Append("\r\n");
            //构造属性
            foreach (DataRow row in dt.Rows)
            {
                builder.Append($"{t}/// <summary>\r\n");
                builder.Append($"{t}/// {row["COLUMN_COMMENT"]}\r\n");
                builder.Append($"{t}/// </summary>\r\n");
                string csType = DBTypeTransformToCSType(row["DATA_TYPE"].ToString());//row[7]
                builder.Append($"{t}public {csType} {row["COLUMN_NAME"].ToString()}\r\n");
                builder.Append(t + "{\r\n");
                builder.Append(t + t + "get{ " + "return " + GetField(row["COLUMN_NAME"].ToString()) + "; }\r\n");
                builder.Append(t + t + "set{ " + GetField(row["COLUMN_NAME"].ToString()) + " = value; }\r\n");
                builder.Append(t + "}\r\n");
            }
            builder.Append("}");
            this.txtTable.Text = builder.ToString();
        }

        private string t = "    ";

        private void btnBuild_Click(object sender, EventArgs e)
        {
            WriteText(GetColumns());
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string FirstLower(string s)
        {
            return s.Substring(0, 1).ToLower() + s.Substring(1, s.Length - 1);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string FirstUpper(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
        }

        private string GetField(string s)
        {
            return "_" + s;
        }


        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtTable.Text);
        }


    }
}
