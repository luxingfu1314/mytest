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

public partial class Form_ShoppingCart : System.Web.UI.Page
{
    /// <summary>
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
    /// <summary>
    /// 购物车数据
    /// </summary>
    private DataTable dt = new DataTable();
    /// <summary>
    /// 总价
    /// </summary>
    public double totalPrice = 0.00;
    /// <summary>
    /// 药品列表脚本
    /// </summary>
    public StringBuilder jsSB = new StringBuilder ();
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
    /// <summary>
    /// 手动发药
    /// </summary>
    public bool FreeCharge = false;
    /// <summary>
    /// 是否展示导航栏（货位时不展示导航栏）
    /// </summary>
    public string EnavDisJS { get; set; }
    /// <summary>
    /// 全选
    /// </summary>
    public string CheckAll { get; set; }
    /// <summary>
    /// 设备编号
    /// </summary>
    public string ConsisNoList { get; set; }
    /// <summary>
    /// 客户端编号
    /// </summary>
    public string PosNo { get; set; }
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
    /// 更新购物车
    /// </summary>
    public void GetCartList()
    {
        //
        DataTable dt = new DataTable();
        //如果Session变量存在，则直接获取
        if (Session["Cart"] != null)
        {
            dt = (DataTable)Session["Cart"];
        }
        else//如果Session变量不存在，创建存储数据的表结构
        {
            dt.Columns.Add(new DataColumn("Flg", typeof(bool)));
            dt.Columns.Add(new DataColumn("Id", typeof(String)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
        }
        bool checkAll = true;
        //生成隔行购物车JS
        foreach (DataRow  row in dt.Rows)
        {
            string drugid = row["Id"].ToString();
            if (string.IsNullOrEmpty(drugid)) continue;
            bool checkflg = (bool)row["Flg"];
            if (!checkflg)
            {
                checkAll = false;
            }
            string quantityStr = row["Quantity"].ToString();
            int quantity=int.Parse(quantityStr);
            //获取药品信息
            GetScript(checkflg, drugid, quantity);
            if (checkAll)
            {
                CheckAll = "ico ico11";
            }
            else
            {
                CheckAll = "ico ico10";
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="checkflg"></param>
    /// <param name="drugid"></param>
    /// <param name="quantity"></param>
    private void GetScript(bool checkflg, string drugid, int quantity)
    {
        DrugInfo ins = new DrugInfo();
        DataTable table = ins.GetDrugDetail(ConsisNoList, drugid);
        if (table == null || table.Rows.Count == 0) return;
        //生成脚本
        StringBuilder sb = new StringBuilder();
        //
        DataRow row = table.Rows[0];
        //药品信息
        string drug_name = row["drug_name"].ToString();
        string drug_spec = row["drug_spec"].ToString();
        string firm_name = row["firm_name"].ToString();
        //待数据库查询
        string druginfo = drug_name + "  " + drug_spec + "  " + firm_name;
        string href = "../Forms/DrugDetail.aspx?id=" + drugid;
        //药品价格
        double price = 0;
        double memberPrice = 0;
        double price2 = 0;
        double value;
        if (double.TryParse(row["price1"].ToString(), out value))//原价
            price = value;
        if (double.TryParse(row["MemberPrice"].ToString(), out value))//会员价
            memberPrice = value;
        if (double.TryParse(row["promotionPrice"].ToString(), out value))//促销价
            price2 = value;
        if (price2 > 0 && price > price2)
        {
            price = price2;
        }
        string prescription = row["IsPrescriptionDrugs"].ToString();//是否为处方药
        string doubleTrack = row["IsDouble"].ToString();//是否为双轨药
        string ephedrine = row["IsEphedrine"].ToString();//是否含黄麻碱
        //库存
        int storage = int.Parse(row["storage"].ToString());
        int catal = 0;//最大购买量
        if (ephedrine == "Y") catal = 2;
        int limitbuy = 0;//最小购买量
        jsSB.AppendLine("<tr id='" + drugid + "' druginfo='" + druginfo + "' price='" + price + "' Prescription='" + prescription + "' DoubleTrack='" + doubleTrack + "' Ephedrine='" + ephedrine + "' storage='" + storage + "' catal='" + catal + "' limitbuy='" + limitbuy + "'>");
        jsSB.AppendLine("<td><i class='ico "+(checkflg? "ico11": "ico10") + "'  refmainitemid='" + drugid + "' name='cart2Checkbox' onclick='checkItemStatus(this)'></i></td>");
        jsSB.AppendLine("<td>");
        jsSB.AppendLine("<dl class='gwcpr clearfix'>");
        jsSB.AppendLine("<dt><a href = '" + href + "' target='_self'><img src = '../Images/no_pic_80.png'></a></dt>");
        jsSB.AppendLine("<dd>");
        jsSB.AppendLine("<p class='tit'><a href = '" + href + "' target='_self'>" + drug_name + "</a></p>");
        jsSB.AppendLine("<p>"+drug_spec+"</p>");
        jsSB.AppendLine("</dd>");
        jsSB.AppendLine("</dl>");
        jsSB.AppendLine("</td>");
        jsSB.AppendLine("<td>￥"+ price.ToString("f2") + "</td>");
        jsSB.AppendLine("<td>");
        jsSB.AppendLine("<div class='scc'>");
        jsSB.AppendLine("<i class='ico ico12' onclick='cartadditem(this)'></i>");
        jsSB.AppendLine("<i class='ico ico13' onclick='cartminusitem(this)'></i>");
        jsSB.AppendLine("<input type='text' class='text' refmainitemid = '" + drugid + "' id='v_" + drugid + "' value='" + quantity + "' onchange='cartprompt(this)' />");
        jsSB.AppendLine("</div>");
        jsSB.AppendLine("<div class='amount-msg' style='display: none;' id='m_" + drugid + "' refmainitemid='" + drugid + "'>");
        jsSB.AppendLine("<span style = 'color:red;'>" + quantity +  "</span>");
        jsSB.AppendLine("<em></em>");
        jsSB.AppendLine("</div>");
        jsSB.AppendLine("</td>");
        jsSB.AppendLine("<td><span span class='red' id = 'tp_" + drugid + "'>￥" + (price * quantity).ToString("f2") + "</span></td>");
        jsSB.AppendLine("<td><a href = 'javascript:void(0);' onclick='deleteitem(" + '"' + drugid + '"' + ")' class='deleteButton'>删除</a></td>");
        jsSB.AppendLine("</tr>");
        //若选中则累计药品价格总和
        if (checkflg)
        {
            totalPrice += price * quantity;
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

        #region 是否展示购物车
        if (SessionHelper.GetIntPar(Session, "CartFlg") > 0)
            GetCartList();
        #endregion

        #region 是否展示货位菜单
        if (SessionHelper.GetIntPar(Session, "LocationFlg") <= 0)
        {
            LocDisJS = "style = 'display:none'";
            count--;
        }
        else
        {
            FreeCharge = true;
            EnavDisJS = "style = 'display:none'";
        }
        #endregion

        //菜单栏宽度
        VcoWidth = "style='width: " + count * 180 + "px;'";
    }

    public string GetChargeJs()
    {
        if (FreeCharge)
            return "<button id=\"payer\" class=\"checkout\" onclick=\"manPresc()\">手动发药</button>";
        else
            return "<button id=\"payer\" class=\"checkout\" onclick=\"gotoCheckout()\">去结算</button>";
    }
}