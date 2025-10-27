using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class CustomerContactNo
    {
        public Guid Id { get; set; } 
        public Guid CustomerId { get; set; } 
        public string ContactNo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
