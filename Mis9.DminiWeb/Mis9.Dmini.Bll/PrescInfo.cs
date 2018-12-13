using Aspose.BarCode;
using Mis9.Dmini.Bll.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace Mis9.Dmini.Bll
{
    /// <summary>
    /// 处方订单，状态说明:0处方药提交审核，1处方药审核未通过，2待付款，3已付款
    /// </summary>
    public class PrescInfo : HttpCommon
    {
        DAL.PrescInfo preConfig = new DAL.PrescInfo();
        /// <summary>
        /// 创建订单并保存数据
        /// </summary>
        public void SavePrescInfo()
        {
            try
            {
                //数据获取
                string dispensary = Config.Instance().Dispensary;//药店编号
                string identity = RequestStr.Request["identity"];//身份证号
                string name = RequestStr.Request["name"];//姓名
                string sex = RequestStr.Request["sex"];//性别
                string strbirthday = RequestStr.Request["birthday"];//出生日期
                DateTime Birthday = DateTime.Now;
                if (!string.IsNullOrEmpty(strbirthday))
                {
                    int value = 0;
                    //取前四位
                    int year = DateTime.Now.Year;
                    string yearStr = strbirthday.Substring(0, 4);
                    if (int.TryParse(yearStr, out value))
                        year = value;
                    //取两位
                    int month = DateTime.Now.Month;
                    string monthStr = strbirthday.Substring(4, 2);
                    if (int.TryParse(monthStr, out value))
                        month = value;
                    //取两位
                    
                    int day = DateTime.Now.Day;
                    string dayStr = strbirthday.Substring(6, 2);
                    if (int.TryParse(dayStr, out value))
                        day = value;

                    Birthday = new DateTime(year,month,day);
                }
                string phoneno = RequestStr.Request["phoneno"];//电话号码
                string coststr = RequestStr.Request["costs"];//总金额
                double costs = double.Parse(coststr);//总金额
                string medicalHistory = RequestStr.Request["medicalHistory"];//病史
                string allergyHistory = RequestStr.Request["allergyHistory"];//过敏史
                string prescPic = RequestStr.Request["prescPic"];//Base64图片
                object picBytes = DBNull.Value;
                if (!string.IsNullOrEmpty(prescPic))
                {
                    picBytes = Convert.FromBase64String(prescPic);
                }
                string druginfo = RequestStr.Request["druginfo"];//drugid列表
                int opflg = int.Parse(RequestStr.Request["opflg"]);
                //创建单号
                DateTime now = DateTime.Now;
                TimeSpan ts = new TimeSpan(now.Ticks);
                long ticks = (long)ts.TotalMilliseconds;
                ticks = ticks % (24 * 3600 * 1000);
                //
                string prescno = now.ToString("yyyyMMdd") + "2" + ticks.ToString("00000000");
                //存储数据
                //bool res = preConfig.CreatePrescs_New(dispensary, identity, name, sex, Birthday, phoneno, costs, medicalHistory, allergyHistory, picBytes, druginfo, opflg, prescno);
                bool res = preConfig.CreatePrescs(dispensary, identity, name, sex, Birthday, phoneno, costs, medicalHistory.Replace("#","") + "##" + allergyHistory.Replace("#", ""), picBytes, druginfo, opflg, prescno);
                //
                if (res)
                {
                    string json = "{\"code\":\"200\",\"prescno\":\"" + prescno + "\",\"msg\":\"订单创建成功\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("订单创建失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SavePrescInfo:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        private YXZCheckEntity GetYXZCheck(string prescno)
        {
            
            DataTable table = preConfig.GetPrescInfo1(prescno);
            //
            if (table != null && table.Rows.Count > 0)
            {
                YXZCheckEntity yXZCheckEntity = new YXZCheckEntity();
                DataRow firstRow = table.Rows[0];
                GetMstPresc(ref yXZCheckEntity, firstRow);
                //
                foreach (DataRow row in table.Rows)
                {
                    GetDtlPresc(ref yXZCheckEntity, row);
                }
                return yXZCheckEntity;
            }
            else
            {
                //数据提取失败
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetYXZCheck:未提取到订单"+ prescno + "数据", @"c:\log\WebError");
                return null;
            }
        }
        private void GetMstPresc(ref YXZCheckEntity yXZCheckEntity, DataRow Row)
        {
            yXZCheckEntity.Presc_date = ((DateTime)Row["presc_date"]).ToString("yyyy-MM-dd HH:mm:ss");
            yXZCheckEntity.Presc_no = Row["presc_no"].ToString() ;
            if (Row["dispensary"] != DBNull.Value)
                yXZCheckEntity.Dispensary = Row["dispensary"].ToString();
            if (Row["patient_id"] != DBNull.Value)
                yXZCheckEntity.Patient_id = Row["patient_id"].ToString();
            if (Row["patient_name"] != DBNull.Value)
                yXZCheckEntity.Patient_name = Row["patient_name"].ToString();
            if (Row["patient_type"] != DBNull.Value)
                yXZCheckEntity.Patient_type = Row["patient_type"].ToString();
            if (Row["date_of_birth"] != DBNull.Value)
                yXZCheckEntity.Date_of_birth = ((DateTime)Row["date_of_birth"]).ToString("yyyy-MM-dd");
            if (Row["sex"] != DBNull.Value)
                yXZCheckEntity.Sex = Row["sex"].ToString();
            if (Row["presc_identity"] != DBNull.Value)
                yXZCheckEntity.Presc_identity = Row["presc_identity"].ToString();
            if (Row["charge_type"] != DBNull.Value)
                yXZCheckEntity.Charge_type = Row["charge_type"].ToString();
            if (Row["rcpt_info"] != DBNull.Value)
                yXZCheckEntity.Rcpt_info = Row["rcpt_info"].ToString();
            if (Row["repetition"] != DBNull.Value)
                yXZCheckEntity.Repetition =int.Parse(Row["repetition"].ToString());
            if (Row["costs"] != DBNull.Value)
                yXZCheckEntity.Costs = Double.Parse(Row["costs"].ToString());
            if (Row["ordered_by"] != DBNull.Value)
                yXZCheckEntity.Ordered_by = Row["ordered_by"].ToString();
            if (Row["ordered_by_name"] != DBNull.Value)
                yXZCheckEntity.Ordered_by_name = Row["ordered_by_name"].ToString();
            if (Row["prescribed_by"] != DBNull.Value)
                yXZCheckEntity.Prescribed_by = Row["prescribed_by"].ToString();
            if (Row["entered_by"] != DBNull.Value)
                yXZCheckEntity.Entered_by = Row["entered_by"].ToString();
            if (Row["entered_by"] != DBNull.Value)
                yXZCheckEntity.Entered_by = Row["entered_by"].ToString();
            if (Row["value"] != DBNull.Value)
            {
                byte[] imgBytes = (byte[])Row["value"];
                string value = Convert.ToBase64String(imgBytes);
                yXZCheckEntity.RcptPic = value;
            }
        }
        private void GetDtlPresc(ref YXZCheckEntity yXZCheckEntity,DataRow Row)
        {
            YXZCheckDTLEntity yXZCheckDTLEntity = new YXZCheckDTLEntity();
            //
            yXZCheckDTLEntity.Presc_no = Row["presc_no"].ToString();
            yXZCheckDTLEntity.Item_no = int.Parse(Row["item_no"].ToString());
            if (Row["advice_code"] != DBNull.Value)
                yXZCheckDTLEntity.Advice_code = Row["advice_code"].ToString();
            yXZCheckDTLEntity.Drug_code = Row["drug_code"].ToString();
            if (Row["drug_spec"] != DBNull.Value)
                yXZCheckDTLEntity.Drug_spec = Row["drug_spec"].ToString();
            yXZCheckDTLEntity.Drug_name = Row["drug_name"].ToString();
            if (Row["firm_id"] != DBNull.Value)
                yXZCheckDTLEntity.Firm_id = Row["firm_id"].ToString();
            if (Row["firm_name"] != DBNull.Value)
                yXZCheckDTLEntity.Firm_name = Row["firm_name"].ToString();
            if (Row["package_spec"] != DBNull.Value)
                yXZCheckDTLEntity.Package_spec = Row["package_spec"].ToString();
            if (Row["package_units"] != DBNull.Value)
                yXZCheckDTLEntity.Package_spec = Row["package_units"].ToString();
            if (Row["makeno"] != DBNull.Value)
                yXZCheckDTLEntity.Makeno = Row["makeno"].ToString();
            if (Row["invalidate"] != DBNull.Value)
                yXZCheckDTLEntity.Invalidate = ((DateTime)Row["invalidate"]).ToString("yyyy-MM-dd");
            if (Row["makedate"] != DBNull.Value)
                yXZCheckDTLEntity.Makedate = ((DateTime)Row["makedate"]).ToString("yyyy-MM-dd");
            yXZCheckDTLEntity.Quantity = (int)double.Parse(Row["quantity"].ToString());
            yXZCheckDTLEntity.Costs = double.Parse(Row["itemcosts"].ToString());
            yXZCheckDTLEntity.Price = double.Parse(Row["price"].ToString());
            if (Row["dosage"] != DBNull.Value)
                yXZCheckDTLEntity.Dosage = Row["dosage"].ToString();
            if (Row["dosage_units"] != DBNull.Value)
                yXZCheckDTLEntity.Dosage_units = Row["dosage_units"].ToString();
            if (Row["administration"] != DBNull.Value)
                yXZCheckDTLEntity.Administration = Row["administration"].ToString();
            if (Row["frequency"] != DBNull.Value)
                yXZCheckDTLEntity.Frequency = Row["frequency"].ToString();
            //
            yXZCheckEntity.productList.Add(yXZCheckDTLEntity);
        }
        /// <summary>
        /// 获取审方返回情况
        /// </summary>
        public void GetCheckResult()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //
                string retmsg = "";
                int opflg = preConfig.GetCheckResult(prescno, ref retmsg);
                string json = "{\"opflg\":\"" + opflg + "\",\"retmsg\":\"" + retmsg + "\"}";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetCheckResult:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 核查该顾客今日是否存在黄麻碱药品订单
        /// </summary>
        public void CheckEphedrine()
        {
            try
            {
                string identity = HttpUtility.UrlDecode(RequestStr.Request["identity"]);
                //200 可购买，300 不可购买（列出药品列表）
                int count = preConfig.GetCheckEphedrine(identity);
                int code = 200;
                if (count > 0)
                    code = 300;
                //返回回馈数据
                string json = "{\"code\":\""+ code + "\"}";
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->CheckEphedrine:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 支付成功后更新订单状态
        /// </summary>
        public void UpdatePrescPay()
        {
            //订单号，付款总金额，付款方式1，付款金额1，付款方式2，付款金额2
            try
            {
                //处方详细信息
                string prescno = RequestStr.Request["prescno"];
                string payments = RequestStr.Request["payments"];
                string paytype1 = RequestStr.Request["paytype1"];
                string paysum1 = RequestStr.Request["paysum1"];
                string paytype2 = RequestStr.Request["paytype2"];
                string paysum2 = RequestStr.Request["paysum2"];
                //
                bool res = preConfig.UpdatePrescPay(prescno, payments, paytype1, paysum1, paytype2, paysum2);
                //返回
                string json = "{\"code\":\"200\",\"msg\":\"" + "成功" + "\"}";
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                string json = "{\"code\":\"300\",\"msg\":\"" + ex.ToString() + "\"}";
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 发药
        /// </summary>
        public void SendDrugs()
        {
            try
            {
                //
                string dispensary = Config.Instance().Dispensary;//药店编号
                string prescno = RequestStr.Request["prescno"];
                //处方详细信息
                string posNo = RequestStr.Request["posno"];
                //获取订单信息
                DataTable table = preConfig.GetPrescInfo2(prescno,"3");
                //状态为0
                if (table != null && table.Rows.Count > 0)
                {
                    //获取当前窗口
                    int disportNumber = 0;
                    DAL.DeviceInfo device = new DAL.DeviceInfo();
                    device.GetDisportNumber(posNo, ref disportNumber);
                    //组合200XML
                    string Xml200=Get200XML(table, disportNumber);
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendDrugs200数据:" + Xml200, @"c:\log\WebInfo");
                    //发送201
                    DAL.PrescInfo prescinfo = new DAL.PrescInfo();
                    bool res=prescinfo.PrescProdure("HIS", disportNumber, "200", Xml200, "","","","24H", dispensary);
                    if (!res)
                    {
                        string json1 = "{\"code\":\"300\",\"msg\":\"" + "处方传输失败！" + "\"}";
                        RequestStr.Response.Write(json1);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    ////休眠10s
                    //Thread.Sleep(10000);
                    ////组合208XML
                    //string Xml208 = Get208XML(table.Rows[0],disportNumber);
                    //Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendDrugs202数据:" + Xml208, @"c:\log\WebInfo");
                    ////发送208
                    //prescinfo.PrescProdure("HIS", disportNumber, "208", Xml208, "", "", "", "24H", dispensary);
                    //if (!res)
                    //{
                    //    string json1 = "{\"code\":\"300\",\"msg\":\"" + "处方发药失败！" + "\"}";
                    //    RequestStr.Response.Write(json1);
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    return;
                    //}
                    //记录发药状态
                    prescinfo.UpDateSendDrugFlag(prescno);
                    //返回
                    string json = "{\"code\":\"200\",\"msg\":\"" + "成功" + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    //返回
                    string json = "{\"code\":\"300\",\"msg\":\"" + "无订单信息" + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                string json = "{\"code\":\"300\",\"msg\":\"" + ex.ToString() + "\"}";
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 组合200
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private string Get200XML(DataTable table, int winid)
        {
            DataRow row = table.Rows[0];
            ROOT root = new ROOT();
            root.OPWINID = winid.ToString();
            Get200MSTXML(ref root,row);
            root.CONSIS_PRESC_MSTVW.DTLLIST = new List<CONSIS_PRESC_DTLVW>();
            foreach (DataRow item in table.Rows)
            {
                Get200DTLXML(ref root, item);
            }
            //
            string xml= XmlHelper.XmlSerialize(root);
            //
            xml = xml.Replace("<DTLLIST>", "");
            xml = xml.Replace("</DTLLIST>", "");
            return xml;
        }
        /// <summary>
        /// 主表
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private void Get200MSTXML(ref ROOT root,DataRow row)
        {
            CONSIS_PRESC_MSTVW mst = new CONSIS_PRESC_MSTVW();
            mst.PRESC_DATE = ((DateTime)row["presc_date"]).ToString("yyyy-MM-dd HH:mm:ss");
            mst.PRESC_NO = row["presc_no"].ToString();
            if (row["dispensary"] != DBNull.Value)
                mst.DISPENSARY = row["dispensary"].ToString();
            if (row["patient_id"] != DBNull.Value)
                mst.PATIENT_ID = row["patient_id"].ToString();
            if (row["patient_name"] != DBNull.Value)
                mst.PATIENT_NAME = row["patient_name"].ToString();
            if (row["invoice_no"] != DBNull.Value)
                mst.INVOICE_NO = row["invoice_no"].ToString();
            if (row["patient_type"] != DBNull.Value)
                mst.PATIENT_TYPE = row["patient_type"].ToString();
            if (row["date_of_birth"] != DBNull.Value)
                mst.DATE_OF_BIRTH = ((DateTime)row["presc_date"]).ToString("yyyy-MM-dd HH:mm:ss");
            if (row["sex"] != DBNull.Value)
                mst.SEX = row["sex"].ToString();
            if (row["presc_identity"] != DBNull.Value)
                mst.PRESC_IDENTITY = row["presc_identity"].ToString();
            if (row["charge_type"] != DBNull.Value)
                mst.CHARGE_TYPE = row["charge_type"].ToString();
            if (row["presc_attr"] != DBNull.Value)
                mst.PRESC_ATTR = row["presc_attr"].ToString();
            if (row["presc_info"] != DBNull.Value)
                mst.PRESC_INFO = row["presc_info"].ToString();
            if (row["rcpt_info"] != DBNull.Value)
                mst.RCPT_INFO = row["rcpt_info"].ToString();
            if (row["rcpt_remark"] != DBNull.Value)
                mst.RCPT_REMARK = row["rcpt_remark"].ToString();
            if (row["repetition"] != DBNull.Value)
                mst.REPETITION =int.Parse(row["repetition"].ToString());
            if (row["costs"] != DBNull.Value)
            {
                mst.COSTS = double.Parse(row["costs"].ToString());
                mst.PAYMENTS = mst.COSTS;
            }
            if (row["ordered_by"] != DBNull.Value)
                mst.ORDERED_BY = row["ordered_by"].ToString();
            if (row["ordered_by_name"] != DBNull.Value)
                mst.ORDERED_BY_NAME = row["ordered_by_name"].ToString();
            if (row["prescribed_by"] != DBNull.Value)
                mst.PRESCRIBED_BY = row["prescribed_by"].ToString();
            if (row["entered_by"] != DBNull.Value)
                mst.ENTERED_BY = row["entered_by"].ToString();
            mst.DISPENSE_PRI =int.Parse(row["dispense_pri"].ToString());
            if (row["IsCharged"] != DBNull.Value)
                mst.ISCHARGED = row["IsCharged"].ToString();
            if (row["IsDrug"] != DBNull.Value)
                mst.ISDRUG = row["IsDrug"].ToString();

            root.CONSIS_PRESC_MSTVW = mst;
        }
        /// <summary>
        /// 详细表
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private void Get200DTLXML(ref ROOT root, DataRow row)
        {
            CONSIS_PRESC_DTLVW dtl = new CONSIS_PRESC_DTLVW();
            dtl.PRESC_NO = row["presc_no"].ToString();
            dtl.ITEM_NO = int.Parse(row["item_no"].ToString());
            if (row["advice_code"] != DBNull.Value)
                dtl.ADVICE_CODE = row["advice_code"].ToString();
            dtl.DRUG_CODE = row["drug_code"].ToString();
            if (row["drug_spec"] != DBNull.Value)
                dtl.DRUG_SPEC = row["drug_spec"].ToString();
            dtl.DRUG_NAME = row["drug_name"].ToString();
            if (row["firm_id"] != DBNull.Value)
                dtl.FIRM_ID = row["firm_id"].ToString();
            if (row["firm_name"] != DBNull.Value)
                dtl.FIRM_NAME = row["firm_name"].ToString();
            if (row["package_spec"] != DBNull.Value)
                dtl.PACKAGE_SPEC = row["package_spec"].ToString();
            if (row["package_units"] != DBNull.Value)
                dtl.PACKAGE_UNITS = row["package_units"].ToString();
            if (row["makeno"] != DBNull.Value)
                dtl.MAKENO = row["makeno"].ToString();
            if (row["invalidate"] != DBNull.Value)
                dtl.INVALIDATE = ((DateTime)row["invalidate"]).ToString("yyyy-MM-dd HH:mm:ss");
            if (row["makedate"] != DBNull.Value)
                dtl.MAKEDATE = ((DateTime)row["makedate"]).ToString("yyyy-MM-dd HH:mm:ss");
            dtl.QUANTITY = (int)double.Parse(row["consisquantity"].ToString());//小单位发药量
            dtl.UNIT = row["unit"].ToString();
            dtl.COSTS = double.Parse(row["itemcosts"].ToString());
            dtl.PAYMENTS = dtl.COSTS;
            if (row["dosage"] != DBNull.Value)
                dtl.DOSAGE = row["dosage"].ToString();
            if (row["dosage_units"] != DBNull.Value)
                dtl.DOSAGE_UNITS = row["dosage_units"].ToString();
            if (row["administration"] != DBNull.Value)
                dtl.ADMINISTRATION = row["administration"].ToString();
            if (row["frequency"] != DBNull.Value)
                dtl.FREQUENCY = row["frequency"].ToString();
            if (row["additionusage"] != DBNull.Value)
                dtl.ADDITIONUSAGE = row["additionusage"].ToString();
            if (row["itemremark"] != DBNull.Value)
                dtl.RCPT_REMARK = row["itemremark"].ToString();
            //
            root.CONSIS_PRESC_MSTVW.DTLLIST.Add(dtl);
        }
        /// <summary>
        /// 组合208
        /// </summary>
        /// <param name="row"></param>
        /// <param name="winid"></param>
        /// <returns></returns>
        private string Get208XML(DataRow row, int winid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ROOT>");
            sb.Append("<OPSYSTEM>HIS</OPSYSTEM>");
            sb.Append("<OPWINID>"+ winid + "</OPWINID>");
            sb.Append("<OPTYPE>208</OPTYPE>");
            sb.Append("<OPIP></OPIP>");
            sb.Append("<OPMANNO></OPMANNO>");
            sb.Append("<OPMANNAME>24H</OPMANNAME>");
            sb.Append("<CONSIS_PRESC_MSTVW>");
            sb.Append("<PATIENT_ID>" + row["presc_no"] + "</PATIENT_ID>");
            sb.Append("<DISPENSARY>" + row["dispensary"] + "</DISPENSARY>");
            sb.Append("</CONSIS_PRESC_MSTVW>");
            sb.Append("</ROOT>");
            return sb.ToString();
        }
        /// <summary>
        /// 获取发药状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public void GetSendOpflg()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //
                int opflg = preConfig.GetSendOpflg(prescno);
                string json = "{\"opflg\":" + opflg + "}";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetSendOpflg:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 获取发药状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public void GetManSendOpflg()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //
                int opflg = preConfig.GetManSendOpflg(prescno);
                string json = "{\"opflg\":" + opflg + "}";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetManSendOpflg:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        
        /// <summary>
        /// 获取付款状态
        /// </summary>
        /// <param name="prescno"></param>
        /// <returns></returns>
        public void GetPayOpflg()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //
                int opflg = preConfig.GetPayOpflg(prescno);
                string json = "{\"opflg\":" + opflg + "}";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayOpflg:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 获取打印数据
        /// </summary>
        public void GetPrintData()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //
                DataTable table = preConfig.GetPrescInfo2(prescno, "3");
                if (table != null && table.Rows.Count > 0)
                {
                    string json = "{\"prescno\":\"" + prescno + "\",";
                    json += "\"prescdate\":\"" + ((DateTime)table.Rows[0]["presc_date"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",";
                    json += "\"costs\":" + table.Rows[0]["costs"].ToString() + ",";
                    json += "\"payments\":" + table.Rows[0]["payments"].ToString() + ",";
                    json += "\"rows\":" + table.Rows.Count + ",";
                    json += "\"drugs\":[";
                    foreach (DataRow row in table.Rows)
                    {
                        json += "{\"drug_name\":\"" + row["drug_name"].ToString() + "\",";
                        json += "\"drug_spec\":\"" + row["drug_spec"].ToString() + "\",";
                        json += "\"unit\":\"" + row["unit"].ToString() + "\",";
                        json += "\"firm_name\":\"" + row["firm_name"].ToString() + "\",";
                        json += "\"makeno\":\"" + row["makeno"].ToString() + "\",";
                        json += "\"price\":" + row["price"].ToString() + ",";
                        json += "\"quantity\":" + row["quantity"].ToString() + ",";
                        json += "\"costs\":" + row["costs"].ToString() + "},";
                    }
                    json = json.TrimEnd(',') + "]";
                    json +="}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("打印数据提取失败"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPrintData:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 创建手动发药订单并发药
        /// </summary>
        public void ManPrescProdure()
        {
            try
            {
                //数据获取
                string posno = RequestStr.Request["posno"];//药店编号
                string consisposno = RequestStr.Request["consisposno"];//身份证号
                string druginfo = RequestStr.Request["druginfo"];//drugid列表
                string prescno = "";
                //存储数据
                bool res = preConfig.CreateManPrescs(ref prescno,posno, consisposno, druginfo);
                //
                if (res)
                {
                    string json = "{\"code\":\"200\",\"prescno\":\"" + prescno + "\",\"msg\":\"手动发药成功\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("手动发药订单创建失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->ManPrescProdure:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 获取下单XML
        /// </summary>
        /// <param name="type"></param>
        /// <param name="A2"></param>
        /// <param name="A3"></param>
        /// <param name="A4"></param>
        /// <param name="A5"></param>
        /// <param name="B3"></param>
        /// <param name="key"></param>
        /// <param name="prescno"></param>
        /// <param name="costs"></param>
        /// <returns></returns>
        public string GetPayXMLA(string A2, string A3, string A4, string A5,string B2, string B3, string key, string billno, int costs)
        {
            string str = "&A1=A&A11=A&A2={0}&A3={1}&A4={2}&A5={3}&A6=A&A7=1.5&B1={4}&B2={6}&B3={7}&B4={5}&KEY=" + key;
            string a8 = GetMD5(string.Format(str, A2, A3, A4, A5, billno, costs, B2, B3));

            string xml = "<xml><request><A1>A</A1><A2>{0}</A2><A3>{1}</A3><A4>{2}</A4><A5>{3}</A5><A6>A</A6><A7>1.5</A7><A8>{8}</A8><A11>A</A11></request><data><B1>{4}</B1><B2>{6}</B2><B3>{7}</B3><B4>{5}</B4></data></xml>";
            // MD5加密
            return string.Format(xml, A2, A3, A4, A5, billno, costs, B2, B3, a8.ToUpper());
        }
        /// <summary>
        /// 获取预下单XML
        /// </summary>
        /// <param name="type"></param>
        /// <param name="A2"></param>
        /// <param name="A3"></param>
        /// <param name="A4"></param>
        /// <param name="A5"></param>
        /// <param name="B3"></param>
        /// <param name="key"></param>
        /// <param name="prescno"></param>
        /// <param name="costs"></param>
        /// <returns></returns>
        public string GetPayXMLF(int type,string A2, string A3, string A4, string A5, string B3, string key,string billno, int costs)
        {
            string str = "&A1=A&A12=" + type + "&A2={0}&A3={1}&A4={2}&A5={3}&A6=F&A7=1.5&B1={4}&B3={6}&B4={5}&KEY=" + key;
            string a8 = GetMD5(string.Format(str, A2, A3, A4, A5, billno, costs, B3));

            string xml = "<xml><request><A1>A</A1><A2>{0}</A2><A3>{1}</A3><A4>{2}</A4><A5>{3}</A5><A6>F</A6><A7>1.5</A7><A8>{7}</A8><A12>" + type + "</A12></request><data><B1>{4}</B1><B3>{6}</B3><B4>{5}</B4></data></xml>";
            // MD5加密
            return string.Format(xml, A2, A3, A4, A5, billno, costs, B3, a8.ToUpper());
        }
        /// <summary>
        /// 订单状态查询
        /// </summary>
        /// <param name="A2"></param>
        /// <param name="A3"></param>
        /// <param name="A4"></param>
        /// <param name="A5"></param>
        /// <param name="key"></param>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string GetPayXMLB(string A2, string A3, string A4, string A5, string key, string billno)
        {
            string str = "&A1=A&A2={0}&A3={1}&A4={2}&A5={3}&A6=B&A7=1.5&B1={4}&KEY=" + key;
            string a8 = GetMD5(string.Format(str, A2, A3, A4, A5, billno));

            string xml = "<xml><request><A1>A</A1><A2>{0}</A2><A3>{1}</A3><A4>{2}</A4><A5>{3}</A5><A6>B</A6><A7>1.5</A7><A8>{5}</A8></request><data><B1>{4}</B1></data></xml>";
            // MD5加密
            return string.Format(xml, A2, A3, A4, A5, billno, a8.ToUpper());
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private string GetMD5(string strValue)
        {
            byte[] b = Encoding.UTF8.GetBytes(strValue);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
        /// <summary>
        /// 服务端访问
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="postData"></param>
        /// <param name="contextType"></param>
        /// <param name="s_token"></param>
        /// <returns></returns>
        private static string HttpPostConnectToServer(string serverUrl, string postData, string contextType, string s_token = "")
        {
            var dataArray = Encoding.UTF8.GetBytes(postData);
            //创建请求
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //设置上传服务的数据格式
            request.ContentType = contextType;
            //请求的身份验证信息为默认
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间
            request.Timeout = 10000;
            //
            if (s_token != "")
            {
                WebHeaderCollection heard = new WebHeaderCollection();
                heard.Add("Authorization", s_token);
                request.Headers = heard;
            }

            //创建输入流
            Stream dataStream;
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败
            }
            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception)
            {
                return null;//连接服务器失败
            }
            return res;
        }
        /// <summary>
        /// 获取订单信息，ERP数据
        /// </summary>
        public void SendERP()
        {
            try
            {
                string dispensary = Config.Instance().Dispensary;
                string enterpriceid = Config.Instance().Enterpriceid;
                string compid = Config.Instance().Compid;
                string busno = Config.Instance().Busno;
                string prescno = RequestStr.Request["prescno"];//单号
                DataTable table = preConfig.GetPrescInfo2(prescno, "3");
                //状态为2或3
                if (table != null && table.Rows.Count > 0)
                {
                    string json=GetERPjson(table, dispensary, enterpriceid, compid, busno, prescno);

                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendERP数据:" + json, @"c:\log\WebInfo");
                    ERPService.fyj fyj = new ERPService.fyj();
                    fyj.Url = Config.Instance().ErpUrl1;
                    string ret = fyj.SaleList(json);
                    RequestStr.Response.Write(ret);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendERP:数据提取失败!", @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("数据提取失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendERP:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// ERP销售订单数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dispensary"></param>
        /// <param name="enterpriceid"></param>
        /// <param name="compid"></param>
        /// <param name="busno"></param>
        /// <param name="prescno"></param>
        /// <returns></returns>
        private string GetERPjson(DataTable table,string dispensary,string enterpriceid, string compid, string busno, string prescno)
        {
            DataRow firstRow = table.Rows[0];
            //
            ERPEntity eRPEntity = new ERPEntity();
            eRPEntity.enterpriceid = enterpriceid;
            eRPEntity.compid = compid;
            eRPEntity.busno = busno;
            //
            List<ERPSaleEntity> eRPSaleEntityList = new List<ERPSaleEntity>();
            foreach (DataRow row in table.Rows)
            {
                ERPSaleEntity eRPSaleEntity = new ERPSaleEntity();
                eRPSaleEntity.warecode = row["drug_code"].ToString();
                eRPSaleEntity.wareqty =(int)double.Parse(row["quantity"].ToString());
                eRPSaleEntity.price = double.Parse(row["price"].ToString());
                if (row["makeno"] != DBNull.Value)
                    eRPSaleEntity.makeno = row["makeno"].ToString();
                eRPSaleEntityList.Add(eRPSaleEntity);
            }
            eRPEntity.salelist = eRPSaleEntityList;
            //
            eRPEntity.tel = firstRow["phoneno"].ToString();
            eRPEntity.netsum =double.Parse(firstRow["payments"].ToString());
            //
            List<ERPPayEntity> eRPPayEntityList = new List<ERPPayEntity>();
            if (firstRow["paytype1"] != DBNull.Value)
            {
                ERPPayEntity eRPPayEntity = new ERPPayEntity();
                eRPPayEntity.paytype = firstRow["paytype1"].ToString();
                eRPPayEntity.paysum = double.Parse(firstRow["paysum1"].ToString());
                eRPPayEntityList.Add(eRPPayEntity);
            }
            if (firstRow["paytype2"] != DBNull.Value)
            {
                ERPPayEntity eRPPayEntity = new ERPPayEntity();
                eRPPayEntity.paytype = firstRow["paytype2"].ToString();
                eRPPayEntity.paysum = double.Parse(firstRow["paysum2"].ToString());
                eRPPayEntityList.Add(eRPPayEntity);
            }
            eRPEntity.paylist = eRPPayEntityList;
            //
            eRPEntity.srcsaleno = firstRow["presc_no"].ToString();
            eRPEntity.saledate = ((DateTime)firstRow["presc_date"]).ToString("yyyy-MM-dd HH:mm:ss");
            //
            eRPEntity.ispay = "1";
            eRPEntity.issend = "1";
            if (firstRow["patient_name"] != DBNull.Value)
                eRPEntity.name = firstRow["patient_name"].ToString();
            if (firstRow["sex"] != DBNull.Value)
            {
                if (firstRow["sex"].ToString()!="男")
                    eRPEntity.sex = "1";
            }
            if (firstRow["date_of_birth"] != DBNull.Value)
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(firstRow["date_of_birth"].ToString(), out dt))
                {
                    int year = DateTime.Now.Year - dt.Year;
                    eRPEntity.age = year.ToString();
                }
            }
            if (firstRow["medicalhistory"] != DBNull.Value)
                eRPEntity.disease = firstRow["medicalhistory"].ToString();
            if (firstRow["allergyHistory"] != DBNull.Value)
                eRPEntity.anaphylaxis = firstRow["allergyHistory"].ToString();
            if (firstRow["presc_identity"] != DBNull.Value)
                eRPEntity.userid = firstRow["presc_identity"].ToString();
            //
            eRPEntity.attribute1 = firstRow["billno"].ToString();
            //
            return JsonHelper.ConvertObjectToString(eRPEntity);
        }
        /// <summary>
        /// 获取付款二维码
        /// </summary>
        public void GetPayPic()
        {
            try
            {
                int type =int.Parse(RequestStr.Request["type"]);
                string A2 = Config.Instance().A2;
                string A3 = Config.Instance().A3;
                string A4 = Config.Instance().A4;
                string A5 = Config.Instance().A5;
                string B3 = Config.Instance().B3;
                string key = Config.Instance().KEY;
                string billno = RequestStr.Request["billno"];
                int costs = (int)double.Parse(RequestStr.Request["costs"]);
                string xmlinfo = GetPayXMLF(type, A2, A3, A4, A5, B3, key, billno, costs);
                //
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic数据:" + xmlinfo, @"c:\log\WebInfo");
                //
                string retData=HttpPostConnectToServer(Config.Instance().MiyaUrl1, xmlinfo, "application/xml");
                if (retData == null)
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic:请求失败!" + retData, @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 500;
                    RequestStr.Response.Write(Failure("请求失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //解析XML
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(retData);//加载xml
                //支付状态
                XmlNodeList xList1 = xml.GetElementsByTagName("C1");
                string c1 = xList1[0].InnerText;
                XmlNodeList xList2 = xml.GetElementsByTagName("C2");
                string c2 = xList2[0].InnerText;
                if (string.IsNullOrEmpty(c1) ||c1.ToUpper() != "SUCCESS"||string.IsNullOrEmpty(c2) || c2.ToUpper() != "PAYWAIT")
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic:请求异常!" + retData, @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 500;
                    RequestStr.Response.Write(Failure("请求异常！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //支付串
                XmlNodeList xList26 = xml.GetElementsByTagName("C26");
                string c26 = xList26[0].InnerText;
                //获取二维码图片
                Image image = GetOrCodeImage(c26);
                MemoryStream mStream = new MemoryStream();
                image.Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //
                string json = "{\"value\":\"" + Convert.ToBase64String(mStream.ToArray()) + "\"}";
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 下单支付
        /// </summary>
        public void GetPay()
        {
            try
            {
                string A2 = Config.Instance().A2;
                string A3 = Config.Instance().A3;
                string A4 = Config.Instance().A4;
                string A5 = Config.Instance().A5;
                string B3 = Config.Instance().B3;
                string key = Config.Instance().KEY;
                string barcode = RequestStr.Request["barcode"];
                string billno = RequestStr.Request["billno"];
                int costs = (int)double.Parse(RequestStr.Request["costs"]);
                string xmlinfo = GetPayXMLA( A2, A3, A4, A5, barcode, B3, key, billno, costs);
                //
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPay数据:" + xmlinfo, @"c:\log\WebInfo");
                //
                string retData = HttpPostConnectToServer(Config.Instance().MiyaUrl1, xmlinfo, "application/xml");
                if (retData == null)
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPay:请求失败!" + retData, @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 500;
                    RequestStr.Response.Write(Failure("请求失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //解析XML
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(retData);//加载xml
                //支付状态
                XmlNodeList xList1 = xml.GetElementsByTagName("C1");
                string c1 = xList1[0].InnerText;
                XmlNodeList xList2 = xml.GetElementsByTagName("C2");
                string c2 = xList2[0].InnerText;
                //
                XmlNodeList xList5 = xml.GetElementsByTagName("C5");
                string c5 = xList5[0].InnerText;
                string prescno = c5.Substring(0, 17);
                //订单成功
                string json = "";
                if (!string.IsNullOrEmpty(c1) && c1.ToUpper().Trim() == "SUCCESS" && !string.IsNullOrEmpty(c2) && c2.ToUpper().Trim() == "PAYSUCCESS")
                {
                    //支付金额
                    XmlNodeList xList7 = xml.GetElementsByTagName("C7");
                    string c7 = xList7[0].InnerText;
                    //支付方式
                    XmlNodeList xList9 = xml.GetElementsByTagName("C8");
                    string c8 = xList9[0].InnerText;
                    //交易金额
                    double payments = double.Parse(c7)/100;
                    //交易方式
                    if (c8 == "1")
                        c8 = "微信";
                    else
                        c8 = "支付宝";
                    preConfig.MiyaPayRetSuccess(prescno, c8, payments, c5);
                    //
                    json = "{\"opflg\":\"2\",\"billno\":\"" + c5 + "\"}";
                }
                //等待结果
                else if (!string.IsNullOrEmpty(c1) && c1.ToUpper().Trim() == "SUCCESS" && !string.IsNullOrEmpty(c2) && c2.ToUpper().Trim() == "PAYWAIT")
                {
                    json = "{\"opflg\":\"0\",\"billno\":\""+c5+ "\"}";
                }
                //订单失败
                else
                {
                    //失败
                    preConfig.MiyaPayRetFail(prescno);
                    //
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPay:订单支付失败!" + retData, @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 500;
                    RequestStr.Response.Write(Failure("订单支付失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 订单状态查询
        /// </summary>
        public void SearchPay() {
            try
            {
                string A2 = Config.Instance().A2;
                string A3 = Config.Instance().A3;
                string A4 = Config.Instance().A4;
                string A5 = Config.Instance().A5;
                string key = Config.Instance().KEY;
                string billno = RequestStr.Request["billno"];
                string xmlinfo = GetPayXMLB(A2, A3, A4, A5, key, billno);
                //
                //Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SearchPay数据:" + xmlinfo, @"c:\log\WebInfo");
                //
                string retData = HttpPostConnectToServer(Config.Instance().MiyaUrl1, xmlinfo, "application/xml");
                string json = "";
                if (retData == null)
                {
                    json = "{\"opflg\":\"0\",\"billno\":\"" + billno + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //解析XML
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(retData);//加载xml
                //支付状态
                XmlNodeList xList1 = xml.GetElementsByTagName("C1");
                string c1 = xList1[0].InnerText;
                XmlNodeList xList2 = xml.GetElementsByTagName("C2");
                string c2 = xList2[0].InnerText;
                XmlNodeList xList5 = xml.GetElementsByTagName("C5");
                string c5 = xList5[0].InnerText;
                //订单成功
                if (!string.IsNullOrEmpty(c1) && c1.ToUpper().Trim() == "SUCCESS" && !string.IsNullOrEmpty(c2) && c2.ToUpper().Trim() == "PAYSUCCESS")
                {
                    //更新到数据库
                    //订单号17位
                    string prescno = c5.Substring(0, 17);
                    //支付金额
                    XmlNodeList xList7 = xml.GetElementsByTagName("C7");
                    string c7 = xList7[0].InnerText;
                    //支付方式
                    XmlNodeList xList9 = xml.GetElementsByTagName("C8");
                    string c8 = xList9[0].InnerText;
                    //交易金额
                    double payments = double.Parse(c7)/100;
                    //交易方式
                    if (c8 == "1")
                        c8 = "微信";
                    else
                        c8 = "支付宝";
                    preConfig.MiyaPayRetSuccess(prescno, c8, payments, c5);
                    //
                    json = "{\"opflg\":\"2\",\"billno\":\"" + c5 + "\"}";
                }
                //等待结果
                else
                {
                    json = "{\"opflg\":\"0\",\"billno\":\"" + c5 + "\"}";
                }
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetPayPic:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="barCode">字符数据</param>
        /// <returns></returns>
        private Image GetOrCodeImage(string barCode)
        {
            try
            {
                BarCodeBuilder _barCodeBuilder;

                int QrCodeErrorLevel = 1;
                //生成二维码
                var enCodeString = barCode.Trim();

                _barCodeBuilder = new BarCodeBuilder
                {
                    SymbologyType = Symbology.QR,
                    QREncodeMode = QREncodeMode.Auto,
                    CodeLocation = CodeLocation.None,
                    AutoSize = true
                };
                switch (QrCodeErrorLevel)
                {
                    case 0:
                        _barCodeBuilder.QRErrorLevel = QRErrorLevel.LevelL;
                        break;
                    case 1:
                        _barCodeBuilder.QRErrorLevel = QRErrorLevel.LevelM;
                        break;
                    case 2:
                        _barCodeBuilder.QRErrorLevel = QRErrorLevel.LevelQ;
                        break;
                    default:
                        _barCodeBuilder.QRErrorLevel = QRErrorLevel.LevelH;
                        break;
                        ;
                }
                _barCodeBuilder.CodeText = enCodeString;
                return _barCodeBuilder.BarCodeImage;

            }
            catch (Exception)
            {
                //
            }

            return null;
        }
        /// <summary>
        /// 药智反馈发药状态
        /// </summary>
        public void SendYaoXZ()
        {
            try
            {
                string orderNo = RequestStr.Request["orderNo"];//
                string orderState = RequestStr.Request["orderState"];//
                string json = "{\"orderNo\":\""+ orderNo + "\",\"orderState\":\"" + orderState + "\"}";
                //
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZ数据:" + json, @"c:\log\WebInfo");
                //
                string retData = HttpPostConnectToServer(Config.Instance().YxzUrl3, json, "application/json");
                if(retData!=null)
                { 
                    RequestStr.Response.Write(retData);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZCheck:数据提取失败!", @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("数据提取失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZ:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 处方审核发送
        /// </summary>
        public void SendYaoXZCheck()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //状态为0
                YXZCheckEntity yXZCheckEntity = GetYXZCheck(prescno);
                if (yXZCheckEntity != null)
                {
                    string json = JsonHelper.ConvertObjectToString(yXZCheckEntity);
                    //
                    yXZCheckEntity.RcptPic = null;
                    string log = JsonHelper.ConvertObjectToString(yXZCheckEntity);
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZCheck数据:" + log, @"c:\log\WebInfo");
                    //
                    string retData = HttpPostConnectToServer(Config.Instance().YxzUrl1, json, "application/json");
                    if (retData != null)
                    {
                        RequestStr.Response.Write(retData);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZCheck:数据传输失败!", @"c:\log\WebError");
                        RequestStr.Response.StatusCode = 300;
                        RequestStr.Response.Write(Failure("数据传输失败！"));
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZCheck:数据提取失败!", @"c:\log\WebError");
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("数据提取失败！"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->SendYaoXZCheck:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 获取审方返回情况
        /// </summary>
        public void GetYaoXZResult()
        {
            try
            {
                string prescno = RequestStr.Request["prescno"];//单号
                //查询
                string json = "{\"orderNo\":\"" + prescno + "\"}";
                string retData = HttpPostConnectToServer(Config.Instance().YxzUrl2, json, "application/json");
                //
                if (retData != null)
                {
                    Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetYaoXZResult:" +retData, @"c:\log\WebInfo");
                    //-1 正在处理 0 不通过 1 通过
                    RetInfo retInfo = JsonHelper.ConvertStringToObject<RetInfo>(retData);
                    //
                    //处方详细信息
                    string presc_info = retInfo.presc_info;///XML
                    string retvalue = retInfo.retvalue;
                    string retmsg = retInfo.retmsg;
                    //string retvalue = "1";
                    //string retmsg = "通过";
                    int ret = 0;
                    int val = 0;
                    if (int.TryParse(retvalue, out val))
                    {
                        ret = val;
                    }
                    //
                    if (val >= 0)
                    {
                        //正在处理
                        bool res = preConfig.UpdatePrescs(retvalue, retmsg, presc_info);
                    }
                }
                //
                string retMsg = "";
                int opflg = preConfig.GetCheckResult(prescno, ref retMsg);
                string Json = "{\"opflg\":\"" + opflg + "\",\"retmsg\":\"" + retMsg + "\"}";
                //
                RequestStr.Response.Write(Json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->GetYaoXZResult:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
    
}
