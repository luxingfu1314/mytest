using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Web;

namespace Mis9.Dmini.Bll
{
    public class HttpCommon  
    {
        protected  HttpContext RequestStr = null;

        public void ProcessRequest(HttpContext context)
        {
            String methodName = context.Request["method"];

            if (string.IsNullOrEmpty(methodName))
            {
                return;
            }

            RequestStr = context;
            RequestStr.Response.ContentType = "text/plain";
            RequestStr.Response.Charset = "utf-8";
            
            Type type = this.GetType();
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure("未找到methodName"));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                method.Invoke(this, null);
            }
            catch(Exception ex) {
                Mis9.CommonTools.Apis.WinApi.WriteLogFiles(ex.ToString(), @"c:\log\WebError");
                RequestStr.Response.StatusCode = 300;
                RequestStr.Response.Write(Failure(ex.ToString()));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected string Success(string str)
        {
            return "{\"statusCode\":\"200\", \"message\":\"" + str + "\"}";
        }

        protected string Failure(string str)
        {
            return "{\"statusCode\":\"300\", \"message\":\"" + str + "\"}";
        }
        
    }
}
