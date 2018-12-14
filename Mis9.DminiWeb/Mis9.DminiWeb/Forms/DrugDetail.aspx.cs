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

public partial class Form_DrugDetail : System.Web.UI.Page
{
    /// <summary>
    /// 快捷栏宽度
    /// </summary>
    public string VcoWidth { get; set; }
    /// <summary>
    /// 药品编码
    /// </summary>
    public string Drugid { get; set; }
    /// <summary>
    /// 药品列表脚本
    /// </summary>
    public StringBuilder jsSB = new StringBuilder();
    /// <summary>
    /// 购物车药品数量
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// 购物车中当前药品数量
    /// </summary>
    public int CurDrugQuantity { get; set; }
    /// <summary>
    /// 总库存
    /// </summary>
    public int Storage { get; set; }
    /// <summary>
    /// 最大购买量
    /// </summary>
    public int Catal { get; set; }
    /// <summary>
    /// 最小购买量
    /// </summary>
    public int Limitbuy { get; set; }
    /// <summary>
    /// 是否展示购物车
    /// </summary>
    public string CartDisJS { get; set; }
    /// <summary>
    /// 是否展示导航栏（货位时不展示导航栏）
    /// </summary>
    public string EnavDisJS { get; set; }
    /// <summary>
    /// 是否展示货位
    /// </summary>
    public string LocDisJS { get; set; }
    /// <summary>
    /// 设备编号
    /// </summary>
    public string ConsisNoList { get; set; }
    /// <summary>
    /// 药品脚本
    /// </summary>
    public string DrugJS { get; set; }
    /// <summary>
    /// 说明书脚本
    /// </summary>
    public string InstructionsJS { get; set; }
    /// <summary>
    /// 是否可购买
    /// </summary>
    public int BuyFlag = 1;
    public int _3DFlg = 0;

