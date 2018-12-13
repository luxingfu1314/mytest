using Mis9.CommonTools.Json;
using Mis9.Dmini.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Runtime.Serialization;

public partial class Forms_HomePage : System.Web.UI.Page
{
    /// <summary>
    /// 购物车药品数量
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// 是否展示购物车
    /// </summary>
    public string CartDisJS { get; set; }
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
    /// <summary>
    /// 菜单间隔
    /// </summary>
    public string MarginJS { get; set; }
    /// <summary>
    /// 设备编号
    /// </summary>
    public string ConsisNoList { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //初始SessionPar
            SessionHelper.SetSessionPar(Session, Page.Request);
            //获取参数
            GetSessionPar();
            //
            //string cityName=GetCityName();
            //string cityCode = GetCityCode(cityName);
            
        }
    }
    /// <summary>
    /// 温馨提示
    /// </summary>
    /// <returns></returns>
    protected string GetYQTS()
    {
        string path = HostingEnvironment.MapPath("/UserImage");
        string file = Path.Combine(path, "友情提示.txt");
        if (File.Exists(file))
        {
            using (StreamReader sr = new StreamReader(file, Encoding.Default))
            {
               return sr.ReadToEnd();
            }
        }
        return "";
    }
    /// <summary>
    /// 二维码
    /// </summary>
    /// <returns></returns>
    protected string GetQRJS()
    {
        string path = HostingEnvironment.MapPath("/UserImage");
        string[] files = Directory.GetFiles(path, "QR*", SearchOption.TopDirectoryOnly);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < files.Length; i++)
        {
            sb.AppendLine("<li>");
            sb.AppendLine("<p class=\"img\">");
            string filename1 = Path.GetFileName(files[i]);
            sb.AppendLine("<img src=\"../UserImage/"+ filename1 + "\" width=\"242\" height=\"242\"/></p>");
            string filename2 = Path.GetFileNameWithoutExtension(files[i]);
            filename2 = filename2.Substring(3, filename2.Length - 3);
            sb.AppendLine("<p class=\"mt20\">"+ filename2 + "</p>");
            sb.AppendLine("</li>");
        }
        string file = Path.Combine(path, "友情提示.txt");
        
        return sb.ToString();
    }
    /// <summary>
    /// 获取参数
    /// </summary>
    private void GetSessionPar()
    {
        #region 设备信息
        ConsisNoList = SessionHelper.GetStringPar(Session, "ConsisNoList");
        #endregion

        #region 是否展示货位菜单
        if (SessionHelper.GetIntPar(Session, "LocationFlg") <= 0)
        {
            LocDisJS = "style = 'display:none'";
            MarginJS = "style=\"margin-bottom:100px;margin-top:50px\"";
        }
        #endregion
        #region 是否展示推荐菜单
        //if (SessionHelper.GetIntPar(Session, "LocationFlg") <= 0)
        //{
        //    LocDisJS = "style = 'display:none'";
        //    MarginJS = "style=\"margin-bottom:73px\"";
        //}
        #endregion
    }
    /// <summary>
    /// 获取日期
    /// </summary>
    /// <returns></returns>
    protected string GetDayString()
    {
        return DateTime.Now.ToString("yyyy年MM月dd日");
    }
    /// <summary>
    /// 获取滚动加载的图片
    /// </summary>
    /// <returns></returns>
    protected string PicJs()
    {
        string path = Server.MapPath("/UserImage");
        StringBuilder sb = new StringBuilder();
        string[] files=Directory.GetFiles(path, "Pic*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
            sb.AppendLine("<img src=\"../UserImage/"+ Path.GetFileName(files[i])+"\" width=\"341\" height=\"435\" />");
        }

        return sb.ToString();
    }
    /// <summary>
    /// 获取天气
    /// </summary>
    /// <returns></returns>
    protected string GetWeather()
    {
        string url = "http://i.tianqi.com/index.php?c=code&py=beijing&id=11&site=24&color=%23FFFFFF&num=1";
        //
        string info = HttpPostConnectToServer(url);
        //
        string[] lines = info.Split(new char[] { '>', '<', '/' }, StringSplitOptions.RemoveEmptyEntries);
        bool start = false;
        string weatherinfo = "";
        int index = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("strong") || lines[i].StartsWith("span"))
                continue;
            if (lines[i].StartsWith("今天"))
            {
                start = true;
                continue;
            }
            else if (lines[i].StartsWith("明天"))
            {
                start = false;
                break;
            }
            else if (lines[i].Contains("℃"))
            {
                index++;
            }
            
            if (start)
            {
                weatherinfo += lines[i];
                if (index >= 2) break;
            }
        }
        return weatherinfo;
    }
    /// <summary>
    /// 获取LOGO
    /// </summary>
    /// <returns></returns>
    protected string GetLogo()
    {
        string path = Server.MapPath("/UserImage");
        StringBuilder sb = new StringBuilder();
        string[] files = Directory.GetFiles(path, "Logo*", SearchOption.TopDirectoryOnly);
        //
        string src = "";
        if (files.Length > 0)
        {
            src = "../UserImage/" + Path.GetFileName(files[0]);
        }
        //
        return src;
    }
    /// <summary>
    /// 天气请求
    /// </summary>
    /// <param name="serverUrl"></param>
    /// <returns></returns>
    private static string HttpPostConnectToServer(string serverUrl)
    {

        //创建请求
        var request = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
        var response = (HttpWebResponse)request.GetResponse();
        var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //读取返回消息
        string res = "";
        while (!reader.EndOfStream)
        {
            string info = reader.ReadLine();
            if (info.Contains("今天") && info.Contains("明天"))
            {
                res = info;
            }
        }

        reader.Close();

        return res;
    }
    
}