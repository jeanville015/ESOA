using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class Customer_Excel
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string LegalEntityName { get; set; }
        public string Tin { get; set; }
        public string Address { get; set; }
        public string SalesExec_LBC { get; set; }
        public decimal ApprovedAFC { get; set; }
        public string SOAFormat { get; set; }
        public string RateCard { get; set; }
        public string Domestic_Intl { get; set; }
        public string Country { get; set; }
        public string TransmissionMode { get; set; }
        public string OfficeCode { get; set; }
        public string Area { get; set; }
        public long SAPCustomerId { get; set; }
        public long SAPVendorId_IPP { get; set; }
        public long SAPVendorId_PP_SC { get; set; }
        public long SAPVendorId_RTA { get; set; }
        public long SAPVendorId_SNS { get; set; }
        public long SAPVendorId_IPPX { get; set; }
        public string PaymentCurrency { get; set; }
        public string SFModeOfSettlement { get; set; }
        public string WithHoldingTax { get; set; }
        public string VatStatus { get; set; }
        public string Status { get; set; }
        public string EmailAddresses { get; set; }
        public string ContactNos { get; set; } 
        public string ContactPersons { get; set; }
    }
}
