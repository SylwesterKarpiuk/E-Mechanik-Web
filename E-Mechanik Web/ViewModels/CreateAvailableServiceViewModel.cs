using E_Mechanik_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mechanik_Web.ViewModels
{
    public class CreateAvailableServiceViewModel
    {
        [Required]
        public string Name { get; set; }
        public int ServiceCategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}