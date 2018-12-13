<%@ WebHandler Language="C#" Class="CacheHandler" %>

using System;
using System.Web;
using Mis9.Dmini.Bll;
using System.IO;


public class CacheHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {

        try
        {
            string optype = context.Request["optype"];
            if (optype == "1")
            {
                GetBodyPic(context);
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

    private void GetBodyPic(HttpContext context)
    {
        try
        {
            string sexcode = context.Request["sexcode"];
            string systemcode = context.Request["systemcode"];
            //判断cache里有没有
            string key = sexcode + "#" + systemcode;
            string odertype = context.Request["odertype"];//0为保存，1为加载
            if (odertype == "0" && CookiesHelper.GetCache(key)==null)
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

                //flg 0 新数据，1 已存在
                if (value == null)
                {
                    Mis9.Dmini.Bll.HttpHandler.Instance().ProcessRequest(context);
                }
                else
                {
                    string json="{\"flg\":\"1\",\"value\":\"" + value.ToString() + "\"}";
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

    protected string Success(string str)
    {
        return "{\"StatusCode\":\"200\",\"Message\":\"" + str + "\"}";
    }

    protected string Failure(string str)
    {
        return "{\"StatusCode\":\"300\",\"Message\":\"" + str + "\"}";
    }

}