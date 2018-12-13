<%@ WebHandler Language="C#" Class="ValidatorSnCode" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;

public class ValidatorSnCode : IHttpHandler,System.Web.SessionState.IRequiresSessionState {
    //验证码长度
    private int SnCodeLength = 6;
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //加载图片场景
        string path = context.Server.MapPath("~/Images/Sncodebg.jpg");
        string sncode = GeneralClass.CreateSnCode(SnCodeLength);

        Font font = new Font("宋体", 32, FontStyle.Bold);
        Color color = Color.Blue;
        Point point = new Point(10, 5);

        Image image = PhotoText.GetPhotoText(path, sncode, font, color, point);

        //将随即生成的验证码保存在Session中，方便在需要验证码的页面来验证
        context.Session["SnCode"] = sncode;
        //将图片场景中的数据保存到输出流中
        //通常利用Response的响应流来达到不同的数据请求目的
        image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        //销毁资源
        image.Dispose();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}