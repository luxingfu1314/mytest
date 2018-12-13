using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public class PrescInfo
    {
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="sqlParameter"></param>
        /// <param name="values"></param>
        private void SetValue(ref SqlParameter[] sqlParameter, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                sqlParameter[i].Value = values[i];
            }
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public bool CreatePrescs(params object[] parameters)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[12];
                Parameters[0] = new SqlParameter("@DISPANSARY", SqlDbType.NVarChar, 40);
                Parameters[1] = new SqlParameter("@IDENTITY", SqlDbType.NVarChar, 40);
                Parameters[2] = new SqlParameter("@NAME", SqlDbType.NVarChar, 200);
                Parameters[3] = new SqlParameter("@SEX", SqlDbType.NVarChar, 40);
                Parameters[4] = new SqlParameter("@BIRTHDAY", SqlDbType.DateTime);
                Parameters[5] = new SqlParameter("@PHONENO", SqlDbType.NVarChar, 40);
                Parameters[6] = new SqlParameter("@COSTS", SqlDbType.Float);
                Parameters[7] = new SqlParameter("@MEDICALHISTORY", SqlDbType.NVarChar, 1000);
                Parameters[8] = new SqlParameter("@IMAGE", SqlDbType.Image);
                Parameters[9] = new SqlParameter("@DRUGINFO", SqlDbType.NVarChar, 2000);
                Parameters[10] = new SqlParameter("@OPFLG", SqlDbType.Char, 1);
                Parameters[11] = new SqlParameter("@PRESCNO", SqlDbType.NVarChar, 40);
                //
                SetValue(ref Parameters, parameters);
                //执行
                int res=SqlHelper.ExecuteNonquery("DT2_PROC_PRESC_CREATE", CommandType.StoredProcedure, Parameters);

                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public bool CreatePrescs_New(params object[] parameters)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[13];
                Parameters[0] = new SqlParameter("@DISPANSARY", SqlDbType.NVarChar, 40);
                Parameters[1] = new SqlParameter("@IDENTITY", SqlDbType.NVarChar, 40);
                Parameters[2] = new SqlParameter("@NAME", SqlDbType.NVarChar, 200);
                Parameters[3] = new SqlParameter("@SEX", SqlDbType.NVarChar, 40);
                Parameters[4] = new SqlParameter("@BIRTHDAY", SqlDbType.DateTime);
                Parameters[5] = new SqlParameter("@PHONENO", SqlDbType.NVarChar, 40);
                Parameters[6] = new SqlParameter("@COSTS", SqlDbType.Float);
                Parameters[7] = new SqlParameter("@MEDICALHISTORY", SqlDbType.NVarChar, 1000);
                Parameters[8] = new SqlParameter("@ALLERGYHISTORY", SqlDbType.NVarChar, 1000);
                Parameters[9] = new SqlParameter("@IMAGE", SqlDbType.Image);
                Parameters[10] = new SqlParameter("@DRUGINFO", SqlDbType.NVarChar, 2000);
                Parameters[11] = new SqlParameter("@OPFLG", SqlDbType.Char, 1);
                Parameters[12] = new SqlParameter("@PRESCNO", SqlDbType.NVarChar, 40);
                //
                SetValue(ref Parameters, parameters);
                //执行
                int res = SqlHelper.ExecuteNonquery("DT2_PROC_PRESC_CREATE_NEW", CommandType.StoredProcedure, Parameters);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取订单信息，含图片，药先知审方
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrescInfo1(string prescno)
        {
            string sql = "select * from DT2_MN_GET_PRESC1('" + prescno + "')";
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取订单信息，ERP以及自己订单详情数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrescInfo2(string prescno,string opflg)
        {
            string sql = "select * from DT2_MN_GET_PRESC2('" + prescno + "'" + ",'" + opflg + "')";
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 审方返回修改状态
        /// </summary>
        /// <returns></returns>
        public bool UpdatePrescs(params object[] parameters)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[3];
                Parameters[0] = new SqlParameter("@RETVALUE", SqlDbType.Char, 1);
                Parameters[1] = new SqlParameter("@RETMSG", SqlDbType.NVarChar, 40);
                Parameters[2] = new SqlParameter("@PRESCINFO", SqlDbType.Xml);

                SetValue(ref Parameters, parameters);
                //执行
                int res = SqlHelper.ExecuteNonquery("DT2_PROC_PRESC_UPDATE", CommandType.StoredProcedure, Parameters);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取审方返回情况
        /// </summary>
        public int GetCheckResult(string prescno, ref string retmsg)
        {
            string sql = "select isnull(max(opflg),0) as opflg,isnull(max(retmsg),'') as retmsg from dt2_presc_network_mstdb where presc_no='" + prescno + "'";
            DataTable table= SqlHelper.GetDataTable(sql, CommandType.Text);
            //
            int res = 0;
            if (table != null && table.Rows.Count > 0)
            {
                res=int.Parse(table.Rows[0]["opflg"].ToString());
                retmsg = table.Rows[0]["retmsg"].ToString();
            }
            return res;
        }

        /// <summary>
        /// 获取审方返回情况
        /// </summary>
        public int GetCheckEphedrine(string identity)
        {
            return SqlHelper.GetIntValue("DC2_FUNC_CHECK_EPHEDRINE", identity);
        }

        /// <summary>
        /// 支付成功后更新订单状态
        /// </summary>
        public bool UpdatePrescPay(params object[] parameters)
        {
            //订单号，付款总金额，付款方式1，付款金额1，付款方式2，付款金额2
            try
            {
                SqlParameter[] Parameters = new SqlParameter[6];
                Parameters[0] = new SqlParameter("@PRESCNO", SqlDbType.NVarChar, 40);
                Parameters[1] = new SqlParameter("@PAYMENTS", SqlDbType.Float);
                Parameters[2] = new SqlParameter("@PAYTYPE1", SqlDbType.NVarChar, 40);
                Parameters[3] = new SqlParameter("@PAYSUM1", SqlDbType.Float);
                Parameters[4] = new SqlParameter("@PAYTYPE2", SqlDbType.NVarChar, 40);
                Parameters[5] = new SqlParameter("@PAYSUM2", SqlDbType.Float);
               

                SetValue(ref Parameters, parameters);
                //执行
                int res = SqlHelper.ExecuteNonquery("DT2_PROC_PRESC_UPDATEFLG", CommandType.StoredProcedure, Parameters);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 201,208
        /// </summary>
        /// <returns></returns>
        public bool PrescProdure(params object[] parameters)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[11];
                Parameters[0] = new SqlParameter("@OPSYSTEM", SqlDbType.NVarChar, 40);
                Parameters[1] = new SqlParameter("@OPWINID", SqlDbType.NVarChar, 40);
                Parameters[2] = new SqlParameter("@OPTYPE", SqlDbType.NVarChar, 40);
                Parameters[3] = new SqlParameter("@OPSTR", SqlDbType.Xml);
                Parameters[4] = new SqlParameter("@OPSTR2", SqlDbType.NVarChar, 1000);
                Parameters[5] = new SqlParameter("@OPIP", SqlDbType.NVarChar, 200);
                Parameters[6] = new SqlParameter("@OPMANNO", SqlDbType.NVarChar, 200);
                Parameters[7] = new SqlParameter("@OPMANNAME", SqlDbType.NVarChar, 200);
                Parameters[8] = new SqlParameter("@DISPENSARY", SqlDbType.NVarChar, 40);
                Parameters[9] = new SqlParameter("@RETVAL", SqlDbType.Int);
                Parameters[9].Direction = ParameterDirection.Output;
                Parameters[9].Value = 0;
                Parameters[10] = new SqlParameter("@RETMSG", SqlDbType.NVarChar, 200);
                Parameters[10].Direction = ParameterDirection.Output;
                SetValue(ref Parameters, parameters);
                //执行
                int res = SqlHelper.ExecuteNonquery("DT2_CONSIS_TRANSDATA", CommandType.StoredProcedure, Parameters);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取发药状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public int GetSendOpflg(string prescno)
        {
            string sql= "select isnull(max(opflg),0) opflg from [dbo].[dt2_presc_mstdb] where presc_no = '"+ prescno + "'";
            DataTable table = SqlHelper.GetDataTable(sql, CommandType.Text);
            //
            int res = 0;
            if (table != null && table.Rows.Count > 0)
            {
                res = int.Parse(table.Rows[0]["opflg"].ToString());
            }
            return res;
        }
        /// <summary>
        /// 获取手动发药状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public int GetManSendOpflg(string prescno)
        {
            string sql = "select isnull(max(flg3),0) opflg from [dbo].[dc2_stu_man_presc_dtldb] where billno = '" + prescno + "'";
            DataTable table = SqlHelper.GetDataTable(sql, CommandType.Text);
            //
            int res = 0;
            if (table != null && table.Rows.Count > 0)
            {
                res = int.Parse(table.Rows[0]["opflg"].ToString());
            }
            return res;
        }
        /// <summary>
        /// 支付成功后米雅返回
        /// </summary>
        public bool MiyaPayRetSuccess(string prescno,string type,double payment,string billno)
        {
            try
            {
                string sql = "update dt2_presc_network_mstdb set opflg=3, flg1=2,payments="+ payment + ",billno='" + billno + "',paysum1=" + payment + ",paytype1=(select isnull(max(PaymentID),'') from dt2_basic_payment where PaymentName like '%" + type + "%') where presc_no='" + prescno + "'";
                //执行
                int res = SqlHelper.ExecuteNonquery(sql, CommandType.Text);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 支付成功后米雅返回
        /// </summary>
        public bool MiyaPayRetFail(string prescno)
        {
            //订单号，付款总金额，付款方式1，付款金额1，付款方式2，付款金额2
            try
            {
                string sql = "update dt2_presc_network_mstdb set flg1=1 where presc_no='" + prescno + "'";
                //执行
                int res = SqlHelper.ExecuteNonquery(sql, CommandType.Text);
                if (res < 0) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取付款状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public int GetPayOpflg(string prescno)
        {
            string sql = "select isnull(max(flg1),0) opflg from [dbo].[dt2_presc_network_mstdb] where presc_no = '" + prescno + "'";
            DataTable table = SqlHelper.GetDataTable(sql, CommandType.Text);
            //
            int res = 0;
            if (table != null && table.Rows.Count > 0)
            {
                res = int.Parse(table.Rows[0]["opflg"].ToString());
            }
            return res;
        }

        public bool UpDateSendDrugFlag(string prescno)
        {
            string sql = "update dt2_presc_network_mstdb set flg2=1 where presc_no = '" + prescno + "'";
            int res = SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            if (res < 0) return false;
            return true;
        }

        /// <summary>
        /// 创建手动发药订单
        /// </summary>
        /// <returns></returns>
        public bool CreateManPrescs(ref string prescno,params object[] parameters)
        {
            try
            {
                SqlParameter[] Parameters = new SqlParameter[4];
                Parameters[0] = new SqlParameter("@POSNO", SqlDbType.NVarChar, 40);
                Parameters[1] = new SqlParameter("@CONSISPOSNOS", SqlDbType.NVarChar, 1000);
                Parameters[2] = new SqlParameter("@DRUGINFO", SqlDbType.NVarChar, 2000);
                Parameters[3] = new SqlParameter("@PRESCNO", SqlDbType.NVarChar, 40);
                Parameters[3].Direction = ParameterDirection.Output;

                SetValue(ref Parameters, parameters);
                //执行
                int res = SqlHelper.ExecuteNonquery("DT2_PROC_MAN_PRESC", CommandType.StoredProcedure, Parameters);
                if (res < 0) return false;
                //
                prescno = Parameters[3].Value.ToString();
                //
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
