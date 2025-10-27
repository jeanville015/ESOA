using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class SOAFormatB
    { 
        public string Date { get; set; }
        public int Unit_IPP { get; set; }
        public int Unit_PP_SC { get; set; }
        public int Unit_RTA { get; set; }
        public int Unit_SNS { get; set; }
        public decimal Amt_IPP { get; set; }
        public decimal Amt_PP_SC { get; set; }
        public decimal Amt_RTA { get; set; }
        public decimal Amt_SNS { get; set; }
        public decimal Amt_Total { get; set; }
        public decimal Sf_IPP { get; set; }
        public decimal Sf_PP_SC { get; set; }
        public decimal Sf_RTA { get; set; }
        public decimal Sf_SNS { get; set; }
        public decimal Sf_Total { get; set; }
        public decimal WithholdingTax { get; set; }
        public decimal Total { get; set; }
        public decimal Total_2nd { get; set; }
        public decimal Rv_IPP { get; set; }
        public decimal Rv_PP_SC { get; set; }
        public decimal Rv_RTA { get; set; }
        public decimal Rv_SNS { get; set; }
        public decimal Settlement { get; set; }
        public decimal RunningBalance { get; set; }
        public decimal RunningBalance_2nd { get; set; }
        public decimal BalancePerAgent { get; set; }
        public decimal Variance { get; set; }
        public string AcceptanceDocNumber { get; set; }
        public string ServiceFeeDocNumber { get; set; }
    }
}
