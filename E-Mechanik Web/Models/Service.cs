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
        [Display(Name = "Nazwa usługi")]
        public string Name { get; set; }
        [Display(Name = "Odległość")]
        public string Distance { get; set; }
        [Display(Name = "Login mechanika")]
        public string MechanicName { get; set; }
        public virtual MechanicProfiles mechanicProfile { get; set; }
        public int AvailableServiceCategoryId { get; set; }
    }
}