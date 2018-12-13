using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Mis9.Dmini.Bll
{
    public class DrugInfo : HttpCommon
    {
        DAL.DrugInfo drugConfig = new DAL.DrugInfo();
        /// <summary>
        /// 获取药品图片
        /// </summary>
        public void GetDrugPic()
        {
            try
            {
                string drugid = HttpUtility.UrlDecode(RequestStr.Request["drugid"]);
                string id = HttpUtility.UrlDecode(RequestStr.Request["id"]);
                string grade = HttpUtility.UrlDecode(RequestStr.Request["grade"]);
                string parname = HttpUtility.UrlDecode(RequestStr.Request["parname"]);
                string value = "";
                
                DataTable table = drugConfig.GetDrugMedia1(drugid, grade, parname);
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow picRow = table.Rows[0];
                    byte[] imgBytes = new byte[0];
                    if (picRow["drugpic"] != DBNull.Value && picRow["drugpic"] != null)
                    {
                        imgBytes = (byte[])picRow["drugpic"];
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
                    string json = "{\"key\":\"" + id + "\",\"flg\":\"0\",\"value\":\"" + value + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetDrugPic:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 加载模型文件
        /// </summary>
        public void GetDrugModel()
        {
            try
            {
                string drugid = HttpUtility.UrlDecode(RequestStr.Request["drugid"]);
                string id = HttpUtility.UrlDecode(RequestStr.Request["id"]);
                string grade = HttpUtility.UrlDecode(RequestStr.Request["grade"]);
                string parname = HttpUtility.UrlDecode(RequestStr.Request["parname"]);
                string url = "";
                string path = HostingEnvironment.MapPath("/3DModel");

                if (File.Exists(Path.Combine(path, drugid, "index.html")))
                {
                    url = "../3DModel/" + drugid + "/index.html";
                }
                else
                {
                    DataTable table = drugConfig.GetDrugMedia1(drugid, grade, parname);
                    if (table != null && table.Rows.Count > 0)
                    {
                        DataRow picRow = table.Rows[0];
                        byte[] Bytes = new byte[0];
                        if (picRow["drugpic"] != DBNull.Value && picRow["drugpic"] != null)
                        {
                            Bytes = (byte[])picRow["drugpic"];
                        }
                        if (Bytes.Length > 0)
                        {
                            string filter = picRow["filter"].ToString();
                            //下载文件至服务器并解压
                            if (WriteObjFile(drugid, Bytes, filter))
                                url = "../3DModel/" + drugid + "/index.html";
                        }
                    }
                }
                //
                if (string.IsNullOrEmpty(url))
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    string json = "{\"key\":\"" + id + "\",\"flg\":\"0\",\"value\":\"" + url + "\"}";
                    RequestStr.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetDrugModel:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="drugid"></param>
        /// <param name="buffers"></param>
        private bool WriteObjFile(string drugid, byte[] buffers, string filter)
        {
            string path = HostingEnvironment.MapPath("/3DModel");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.Combine(path, drugid + filter);
            //
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //存储文件
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            BinaryWriter binWriter = new BinaryWriter(fs);

            binWriter.Write(buffers, 0, buffers.Length);

            binWriter.Close();
            fs.Close();
            //解压
            if (!DeCompress(fileName, Path.Combine(path, drugid))) return false;
            //删除压缩文件
            if (File.Exists(fileName))
                File.Delete(fileName);
            //
            return true;
        }

        /// <summary>
        /// 根据拼音查寻药品
        /// </summary>
        public void GetDrugsByPinYin()
        {
            try
            {
                string consisposno = HttpUtility.UrlDecode(RequestStr.Request["consisposno"]);
                string pinyin = HttpUtility.UrlDecode(RequestStr.Request["pinyin"]);
                
                DataTable table = drugConfig.GetDrugsByPinYin(consisposno, pinyin);

                if (table == null || table.Rows.Count <= 0)
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //
                string json = "[";
                //
                foreach (DataRow row in table.Rows)
                {
                    double price = double.Parse(row["price1"].ToString());//原价
                    double price2 = 0.0;
                    if (row["promotionPrice"] != null&& row["promotionPrice"]!=DBNull.Value)
                    price2 = double.Parse(row["promotionPrice"].ToString());//促销价
                    if (price2 > 0 && price > price2)
                    {
                        price = price2;
                    }
                    json += "{\"drugid\":\"" + row["drugid"].ToString()
                  + "\",\"drug_name\":\"" + row["drug_name"].ToString()
                  + "\",\"drug_spec\":\"" + row["drug_spec"].ToString()
                  + "\",\"firm_name\":\"" + row["firm_name"].ToString()
                  + "\",\"price\":\"" + price.ToString("f2")
                  + "\",\"PromotionDetail\":\"" + row["PromotionDetail"].ToString() + "\"},";
                }
                json = json.TrimEnd(',');
                json += "]";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetDrugs:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 根据分类及首字母查询药品
        /// </summary>
        public void GetDrugsByTypeChar()
        {
            try
            {
                string consisposno = HttpUtility.UrlDecode(RequestStr.Request["consisposno"]);
                string typecode = HttpUtility.UrlDecode(RequestStr.Request["typecode"]);
                string character = HttpUtility.UrlDecode(RequestStr.Request["character"]);
                DataTable table = drugConfig.GetDrugsByTypeChar(consisposno, typecode, character);

                if (table == null || table.Rows.Count <= 0)
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //
                string json = "[";
                //
                foreach (DataRow row in table.Rows)
                {
                    double price = double.Parse(row["price1"].ToString());//原价
                    double price2 = 0.0;
                    if (row["promotionPrice"] != null && row["promotionPrice"] != DBNull.Value)
                        price2 = double.Parse(row["promotionPrice"].ToString());//促销价
                    if (price2 > 0 && price > price2)
                    {
                        price = price2;
                    }
                    json += "{\"drugid\":\"" + row["drugid"].ToString()
                  + "\",\"drug_name\":\"" + row["drug_name"].ToString()
                  + "\",\"drug_spec\":\"" + row["drug_spec"].ToString()
                  + "\",\"firm_name\":\"" + row["firm_name"].ToString()
                  + "\",\"price\":\"" + price.ToString("f2")
                  + "\",\"PromotionDetail\":\"" + row["PromotionDetail"].ToString() + "\"},";
                }
                json = json.TrimEnd(',');
                json += "]";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetDrugs:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="RarFilePath">压缩文件名路径</param>
        /// <param name="FloderPath">解压文件夹路径</param>
        /// <returns></returns>
        private bool DeCompress(string RarFilePath, string FloderPath)
        {
            if (!Directory.Exists(FloderPath))
            {
                Directory.CreateDirectory(FloderPath);
            }
            //
            using (Process encode = new Process())
            {
                try
                {
                    string path = HostingEnvironment.MapPath("/Bin");
                    string arguments = "x -o+ -ep1 " + "\"" + RarFilePath + "\"" + " " + "\"" + FloderPath + "\"";
                    encode.StartInfo.FileName =Path.Combine(path, "rar.exe");
                    encode.StartInfo.Arguments = arguments;
                    encode.StartInfo.CreateNoWindow = true;
                    encode.StartInfo.UseShellExecute = false;
                    encode.Start();
                    encode.WaitForExit();
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    if (!encode.HasExited)
                    {
                        encode.Kill();
                    }
                    encode.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 获取货位查询药品数量
        /// </summary>
        public void GetDrugCount()
        {
            try
            {
                string consisposno = HttpUtility.UrlDecode(RequestStr.Request["consisposno"]);
                string pinyin = HttpUtility.UrlDecode(RequestStr.Request["pinyin"]);
                string strpercount = HttpUtility.UrlDecode(RequestStr.Request["percount"]);
                int value = 0;
                int percount = 40;
                if (int.TryParse(strpercount, out value))
                    percount = value;
                DataTable table = drugConfig.GetDrugCount(consisposno, pinyin);

                if (table == null || table.Rows.Count <= 0)
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }

                int count = int.Parse(table.Rows[0]["count"].ToString());
                int pageNum = (int)Math.Ceiling(count * 1.0 / percount);
                //
                string json = "{\"pageNum\":" + pageNum + "}";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetDrugCount:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// 获取药品图片
        /// </summary>
        public void GetPageDrugs()
        {
            try
            {
                string consisposno = HttpUtility.UrlDecode(RequestStr.Request["consisposno"]);
                string pinyin = HttpUtility.UrlDecode(RequestStr.Request["pinyin"]);
                string strcurpage = HttpUtility.UrlDecode(RequestStr.Request["curpage"]);
                int value = 0;
                int curpage = 1;
                if (int.TryParse(strcurpage, out value))
                    curpage = value;
                string strpercount = HttpUtility.UrlDecode(RequestStr.Request["percount"]);
                int percount = 40;
                if (int.TryParse(strpercount, out value))
                    percount = value;

                DataTable table = drugConfig.GetPageDrugs(consisposno, pinyin, percount,curpage);

                if (table == null || table.Rows.Count <= 0)
                {
                    RequestStr.Response.StatusCode = 300;
                    RequestStr.Response.Write(Failure("Json数据为空"));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                //
                string json = "[";
                foreach (DataRow row in table.Rows)
                {
                    double price = double.Parse(row["price1"].ToString());//原价
                    double price2 = 0.0;
                    if (row["promotionPrice"] != null && row["promotionPrice"] != DBNull.Value)
                        price2 = double.Parse(row["promotionPrice"].ToString());//促销价
                    if (price2 > 0 && price > price2)
                    {
                        price = price2;
                    }
                    json += "{\"drugid\":\"" + row["drugid"].ToString()
                  + "\",\"consisposno\":\"" + row["consisposno"].ToString()
                  //+ "\",\"location\":\"" + row["location"].ToString()
                  + "\",\"price\":\"" + price.ToString("f2")
                  + "\"},";
                }
                json = json.TrimEnd(',');
                json += "]";
                //
                RequestStr.Response.Write(json);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles("DrugInfo->GetPageDrugs:" + ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}