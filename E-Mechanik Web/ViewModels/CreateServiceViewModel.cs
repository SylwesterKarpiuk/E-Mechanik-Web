using E_Mechanik_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mechanik_Web.ViewModels
{
    public class CreateServiceViewModel : IEntity
    {
        [Required]
        public string Name { get; set; }
        public string MechanicName { get; set; }
        public int AvailableServiceCategoryId { get; set; }
        public string SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string SelectedService { get; set; }
        public IEnumerable<SelectListItem> Services { get; set; }

    }
}