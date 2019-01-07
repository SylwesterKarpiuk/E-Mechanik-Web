using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mechanik_Web.ViewModels
{
    public class CreateServiceViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public string ExecutionTime { get; set; }
        public string MechanicName { get; set; }
        public int ServiceCategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}