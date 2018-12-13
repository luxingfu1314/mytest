using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_UpLoad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string userno = SessionHelper.GetStringPar(Session,"userno");
        if (string.IsNullOrEmpty(userno))
        {
            string referUrl = "UpLoad.aspx";
            Context.Response.Redirect("Login.aspx"+ "?referUrl=" + referUrl);
        }
    }
}