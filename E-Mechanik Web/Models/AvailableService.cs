using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class AvailableService : IEntity
    {
        public string Name { get; set; }
        public int ServiceCategoryId { get; set; }
    }
}