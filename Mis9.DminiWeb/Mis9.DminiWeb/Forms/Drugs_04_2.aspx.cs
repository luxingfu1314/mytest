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

public partial class Form_Drugs_04_2 : System.Web.UI.Page
{
    /// <summary>
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
    /// <summary>
    /// 分类编码
    /// </summary>
    string typecode = "";
    /// <summary>
    /// 购物车药品数量
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// 是否展示购物车
    /// </summary>
    public string CartDisJS { get; set; }
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
    /// <summary>
    /// 导航栏脚本
    /// </summary>
    public string MainJS { get; set; }
    /// <summary>
    /// 主脚本
    /// </summary>
    public string NavigationJs { get; set; }
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
            typecode = Request.QueryString["type"];
            //
            GetNavigationJs();
            //加载脚本
            GetJS();
        }
    }
    /// <summary>
    /// 导航页脚本
    /// </summary>
    private void GetNavigationJs()
    {
        DrugInfo ins = new DrugInfo();
        DataTable table = ins.GetDrugTypeTree(typecode);
        if (table == null || table.Rows.Count == 0) return;
        //
        string[] list = new string[table.Rows.Count];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            if (i == 0)
            {
                list[i] = "<a href = '../Forms/Drugs_04_2.aspx?type=" + row["typecode"] + "'>" + row["typename"] + "</a>";
            }
            else if (table.Rows.Count-1 == i)
            {
                list[i] = "<a href = '../Forms/Drugs_04.aspx'>人体导航</a><u>></u>";
            }
            else
            {
                list[i] = "<a href = '../Forms/Drugs_04_1.aspx?type=" + row["typecode"] + "'>" + row["typename"] + "</a><u>></u>";
            }
        }
        //
        StringBuilder sb = new StringBuilder();
        for (int i = table.Rows.Count - 1; i >= 0; i--)
        {
            sb.Append(list[i]);
        }
        NavigationJs = sb.ToString();
    }
    /// <summary>
    /// 获取脚本
    /// </summary>
    private void GetJS()
    {
        //获取药品数据
        DrugInfo ins = new DrugInfo();
        DataTable table = ins.GetDrugsByType(ConsisNoList, typecode);
        if (table == null) return;
        int seqno = 1;
        //生成脚本
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in table.Rows)
        {
            string drugid = row["drugid"].ToString();
            string src = "../Images/no_pic_200.png";
            sb.AppendLine("<li>");
            sb.AppendLine("<div class='ypli'> ");
            sb.AppendLine("<p class='img'>");
            sb.AppendLine("<a href = 'DrugDetail.aspx?id=" + drugid + "' target='_self'>");
            sb.AppendLine("<img id='pic_"+seqno+"' src = '" + src + "' drugid='" + drugid +"'/>");
            seqno++;
            sb.AppendLine("</a>");
            sb.AppendLine("</p>");
            double price = 0;
            double memberPrice = 0;
            double price2 = 0;
            double value;
            if (double.TryParse(row["price1"].ToString(), out value))//原价
                price = value;
            if (row["MemberPrice"] != null && row["MemberPrice"] != DBNull.Value && double.TryParse(row["MemberPrice"].ToString(), out value))//会员价
                memberPrice = value;
            if (row["promotionPrice"] != null && row["promotionPrice"] != DBNull.Value && double.TryParse(row["promotionPrice"].ToString(), out value))//促销价
                price2 = value;
            if (price2 > 0 && price > price2)
            {
                price = price2;
            }
            sb.AppendLine("<p class='jg'>￥" + price.ToString("f2") + "</p>");
            sb.AppendLine("<a class='ti twoRow' href='DrugDetail.aspx?id=" + drugid + "' target='_self'>");
            string drug_name = row["drug_name"].ToString();
            string drug_spec = row["drug_spec"].ToString();
            string firm_name = row["firm_name"].ToString();
            sb.AppendLine(drug_name + "  " + drug_spec + "  " + firm_name);
            sb.AppendLine("</a>");
            sb.AppendLine("<p class='ci'>" + row["promotiondetail"].ToString()+ "</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</li>");
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

        #region 是否展示购物车
        if (SessionHelper.GetIntPar(Session, "CartFlg") <= 0)
        {
            CartDisJS = "style = 'display:none'";
        }
        else
        {
            Quantity = SessionHelper.GetCartQuantity(Session);
        }
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