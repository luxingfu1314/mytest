<%@ WebHandler Language="C#" Class="UserHander" %>

using System;
using System.Web;
using System.Web.SessionState;

public class UserHander : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //
        string optype = context.Request["optype"];
        //退出
        if (optype == "0")
        {
            context.Session["userno"] = null;
            string json = "{\"code\":\"1\",\"errMsg\":\"\"}";
            context.Response.Write(json);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        //登录
        else if (optype == "1")
        { Login(context); }
        //添加用户
        else if (optype == "2")
        { AddUser(context); }
        //编辑用户
        else if (optype == "3")
        { EditUser(context); }
        //删除用户
        else if (optype == "4")
        { DelUser(context); }
        //其他
        else {
            string json = "{\"code\":\"-1\",\"errMsg\":\"未能找到方法\"}";
            context.Response.Write(json);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    public void Login(HttpContext context) {
        //
        string userno = context.Request["txtNo"];
        string password = context.Request["txtPwd"];
        string json ;
        //核查用户名密码
        //执行查询语句并校验
        //
        bool flg = false;
        string errMsg = "用户不存在！";
        if (!flg)
        {
            json = "{\"code\":\"-1\",\"errMsg\":\""+errMsg+"\"}";
        }
        else
        {
            //成功后放入session
            context.Session["userno"] = userno;
            json = "{\"code\":\"1\",\"errMsg\":\""+userno+"\"}";
        }
        context.Response.Write(json);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public void AddUser(HttpContext context) {
        //
        string userno = context.Request["txtNo"];
        //核查改用户编号是否已经存在
        //执行查询语句
        //
        string json ;
        bool flg = false;
        string errMsg = "用户已存在！";
        if (!flg)
        {
            json = "{\"code\":\"-1\",\"errMsg\":\""+errMsg+"\"}";
        }
        else
        {
            //
            string username = context.Request["txtName"];
            string password = context.Request["txtPwd"];
            //
            //插入用户信息
            //执行存储过程
            //
            //
            json = "{\"code\":\"1\",\"errMsg\":\""+userno+"\"}";
        }
        //
        context.Response.Write(json);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public void EditUser(HttpContext context) {
        //根据用户编号更新
        string userno = context.Request["txtNo"];
        string username = context.Request["txtName"];
        string password = context.Request["txtPwd"];
        string json ;
        //更新数据
        bool flg = false;
        string errMsg = "用户信息修改失败！";
        //执行存储过程
        //
        if (!flg)
        {
            json = "{\"code\":\"-1\",\"errMsg\":\""+errMsg+"\"}";
        }
        else
        {
            json = "{\"code\":\"1\",\"errMsg\":\""+userno+"\"}";
        }
        context.Response.Write(json);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public void DelUser(HttpContext context) {
        //根据用户编号删除
        string userno = context.Request["txtNo"];
        string json ;
        //更新数据
        bool flg = false;
        string errMsg = "用户信息删除失败！";
        //执行存储过程
        //
        if (!flg)
        {
            json = "{\"code\":\"-1\",\"errMsg\":\""+errMsg+"\"}";
        }
        else
        {
            json = "{\"code\":\"1\",\"errMsg\":\""+userno+"\"}";
        }
        context.Response.Write(json);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}