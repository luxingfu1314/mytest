using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.Bll.Data
{
    public class ROOT
    {
        public string OPSYSTEM = "HIS";
        public string OPWINID = "";
        public string OPTYPE = "200";
        public string OPIP = "";
        public string OPMANNO = "";
        public string OPMANNAME = "24H";
        public CONSIS_PRESC_MSTVW CONSIS_PRESC_MSTVW = new CONSIS_PRESC_MSTVW();
    }
    public class CONSIS_PRESC_MSTVW
    {
        public string PRESC_NO = "";
        public string PRESC_DATE = "";
        public string DISPENSARY = "";
        public string PATIENT_ID = "";
        public string PATIENT_NAME = "";
        public string INVOICE_NO = "";
        public string PATIENT_TYPE = "";
        public string DATE_OF_BIRTH = "";
        public string SEX = "";
        public string PRESC_IDENTITY = "";
        public string CHARGE_TYPE = "";
        public string PRESC_ATTR = "";
        public string PRESC_INFO = "";
        public string RCPT_INFO = "";
        public string RCPT_REMARK = "";
        public int REPETITION = 0;
        public double COSTS = 0;
        public double PAYMENTS = 0;
        public string ORDERED_BY = "";
        public string ORDERED_BY_NAME = "";
        public string PRESCRIBED_BY = "";
        public string ENTERED_BY = "";
        public int DISPENSE_PRI = 0;
        public string ISCHARGED = "";
        public string ISDRUG = "";
        public List<CONSIS_PRESC_DTLVW> DTLLIST = null;
    }
    public class CONSIS_PRESC_DTLVW
    {
        public string PRESC_NO = "";
        public decimal ITEM_NO = 0;
        public string ADVICE_CODE = "";
        public string DRUG_CODE = "";
        public string DRUG_SPEC = "";
        public string DRUG_NAME = "";
        public string FIRM_ID = "";
        public string FIRM_NAME = "";
        public string PACKAGE_SPEC = "";
        public string PACKAGE_UNITS = "";
        public string MAKENO = "";
        public string INVALIDATE = "";
        public string MAKEDATE = "";
        public int QUANTITY = 0;
        public string UNIT = "";
        public double COSTS = 0;
        public double PAYMENTS = 0;
        public string DOSAGE = "";
        public string DOSAGE_UNITS = "";
        public string ADMINISTRATION = "";
        public string FREQUENCY = "";
        public string ADDITIONUSAGE = "";
        public string RCPT_REMARK = "";
    }
}
