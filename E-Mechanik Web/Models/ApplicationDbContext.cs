﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("EmechanikDatabase", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<E_Mechanik_Web.Models.Service> Services { get; set; }
        public System.Data.Entity.DbSet<E_Mechanik_Web.Models.ServiceCategory> ServiceCategories { get; set; }
        public System.Data.Entity.DbSet<E_Mechanik_Web.Models.Car> Cars { get; set; }
    }

}