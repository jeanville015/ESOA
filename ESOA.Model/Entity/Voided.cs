using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class Voided
    {
        public int Id { get; set; } 
        public string OriginAgentName { get; set; }
        public string OfficeCode { get; set; }
        public string Date { get; set; }
        public string ProductType { get; set; }
        public string TrackingNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string EntBranch { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public int Unit { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public string RefundDate { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription{ get; set; }
        public string Status { get; set; }
        public string EncashmentBranchHub { get; set; }
        public string Country { get; set; }
    }
}
