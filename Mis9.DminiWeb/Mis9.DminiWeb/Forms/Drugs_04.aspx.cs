using Mis9.Dmini.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Form_Drugs_04 : System.Web.UI.Page
{
    /// <summary>
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
    /// <summary>
    /// 购物车药品总量
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
    /// 设备编号
    /// </summary>
    public string ConsisNoList { get; set; }
    /// <summary>
    /// 性别JS
    /// </summary>
    public string SexJS { get; set; }
    /// <summary>
    /// 系统JS
    /// </summary>
    public string SystemJS { get; set; }
    /// <summary>
    /// BuwStyle
    /// </summary>
    public string BuwStyle { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //初始SessionPar
            SessionHelper.SetSessionPar(Session, Page.Request);
            //获取参数
            GetSessionPar();
            //获取脚本
            GetJS();
        }
    }
    /// <summary>
    /// 获取脚本
    /// </summary>
    private void GetJS()
    {
        BuwStyle = "style = \"width: 0px; margin-left:-0px\"";
        //
        HotspotInfo hotspot = new HotspotInfo();
        //查询所有性别设置
        DataTable sextable= hotspot.GetClassifyList(1);
        if (sextable == null || sextable.Rows.Count <= 0)
            return;
        //
        SexJS = "";
        for (int i = 0; i < sextable.Rows.Count; i++)
        {
            DataRow row = sextable.Rows[i];
            string sexcode = row["classifycode"].ToString();
            string sexclass = row["classname"].ToString();
            if (i == 0)
            {
                SexJS += "<li class='cur' onclick='TypeClick(this)' sexcode='"+ sexcode + "'>";
            }
            else
            {
                SexJS += "<li onclick='TypeClick(this)' sexcode='" + sexcode + "'>";
            }
            SexJS += "<div class='ss s"+((i % 3) + 1) +"'><i class='"+ sexclass + "'></i></div>";
            SexJS += "</li>";
        }
        //查询所有系统设置
        DataTable systemtable = hotspot.GetClassifyList(0);
        //
        if (systemtable == null || systemtable.Rows.Count <= 0)
            return;
        //
        BuwStyle = "style = \"width: " + 120 * systemtable.Rows.Count + "px; margin-left:-" + 60 * systemtable.Rows.Count + "px\"";
        StringBuilder sb = new StringBuilder(); ;
        for (int i = 0; i < systemtable.Rows.Count; i++)
        {
            DataRow row = systemtable.Rows[i];
            string systemcode = row["classifycode"].ToString();
            string systemname = row["classifyname"].ToString();
            string systemclass = row["classname"].ToString();
            if (i == 0)
            {
                sb.Append("<li class='cur' onclick='TypeClick(this)' systemcode='" + systemcode + "'>");
            }
            else
            {
                sb.Append("<li onclick='TypeClick(this)' systemcode='" + systemcode + "'>");
            }
            sb.Append("<p><i class='"+ systemclass + "'></i></p>");
            sb.Append("<p>"+ systemname + "</p>");
            sb.Append("</li>");
        }
        SystemJS = sb.ToString();
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