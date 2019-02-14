using E_Mechanik_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace E_Mechanik_Web.ViewModels
{
    public class ViewServiceInfoViewModel
    {
        [Key]
        public int Id { get; set; }
        public Service service { get; set; }
        public MechanicProfiles mechanicProfile { get; set; }
    }
}