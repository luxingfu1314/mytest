using Mis9.Dmini.DAL;
using Mis9.Dmini.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// SessionHelper 的摘要说明
/// </summary>
public class SessionHelper
{
    /// <summary>
    /// 初始session 参数
    /// </summary>
    /// <param name="Session"></param>
    /// <param name="Request"></param>
    public static void SetSessionPar(HttpSessionState Session, HttpRequest Request)
    {
        #region 设备信息
        if (Session["ConsisNoList"] == null)
        {
            //客户端名,Request.UserHostName;
            string hostname = Request.ServerVariables["Remote_Host"];
            //客户端IP,Request.UserHostAddress;
            string hostip = Request.ServerVariables["Remote_Addr"];
            //根据客户端名获取绑定设备编号
            DeviceInfo device = new DeviceInfo();
            //GetConsisNo
            string posNo = "";
            string consisNoList = "";
            device.GetConsisNo(hostname, hostip, ref posNo, ref consisNoList);
            Session["ConsisNoList"] = consisNoList;
            Session["PosNo"] = posNo;
        }
        #endregion

        #region 是否展示购物车
        if (Session["CartFlg"] == null)
        {
            Session["CartFlg"]= ParConfig.GetIntVal("CartFlg", Session["PosNo"].ToString());
        }
        #endregion

        #region 是否展示货位
        if (Session["LocationFlg"] == null)
        {
            Session["LocationFlg"] = ParConfig.GetIntVal("LocationFlg", Session["PosNo"].ToString());
        }
        #endregion

        #region 含黄麻碱药品是否可销售
        if (Session["EphedrineFlg"] == null)
        {
            Session["EphedrineFlg"] = ParConfig.GetIntVal("EphedrineFlg", Session["PosNo"].ToString());
        }
        #endregion

        #region 处方药品是否可销售
        if (Session["PrescriptionFlg"] == null)
        {
            Session["PrescriptionFlg"] = ParConfig.GetIntVal("PrescriptionFlg", Session["PosNo"].ToString());
        }
        #endregion
    }
    /// <summary>
    /// 获取session int参数
    /// </summary>
    /// <param name="Session"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int GetIntPar(HttpSessionState Session, string key)
    {
        int value = 0;
        if (Session[key] == null)
        {
            value = 0;
        }
        else
        {
            int tempvalue = 0;
            if (int.TryParse(Session[key].ToString(), out tempvalue))
            {
                value = tempvalue;
            }
        }
        return value;
    }
    /// <summary>
    /// 获取session string参数
    /// </summary>
    /// <param name="Session"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetStringPar(HttpSessionState Session, string key)
    {
        string value = "";
        if (Session[key] != null)
        {
            value = Session[key].ToString();
        }
        return value;
    }
    /// <summary>
    /// 购物车药品数量
    /// </summary>
    /// <returns></returns>
    public static int GetCartQuantity(HttpSessionState Session)
    {
        //如果Session变量存在，则直接获取
        if (Session["Cart"] != null)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Cart"];
            //
            int quantity = 0;
            //
            DataRow[] rows = null;
            rows = dt.Select("Flg='true'");
            foreach (DataRow row in rows)
            {
                quantity+=int.Parse(row["Quantity"].ToString());
            }
            //
            return quantity;
        }
        return 0;
    }
    /// <summary>
    /// 购物车药品数量
    /// </summary>
    /// <returns></returns>
    public static int GetCartQuantity(HttpSessionState Session,string drugid)
    {
        //如果Session变量存在，则直接获取
        if (Session["Cart"] != null)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Cart"];
            //
            int quantity = 0;
            //
            DataRow[] rows = null;
            rows = dt.Select("Id='"+ drugid + "'");
            foreach (DataRow row in rows)
            {
                quantity += int.Parse(row["Quantity"].ToString());
            }
            //
            return quantity;
        }
        return 0;
    }
}