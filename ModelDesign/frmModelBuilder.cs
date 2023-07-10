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

        public void WriteText(DataTable dt)
        {

            //创建类
            string uid = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string db = txtDB.Text.Trim();
            string tableName = cmbTables.Text;
            string tbName = tableName;
            //if (tableName.StartsWith("t_"))
            //{
            //    tbName = tableName.Substring(2, tableName.Length - 2);
            //}
            //else
            //{
            //    tbName = tableName;
            //}
            string conText = $"public class {tbName}\r\n";
            conText += "{\r\n";

            //构造字段
            foreach (System.Data.DataRow row in dt.Rows)
            {
                switch (row["DATA_TYPE"].ToString())//row[7]
                {
                    case "text":
                    case "varchar":
                    case "nvarchar":
                        conText += $"{t}private string {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                    case "bit":
                    case "tinyint":
                    case "int":                  
                        conText += $"{t}private int {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                    case "bigint":
                        conText += $"{t}private long {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                    case "datetime":
                        conText += $"{t}private DateTime {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                    case "float":
                    case "decimal":
                        conText += $"{t}private float {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                    default:
                        conText += $"{t}private string {GetField(row["COLUMN_NAME"].ToString())};\r\n";//row[3]
                        break;
                }
            }

            //构造空的类方法
            conText += $"{t}public {tbName}()" + " {  }\r\n";

            //构造属性
            foreach (System.Data.DataRow row in dt.Rows)
            {
                conText += $"{t}/// <summary>\r\n";
                conText += $"{t}/// {row["COLUMN_COMMENT"]}\r\n";
                conText += $"{t}/// </summary>\r\n";
                switch (row["DATA_TYPE"].ToString())//row[7]
                {
                    case "text":
                    case "varchar":
                    case "nvarchar":
                        {
                            conText += $"{t}public string {row["COLUMN_NAME"].ToString()}\r\n";
                            break;
                        }
                    case "bit":
                    case "tinyint":
                    case "int":
                    case "bigint":
                        {
                            conText += $"{t}public int {row["COLUMN_NAME"].ToString()}\r\n";
                            break;
                        }
                    case "datetime":
                        conText += $"{t}public DateTime {row["COLUMN_NAME"].ToString()}\r\n";
                        break;
                    case "float":
                    case "decimal":
                        conText += $"{t}public float {row["COLUMN_NAME"].ToString()}\r\n";
                        break;
                    default:
                        conText += $"{t}public string {row["COLUMN_NAME"].ToString()}\r\n";
                        break;
                }
                conText += (t + "{\r\n");
                conText += (t + t + "get{ " + "return " + GetField(row["COLUMN_NAME"].ToString()) + "; }\r\n");
                conText += (t + t + "set{ " + GetField(row["COLUMN_NAME"].ToString()) + " = value; }\r\n");
                conText += (t + "}\r\n");
            }
            conText += "}";
            this.txtTable.Text = conText;
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
