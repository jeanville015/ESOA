using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class SOAFormatA<T>
    {
        public string CustomerName { get; set; }
        public int Unit_IPP_Total { get; set; }
        public int Unit_PP_SC_Total { get; set; }
        public int Unit_RTA_Total { get; set; }
        public int Unit_SNS_Total { get; set; }
        public decimal Amt_IPP_Total { get; set; }
        public decimal Amt_PP_SC_Total { get; set; }
        public decimal Amt_RTA_Total { get; set; }
        public decimal Amt_SNS_Total { get; set; }
        public decimal Amt_Total_Total { get; set; }
        public decimal Sf_IPP_Total { get; set; }
        public decimal Sf_PP_SC_Total { get; set; }
        public decimal Sf_RTA_Total { get; set; }
        public decimal Sf_SNS_Total { get; set; }
        public decimal Sf_Total_Total { get; set; }
        public decimal WithholdingTax_Total { get; set; }
        public decimal TotalLBCReceivable_Total { get; set; }
        public decimal Rv_IPP_Total { get; set; }
        public decimal Rv_PP_SC_Total { get; set; }
        public decimal Rv_RTA_Total { get; set; }
        public decimal Rv_SNS_Total { get; set; }
        public decimal Rv_Total_Total { get; set; }
        public decimal Settlement_Total { get; set; }
        //public decimal BeginningBalance { get; set; }
        //public decimal Email_AFC_Amount { get; set; }
        //public decimal Email_BeginningBalance_Amount { get; set; }
        //public decimal Email_BeginningBalance_Date { get; set; }
        //public decimal Email_Collections { get; set; }
        //public decimal Email_Encashments { get; set; }
        //public decimal Email_EWT { get; set; }
        //public decimal Email_Adjustment { get; set; }
        //public decimal Email_EndingBalance_Amount { get; set; }
        //public decimal Email_EndingBalance_Date { get; set; }
        //public decimal Email_NumberOfUnitsProcessed { get; set; }
        public List<T> Data { get; set; } 
        public SOAFormatAPreviousBalanceVariables sOAFormatAPreviousBalanceVariables { get; set; }
    }
}
