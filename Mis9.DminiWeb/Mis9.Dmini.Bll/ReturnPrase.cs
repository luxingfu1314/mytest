using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml;

namespace Mis9.Dmini.Bll
{
    public static class ReturnPrase
    {
        static DAL.PrescInfo preConfig = new DAL.PrescInfo();
        public static void PraseXML(HttpContext context)
        {
            Stream stream = context.Request.InputStream;
            //获取数据
            stream.Position = 0;
            StreamReader reader = new System.IO.StreamReader(stream);
            string text = reader.ReadToEnd();
            Mis9.CommonTools.Apis.WinApi.WriteLogFiles("ReturnPrase->Prase数据:" + text, @"c:\log\WebInfo");
            //解析XML
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);//加载xml
            //支付状态
            XmlNodeList xList1 = xml.GetElementsByTagName("C1");
            string c1 = xList1[0].InnerText;
            //支付订单
            XmlNodeList xList5 = xml.GetElementsByTagName("C5");
            string c5 = xList5[0].InnerText;
            //支付金额
            XmlNodeList xList7 = xml.GetElementsByTagName("C7");
            string c7 = xList7[0].InnerText;
            //支付方式
            XmlNodeList xList9 = xml.GetElementsByTagName("C8");
            string c8 = xList9[0].InnerText;
            //订单号17位
            string prescno = c5.Substring(0, 17);

            try
            {
                //
                if (c1.ToUpper().Trim() == "SUCCESS")
                {
                    //交易金额
                    double payments=double.Parse(c7);
                    //交易方式
                    if (c8 == "1")
                        c8 = "微信";
                    else
                        c8 = "支付宝";

                    preConfig.MiyaPayRetSuccess(prescno, c8, payments, c5);
                }
                else
                {
                    preConfig.MiyaPayRetFail(prescno);
                }
                context.Response.Write("<xml><D1>SUCCESS</D1></xml>");
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception )
            {
                context.Response.Write("<xml><D1>FAIL</D1></xml>");
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public static void PraseJson(HttpContext context)
        {
            Stream stream = context.Request.InputStream;
            //获取数据
            stream.Position = 0;
            StreamReader reader = new System.IO.StreamReader(stream);
            string text = reader.ReadToEnd();
            Mis9.CommonTools.Apis.WinApi.WriteLogFiles("ReturnPrase->PraseJson数据:" + text, @"c:\log\WebInfo");
            RetInfo retInfo = JsonHelper.ConvertStringToObject<RetInfo>(text);
            //
            try
            {
                //处方详细信息
                string presc_info = retInfo.presc_info;///XML
                string retvalue = retInfo.retvalue;
                string retmsg = retInfo.retmsg;
                //存储返回状态值，并修改订单状态0-》1,2；失败，成功

                bool res = preConfig.UpdatePrescs(retvalue, retmsg, presc_info);
                //返回回馈数据
                string json = "{\"code\":\"200\",\"msg\":\"" + "已收到" + "\"}";
                context.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("PrescInfo->PrescCheckState:" + ex.ToString(), @"c:\log\WebError");
                string json = "{\"code\":\"300\",\"msg\":\"" + ex.ToString() + "\"}";
                context.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
    [DataContract]
    public class RetInfo
    {
        [DataMember]
        public string presc_info { get; set; }
        [DataMember]
        public string retmsg { get; set; }
        [DataMember]
        public string retvalue { get; set; }
        [DataMember]
        public string presc_no { get; set; }
    }
}
