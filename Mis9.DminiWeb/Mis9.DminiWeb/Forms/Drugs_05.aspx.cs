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

public partial class Form_Drugs_05 : System.Web.UI.Page
{
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
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
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
        }
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