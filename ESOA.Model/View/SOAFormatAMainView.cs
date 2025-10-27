using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.View

{
    public class SOAFormatAMainView<T>
    {
        public string CurrentUserRole { get; set; }
        public string CustomerName { get; set; }
        public string Unit_IPP_Total { get; set; }
        public string Unit_PP_SC_Total { get; set; }
        public string Unit_RTA_Total { get; set; }
        public string Unit_SNS_Total { get; set; }
        public string Amt_IPP_Total { get; set; }
        public string Amt_PP_SC_Total { get; set; }
        public string Amt_RTA_Total { get; set; }
        public string Amt_SNS_Total { get; set; }
        public string Amt_Total_Total { get; set; }
        public string Sf_IPP_Total { get; set; }
        public string Sf_PP_SC_Total { get; set; }
        public string Sf_RTA_Total { get; set; }
        public string Sf_SNS_Total { get; set; }
        public string Sf_Total_Total { get; set; }
        public string WithholdingTax_Total { get; set; }
        public string TotalLBCReceivable_Total { get; set; }
        public string Rv_IPP_Total { get; set; }
        public string Rv_PP_SC_Total { get; set; }
        public string Rv_RTA_Total { get; set; }
        public string Rv_SNS_Total { get; set; }
        public string Rv_Total_Total { get; set; }
        public string Settlement_Total { get; set; }
        public string BeginningBalance { get; set; }
        public string Email_AFC_Amount { get; set; }
        public string Email_BeginningBalance_Amount { get; set; }
        public string Email_BeginningBalance_Date { get; set; }
        public string Email_Collections { get; set; }
        public string Email_Encashments { get; set; }
        public string Email_EWT { get; set; }
        public string Email_Adjustment { get; set; }
        public string Email_EndingBalance_Amount { get; set; }
        public string Email_EndingBalance_Date { get; set; }
        public string Email_NumberOfUnitsProcessed { get; set; }
        public List<T> Data { get; set; }
    } 
}
