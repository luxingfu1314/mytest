using Mis9.Dmini.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_PaySuccess : System.Web.UI.Page
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
    /// 设备编号
    /// </summary>
    public string ConsisNoList { get; set; }
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
    /// <summary>
    /// 客户端编号
    /// </summary>
    public string PosNo { get; set; }
    public string A3 { get; set; }
    public string A4 { get; set; }
    public string A5 { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //配置参数
            A3 = Config.Instance().A3;
            A4 = Config.Instance().A4;
            A5 = Config.Instance().A5;
            //初始SessionPar
            SessionHelper.SetSessionPar(Session, Page.Request);
            //获取参数
            GetSessionPar();
            //
            Prescno = Request.QueryString["prescno"];
            Flg = Request.QueryString["flg"];//是否为处方药,1为有
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
        PosNo = SessionHelper.GetStringPar(Session, "PosNo");
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