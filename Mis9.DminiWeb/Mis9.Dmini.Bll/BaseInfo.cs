using Aspose.BarCode;
using Mis9.CommonTools.Encode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Mis9.Dmini.Bll
{
    public class BaseInfo : HttpCommon
    {
        DAL.BaseInfo baseConfig = new DAL.BaseInfo();
        public void CheckLeaf()
        {
            try
            {
                string typecode = HttpUtility.UrlDecode(RequestStr.Request["typecode"]);

                string leafflg = baseConfig.CheckLeaf(typecode);
                //
                if (string.IsNullOrEmpty(leafflg))
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    string json = "{\"flg\":\"0\",\"value\":\"" + leafflg + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("BaseInfo->CheckLeaf:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        
        /// <summary>
        /// 支付类型
        /// </summary>
        public void GetPayType()
        {
            try
            {
                DataTable table = baseConfig.GetPayType();

                string json = "[";
                if (table != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        json += "{\"paytype\":\"" + row["PaymentID"] + "\",\"payname\":\"" + row["PaymentName"] + "\",\"miya\":" + row["MiyaType"] + "},";
                    }
                }
                json=json.TrimEnd(',');
                //
                json += "]";
                if (string.IsNullOrEmpty(json))
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("BaseInfo->GetPayType:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
