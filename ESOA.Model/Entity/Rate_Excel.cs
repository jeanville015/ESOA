using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class Rate_Excel
    {
        public Guid Id { get; set; } 
        public string Reference { get; set; }
        public decimal IPP { get; set; }
        public string RateType_IPP { get; set; }
        public decimal PP_SC { get; set; }
        public string RateType_PP_SC { get; set; }
        public decimal RTA { get; set; }
        public string RateType_RTA { get; set; }
        public decimal SNS { get; set; }
        public string RateType_SNS { get; set; }
        public decimal IPPX { get; set; }
        public string RateType_IPPX { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        //paginated listview variables
    }
}
