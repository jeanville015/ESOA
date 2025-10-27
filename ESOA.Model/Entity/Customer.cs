using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class AgentListing
    {
        public int Index {  get; set; }
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string LegalEntityName { get; set; }
        public string Tin { get; set; }
        public string Address { get; set; }
        public string SalesExec_LBC { get; set; }
        public decimal ApprovedAFC { get; set; }
        public Guid SOAFormatId { get; set; }
        public Guid RateCardId { get; set; }
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
        public decimal BeginningBalance { get; set; }
        public Guid DepositoryBankAccountId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        //formatting variables
        public string ApprovedAFC_str { get; set; }

        //paginated listview variables
        public string SoaTemplateName { get; set; }
        public string RateCardName { get; set; }
    }
}
