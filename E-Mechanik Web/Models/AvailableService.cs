using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class AvailableService : IEntity
    {
        [Display(Name = "Nazwa usługi")]
        public string Name { get; set; }
        public int AvailableServiceCategoryId { get; set; }
    }
}