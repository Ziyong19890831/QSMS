using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CertificateExpireAuto.App_Code
{
    public class DataHelper
    {

        private string connstr = string.Empty;
        private bool showSQL = false;
        private TimeSpan mExecutionTime;

        public TimeSpan ExecutionTime
        {
            get { return mExecutionTime; }
            set { mExecutionTime = value; }
        }

        public DataHelper(string connectionString)
        {
            connstr = connectionString;
        }

        public DataHelper()
        {
            connstr = ConfigurationManager.AppSettings["connectionstring"];
        }

        public DataTable getDataTable(string SQLString, Dictionary<string, object> SQLParameter, List<Dictionary<string, object>> arr, int type)
        {
            Stopwatch sw = new Stopwatch();

            DataTable dtable = new DataTable();
            List<string> sqllist = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    if (type == 1)
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(SQLString, conn))
                        {
                            sw.Start();
                            if (SQLParameter != null)
                            {
                                foreach (KeyValuePair<string, object> tempParameter in SQLParameter)
                                {
                                    da.SelectCommand.Parameters.AddWithValue(tempParameter.Key, tempParameter.Value);
                                }
                            }
                            sqllist.Add(getRealSql(da.SelectCommand));
                            da.Fill(dtable);

                            sw.Stop();
                            ExecutionTime = sw.Elapsed;

                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand(SQLString, conn))
                        {
                            if (arr != null)
                            {
                                sw.Start();
                                var trn = cmd.Connection.BeginTransaction();
                                cmd.Transaction = trn;
                                try
                                {
                                    foreach (Dictionary<string, object> dic in arr)
                                    {
                                        foreach (KeyValuePair<string, object> tempParameter in dic)
                                        {
                                            cmd.Parameters.AddWithValue(tempParameter.Key, tempParameter.Value);
                                        }
                                        sqllist.Add(getRealSql(cmd));
                                        cmd.ExecuteNonQuery();
                                    }
                                    trn.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trn.Rollback();
                                    throw ex;
                                }
                                sw.Stop();
                                ExecutionTime = sw.Elapsed;
                            }
                            else if (SQLParameter != null)
                            {
                                sw.Start();
                                foreach (KeyValuePair<string, object> tempParameter in SQLParameter)
                                {
                                    cmd.Parameters.AddWithValue(tempParameter.Key, tempParameter.Value);
                                }
                                sqllist.Add(getRealSql(cmd));
                                cmd.ExecuteNonQuery();
                                sw.Stop();
                                ExecutionTime = sw.Elapsed;
                            }
                        }
                    }
                    conn.Close();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return dtable;
        }

        //查詢數量
        public int queryCount(string SQLString)
        {
            return Convert.ToInt32(getDataTable(SQLString, null, null, 1).Rows[0][0]);
        }

        //查詢
        public DataTable queryData(string SQLString, Dictionary<string, object> SQLParameter)
        {
            return getDataTable(SQLString, SQLParameter, null, 1);
        }

        //新增
        public void executeNonQuery(string SQLString, Dictionary<string, object> SQLParameter)
        {
            try
            {
                getDataTable(SQLString, SQLParameter, null, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getRealSql(SqlCommand sqlcmd)
        {
            var sql = sqlcmd.CommandText;

            for (int i = 0; i < sqlcmd.Parameters.Count - 1; i++)
            {
                var tmp_op = sqlcmd.Parameters[i];
                var tmp_p = ":" + tmp_op.ParameterName.ToString().Replace(":", "");
                //age < 20 ? "What's up?" : "Hello";
                string name =
                    tmp_op.ParameterName.StartsWith(":")
                        ? tmp_op.ParameterName
                        : ":" + tmp_op.ParameterName;

                string value =
                    tmp_op.Value is DateTime    //obj is SuperHero
                        ? String.Format(
                            "TO_DATE('{0}', '{1}')",
                            Convert.ToDateTime(tmp_op.Value).ToString("yyyy/MM/dd HH:mm:ss"), "yyyy/mm/dd hh24:mi:ss"
                         )
                         : String.Format("'{0}'", tmp_op.Value);
                string pattern = string.Format("{0}(?=[/W])|{0}$", name);
                sql = Regex.Replace(sql, pattern, value, RegexOptions.IgnoreCase);
            }
            return sql;

        }
    }
}
