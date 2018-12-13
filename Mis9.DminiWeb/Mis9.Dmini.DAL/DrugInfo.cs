using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.DAL
{
    public class DrugInfo
    {
        /// <summary>
        /// 获取根节点分类
        /// </summary>
        /// <returns></returns>
        public string GetRootDrugType(int type)
        {
            return  SqlHelper.GetStringValue("DT2_FUNC_MN_GETROOTDRUGTYPE", type);
        }
        /// <summary>
        /// 获取药品子分类
        /// </summary>
        /// <param name="drugtype">药品分类，空着默认根节点</param>
        /// <returns></returns>
        public DataTable GetDrugTypes(string consisposno, string drugtype)
        {
            string sql = "select * from DT2_MN_GET_CONSISTYPES('" + consisposno + "'" + ",'" + drugtype + "')";
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品分类所含9药品编码
        /// </summary>
        /// <param name="drugtype">药品分类</param>
        /// <returns></returns>
        public DataTable GetDrugTypePics(string consisposno, string drugtype)
        {
            //
            string sql = "select * from DT2_MN_GET_CONSISTYPEPIC('" + consisposno + "'" + ",'" + drugtype + "')";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据药品drugtype获取药品列表信息
        /// </summary>
        /// <param name="drugtype"></param>
        /// <returns></returns>
        public DataTable GetDrugsByType(string consisposno, string drugtype)
        {
            //
            string sql = "select * from DT2_MN_GET_CONSISTYPE_DRUGS('" + consisposno + "'" + ",'" + drugtype + "')";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据药品拼音获取药品列表信息
        /// </summary>
        /// <param name="consisposno"></param>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public DataTable GetDrugsByPinYin(string consisposno, string pinyin)
        {
            //
            string sql = "select * from DT2_MN_GET_CONSIS_DRUGS('" + consisposno + "'" + ",'" + pinyin + "')";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据药品拼音获取药品列表信息
        /// </summary>
        /// <param name="consisposno"></param>
        /// <param name="typecode"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public DataTable GetDrugsByTypeChar(string consisposno, string typecode,string character)
        {
            //
            string sql = "select * from DT2_MN_GET_CONSISTYPE_DRUGS2('" + consisposno + "','" + typecode + "','" + character + "')";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品行号分组
        /// </summary>
        /// <returns></returns>
        public DataTable GetDrugRows(string consisposno)
        {
            string sql = "select * from DT2_MN_GET_CONSISROWS('"+ consisposno + "') order by consisposno, rowno";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 药品行号所含9药品编码
        /// </summary>
        /// <param name="row">药品分类</param>
        /// <returns></returns>
        public DataTable GetDrugRowPics(string consisposno,int row)
        {
            string sql = "select * from DT2_MN_GET_CONSISROWPIC('" + consisposno + "'" + "," + row + ")";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品货位列表
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public DataTable GetDrugs_Locatin(string consisposno,int row)
        {
            string sql = "select * from DT2_MN_GET_CONSISLOCATIONINFO('" + consisposno + "'"+","+ row + ") order by groupno,rowno,colno";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品图片或3D模型
        /// </summary>
        /// <param name="drugid"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public DataTable GetDrugMedia1(string drugid,string grade,string parname)
        {
            string sql = "select * from DT2_MN_GET_DRUGMEDIA1('" + drugid + "','" + grade + "','" + parname + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品详细信息
        /// </summary>
        /// <param name="consisposno"></param>
        /// <param name="drugid"></param>
        /// <param name="seqno"></param>
        /// <returns></returns>
        public DataTable GetDrugDetail(string consisposno,string drugid,int seqno=0)
        {
            string sql = "select * from DT2_MN_GET_DRUGINFO('" + consisposno + "'" + ",'" + drugid + "'" + "," + seqno + ")";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 获取药品多媒体数据
        /// </summary>
        /// <param name="drugid"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public DataTable GetDrugMedia2(string drugid, string grade, int partype)
        {
            string sql = "select * from DT2_MN_GET_DRUGMEDIA2('" + drugid + "'" + ",'" + grade + "'" + "," + partype + ")";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }

        public bool Exist3DModel(string drugid)
        {
            string sql = "select drugid from dt2_basic_drugs_mediadb where drugid='"+ drugid + "' and partype='1' and allowind='Y'";
            //
            DataTable table= SqlHelper.GetDataTable(sql, CommandType.Text);
            //
            if (table == null || table.Rows.Count <= 0) return false;
            else  return true;
        }
        /// <summary>
        /// 获取分类树结构
        /// </summary>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public DataTable GetDrugTypeTree(string typecode)
        {
            string sql = "select * from DT2_GET_DRUGTYPE_TREE('" + typecode + "')";
            //
            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据药品拼音获取药品数量
        /// </summary>
        /// <param name="drugtype"></param>
        /// <returns></returns>
        public DataTable GetDrugCount(string consisposno, string pinyin)
        {
            //
            string sql = "select count(*) as count from DT2_MN_GET_CONSIS_DRUGS2('" + consisposno + "'" + ",'" + pinyin + "')";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据药品拼音获取药品列表信息
        /// </summary>
        /// <param name="drugtype"></param>
        /// <returns></returns>
        public DataTable GetPageDrugs(string consisposno, string pinyin,int persize,int curpage)
        {
            //
            string sql = "select * from DT2_MN_GET_CONSIS_DRUGSPAGE('" + consisposno + "'" + ",'" + pinyin + "',"+ persize +","+ curpage + ")";

            return SqlHelper.GetDataTable(sql, CommandType.Text);
        }
    }
}
