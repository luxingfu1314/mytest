using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public class HotspotInfo
    {
        /// <summary>
        /// 获取热点性别列表
        /// </summary>
        /// <param name="consisposno"></param>
        /// <returns></returns>
        public DataTable GetSexList(string consisposno)
        {
            string sql = "select * from DT2_MN_GET_HOTSPOT_SEXLIST('" + consisposno + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取热点系统列表
        /// </summary>
        /// <param name="consisposno"></param>
        /// <returns></returns>
        public DataTable GetSystemList(string consisposno)
        {
            string sql = "select * from DT2_MN_GET_HOTSPOT_SYSTEMLIST('" + consisposno + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取热点列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetClassifyList(int type)
        {
            string sql = "select * from DT2_MN_GET_HOTSPOT_CLASSIFYLIST(" + type + ")";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        //

        /// <summary>
        /// 获取热点图片
        /// </summary>
        /// <param name="sexcode"></param>
        /// <param name="systemcode"></param>
        /// <returns></returns>
        public DataTable GetHotspotPic(string sexcode, string systemcode)
        {
            string sql = "select * from DT2_MN_GET_HOTSPOT_PIC('" + sexcode + "','" + systemcode + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取热点参数信息
        /// </summary>
        /// <param name="consisposno"></param>
        /// <param name="sexcode"></param>
        /// <param name="systemcode"></param>
        /// <returns></returns>
        public DataTable GetHotspotList(string consisposno, string sexcode, string systemcode)
        {
            string sql = "select * from DT2_MN_GET_HOTSPOTLIST('" + consisposno + "','" + sexcode + "','" + systemcode + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
    }
}
