using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Service : IEntity
    {
        [Required]
        public string Name { get; set; }
        public string Distance { get; set; }
        public string MechanicName { get; set; }
        public virtual MechanicProfiles mechanicProfile { get; set; }
        public int AvailableServiceCategoryId { get; set; }
    }
}