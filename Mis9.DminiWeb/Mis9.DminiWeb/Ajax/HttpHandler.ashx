<%@ WebHandler Language="C#" Class="HttpHandler" %>

using System;
using System.Web;
using Mis9.Dmini.Bll;

public class HttpHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //类名、方法名、参数
        Mis9.Dmini.Bll.HttpHandler.Instance().ProcessRequest(context);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}