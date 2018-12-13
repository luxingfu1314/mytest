<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;

public class upload : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Request.Files.Count > 0)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                HttpPostedFile file = context.Request.Files[i];
                UpLoad.UploadFile(file, "~/UserImage/");  //这里引用的是上面封装的方法
            }
            WriteJson(context.Response, "true", "");
        }
        else
        {
            WriteJson(context.Response, "error", "请选择要上传的文件");
        }

    }
    public static void WriteJson(HttpResponse response,
       string status, string msg, object data = null)
    {
        response.ContentType = "application/json";
        string json = "{\"status\":\""+status+"\",\"msg\":\""+msg+"\",\"data\":\""+data+"\"}";
        response.Write(json);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}