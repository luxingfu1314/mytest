using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.Bll.Data
{
    public class ERPEntity
    {
        public string enterpriceid = "";
        public string compid = "";
        public string busno = "";
        public List<ERPSaleEntity> salelist = new List<ERPSaleEntity>();
        public string tel = "";
        public double netsum = 0.0;
        public List<ERPPayEntity> paylist = new List<ERPPayEntity>();
        public string srcsaleno = "";
        public string saledate =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //
        public string ispay = "1";
        public string issend = "1";
        public string name = "";
        public string sex = "0";
        public string age = "";
        public string disease = "";
        public string anaphylaxis = "";
        public string userid = "";
        //
        public string attribute1 = "";
        public string attribute2 = "";
        public string attribute3 = "";
    }
    public class ERPSaleEntity
    {
        public string warecode = "";
        public int wareqty = 0;
        public double price = 0.0;
        public string makeno = "";
    }

    public class ERPPayEntity
    {
        public string paytype = "";
        public double paysum = 0.0;
    }
}
