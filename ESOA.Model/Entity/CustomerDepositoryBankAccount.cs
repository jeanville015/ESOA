using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class CustomerDepositoryBankAccount    {
        public Guid Id { get; set; }  
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
