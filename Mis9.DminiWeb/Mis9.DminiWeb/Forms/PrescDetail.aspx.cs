using Mis9.CommonTools.Encode;
using Mis9.Dmini.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Forms_PrescDetail : System.Web.UI.Page
{
    /// <summary>
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
    /// <summary>
    /// 订单编号
    /// </summary>
    public string Prescno { get; set; }
    /// <summary>
    /// 订单是否含有处方药
    /// </summary>
    public string Flg { get; set; }
    /// <summary>
    /// 总金额
    /// </summary>
    public double Costs { get; set; }
    /// <summary>
    /// 主脚本
    /// </summary>
    public string MainJS { get; set; }
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
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
            Prescno = Request.QueryString["prescno"];
            Flg= Request.QueryString["flg"];//是否为处方药,1为有
            //加载脚本
            GetJS();
        }
    }
    /// <summary>
    /// 获取脚本
    /// </summary>
    private void GetJS()
    {
        //获取药品数据
        PrescInfo ins = new PrescInfo();
        DataTable table = ins.GetPrescInfo2(Prescno, "2");
        if (table == null|| table.Rows.Count<=0) return;
        string src = "../Images/no_pic_200.png";
        //总金额
        Costs = double.Parse(table.Rows[0]["costs"].ToString());
        //生成脚本
        int seqno = 1;
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in table.Rows)
        {
            string drugid = row["drugid"].ToString();
            string drug_name = row["drug_name"].ToString();
            string drug_spec = row["drug_spec"].ToString();
            string firm_name = row["firm_name"].ToString();
            double price=double.Parse(row["price"].ToString());
            int quantity = (int)double.Parse(row["quantity"].ToString());
            sb.AppendLine("<dl class='clearfix'>");
            sb.AppendLine("<dt>");
            sb.AppendLine("<img id='pic_"+ seqno + "' drugid = '"+ drugid + "' src='"+ src + "' width='167' height='167'/>");
            seqno++;
            sb.AppendLine("</dt>");
            sb.AppendLine("<dd>");
            sb.AppendLine("<p class='ti'>" + drug_name + "</p>");
            sb.AppendLine("<p class='ci'>"+ drug_spec + "</p>");
            sb.AppendLine("<p class='ji'>￥" + price.ToString("f2") + "</p>");
            sb.AppendLine("<div class='sumg'>数量："+ quantity + "</div>");
            sb.AppendLine("</dd>");
            sb.AppendLine("</dl>");
        }
        MainJS = sb.ToString();
    }
    /// <summary>
    /// 获取参数
    /// </summary>
    private void GetSessionPar()
    {
        //菜单个数
        int count = 5;
        #region 设备信息
        ConsisNoList = SessionHelper.GetStringPar(Session, "ConsisNoList");
        #endregion

        #region 是否展示货位菜单
        if (SessionHelper.GetIntPar(Session, "LocationFlg") <= 0)
        {
            LocDisJS = "style = 'display:none'";
            count--;
        }
        #endregion

        //菜单栏宽度
        VcoWidth = "style='width: " + count * 180 + "px;'";
    }
}