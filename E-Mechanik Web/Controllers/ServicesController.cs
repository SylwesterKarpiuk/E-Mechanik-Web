using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Services;
using E_Mechanik_Web.Models;
using E_Mechanik_Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace E_Mechanik_Web.Controllers
{
    public class ServicesController : BaseController
    {
        // GET: Services
        public ActionResult Index(int? categoryId = null)
        {
            IEnumerable<Service> services = _db.Services;
            List<Service> list = services.ToList();
            var list2 = new List<string>();
            foreach (var item in services)
            {
                if (list2.Contains(item.Name))
                {
                    list.Remove(item);
                }
                else
                {
                    list2.Add(item.Name);
                }
            }
            return View(list);
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
            if (_db.AvailableServices.Any(c => c.AvailableServiceCategoryId == categoryId))
            {
                services = _db.AvailableServices.Where(p => p.AvailableServiceCategoryId == categoryId).Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name });
                return Json(services, JsonRequestBehavior.AllowGet);
            }
            return Json("Przyklad", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Mechanic")]
        // GET: Services/Create
        public ActionResult Create()
        {
            MechanicProfiles k = _db.MechanicProfiles.Where(c => c.MechanicName == this.User.Identity.Name).FirstOrDefault();
            if (k!=null)
            {
                if (k.MechanicName == null || k.CompanyName == null || k.City == null || k.Address == null)
                {
                    return RedirectToAction("FillMechanicProfile", "Account");
                }
                var model = new CreateServiceViewModel();
                model.Categories = _db.AvailableServiceCategories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                model.Services = _db.AvailableServices.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                return View(model);
            }
            else
            {
                return RedirectToAction("FillMechanicProfile", "Account");
            }
           
        }

        // POST: Services/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price,ExecutionTime,AvailableServiceCategoryId")] Service service)
        {
            if (ModelState.IsValid)
            {
                var x = service.AvailableServiceCategoryId;
                var Name = this.HttpContext.User.Identity.Name;
                service.MechanicName = Name;
                service.mechanicProfile = _db.MechanicProfiles.Where(c => c.MechanicName == service.MechanicName).FirstOrDefault();
                _db.Services.Add(service);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }



        [Authorize(Roles = "Mechanic")]
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

        [Authorize(Roles = "Mechanic")]
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

        public ActionResult GetServicesByName(string name)
        {
            IEnumerable<Service> services = _db.Services.Where(c => c.Name == name);
        
            return View(services);
        }
    }
}