    private bool EphedrineFlg = true;
    private bool PrescriptionFlg = true;
    public int priceError = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //
            Drugid = Request.QueryString["id"];
            //初始SessionPar
            SessionHelper.SetSessionPar(Session, Page.Request);
            //获取参数
            GetSessionPar();
            //
            string consisno = Request.QueryString["consisno"];
            if (!string.IsNullOrEmpty(consisno))
            {
                ConsisNoList = consisno;
            }
            int seqNo = 0;
            string seqno = Request.QueryString["seqno"];
            if (!string.IsNullOrEmpty(seqno))
            {
                seqNo = int.Parse(seqno);
            }
            //加载脚本
            GetDrugJS(seqNo);
            GetInstructionsJS();
        }
    }
    /// <summary>
    /// 药品信息脚本
    /// </summary>
    /// <param name="seqno"></param>
    private void GetDrugJS(int seqno)
    {
        //根据id 获取药品信息与库存信息
        Storage = 0;//库存
        Catal = 0;//最大购买量
        Limitbuy = 0;//最小购买量
        //
        DrugInfo ins = new DrugInfo();
        DataTable table = ins.GetDrugDetail(ConsisNoList, Drugid, seqno);
        if (table == null || table.Rows.Count==0) return;
        //生成脚本
        StringBuilder sb = new StringBuilder();
        DataRow row = table.Rows[0];
        //药品信息
        //是否存在3D模型
        if (ins.Exist3DModel(Drugid))
        {//展示3D模型超链接
            sb.AppendLine("<iframe width=\"824\" height=\"480\" style=\"margin:-50px -100px  0 -50px;\" name = \"ifm\" id = \"ifm\" style = \"background-color: transparent;\"></iframe>");
            //sb.AppendLine("<a class='mi' style='font-size:18px' href = '../Forms/Drug3D.aspx?id=" + Drugid + "'> 3D模型展示 </a>");
            _3DFlg = 1;
        }
        else
        {
            sb.AppendLine("<p class='img'><img src = '../Images/no_pic.png' id='" + Drugid + "' width='724' height='430'/></p>");
        }
        //
        string drug_name = row["drug_name"].ToString();
        string drug_spec = row["drug_spec"].ToString();
        string firm_name = row["firm_name"].ToString();
        //
        sb.AppendLine("<p class='ti'>"+ drug_name + "</p>");
        sb.AppendLine("<p class='ci'>" + drug_spec + "  " + firm_name + "</p>");
        //
        string IsImported = row["IsImported"].ToString();
        string IsDrugs = row["IsDrugs"].ToString();
        string IsOTC = row["IsOTC"].ToString();
        string IsPrescriptionDrugs = row["IsPrescriptionDrugs"].ToString();
        string IsDouble = row["IsDouble"].ToString();
        //
        string type = ""; 
        if (IsImported == "Y") type += "进口";
        if (IsDrugs == "Y")
        {
            if (IsPrescriptionDrugs == "Y")
            {
                if (!PrescriptionFlg) BuyFlag = 0;
                type += "处方药";
                if(IsDouble == "Y")
                    type += "(双轨，需凭身份证购买)";
                else
                    type += "(单轨，需凭处方购买)";
            }
            else
            {
                type += "OTC";
                //
                string ephedrine = row["IsEphedrine"].ToString();//是否含黄麻碱
                if (ephedrine == "Y")
                {
                    if (!EphedrineFlg) BuyFlag = 0;
                    Catal = 2;
                    type += "(含黄麻碱，需凭身份证购买)";
                }
            }
        }
        else
        {
            type += "商品";
        }
        //
        sb.AppendLine("<p class='sl'>类型：<span>" + type + "</span></p>");
        //药品价格
        double price = 0;
        double memberPrice = 0;
        double price2 = 0;
        double value;
        if (double.TryParse(row["price1"].ToString(), out value))//原价
            price = value;
        else
            priceError = 1;
        if (double.TryParse(row["MemberPrice"].ToString(), out value))//会员价
            memberPrice = value;
        if (double.TryParse(row["promotionPrice"].ToString(), out value))//促销价
            price2 = value;
        //促销内容
        string PromotionDetail = row["PromotionDetail"].ToString();
        //
        if (price2 > 0 && price > price2)
        {
            sb.AppendLine("<span style='font-size:18px;margin:10px;color:#d85a26;'>" + PromotionDetail + "</span>");
            sb.AppendLine("<p class='ji'>");
            sb.AppendLine("<span>￥" + price2 + "</span>");
            sb.AppendLine("<span class='yj'>￥" + price + "</span>");
            sb.AppendLine("</p>");
        }
        else
        {
            sb.AppendLine("<p class='ji'>￥" + price + "</p>");
        }
        //
        int storage = 0;
        int.TryParse(row["storage"].ToString(), out storage);
        if (storage > 0) Storage = storage;
        //
        DrugJS = sb.ToString();
    }
    /// <summary>
    /// 获取库存
    /// </summary>
    /// <returns></returns>
    public string GetStorage()
    {
        if (Storage <= 0)
        {
            //缺货
            return "<span style='color:red;font-size:24px'>缺货</span>";
        }
        else
        {
            return "<span>" + Storage + "</span>";
        }
    }
    /// <summary>
    /// 获取说明书脚本
    /// </summary>
    private void GetInstructionsJS()
    {
        DrugInfo ins = new DrugInfo();
        DataTable table = ins.GetDrugMedia2(Drugid, "A", 2);
        if (table == null || table.Rows.Count == 0) return;
        string firstValue = "";
        //生成脚本
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<div class='cfli'>");
        sb.AppendLine("<dl>");
        //
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            string key = row["parname"].ToString();
            //
            string value = "";

            if (row["value"] != DBNull.Value && row["value"] != null)
            {
                byte[] byteArray = (byte[])row["value"];
                value = System.Text.Encoding.UTF8.GetString(byteArray);
            }
            //
            if (i == 0)
            {
                firstValue = value;
                sb.AppendLine("<dd onmouseover = 'showInstructions(this)' value='" + value + "' class='current'>" + key + "</dd>");
            }
            else
            {
                sb.AppendLine("<dd onmouseover = 'showInstructions(this)' value='" + value + "'>" + key + "</dd>");
            }
        }
        sb.AppendLine("</dl>");
        sb.AppendLine("</div>");
        sb.AppendLine("<div class='cfri'>");
        sb.AppendLine("<span>" + firstValue + "</span>");
        sb.AppendLine("</div>");
        InstructionsJS = sb.ToString();
    }
    /// <summary>
    /// 获取参数
    /// </summary>
    private void GetSessionPar()
    {
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
            CurDrugQuantity = SessionHelper.GetCartQuantity(Session,Drugid);
        }
        #endregion

        #region 是否展示货位菜单
        if (SessionHelper.GetIntPar(Session, "LocationFlg") <= 0)
        {
            LocDisJS = "style = 'display:none'";
            count--;
        }
        else
        {
            EnavDisJS = "style = 'display:none'";
        }
        #endregion

        #region 含黄麻碱药品是否可销售
        if (SessionHelper.GetIntPar(Session, "EphedrineFlg") <= 0)
        {
            EphedrineFlg = false;
        }
        #endregion

        #region 处方药品是否可销售
        if (SessionHelper.GetIntPar(Session, "PrescriptionFlg") <= 0)
        {
            PrescriptionFlg = false;
        }
        #endregion

        //菜单栏宽度
        VcoWidth = "style='width: " + count * 180 + "px;'";
    }
}