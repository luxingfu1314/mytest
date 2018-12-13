<%@ WebHandler Language="C#" Class="LoadDrugMedia" %>

using System;
using System.Web;
using Mis9.Dmini.Bll;
using System.IO;


public class LoadDrugMedia : IHttpHandler {

    public void ProcessRequest (HttpContext context) {

        try
        {
            string drugid = HttpUtility.UrlDecode(context.Request["drugid"]);
            string partype = context.Request["partype"];//0图片,3D模型
            string parname = HttpUtility.UrlDecode(context.Request["parname"]);
            string grade = HttpUtility.UrlDecode(context.Request["grade"]);//A图片，B图片，C图片
            //判断cache里有没有
            string key = drugid + "#" + partype;
            if (partype == "0")
            {
                key += "#" + parname + "#" + grade;
            }
            string optype = context.Request["optype"];
            if (optype == "0" && CookiesHelper.GetCache(key)==null)
            {
                string value = context.Request["value"];
                if (context.Request["value"]!=null)
                {
                    CookiesHelper.SetCache(key, value);
                }
                //
                context.Response.Write(Success("已添加！"));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                object value = CookiesHelper.GetCache(key);

                //flg 0 新加载，1 已存在
                if (value == null)
                {
                    Mis9.Dmini.Bll.HttpHandler.Instance().ProcessRequest(context);
                }
                else
                {
                    string id = HttpUtility.UrlDecode(context.Request["id"]);
                    //
                    if (string.IsNullOrEmpty(id)) id = "";
                    string json="{\"key\":\""+id+"\",\"flg\":\"1\",\"value\":\"" + value.ToString() + "\"}";
                    context.Response.Write(json);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 300;
            context.Response.Write(Failure(ex.ToString()));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    protected string Success(string str)
    {
        return "{\"StatusCode\":\"200\",\"Message\":\"" + str + "\"}";
    }

    protected string Failure(string str)
    {
        return "{\"StatusCode\":\"300\",\"Message\":\"" + str + "\"}";
    }

}