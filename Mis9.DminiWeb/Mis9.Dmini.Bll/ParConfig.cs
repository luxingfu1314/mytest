using Mis9.Dmini.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.Bll
{
    public static class ParConfig
    {
        public static int GetIntVal(string parname, string consisposno = "000000", string sectionname = "Consis.Config.ClassPar.Item")
        {
            DataTable table = GetParVal(parname, consisposno, sectionname);
            if (table == null) return 0;
            //
            int d = 0;
            int.TryParse(table.Rows[0]["intval"].ToString(), out d);
            //
            return d;
        }
        public static string GetStringVal(string parname, string consisposno = "000000", string sectionname = "Consis.Config.ClassPar.Item")
        {
            DataTable table = GetParVal(parname, consisposno, sectionname);
            if (table == null) return "";
            //
            return table.Rows[0]["stringval"].ToString();
        }
        public static double GetDoubleVal(string parname, string consisposno = "000000", string sectionname = "Consis.Config.ClassPar.Item")
        {
            DataTable table = GetParVal(parname, consisposno, sectionname);
            if (table == null) return 0.0;
            //
            double d = 0.0;
            double.TryParse(table.Rows[0]["doubleval"].ToString(), out d);
            //
            return d;
        }
        public static DateTime GetDateVal(string parname, string consisposno = "000000", string sectionname = "Consis.Config.ClassPar.Item")
        {
            DataTable table = GetParVal(parname, consisposno, sectionname);
            if (table == null) return DateTime.Now;
            //
            DateTime d = System.DateTime.Now;
            DateTime.TryParse(table.Rows[0]["dateval"].ToString(), out d);
            //
            return d;
        }
        private static DataTable GetParVal(string parname, string consisposno, string sectionname)
        {
            string sql= "select top 1 partype,stringval,intval,doubleval,dateval from [dbo].[dc2_stu_consis_pardb] where  parname = '{0}' and consisposno = '{1}' and sectionname = '{2}' and allowind = 'Y'";
            //
            DataTable table = SqlHelper.GetDataTable(string.Format(sql, parname, consisposno, sectionname), CommandType.Text);
            if (table == null || table.Rows.Count == 0)
                return null;
            return table;
        }
    }
}
