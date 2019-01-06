using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Car : IEntity
    {
        public int ClientId { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string ModelName { get; set; }
        public string Body { get; set; }
        public string EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public int Year { get; set; }
        public List<string> Equipment { get; set; }
    }
}