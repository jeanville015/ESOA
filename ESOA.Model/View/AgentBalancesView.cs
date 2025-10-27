namespace ESOA.Model.View
{
    public class AgentBalancesView
    {
        public decimal _Amt_Total { get; set; }
        public decimal _Sf_IPP { get; set; }
        public decimal _Sf_PP_SC { get; set; }
        public decimal _Sf_RTA { get; set; }
        public decimal _Sf_SNS { get; set; }
        public decimal _Sf_Total { get; set; } 
        public decimal _WithholdingTax { get; set; }
        public decimal _Settlement { get; set; }

        public string CustomerName { get; set; }
        public string OfficeCode { get; set; }
        public string AgentBalances { get; set; }
    } 
}
