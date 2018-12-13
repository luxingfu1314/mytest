<%@ WebHandler Language="C#" Class="SessionHandler" %>

using System;
using System.Web;
using System.Web.SessionState;

public class SessionHandler : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        string key=context.Request["key"];
        context.Response.Write(SessionHelper.GetStringPar(context.Session, key));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}