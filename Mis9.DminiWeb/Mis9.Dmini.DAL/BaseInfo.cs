using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public class BaseInfo
    {
        /// <summary>
        /// 核查是否分类为叶子节点
        /// </summary>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public string CheckLeaf(string typecode)
        {
            return SqlHelper.GetStringValue("DC2_FUNC_CHECK_MAINCODE", typecode);
        }
        /// <summary>
        /// 获取支付类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetPayType()
        {
            string sql = "select *,case when PaymentName like '%支付宝%' then 3 else 1 end as MiyaType from [dbo].[dt2_basic_payment] where PaymentName like '%支付宝%' or PaymentName like '%微信%'";
            return SqlHelper.GetDataTable(sql,System.Data.CommandType.Text);
        }
    }
}
