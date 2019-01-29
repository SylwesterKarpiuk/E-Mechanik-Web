using E_Mechanik_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mechanik_Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string search = null)
        {
            List<AvailableServiceCategory> model;
            if (!string.IsNullOrEmpty(search))
            {
                model = _db.AvailableServiceCategories.Where(c => c.Name.Contains(search)).ToList();
                var categoryIds = _db.Services.Where(p => p.Name.Contains(search)).Select(p => p.AvailableServiceCategoryId);
                model.AddRange(_db.AvailableServiceCategories.Where(c => categoryIds.Contains(c.Id)));
                model.Distinct();
            }
            else
            {
                model = _db.AvailableServiceCategories.ToList();
            }
            
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}