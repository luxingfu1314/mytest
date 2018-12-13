using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.Bll.Data
{
    public class YXZCheckEntity
    {
        public string Presc_no = "";
        public string Presc_date = "";
        public int Prescdateint = 0;
        public string Dispensary = "";
        public string Patient_id = null;
        public string Patient_name = null;
        public string Patient_type = null;
        public string Date_of_birth = null;
        public string Sex = null;
        public string Phoneno = null;
        public string Medicalhistory = null;
        public string Presc_identity = null;
        public string Charge_type = null;
        public string Rcpt_info = null;
        public int Repetition = 0;
        public double Costs = 0;
        public string Ordered_by = "";
        public string Ordered_by_name = "";
        public string Prescribed_by = "";
        public string Entered_by = "";
        public string RcptPic = "";
        //
        public List<YXZCheckDTLEntity> productList = new List<YXZCheckDTLEntity>();
    }
    public class YXZCheckDTLEntity
    {
        public string Presc_no = "";
        public int Item_no = 0;
        public string Advice_code = null;
        public string Drugid = "";
        public string Drug_code = "";
        public string Drug_spec = null;
        public string Drug_name = "";
        public string Firm_id = null;
        public string Firm_name = null;
        public string Unit = null;
        public string Package_spec = null;
        public string Package_units = null;
        public string Makeno = "";
        public string Invalidate = DateTime.Now.ToString("yyyy-MM-dd");
        public string Makedate = DateTime.Now.ToString("yyyy-MM-dd");
        public int Quantity = 0;
        public double Costs = 0;
        public double Price = 0;
        public string Dosage = null;
        public string Dosage_units = null;
        public string Administration = null;
        public string Frequency = null;
    }
}
