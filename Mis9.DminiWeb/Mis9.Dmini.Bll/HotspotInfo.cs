using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mis9.Dmini.Bll
{
    public class HotspotInfo : HttpCommon
    {
        DAL.HotspotInfo spotConfig = new DAL.HotspotInfo();
        /// <summary>
        /// 获取热点图片
        /// </summary>
        /// <returns></returns>
        public void GetHotspotPic()
        {
            try
            {
                string sexcode=RequestStr.Request["sexcode"];
                string systemcode= RequestStr.Request["systemcode"];
                string value = "";

                DataTable table = spotConfig.GetHotspotPic(sexcode, systemcode);
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow picRow = table.Rows[0];
                    byte[] imgBytes = new byte[0];
                    if (picRow["picvalue"] != DBNull.Value && picRow["picvalue"] != null)
                    {
                        imgBytes = (byte[])picRow["picvalue"];
                    }
                    if (imgBytes.Length > 0)
                    {
                        value = Convert.ToBase64String(imgBytes);//将流读入到字节数组中
                    }
                }
                //
                if (string.IsNullOrEmpty(value))
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    string json = "{\"flg\":\"0\",\"value\":\"" + value + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("HotspotInfo->GetHotspotPic:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 获取热点参数信息
        /// </summary>
        /// <param name="consisposno"></param>
        /// <param name="sexcode"></param>
        /// <param name="systemcode"></param>
        /// <returns></returns>
        public void GetHotspotList()
        {
            try
            {
                string consisposno = RequestStr.Request["consisposno"];
                string sexcode = RequestStr.Request["sexcode"];
                string systemcode = RequestStr.Request["systemcode"];
                string json = "";

                DataTable table = spotConfig.GetHotspotList(consisposno,sexcode, systemcode);
                if (table != null && table.Rows.Count > 0)
                {
                    json += "[";
                    foreach (DataRow row in table.Rows)
                    {
                        json += "{\"typecode\":\"" + row["typecode"] + "\",";
                        json += "\"leafflg\":\"" + row["leafflg"] + "\",";
                        json += "\"spotname\":\"" + row["spotname"] + "\",";
                        json += "\"shapename\":\"" + row["shapename"] + "\","; 
                        json += "\"drugcount\":" + row["drugcount"] + ","; 
                        json += "\"coords\":\"" + row["coords"] + "\"},";
                    }
                    json = json.TrimEnd(',') ;
                    json += "]";
                }
                //
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
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("HotspotInfo->GetHotspotList:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
