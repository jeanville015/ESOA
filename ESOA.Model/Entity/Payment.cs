using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class Payment
    {
        public int Id { get; set; } 
        public string UploadedBy { get; set; }
        public string Date { get; set; }
        public string OriginAgentName { get; set; }
        public string CustomerId { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountGLCode { get; set; }
        public decimal USDPayment { get; set; }
        public decimal ExcRate { get; set; }
        public decimal PHPPayment { get; set; }
        public string Assignment { get; set; }
        public string Text { get; set; }
        public string SAPDocNumber { get; set; }
    }
}
