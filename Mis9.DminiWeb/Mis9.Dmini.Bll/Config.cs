using Mis9.Dmini.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.SessionState;
using System.Data;
using Mis9.Dmini.Bll;

namespace Mis9.Dmini.Bll
{
    /// <summary>
    /// AppConfig 的摘要说明
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string SystemNo { get; set; }

        public string Dispensary { get; set; }

        public string Enterpriceid { get; set; }

        public string Compid { get; set; }

        public string Busno { get; set; }

        /// <summary>
        /// 米雅提供的商户号 
        /// </summary>
        public string A2 { get; set; }
        /// <summary>
        /// 通常为商户门店号 
        /// </summary>
        public string A3 { get; set; }
        /// <summary>
        /// 通常为商户门店 pos 机编号 
        /// </summary>
        public string A4 { get; set; }
        /// <summary>
        /// 通常为商户门店收银员编号 
        /// </summary>
        public string A5 { get; set; }
        /// <summary>
        /// 手机小票标
        /// </summary>
        public string B3 { get; set; }
        /// <summary>
        /// KEY 
        /// </summary>
        public string KEY { get; set; }

        /// <summary>
        /// 米雅支付接口
        /// </summary>
        public string MiyaUrl1 { get; set; }
        /// <summary>
        ///ERP销售订单接口
        /// </summary>
        public string ErpUrl1 { get; set; }
        /// <summary>
        /// 药先知审方接口
        /// </summary>
        public string YxzUrl1 { get; set; }
        /// <summary>
        /// 药先知审方状态查询接口
        /// </summary>
        public string YxzUrl2 { get; set; }
        /// <summary>
        /// 药先知订单状态接口
        /// </summary>
        public string YxzUrl3 { get; set; }

        private static Config instance = null;
        public static Config Instance()
        {
            if (instance == null)
                instance = new Config();
            return instance;
        }

        public void InitConfig(string connStr,string systemNo)
        {
            //加载其他字段
            CommonTools.Apis.WinApi.WriteLogFiles("InitConfig:参数初始化", @"c:\log\WebInfo");
            //ConnStr
            SqlHelper.ConnStr = connStr;
            //SystemNo
            SystemNo = systemNo;
            //ERP字段
            Dispensary=ParConfig.GetStringVal("Dispensary");
            Enterpriceid=ParConfig.GetStringVal("Enterpriceid");
            Compid = ParConfig.GetStringVal("Compid");
            Busno = ParConfig.GetStringVal("Busno");
            //米雅字段
            A2 = ParConfig.GetStringVal("A2");
            A3 = ParConfig.GetStringVal("A3");
            A4 = ParConfig.GetStringVal("A4");
            A5 = ParConfig.GetStringVal("A5");
            B3 = ParConfig.GetStringVal("B3");
            KEY = ParConfig.GetStringVal("KEY");
            //URL
            MiyaUrl1 = ParConfig.GetStringVal("MiyaUrl1");
            ErpUrl1 = ParConfig.GetStringVal("ErpUrl1");
            YxzUrl1 = ParConfig.GetStringVal("YxzUrl1");
            YxzUrl2 = ParConfig.GetStringVal("YxzUrl2");
            YxzUrl3 = ParConfig.GetStringVal("YxzUrl3");
            //
            CommonTools.Apis.WinApi.WriteLogFiles("MiyaUrl1:"+ MiyaUrl1, @"c:\log\WebInfo");
            CommonTools.Apis.WinApi.WriteLogFiles("ErpUrl1:" + ErpUrl1, @"c:\log\WebInfo");
            CommonTools.Apis.WinApi.WriteLogFiles("YxzUrl1:" + YxzUrl1, @"c:\log\WebInfo");
            CommonTools.Apis.WinApi.WriteLogFiles("YxzUrl2:" + YxzUrl2, @"c:\log\WebInfo");
            CommonTools.Apis.WinApi.WriteLogFiles("YxzUrl3:" + YxzUrl3, @"c:\log\WebInfo");
        }
    }
}