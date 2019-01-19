using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Services;
using E_Mechanik_Web.Models;
using E_Mechanik_Web.ViewModels;
using Microsoft.AspNet.Identity;

namespace E_Mechanik_Web.Controllers
{
    public class ServicesController : BaseController
    {
        // GET: Services
        public ActionResult Index(int? categoryId = null)
        {
            IEnumerable<Service> services;
            if (categoryId.HasValue)
            {
                if (_db.ServiceCategories.Any(c => c.Id == categoryId.Value))
                {
                    services = _db.Services.Where(p => p.ServiceCategoryId == categoryId.Value);
                    return View(services);
                }
                
            }
            services = _db.Services;
            return View(services);
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = _db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        [HttpGet]
        public ActionResult GetServices(string service)
        {
            IEnumerable<SelectListItem> services;
            int categoryId = Int32.Parse(service);
            if (_db.AvailableServices.Any(c => c.ServiceCategoryId == categoryId))
            {
                services = _db.AvailableServices.Where(p => p.ServiceCategoryId == categoryId).Select(n=> new SelectListItem { Value = n.Id.ToString(), Text = n.Name });
                return Json(services, JsonRequestBehavior.AllowGet);
            }
            return Json("Przyklad", JsonRequestBehavior.AllowGet);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            var model = new CreateServiceViewModel();
            model.Categories = _db.AvailableServiceCategories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            model.Services = _db.AvailableServices.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(model);
        }

        // POST: Services/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price,ExecutionTime,ServiceCategoryId")] Service service)
        {
            if (ModelState.IsValid)
            {

                var Name = this.HttpContext.User.Identity.Name;
                service.MechanicId = Name;
                _db.Services.Add(service);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }



        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = _db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,ExecutionTime,MechanicId,ServiceId")] Service service)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(service).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = _db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = _db.Services.Find(id);
            _db.Services.Remove(service);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
