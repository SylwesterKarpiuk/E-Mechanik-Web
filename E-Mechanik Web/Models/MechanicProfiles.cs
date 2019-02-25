using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class MechanicProfiles : IEntity
    {
        [Display(Name ="Login: ")]
        [Required]
        public string MechanicName { get; set; }
        [Required]
        [Display(Name ="Nazwa zakładu")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Adres")]
        public string Address { get; set; }
        public string ImagePatch { get; set; }
    }
}