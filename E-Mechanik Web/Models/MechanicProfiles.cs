using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class MechanicProfiles : IEntity
    {
        [Required]
        public string MechanicName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string ImagePatch { get; set; }
    }
}