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

public partial class Form_Drugs_04_1 : System.Web.UI.Page
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
    public string NavigationJs { get; set; }
    
    /// <summary>
    /// 主脚本
    /// </summary>
    public string MainJS { get; set; }
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
            //获取分类编码
            if (typecode == null)
            {
                DrugInfo ins = new DrugInfo();
                typecode = ins.GetRootDrugType(0);
            }
            //获取导航栏
            GetNavigationJs();
            //
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
        if (table == null|| table.Rows.Count==0) return;
        //
        string[] list = new string[table.Rows.Count];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            if (i == 0)
            {
                list[i] = "<a href = '../Forms/Drugs_04_1.aspx?type="+ row["typecode"] + "'>"+ row["typename"] + "</a>";
            }
            else if(i== table.Rows.Count-1)
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
        DataTable table = ins.GetDrugTypes(ConsisNoList, typecode);
        if (table == null) return;
        //生成脚本
        StringBuilder sb = new StringBuilder();
        int _index = 1;
        int seqno = 1;
        foreach (DataRow row in table.Rows)
        {
            string code = row["typecode"].ToString();
            string name = row["typename"].ToString();
            //判断maincode
            string href = "../Forms/Drugs_01.aspx";
            if (row["leafflg"] != DBNull.Value && row["leafflg"] != null)
            {
                int flg = 0;
                if (int.TryParse(row["leafflg"].ToString(),out flg)&& flg==1)
                {
                    href = "../Forms/Drugs_04_2.aspx";
                }
            }
            sb.AppendLine("<li>");
            sb.AppendLine("<div class='hwbox'>");
            if (_index >= 6) _index = 1;
            sb.AppendLine("<div class='tit t"+ _index + "'>");
            sb.AppendLine("<a href = '" + href + "?type=" + code + "' target ='_self'>" + name + "</a>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class='con'>");
            sb.AppendLine("<a href = '" + href + "?type=" + code + "' target ='_self'>");
            //获取图片
            DataTable picTable = ins.GetDrugTypePics(ConsisNoList, code);
            if (picTable == null) continue;
            foreach (DataRow picRow in picTable.Rows)
            {
                //药品图片
                string src = "../Images/no_pic_80.png";
                sb.AppendLine("<img id='pic_"+seqno+"' src = '" + src + "' drugid='" + picRow["drugid"].ToString() + "'/>");
                seqno++;
            }
            for (int i = picTable.Rows.Count; i < 9; i++)
            {
                string src = "../Images/no_pic_80.png";
                sb.AppendLine("<img src = '" + src + "'/>");
            }
            sb.AppendLine("</a>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</li>");
            _index++;
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