<%@ WebHandler Language="C#" Class="ShoppingCart" %>

using System;
using System.Web;
using System.Data;
using System.IO;
using System.Web.SessionState;

public class ShoppingCart : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string optype = context.Request["optype"];

        switch (optype)
        {
            case "1":
                //添加药品至购物车
                AddCartItem(context);
                break;
            case "2":
                //从购物车删除药品
                DeleteCartItem(context);
                break;
            case "3":
                //清空购物车
                DeleteCartAll(context);
                break;
            case "4":
                //修改购物车
                UpdateCartItem(context);
                break;
            case "5":
                //选择所有
                CheckCartAll(context);
                break;
            case "6":
                //购物车商品数量
                GetQuantity(context);
                break;
            default:
                //未知操作
                context.Response.StatusCode = 300;
                context.Response.Write(Failure("未知操作！"));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                break;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 添加购物车
    /// </summary>
    /// <param name="context"></param>
    public void AddCartItem(HttpContext context)
    {
        string drugid = context.Request["drugid"];
        if (!string.IsNullOrEmpty(drugid))
        {
            int quantity = 1;
            string quantityStr = context.Request["quantity"];
            int _quantity = 1;
            if (int.TryParse(quantityStr, out _quantity))
            {
                quantity = _quantity;
            }
            DataTable dt = new DataTable();
            //如果Session变量存在，则直接获取
            if (context.Session["Cart"] != null)
            {
                dt = (DataTable)context.Session["Cart"];
            }
            else//如果Session变量不存在，创建存储数据的表结构
            {
                dt.Columns.Add(new DataColumn("Flg", typeof(bool)));
                dt.Columns.Add(new DataColumn("Id", typeof(String)));
                dt.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
            }
            //id不为null
            //则表示选中一件商品添加到购物车
            if (drugid != null)
            {
                //先判断购物车中是否已经存在该商品
                Boolean IsExist = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Id"].ToString() == drugid)
                    {
                        dt.Rows[i]["Quantity"] = int.Parse(dt.Rows[i]["Quantity"].ToString()) + quantity;
                        IsExist = true;
                        break;
                    }
                }
                if (!IsExist)
                //如果购物车中不存在该商品，则添加到购物车
                {
                    dt.Rows.Add(new object[] { true, drugid, quantity });
                }
                //
                context.Session["Cart"] = dt;
                context.Response.Write(Success("已加入购物车！"));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="context"></param>
    public void DeleteCartItem(HttpContext context)
    {
        string drugid = context.Request["drugid"];
        //id不为null
        //则表示选中一件商品从购物车中删除
        if (drugid != null)
        {
            DataTable dt = new DataTable();
            //如果Session变量存在，则直接获取
            if (context.Session["Cart"] != null)
            {
                dt = (DataTable)context.Session["Cart"];
                //复制表结构
                DataTable _dt = dt.Clone();
                //遍历数据
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Id"].ToString() != drugid)
                    {
                        _dt.Rows.Add(dr["Flg"], dr["Id"], dr["Quantity"]);
                    }
                }
                //
                if (_dt.Rows.Count > 0)
                    context.Session["Cart"] = _dt;
                else
                    context.Session["Cart"] = null;
            }
            //
            context.Response.Write(Success("删除成功！"));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="context"></param>
    public void DeleteCartAll(HttpContext context)
    {
        //清空
        context.Session["Cart"] = null;
        context.Response.Write(Success("清空成功！"));
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    /// <summary>
    /// 更新购物车状态
    /// </summary>
    /// <param name="context"></param>
    public void UpdateCartItem(HttpContext context)
    {
        string drugid = context.Request["drugid"];
        string checkflg = context.Request["checkflg"];
        bool flg1 = true;
        if (checkflg == null)
        {
            flg1 = false;
        }
        int quantity = 1;
        string quantityStr = context.Request["quantity"];
        bool flg2 = true;
        if (quantityStr == null)
        {
            flg2 = false;
        }
        else
        {
            int _quantity = 1;
            if (int.TryParse(quantityStr, out _quantity))
            {
                quantity = _quantity;
            }
        }
        DataTable dt = new DataTable();
        //如果Session变量存在，则直接获取
        //id不为null
        //则表示选中一件商品添加到购物车
        if (drugid != null && context.Session["Cart"] != null)
        {
            dt = (DataTable)context.Session["Cart"];
            //复制表结构
            DataTable _dt = dt.Clone();
            //遍历数据
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Id"].ToString() == drugid)
                {
                    _dt.Rows.Add(flg1 ? (checkflg == "0" ? false : true) : dr["Flg"], drugid, flg2 ? (quantity) : dr["Quantity"]);
                }
                else
                {
                    _dt.Rows.Add(dr["Flg"], dr["Id"], dr["Quantity"]);
                }
            }
            //
            context.Session["Cart"] = _dt;
        }
        context.Response.Write(Success("修改成功！"));
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    /// <summary>
    /// 全选购物车
    /// </summary>
    /// <param name="context"></param>
    public void CheckCartAll(HttpContext context)
    {
        string checkflg = context.Request["checkflg"];
        DataTable dt = new DataTable();
        //如果Session变量存在，则直接获取
        if (context.Session["Cart"] != null)
        {
            dt = (DataTable)context.Session["Cart"];
            //复制表结构
            DataTable _dt = dt.Clone();
            //遍历数据
            foreach (DataRow dr in dt.Rows)
            {
                _dt.Rows.Add(checkflg == "0" ? false : true, dr["Id"], dr["Quantity"]);
            }
            //
            context.Session["Cart"] = _dt;
        }
        context.Response.Write(Success("全选成功！"));
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    /// <summary>
    /// 购物车商品数量
    /// </summary>
    /// <returns></returns>
    public void GetQuantity(HttpContext context)
    {
        int quantity = 0;
        //如果Session变量存在，则直接获取
        if (context.Session["Cart"] != null)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)context.Session["Cart"];
            //
            DataRow[] rows = null;
            rows = dt.Select("Flg='true'");
            foreach (DataRow row in rows)
            {
                quantity+=int.Parse(row["Quantity"].ToString());
            }
        }
        context.Response.Write(Success(quantity.ToString()));
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected string Success(string str)
    {
        return "{\"StatusCode\":\"200\",\"Message\":\"" + str + "\"}";
    }

    protected string Failure(string str)
    {
        return "{\"StatusCode\":\"300\",\"Message\":\"" + str + "\"}";
    }
}