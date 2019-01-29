using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class AvailableServiceCategory : IEntity
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<AvailableService> AvailableServices { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}