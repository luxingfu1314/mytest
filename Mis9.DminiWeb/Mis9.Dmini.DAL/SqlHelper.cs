using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public static class SqlHelper
    {
        public static string ConnStr { get; set; }
        /// <summary>
        /// 提取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, CommandType type, params SqlParameter[] pars)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    using (SqlDataAdapter apter = new SqlDataAdapter(sql, conn))
                    {
                        if (pars != null)
                        {
                            apter.SelectCommand.Parameters.AddRange(pars);
                        }
                        apter.SelectCommand.CommandType = type;
                        DataTable da = new DataTable();
                        apter.Fill(da);
                        return da;
                    }
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("SqlHelper->GetDataTable:" + ex.ToString()+ "\r\nConnStr"+ ConnStr, @"c:\log\WebError");
            }
            return null;
        }
        /// <summary>
        /// 执行无返回数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int ExecuteNonquery(string sql, CommandType type, params SqlParameter[] pars)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (pars != null)
                        {
                            cmd.Parameters.AddRange(pars);
                        }
                        cmd.CommandType = type;
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("SqlHelper->ExecuteNonquery:" + ex.ToString() + "\r\nConnStr" + ConnStr, @"c:\log\WebError");
            }
            return -1;
            
        }
        /// <summary>
        /// 执行返回查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, CommandType type, params SqlParameter[] pars)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (pars != null)
                        {
                            cmd.Parameters.AddRange(pars);
                        }
                        cmd.CommandType = type;
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("SqlHelper->ExecuteScalar:" + ex.ToString() + "\r\nConnStr" + ConnStr, @"c:\log\WebError");
            }
            return null;

        }
        /// <summary>
        /// 测试连接数据库是否成功
        /// </summary>
        /// <returns></returns>
        public static bool ConnectionTest()
        {
            //创建连接对象
            var mySqlConnection = new SqlConnection(ConnStr);
            try
            {
                //Open DataBase
                //打开数据库
                mySqlConnection.Open();
                return true;
            }
            catch
            {
                //Can not Open DataBase
                //打开不成功 则连接不成功
                return false;
            }
            finally
            {
                //Close DataBase
                //关闭数据库连接
                mySqlConnection.Close();
            }
        }

        /// <summary>
        /// 获取标量值函数数据信息
        /// </summary>
        /// <param name="FunName">标量值函数名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static string GetStringValue(string FunName, params object[] parameters)
        {
            string sqlstr = "SELECT dbo." + FunName + "(); ";
            if (parameters != null)
                sqlstr = "SELECT dbo." + FunName + "('" + string.Join("','", parameters) + "'); ";
            DataTable table = GetDataTable(sqlstr, CommandType.Text, null);
            if (table == null || table.Rows.Count == 0)
                return "";
            else
                return table.Rows[0][0].ToString();
        }
        /// <summary>
        /// 获取标量值函数数据信息
        /// </summary>
        /// <param name="FunName">标量值函数名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int GetIntValue(string FunName, params object[] parameters)
        {
            string valueStr = GetStringValue(FunName, parameters);
            //
            int value = -1;
            if (int.TryParse(valueStr, out value))
                return value;
            return -1;
        }
        /// <summary>
        /// 获取唯一编码
        /// </summary>
        /// <returns></returns>
        public static string GetNewId()
        {
            string sqlstr = "SELECT NEWID();";
            DataTable table = GetDataTable(sqlstr, CommandType.Text, null);
            if (table == null || table.Rows.Count == 0)
                return "";
            else
                return table.Rows[0][0].ToString();
        }
    }
}
