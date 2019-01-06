using E_Mechanik_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mechanik_Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected ApplicationDbContext _db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}