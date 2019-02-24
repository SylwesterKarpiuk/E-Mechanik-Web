using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Image
    {
        public string Patch { get; set; }
        public HttpPostedFileBase File { get; set; }

    }
}