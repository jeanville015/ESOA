using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Model
{
    public class UserAccount_Excel
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Team { get; set; }
        public string Role { get; set; }
        public bool ModuleAccess_Admin { get; set; }
        public bool ModuleAccess_SOA { get; set; }
        public bool ModuleAccess_Payment { get; set; }
        public bool ModuleAccess_Reports { get; set; }
        public bool ModuleAccess_Granular { get; set; }
        public bool AccessRights_Admin { get; set; }
        public bool AccessRights_SOA { get; set; }
        public bool AccessRights_Payment { get; set; }
        public bool AccessRights_Reports { get; set; }
        public bool AccessRights_Granular { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public string UpdatedBy_Name { get; set; }
        public bool IsDeleted { get; set; }

    }
}
