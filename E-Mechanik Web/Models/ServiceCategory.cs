using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class ServiceCategory : IEntity
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}