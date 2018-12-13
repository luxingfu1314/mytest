using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public class DeviceInfo
    {
        /// <summary>
        /// 根据药品drugid获取药品信息
        /// </summary>
        /// <param name="hostname">设备名称</param>
        /// <returns></returns>
        public void GetConsisNo(string hostname,string hostip,ref string posNo,ref string consisNoList)
        {
          //  if (hostname == "::1")
             //  hostname = "DESKTOP24";
            //hostname = "DESKTOPLEFT";

            posNo = SqlHelper.GetStringValue("DC2_FUNC_GETPOSNO", hostname, hostip);

            consisNoList = SqlHelper.GetStringValue("DC2_FUNC_GETCONSISNOLIST", posNo);
            //记录日志
            Mis9.CommonTools.Apis.WinApi.WriteLogFiles("计算机名："+hostname + ";计算机IP：" + hostip + ";计算机编号：" + posNo + ";设备编号：" + consisNoList, @"c:\log\WebInfo");
        }
        /// <summary>
        /// 获取发药口
        /// </summary>
        /// <param name="posNo"></param>
        /// <param name="disportnumber"></param>
        public void GetDisportNumber(string posNo,ref int disportnumber)
        {
            string sql = "select isnull(max(indexno),0) as disportnumber from dc2_stu_winportdb a ,dc2_stu_winport_eportdb b where a.disportnumber = b.disportnumber and posno02='" + posNo + "'";
            //
            DataTable table = SqlHelper.GetDataTable(sql, CommandType.Text);
            if (table == null || table.Rows.Count == 0) return;
            disportnumber = int.Parse(table.Rows[0]["disportnumber"].ToString());
        }
    }
}
