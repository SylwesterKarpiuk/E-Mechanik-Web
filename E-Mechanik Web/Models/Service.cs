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
        public int Price { get; set; }
        public string ExecutionTime { get; set; }
        public string MechanicId { get; set; }
        public int AvailableServiceCategoryId { get; set; }
    }
}