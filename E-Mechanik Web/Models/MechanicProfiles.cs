using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class MechanicProfiles : IEntity
    {
        public string MechanicName { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public virtual Position position { get; set; }
    }
}