using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class ClientProfile : IEntity
    {
        public string ClientName { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string BodyType { get; set; }
        public string EngineCapacity { get; set; }
        public string GasType { get; set; }
        public string LastTechnicalExamination { get; set; }
        public string InsuranceEndDate { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}